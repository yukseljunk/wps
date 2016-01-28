using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ImageProcessor;
using ImageProcessor.Common.Exceptions;
using MimeTypes;
using PttLib;
using PttLib.Helpers;
using PttLib.TourInfo;
using WordPressSharp;
using WordPressSharp.Models;
using WordpressScraper.Dal;
using WordpressScraper.Ftp;
using WordpressScraper.Helpers;

namespace WindowsFormsApplication1
{
    public class PostFactory
    {

        #region Private Fields

        private readonly WordPressSiteConfig _siteConfig;
        private readonly BlogCache _blogCache;
        private readonly Dal _dal;
        private readonly string _blogUrl;
        private readonly bool _useMySqlFtpWay;
        private readonly int _maxImageDimension;
        private readonly int _thumbnailSize;
        private readonly bool _useCache;
        private readonly bool _useFeatureImage;
        private readonly int _mergeSize;
        private readonly int _defaultCategoryId;
        private IList<int> _userIds;
        private string _ftpDir;
        private IFtp _ftp;
        private static object _lock = new object();
        private Dictionary<int, bool> _imageExceptions = new Dictionary<int, bool>();
        private int _imageRequestCounter = 0;

        #endregion

        #region BackgroundWorker
        static BackgroundWorker _bw;


        #endregion
        #region Event Handlers

        public event EventHandler<Item> PostBeingCreated;

