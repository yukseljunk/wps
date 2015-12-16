using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
            bool useFeatureImage = false)
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
            var userDal = new UserDal(_dal);
            _userIds = userDal.UserIds();

            if (useMySqlFtpWay)
            {
                var ftp = new Ftp();
                _ftpDir = DateTime.Now.Year + "/" + DateTime.Now.Month;
                ftp.MakeFtpDir(_ftpConfiguration.Url, _ftpDir, _ftpConfiguration.UserName, _ftpConfiguration.Password);
            }
            if (maxImageDimension > 0)
            {
                Directory.CreateDirectory(ImagesDir);
            }

        }


        public void Create(IList<Item> items)
        {
            _bw = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _bw.DoWork += (obj, e) => CreatePosts(items, e, 200);
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

                var id = item.Site + "_" + item.Id;
                if (_useCache)
                {
                    if (_blogCache.IdsPresent(_blogUrl).Contains(id))
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
                    var itemPostId = Create(item, id, authorId);

                    item.PostId = itemPostId;

                    if (_useCache)
                    {
                        _blogCache.InsertId(_blogUrl, id);
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
                _bw.ReportProgress(itemIndex / itemCount * 100, item);
                Thread.Sleep(TimeSpan.FromSeconds(1));

                var itemPresent = false;

                var id = item.Site + "_" + item.Id;
                if (_useCache)
                {
                    if (_blogCache.IdsPresent(_blogUrl).Contains(id))
                    {
                        item.PostId = 0;
                        itemPresent = true;
                    }
                }
                if (item.IsInvalid)
                {
                    item.PostId = -2;
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

                _bw.ReportProgress(itemIndex / itemCount * 100, item);
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
            var authorId = _userIds[Helper.GetRandomNumber(0, _userIds.Count)];

             foreach (var qi in mainQueue)
             {
                 var postId = CreateMerged(qi.ToList(), authorId);
                 foreach (var sqi in qi)
                 {
                   //  Console.Write(sqi + ",");
                 }
                 //Console.WriteLine("");

                 /* var itemPostId = Create(item, id, authorId);

                  item.PostId = itemPostId;

                  if (_useCache)
                  {
                      _blogCache.InsertId(_blogUrl, id);
                  }*/
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
            if(items==null || !items.Any())
            {
                return -1;
            }
            if(items.Count==1)
            {
                return Create(items[0], items[0].Site + "_" + items[0].Id, authorId);
            }

            var idCombined = "";// item.Site + "_" + item.Id;
                
            var combinedItem = new Item()
                                   {
                                       Title = items[0].Title,
                                       Price = 0
                                   };
            var contentCombined = "";
            var metaDescCombined = "";
            var combinedImages = new List<string>();
            var combinedTags = new List<string>();
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (idCombined != "") idCombined += ",";
                idCombined += item.Site + "_" + item.Id;
                if (i > 0)
                {
                    contentCombined += "<h2>" + item.Title + "</h2><br/>";
                }               
                contentCombined += item.Content;
                metaDescCombined += item.MetaDescription + " ";
                combinedImages.AddRange(item.Images);
                combinedTags.AddRange(item.Tags);
            }
            combinedItem.MetaDescription = metaDescCombined;
            combinedItem.Content = contentCombined;
            combinedItem.Images = combinedImages;
            combinedItem.Tags = combinedTags;

            combinedItem.Site = "";
            combinedItem.Url = "";

            return Create(combinedItem, idCombined, authorId);
        }

        /// <summary>
        /// create item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="foreignKey"> </param>
        /// <param name="authorId"> </param>
        /// <returns>id crated, -1 if error</returns>
        private int Create(Item item, string foreignKey, int authorId)
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

                var content = new StringBuilder();
                var imageUploads = GetImageUploads(item, postTitle, authorId, client, content);

                var post = new Post
                {
                    PostType = "post",
                    Title = postTitle,
                    Content = string.Format(item.PostBody(), content),
                    PublishDateTime = DateTime.Now,
                    Author = authorId.ToString(),
                    CommentStatus = "open",
                    Status = "draft",
                    BlogUrl = _blogUrl,
                    CustomFields = new[]
                    {
                        new CustomField() {Key = "foreignkey", Value = foreignKey},
                        new CustomField() {Key = "_aioseop_title", Value = item.Title}
                        ,
                        new CustomField()
                            {
                                Key = "_aioseop_description",
                                Value = item.MetaDescription
                            },
                        new CustomField()
                            {
                                Key = "_aioseop_keywords",
                                Value = string.Join(",", item.Tags)
                            },
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
            WordPressClient client, StringBuilder content)
        {
            var converterFunctions = new ConverterFunctions();
            var imageDal = new ImageDal(_dal);
            var ftp = new Ftp();
            var imageIndex = 1;
            IList<UploadResult> imageUploads = new List<UploadResult>();
            var imagePosts = new List<ImagePost>();
            foreach (var imageUrl in item.Images)
            {
                var uri = new Uri(imageUrl);
                var imageUrlWithoutQs = uri.GetLeftPart(UriPartial.Path);

                var extension = Path.GetExtension(imageUrlWithoutQs).ToLower();

                var imageData = GetImageData(extension, imageUrl);
                var imageName = RefineImageName(postTitle);
                imageData.Name = imageName + "-" + imageIndex +
                                 extension;

                UploadResult uploaded = null;
                var thumbnailUrl = String.Empty;
                if (_useMySqlFtpWay)
                {
                    ftp.UploadFileFtp(imageData, _ftpConfiguration.Url + "/" + _ftpDir,
                        _ftpConfiguration.UserName, _ftpConfiguration.Password);
                    uploaded = new UploadResult()
                    {
                        Url = _blogUrl + "/wp-content/uploads/" + _ftpDir + "/" + imageData.Name,
                        Id = "1"
                    };
                    imagePosts.Add(new ImagePost()
                    {
                        Title = converterFunctions.SeoUrl(imageName + "-" + imageIndex),
                        Url = uploaded.Url,
                        Author = authorId.ToString(),
                        Alt = item.Title + imageIndex,
                        PublishDateTime = DateTime.Now,
                        Content = item.Title + imageIndex
                    });
                }
                else
                {
                    uploaded = client.UploadFile(imageData);
                }

                thumbnailUrl = string.Format("{0}/wp-content/uploads/{1}/{2}-{3}-{4}x{4}{5}", _blogUrl, _ftpDir, imageName,
                    imageIndex, _thumbnailSize, extension);
                imageUploads.Add(uploaded);
                content.Append(
                    string.Format(
                        "<div style=\"width: 150px; float: left; margin-right: 15px; margin-bottom: 3px;\"><a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" title=\"{2}\" /></a></div>",
                        _blogUrl + converterFunctions.SeoUrl(postTitle) + "/" +
                        converterFunctions.SeoUrl(imageName + "-" + imageIndex), thumbnailUrl, item.Title));

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

        private Data GetImageData(string extension, string imageUrl)
        {
            if (_maxImageDimension <= 0)
            {
                return Data.CreateFromUrl(imageUrl);
            }

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
                resize = img.Width > _maxImageDimension || img.Height > _maxImageDimension;
            }
            if (!resize) return imageData;

            var size = new Size(_maxImageDimension, _maxImageDimension);
            using (MemoryStream inStream = new MemoryStream(imageData.Bits))
            {
                using (var outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                        imageFactory.Load(inStream)
                            .Constrain(size)
                            .Save(tempImageFileName);
                    }
                }
            }
            imageData = Data.CreateFromFilePath(tempImageFileName, MimeTypeMap.GetMimeType(extension));
            return imageData;
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