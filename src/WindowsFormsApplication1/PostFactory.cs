using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using ImageProcessor;
using MimeTypes;
using PttLib;
using PttLib.Helpers;
using PttLib.TourInfo;
using WordPressSharp;
using WordPressSharp.Models;
using WordpressScraper.Dal;
using WordpressScraper.Helpers;

namespace WindowsFormsApplication1
{
    public class PostFactory
    {

        #region Private Fields

        private readonly WordPressSiteConfig _siteConfig;
        private readonly FtpConfig _ftpConfiguration;
        private readonly BlogCache _blogCache;
        private readonly Dal _dal;
        private readonly string _blogUrl;
        private readonly bool _useMySqlFtpWay;
        private readonly int _maxImageDimension;
        private readonly int _thumbnailSize;
        private readonly bool _useCache;
        private readonly bool _useFeatureImage;
        private readonly int _mergeSize;
        private IList<int> _userIds;
        private string _ftpDir;

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

        public PostFactory(WordPressSiteConfig siteConfig,
            FtpConfig ftpConfiguration,
            BlogCache blogCache,
            Dal dal,
            string blogUrl,
            bool useMySqlFtpWay = true,
            int maxImageDimension = 0,
            int thumbnailSize = 150,
            bool useCache = true,
            bool useFeatureImage = false,
            int mergeSize = 600)
        {
            _siteConfig = siteConfig;
            _ftpConfiguration = ftpConfiguration;
            _blogCache = blogCache;
            _dal = dal;
            _blogUrl = blogUrl;
            _useMySqlFtpWay = useMySqlFtpWay;
            _maxImageDimension = maxImageDimension;
            _thumbnailSize = thumbnailSize;
            _useCache = useCache;
            _useFeatureImage = useFeatureImage;
            _mergeSize = mergeSize;
            var userDal = new UserDal(_dal);
            _userIds = userDal.UserIds();

            if (useMySqlFtpWay)
            {
                var ftp = new Ftp();
                _ftpDir = "wp-content/uploads/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                ftp.MakeFtpDir(_ftpConfiguration.Url, _ftpDir, _ftpConfiguration.UserName, _ftpConfiguration.Password);
            }
            Directory.CreateDirectory(ImagesDir);

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

        private void CreatePosts(IList<Item> items, DoWorkEventArgs e)
        {
            var itemIndex = 0;
            var itemCount = items.Count;
            foreach (var item in items)
            {
                itemIndex++;
                item.PostId = int.MinValue;
                _bw.ReportProgress(itemIndex / itemCount * 100, item);
                Thread.Sleep(TimeSpan.FromSeconds(1));

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
                var authorId = _userIds[Helper.GetRandomNumber(0, _userIds.Count)];

                if (!itemPresent && !item.IsInvalid)
                {
                    var itemPostId = Create(item, authorId);

                    item.PostId = itemPostId;

                    if (_useCache)
                    {
                        _blogCache.InsertId(_blogUrl, item.ForeignKey);
                    }
                }

                _bw.ReportProgress(itemIndex / itemCount * 100, item);
                if (_bw.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
            }
            e.Result = items;
        }


        private void CreatePosts(IList<Item> items, DoWorkEventArgs e, int minWordCount)
        {
            Logger.LogProcess(string.Format("CreatePosts coming minWordCount:{0} for {1} items", minWordCount, items.Count));
            var itemIndex = 0;
            var itemCount = items.Count;
            
            var mainQueue = new Queue<Queue<Item>>();
            var subQueue = new Queue<Item>();
            Queue<Item> lastQueue = null;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                Logger.LogProcess(string.Format("Item in the loop '{0}' with {1} words", item.Title , item.WordCount));
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
                        Logger.LogProcess(string.Format("Item '{0}' is bigger than wordcount:{1}, adding to mainqueue a single itemed queue", item.Title, item.WordCount));
                        var q = new Queue<Item>();
                        q.Enqueue(item);
                        mainQueue.Enqueue(q);
                        lastQueue = q;
                        continue;
                    }

                    subQueue.Enqueue(item);
                    Logger.LogProcess(string.Format("Item '{0}' is smaller than wordcount:{1}, adding to mainqueue sub queue, size of {2} with total word count {3}", item.Title, item.WordCount, subQueue.Count, subQueue.Sum(it => it.WordCount)));
                    
                    if (subQueue.Sum(it => it.WordCount) >= minWordCount)
                    {
                        Logger.LogProcess(string.Format("Subqueue word count sum '{0}' is bigger than min wordcount:{1}, adding to mainqueue", subQueue.Sum(it => it.WordCount), minWordCount));
                        var q = CloneQueue(subQueue);

                        Logger.LogProcess(string.Format("Cloned the Subqueue, item count {1} and word count sum '{0}' adding to mainqueue", q.Sum(it => it.WordCount), q.Count));
                        
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
                    Logger.LogProcess(string.Format("Subqueue item '{0}' moved to lastQueue", subQueue.Peek().Title));
                    lastQueue.Enqueue(subQueue.Dequeue());
                }
            }
            else//nothing on main queue
            {
                Logger.LogProcess(string.Format("Subqueue queued to main queue for {0} items", subQueue.Count));
                mainQueue.Enqueue(subQueue);
            }

            if (mainQueue.Count > 0)
            {
                var authorId = _userIds[Helper.GetRandomNumber(0, _userIds.Count)];
                Logger.LogProcess(string.Format("Main queue has {0} items", mainQueue.Count));
                
                foreach (var qi in mainQueue)
                {
                    if (qi.Count == 0)
                    {
                        Logger.LogProcess("Main queue iterating, queue has 0 items");
                        continue;
                    }

                    _bw.ReportProgress(itemIndex / itemCount * 100, qi.First());

                    var itemsToMerge = qi.Aggregate("", (current, sqi) => current + (sqi.Title + ","));

                    Logger.LogProcess("Merging: "+ itemsToMerge);

                    var postId = CreateMerged(qi.ToList(), authorId);
                    var id = "";
                    foreach (var sqi in qi)
                    {
                        sqi.PostId = postId;
                        _bw.ReportProgress(itemIndex / itemCount * 100, sqi);
                        if (id != "") id = "," + id;
                        id += sqi.Site + "_" + sqi.Id;
                    }

                    if (_useCache)
                    {
                        _blogCache.InsertId(_blogUrl, id);
                    }

                }
            }
            e.Result = items;
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

            return Create(new MultiItem(items), authorId);
        }