        public void OnPostBeingCreated(Item e)
        {
            EventHandler<Item> handler = PostBeingCreated;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<Item> PostCreated;
        public event EventHandler<IList<Item>> PostsCreated;

        public event EventHandler PostCreationStopped;

        public void OnPostCreationStopped(EventArgs e)
        {
            EventHandler handler = PostCreationStopped;
            if (handler != null) handler(this, e);
        }

        public void OnPostsCreated(IList<Item> e)
        {
            EventHandler<IList<Item>> handler = PostsCreated;
            if (handler != null) handler(this, e);
        }

        public void OnPostCreated(Item e)
        {
            EventHandler<Item> handler = PostCreated;
            if (handler != null) handler(this, e);
        }

        #endregion

        public void CancelPostCreation()
        {
            if (_bw.IsBusy) _bw.CancelAsync();
        }

        private const string ImagesDir = "temp";
        private readonly ProgramOptions _options;

        public PostFactory(WordPressSiteConfig siteConfig,
            IFtp ftp,
            BlogCache blogCache,
            Dal dal,
            ProgramOptions options)
        {
            _siteConfig = siteConfig;
            _ftp = ftp;
            _blogCache = blogCache;
            _dal = dal;
            _blogUrl = options.BlogUrl;
            _useMySqlFtpWay = options.UseFtp;
            _maxImageDimension = options.ResizeImages ? options.ResizeSize : 0;
            _thumbnailSize = options.ThumbnailSize;
            _useCache = options.UseCache;
            _useFeatureImage = options.MakeFirstImageAsFeature;
            _mergeSize = options.MergeBlockSize;

            _options = options;
            var userDal = new UserDal(new Dal(MySqlConnectionString));
            _userIds = userDal.UserIds();

            if (options.UseFtp)
            {
                _ftpDir = "wp-content/uploads/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                _ftp.MakeFtpDir(_ftpDir);
            }
            Directory.CreateDirectory(ImagesDir);

            var categoryDal = new CategoryDal(_dal);
            _defaultCategoryId = categoryDal.GetInsertDefaultCategoryId();

        }
        private string MySqlConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3}; Allow User Variables=True", _options.DatabaseUrl, _options.DatabaseName, _options.DatabaseUser, _options.DatabasePassword);
            }
        }

        public void Create(IList<Item> items)
        {

            _bw = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _bw.DoWork += (obj, e) => CreatePosts(items, e, _mergeSize);
            _bw.ProgressChanged += CreatePostProgress;
            _bw.RunWorkerCompleted += CreatePostsFinished;
            _bw.RunWorkerAsync();

        }

        private void CreatePostsFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref _imageRequestCounter, -1000, 0) == -1000)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
                OnPostsCreated(null);
                OnPostCreationStopped(null);
            }
            else if (e.Error != null)
            {
                OnPostsCreated(null);
            }
            else
            {
                if (e.Result != null)
                {
                    while (true)
                    {
                        if (Interlocked.CompareExchange(ref _imageRequestCounter, -1000, 0) == -1000)
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    OnPostsCreated((List<Item>)e.Result);
                }

            }
        }

        private void CreatePostProgress(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                var item = (Item)e.UserState;
                if (item.PostId > int.MinValue)
                {
                    OnPostCreated(item);
                }
                else
                {
                    OnPostBeingCreated(item);
                }
            }
        }

        private void CreatePosts(IList<Item> items, DoWorkEventArgs e, int minWordCount)
        {
            var itemIndex = 0;
            var itemCount = items.Count;

            var mainQueue = new Queue<Queue<Item>>();
            var subQueue = new Queue<Item>();
            Queue<Item> lastQueue = null;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                itemIndex++;
                item.PostId = int.MinValue;

                var itemPresent = false;

                if (_useCache)
                {
                    if (_blogCache.IdsPresent(_blogUrl).Contains(item.ForeignKey))
                    {
                        item.PostId = 0;
                        itemPresent = true;
                    }
                }
                if (item.IsInvalid)
                {
                    item.PostId = -2;
                }

                if (itemPresent || item.IsInvalid)
                {
                    _bw.ReportProgress(itemIndex / itemCount * 100, item);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                if (!itemPresent && !item.IsInvalid)
                {
                    if (item.WordCount >= minWordCount)
                    {
                        var q = new Queue<Item>();
                        q.Enqueue(item);
                        mainQueue.Enqueue(q);
                        lastQueue = q;
                        continue;
                    }

                    subQueue.Enqueue(item);

                    if (subQueue.Sum(it => it.WordCount) >= minWordCount)
                    {
                        var q = CloneQueue(subQueue);
                        mainQueue.Enqueue(q);
                        lastQueue = q;
                        subQueue.Clear();
                    }

                }
                if (itemPresent || item.IsInvalid)
                {
                    _bw.ReportProgress(itemIndex / itemCount * 100, item);

                }
                if (_bw.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
            }

            if (lastQueue != null)
            {
                while (subQueue.Count > 0)
                {
                    lastQueue.Enqueue(subQueue.Dequeue());
                }
            }
            else//nothing on main queue
            {
                mainQueue.Enqueue(subQueue);
            }

            if (mainQueue.Count > 0)
            {
                PostitemsQueued(e, mainQueue, itemIndex, itemCount);
            }
            e.Result = items;
        }


        private void PostitemsQueued(DoWorkEventArgs e, Queue<Queue<Item>> mainQueue, int itemIndex, int itemCount)
        {
            _imageExceptions = new Dictionary<int, bool>();
            var blockSize = 1;
            while (mainQueue.Count > 0)
            {
                Queue<Item> qi = null;
                var tasks = new List<Task<int>>();
                for (int i = 0; i < blockSize; i++)
                {
                    if (mainQueue.Count == 0)
                    {
                        break;
                    }
                    qi = mainQueue.Dequeue();
                    if (qi.Count == 0)
                    {
                        continue;
                    }
                    Queue<Item> qi1 = CloneQueue(qi);
                    tasks.Add(new Task<int>(() =>
                    {

                        var authorId = _userIds[Helper.GetRandomNumber(0, _userIds.Count)];

                        _bw.ReportProgress(itemIndex / itemCount * 100, qi1.First());

                        var itemsToMerge = qi1.Aggregate("", (current, sqi) => current + (sqi.Title + ","));


                        var postId = CreateMerged(qi1.ToList(), authorId);
                        foreach (var sqi in qi1)
                        {
                            sqi.PostId = postId;
                            _bw.ReportProgress(itemIndex / itemCount * 100, sqi);
                        }

                        return 1;
                    }));

                    if (_bw.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }


                }

                if (_bw.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                foreach (var task in tasks)
                {
                    task.Start();
                }

                try
                {
                    Task.WaitAll(tasks.ToArray());
                }
                catch (AggregateException exception)
                {
                    Logger.LogExceptions(exception.Flatten());
                }
            }
        }

        private static Queue<T> CloneQueue<T>(Queue<T> queue)
        {
            var result = new Queue<T>();

            while (queue.Count > 0)
            {
                result.Enqueue(queue.Dequeue());
            }

            return result;
        }

        private int CreateMerged(IList<Item> items, int authorId)
        {
            if (items == null || !items.Any())
            {
                return -1;
            }
            if (items.Count == 1)
            {
                return Create(items[0], authorId);
            }
            var itemsSortedByRelevance = items.OrderByDescending(i => i.Relevance);
            return Create(new MultiItem(itemsSortedByRelevance.ToList()), authorId);
        }

        /// <summary>
        /// create item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="authorId"> </param>
        /// <returns>id crated, -1 if error,-3 if image error</returns>
        private int Create(Item item, int authorId)
        {
            var postDal = new PostDal(new Dal(MySqlConnectionString));
            WordPressClient client = null;

            if (!_useMySqlFtpWay)
            {
                client = new WordPressClient(_siteConfig);
            }

            try
            {
                var postTitle = "";
                lock (_lock)
                {
                    postTitle = GetPostTitle(item);
                    _blogCache.InsertTitle(_blogUrl, postTitle);
                }

                var imageUploads = GetImageUploads(item, postTitle, authorId, client);
                var yoastFocusKey = StopwordTool.RemoveStopwords(postTitle, true);
                var post = new Post
                {
                    PostType = "post",
                    Title = postTitle,
                    Content = item.PostBody(_thumbnailSize, true, _options.TagsAsText),
                    PublishDateTime = DateTime.Now,
                    Author = authorId.ToString(),
                    CommentStatus = "open",
                    Status = "draft",
                    BlogUrl = _blogUrl,
                    CustomFields = new[]
                    {
                        new CustomField() {Key = "foreignkey", Value = item.ForeignKey},
                        new CustomField() {Key = "_aioseop_title", Value = item.Title},
                        new CustomField() {Key = "_aioseop_description", Value = item.MetaDescription},
                        new CustomField() {Key = "_aioseop_keywords", Value = string.Join(",", item.Tags)},
                        new CustomField() {Key = "_yoast_wpseo_focuskw_text_input", Value = yoastFocusKey},
                        new CustomField() {Key = "_yoast_wpseo_focuskw", Value = yoastFocusKey},
                        new CustomField() {Key = "_thumbnail_id", Value = ""},
                    }
                };

                if (imageUploads.Any())
                {
                    if (_useFeatureImage)
                    {
                        post.FeaturedImageId = imageUploads[0].Id;
                    }
                    post.ImageIds = imageUploads.Select(i => i.Id).ToList();
                    post.CustomFields[4].Value = imageUploads[0].Id;
                }
                List<Term> terms = new List<Term>();
                if (!_options.TagsAsText)
                {
                    terms = GetTags(item, client);
                }
                terms.Add(new Term() {Id = _defaultCategoryId.ToString()});
                post.Terms = terms.ToArray();

                string newPost = "-1";
                newPost = _useMySqlFtpWay ? postDal.InsertPost(post).ToString() : client.NewPost(post);
                return Convert.ToInt32(newPost);
            }
            catch (ImageProcessingException exc)
            {
                if (item != null)
                {
                    Logger.LogProcess(item.ToString());
                }
                Logger.LogProcess("Author:" + authorId);
                Logger.LogExceptions(exc);
                return -3;
            }
            catch (Exception exception)
            {
                if (item != null)
                {
                    Logger.LogProcess(item.ToString());
                }
                Logger.LogProcess("Author:" + authorId);
                Logger.LogExceptions(exception);
                return -1;
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }
            return -1;
        }


        private IList<UploadResult> GetImageUploads(Item item, string postTitle, int authorId,
            WordPressClient client)
        {
            var converterFunctions = new ConverterFunctions();
            var imageDal = new ImageDal(new Dal(MySqlConnectionString));
            var imageIndex = 1;
            IList<UploadResult> imageUploads = new List<UploadResult>();
            var imagePosts = new List<ImagePost>();
            foreach (var itemImage in item.ItemImages)
            {
                var imageUrl = itemImage.OriginalSource;
                var uri = new Uri(imageUrl);
                var imageUrlWithoutQs = uri.GetLeftPart(UriPartial.Path);

                var extension = Path.GetExtension(imageUrlWithoutQs).ToLower();
                int imageWidth, imageHeight;

                var imageName = RefineImageName(postTitle) + " " + itemImage.ContainingItem.Site.Substring(0, 2);

                if (!itemImage.Primary)
                {
                    var multiItem = item as MultiItem;
                    if (multiItem != null)
                    {
                        var pt = converterFunctions.FirstNWords(itemImage.ContainingItem.Title, 65, true);
                        imageName = RefineImageName(pt) + " " + itemImage.ContainingItem.Site.Substring(0, 2) + " " +
                                         itemImage.ContainingItem.Id;
                    }
                }
                var imageStart = imageName + "-" + imageIndex;

                UploadResult uploaded = null;

                if (_options.UseRemoteDownloading)
                {

                    uploaded = new UploadResult()
                    {
                        Url = _blogUrl + "/" + _ftpDir + "/" + imageStart + extension,
                        Id = "1",
                        OriginalUrl = itemImage.OriginalSource,
                        FileName = imageStart + extension,
                        ItemOrder = item.Order

                    };
                    var imgPost = new ImagePost()
                                      {
                                          Title = converterFunctions.SeoUrl(imageStart),
                                          Url = uploaded.Url,
                                          Author = authorId.ToString(),
                                          Alt = item.Title + imageIndex,
                                          PublishDateTime = DateTime.Now,
                                          Content = item.Title + imageIndex,
                                          Width = 100,
                                          Height = 100,
                                          ThumbnailWidth = _thumbnailSize,
                                          ThumbnailHeight = _thumbnailSize
                                      };
                    uploaded.ImagePost = imgPost;
                    imagePosts.Add(imgPost);
                }
                else
                {
                    var imageData = GetImageData(extension, imageUrl, out imageWidth, out imageHeight);
                    imageData.Item1.Name = imageStart + extension;
                    imageData.Item2.Name = imageStart + "-" + _thumbnailSize + "x" + _thumbnailSize + extension;

                    if (_useMySqlFtpWay)
                    {
                        _ftp.UploadFileFtp(imageData.Item1, _ftpDir);
                        _ftp.UploadFileFtp(imageData.Item2, _ftpDir);
                        uploaded = new UploadResult()
                                       {
                                           Url = _blogUrl + "/" + _ftpDir + "/" + imageStart + extension,
                                           Id = "1"
                                       };
                        imagePosts.Add(new ImagePost()
                                           {
                                               Title = converterFunctions.SeoUrl(imageStart),
                                               Url = uploaded.Url,
                                               Author = authorId.ToString(),
                                               Alt = item.Title + imageIndex,
                                               PublishDateTime = DateTime.Now,
                                               Content = item.Title + imageIndex,
                                               Width = imageWidth,
                                               Height = imageHeight,
                                               ThumbnailWidth = _thumbnailSize,
                                               ThumbnailHeight = _thumbnailSize
                                           });
                    }
                    else
                    {
                        uploaded = client.UploadFile(imageData.Item1);
                    }
                }

                var thumbnailUrl = string.Format("{0}/{1}/{2}-{3}-{4}x{4}{5}", _blogUrl, _ftpDir, imageName,
                    imageIndex, _thumbnailSize, extension);
                imageUploads.Add(uploaded);
                itemImage.NewSource = thumbnailUrl;
                itemImage.Link = _blogUrl + converterFunctions.SeoUrl(postTitle) + "/" + converterFunctions.SeoUrl(imageStart);
                imageIndex++;
            }

            if (!_useMySqlFtpWay) return imageUploads;

            var imageIds = imageDal.Insert(imagePosts, _ftpDir);
            for (int i = 0; i < imageUploads.Count; i++)
            {
                imageUploads[i].Id = imageIds[i].ToString();
            }
            if (_options.UseRemoteDownloading)
            {
                MakeRemoteImageRequest(imageUploads);
            }

            while (true)
            {
                if (Interlocked.CompareExchange(ref _imageRequestCounter, -1000, 0) == -1000)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            lock (_lock)
            {
                if (_imageExceptions[item.Order])
                {
                    throw new ImageProcessingException("Image donwload or metadata insertion failed, check for previous errors.");
                }
            }
            return imageUploads;
        }


        private void MakeRemoteImageRequest(IList<UploadResult> uploadResults)
        {
            foreach (var uploadResult in uploadResults)
            {
                var url = string.Format("{0}/wp-image.php?url={1}&file={2}&folder={3}&thsize={4}", _options.BlogUrl, uploadResult.OriginalUrl, uploadResult.FileName, _ftpDir, _thumbnailSize);
                Interlocked.Increment(ref _imageRequestCounter);
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadStringCompleted += ImageDownloaded;
                    webClient.DownloadStringAsync(new Uri(url), uploadResult);
                }
                Thread.Sleep(250);
            }
        }

        private void ImageDownloaded(object sender, DownloadStringCompletedEventArgs e)
        {
            Interlocked.Decrement(ref _imageRequestCounter);
            var uploadResult = e.UserState as UploadResult;
            if (uploadResult == null) return;

            lock (_lock)
            {
                if (!_imageExceptions.ContainsKey(uploadResult.ItemOrder))
                {
                    _imageExceptions.Add(uploadResult.ItemOrder, false);
                }
            }

            Size size = new Size(100, 100);

            if (e.Error == null && !string.IsNullOrEmpty(e.Result))
            {
                var result = e.Result.Replace("\n", "").Replace("\r", "");
                var parsed = result.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
                if (parsed.Length > 1)
                {
                    int w, h;
                    if (Int32.TryParse(parsed[0], out w) && Int32.TryParse(parsed[1], out h))
                    {
                        size = new Size(w, h);
                    }
                }
                else
                {
                    lock (_lock)
                    {
                        _imageExceptions[uploadResult.ItemOrder] = true;
                    }
                    Logger.LogExceptions(new Exception(result));
                    return;
                }
            }
            if (e.Error != null)
            {
                lock (_lock)
                {
                    _imageExceptions[uploadResult.ItemOrder] = true;
                }
                Logger.LogExceptions(e.Error);
                return;
            }

            try
            {
                var imageDal = new ImageDal(new Dal(MySqlConnectionString));
                uploadResult.ImagePost.Width = size.Width;
                uploadResult.ImagePost.Height = size.Height;
                imageDal.InsertAttachmentMetaData(int.Parse(uploadResult.Id), uploadResult.ImagePost, _ftpDir);
            }
            catch (Exception exception)
            {
                lock (_lock)
                {
                    _imageExceptions[uploadResult.ItemOrder] = true;
                }

                Logger.LogExceptions(exception);
            }
        }

        /// <summary>
        /// create/get tags on blog, return their ids
        /// </summary>
        /// <param name="item"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private List<Term> GetTags(Item item, WordPressClient client)
        {
            var converterFunctions = new ConverterFunctions();
            var terms = new List<Term>();
            var tagDal = new TagDal(new Dal(MySqlConnectionString));
            foreach (var tag in item.Tags)
            {
                if (_useCache)
                {
                    lock (_lock)
                    {

                        var tagOnBlog =
                            _blogCache.TagsPresent(_blogUrl).FirstOrDefault(
                                t =>
                                HttpUtility.HtmlDecode(t.Name).Trim().ToLowerInvariant() ==
                                HttpUtility.HtmlDecode(converterFunctions.RemoveDiacritics(tag)).Trim().ToLowerInvariant
                                    ());
                        if (tagOnBlog == null)
                        {
                            var t = new Term
                                        {
                                            Name = converterFunctions.RemoveDiacritics(tag),
                                            Description = tag,
                                            Slug = tag.Replace(" ", "_"),
                                            Taxonomy = "post_tag"
                                        };

                            if (_useMySqlFtpWay)
                            {
                                var tId = tagDal.InsertTag(t);
                                t.Id = tId.ToString();
                            }
                            else
                            {
                                var termId = client.NewTerm(t);
                                t.Id = termId;
                            }

                            if (_useCache)
                            {
                                _blogCache.InsertTag(_blogUrl, t);
                            }

                            if (!terms.Select(term => term.Id).Contains(t.Id))
                            {
                                terms.Add(t);
                            }
                        }
                        else
                        {
                            if (!terms.Select(term => term.Id).Contains(tagOnBlog.Id))
                            {
                                terms.Add(tagOnBlog);
                            }
                        }
                    }
                }
            }
            return terms;
        }

        private string GetPostTitle(Item item)
        {
            var converterFunctions = new ConverterFunctions();
            var postTitle = converterFunctions.FirstNWords(item.Title, 65, true);
            postTitle = RefineImageName(postTitle);
            if (!_blogCache.TitlesPresent(_blogUrl).Contains(postTitle)) return postTitle;

            var initialPostTitle = postTitle;
            var postIndex = 2;
            while (true)
            {
                postTitle = initialPostTitle + "-" + postIndex;
                if (!_blogCache.TitlesPresent(_blogUrl).Contains(postTitle))
                {
                    break;
                }
                postIndex++;
            }
            return postTitle;
        }

        /// <summary>
        /// download image, find original width and height, check _maxImagedimension and resize accordingly
        /// create thumbnail and return bits of these 2 images
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="imageUrl"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private Tuple<Data, Data> GetImageData(string extension, string imageUrl, out int width, out int height)
        {
            var tempImageFileName = ImagesDir + "/" + "temp" + extension;
            //download image and resize it...
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(imageUrl, tempImageFileName);
            }

            var imageData = Data.CreateFromFilePath(tempImageFileName, MimeTypeMap.GetMimeType(extension));
            bool resize;
            using (var img = Image.FromFile(tempImageFileName))
            {
                width = img.Width;
                height = img.Height;

                resize = img.Width > _maxImageDimension || img.Height > _maxImageDimension;
            }

            var thumbWidth = width;
            var thumbHeight = height;
            var cropRect = new Rectangle();
            if (width > height)
            {
                thumbHeight = _thumbnailSize;
                thumbWidth = (int)((width / (double)height) * _thumbnailSize);
                cropRect = new Rectangle((thumbWidth - thumbHeight) / 2, 0, thumbHeight, thumbHeight);
            }
            else
            {
                thumbWidth = _thumbnailSize;
                thumbHeight = (int)(((height / (double)width)) * _thumbnailSize);
                cropRect = new Rectangle(0, (thumbHeight - thumbWidth) / 2, thumbWidth, thumbWidth);

            }

            var thumbFileName = ImagesDir + "/" + "temp_thumb" + extension;

            var size = new Size(thumbWidth, thumbHeight);
            using (MemoryStream inStream = new MemoryStream(imageData.Bits))
            {
                using (var outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                        var imgf = imageFactory.Load(inStream)
                            .Constrain(size)
                            .Crop(cropRect)
                            .Save(thumbFileName);
                    }
                }
            }

            var thumbImageData = Data.CreateFromFilePath(thumbFileName, MimeTypeMap.GetMimeType(extension));

            if (!resize) return new Tuple<Data, Data>(imageData, thumbImageData);
            size = new Size(_maxImageDimension, _maxImageDimension);
            using (MemoryStream inStream = new MemoryStream(imageData.Bits))
            {
                using (var outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                        var imgf = imageFactory.Load(inStream)
                            .Constrain(size)
                            .Save(tempImageFileName);
                        width = imgf.Image.Width;
                        height = imgf.Image.Height;
                    }
                }
            }
            imageData = Data.CreateFromFilePath(tempImageFileName, MimeTypeMap.GetMimeType(extension));
            return new Tuple<Data, Data>(imageData, thumbImageData);
        }

        private static string RefineImageName(string imageName)
        {
            var result = imageName;
            var invalidChars = Path.GetInvalidFileNameChars().ToList();
            foreach (var invalidChar in invalidChars)
            {
                result = result.Replace(invalidChar.ToString(), "");
                result = result.Replace("&#" + ((int)invalidChar) + ";", "");
            }
            result = result.Replace("&#39;", "");
            result = result.Replace("&quot;", "");
            var rgx = new Regex("[^a-zA-Z0-9 ]");
            result = rgx.Replace(result, " ").Trim();
            result = result.Replace("     ", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");
            return result;
        }
    }
}