        /// <summary>
        /// create item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="foreignKey"> </param>
        /// <param name="authorId"> </param>
        /// <returns>id crated, -1 if error</returns>
        private int Create(Item item, int authorId)
        {
            var postDal = new PostDal(_dal);
            WordPressClient client = null;

            if (!_useMySqlFtpWay)
            {
                client = new WordPressClient(_siteConfig);
            }

            try
            {
                var postTitle = GetPostTitle(item);
                _blogCache.InsertTitle(_blogUrl, postTitle);

                var imageUploads = GetImageUploads(item, postTitle, authorId, client);
                var yoastFocusKey = StopwordTool.RemoveStopwords(postTitle, true);
                var post = new Post
                {
                    PostType = "post",
                    Title = postTitle,
                    Content = item.PostBody(_thumbnailSize),
                    PublishDateTime = DateTime.Now,
                    Author = authorId.ToString(),
                    CommentStatus = "open",
                    Status = "draft",
                    BlogUrl = _blogUrl,
                    CustomFields = new[]
                    {
                        new CustomField() {Key = "foreignkey", Value = item.ForeignKey},
                        new CustomField() {Key = "_aioseop_title", Value = item.Title},
                        new CustomField(){Key = "_aioseop_description",Value = item.MetaDescription},
                        new CustomField(){Key = "_aioseop_keywords",Value = string.Join(",", item.Tags)},
                        new CustomField(){Key = "_yoast_wpseo_focuskw_text_input",Value = yoastFocusKey},
                        new CustomField(){Key = "_yoast_wpseo_focuskw",Value = yoastFocusKey},
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

                post.Terms = GetTags(item, client).ToArray();
                string newPost = "-1";
                newPost = _useMySqlFtpWay ? postDal.InsertPost(post).ToString() : client.NewPost(post);
                return Convert.ToInt32(newPost);
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
            var imageDal = new ImageDal(_dal);
            var ftp = new Ftp();
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
                var imageData = GetImageData(extension, imageUrl, out imageWidth, out imageHeight);
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
                imageData.Item1.Name = imageStart + extension;
                imageData.Item2.Name = imageStart + "-" + _thumbnailSize + "x" + _thumbnailSize + extension;


                UploadResult uploaded = null;
                var thumbnailUrl = String.Empty;
                if (_useMySqlFtpWay)
                {
                    ftp.UploadFileFtp(imageData.Item1, _ftpConfiguration.Url + "/" + _ftpDir,
                        _ftpConfiguration.UserName, _ftpConfiguration.Password);
                    ftp.UploadFileFtp(imageData.Item2, _ftpConfiguration.Url + "/" + _ftpDir,
                        _ftpConfiguration.UserName, _ftpConfiguration.Password);
                    uploaded = new UploadResult()
                    {
                        Url = _blogUrl + "/" + _ftpDir + "/" + imageData.Item1.Name,
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

                thumbnailUrl = string.Format("{0}/{1}/{2}-{3}-{4}x{4}{5}", _blogUrl, _ftpDir, imageName,
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
            return imageUploads;
        }

        private List<Term> GetTags(Item item, WordPressClient client)
        {
            var converterFunctions = new ConverterFunctions();
            var terms = new List<Term>();
            var tagDal = new TagDal(_dal);
            foreach (var tag in item.Tags)
            {
                if (_useCache)
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
            return terms;
        }

        private string GetPostTitle(Item item)
        {
            var converterFunctions = new ConverterFunctions();
            var postTitle = converterFunctions.FirstNWords(item.Title, 65, true);
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
            invalidChars.Add('\'');
            foreach (var invalidChar in invalidChars)
            {
                result = result.Replace(invalidChar.ToString(), "");
                result = result.Replace("&#" + ((int)invalidChar) + ";", "");
            }
            result = result.Replace("&quot;", "");
            var rgx = new Regex("[^a-zA-Z0-9 ]");
            result = rgx.Replace(result, " ").Trim();
            result = result.Replace("     ", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");
            return result;
        }
    }
}