using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly WordPressSiteConfig _siteConfig;
        private readonly FtpConfig _ftpConfiguration;
        private readonly BlogCache _blogCache;
        private readonly Dal _dal;
        private readonly bool _useMySqlFtpWay;
        private readonly int _maxImageDimension;
        private IList<int> _userIds;
        private string _ftpDir;
        private const string ImagesDir = "temp";

        public PostFactory(WordPressSiteConfig siteConfig, FtpConfig ftpConfiguration, BlogCache blogCache, Dal dal, bool useMySqlFtpWay = true, int maxImageDimension = 0)
        {
            _siteConfig = siteConfig;
            _ftpConfiguration = ftpConfiguration;
            _blogCache = blogCache;
            _dal = dal;
            _useMySqlFtpWay = useMySqlFtpWay;
            _maxImageDimension = maxImageDimension;
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

        /// <summary>
        /// create item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="blogUrl"></param>
        /// <param name="useCache"></param>
        /// <param name="useFeatureImage"></param>
        /// <returns>id crated, 0 if exists, -1 if error, -2 if not valid</returns>
        public int Create(Item item, string blogUrl, bool useCache = true, bool useFeatureImage = false)
        {
            var authorId = _userIds[Helper.GetRandomNumber(0, _userIds.Count)];
            var postDal = new PostDal(_dal);
            var tagDal = new TagDal(_dal);
            var imageDal = new ImageDal(_dal);
            var converterFunctions = new ConverterFunctions();
            WordPressClient client = null;

            if (!_useMySqlFtpWay)
            {
                client = new WordPressClient(_siteConfig);
            }

            try
            {
                var id = item.Site + "_" + item.Id;
                if (useCache)
                {
                    if (_blogCache.IdsPresent(blogUrl).Contains(id))
                    {
                        return 0;
                    }
                }
                var ftp = new Ftp();

                //validation
                if (item.Images.Count == 0 || string.IsNullOrWhiteSpace(item.Title.Trim()) ||
                    string.IsNullOrWhiteSpace(item.Content.Trim()))
                {
                    return -2;
                }

                var postTitle = converterFunctions.FirstNWords(item.Title, 65, true);
                var initialPostTitle = postTitle;
                if (_blogCache.TitlesPresent(blogUrl).Contains(postTitle))
                {
                    var postIndex = 2;
                    while (true)
                    {
                        postTitle = initialPostTitle + "-" + postIndex;
                        if (!_blogCache.TitlesPresent(blogUrl).Contains(postTitle))
                        {
                            break;
                        }
                        postIndex++;
                    }
                }
                _blogCache.InsertTitle(blogUrl, postTitle);

                var content = new StringBuilder("<div style=\"width: 300px; margin-right: 10px;\">");

                var imageIndex = 1;
                IList<UploadResult> imageUploads = new List<UploadResult>();
                var imagePosts = new List<ImagePost>();
                foreach (var imageUrl in item.Images)
                {
                    Data imageData = null;
                    var extension = Path.GetExtension(imageUrl).ToLower();

                    if (_maxImageDimension > 0)
                    {
                        var tempImageFileName = ImagesDir + "/" + "temp" + extension;
                        //download image and resize it...
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.DownloadFile(imageUrl, tempImageFileName);
                        }

                        imageData = Data.CreateFromFilePath(tempImageFileName, MimeTypeMap.GetMimeType(extension));
                        var resize = false;
                        using (var img = Image.FromFile(tempImageFileName))
                        {
                            resize = img.Width > _maxImageDimension || img.Height > _maxImageDimension;
                        }
                        if (resize)
                        {

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
                                    // Do something with the stream.
                                }
                            }
                            imageData = Data.CreateFromFilePath(tempImageFileName, MimeTypeMap.GetMimeType(extension));
                        }
                    }
                    else
                    {
                        imageData = Data.CreateFromUrl(imageUrl);
                    }
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
                                           Url = blogUrl + "/wp-content/uploads/" + _ftpDir + "/" + imageData.Name,
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
                    //todo url su sekilde olmali, tabii bunu nasil cekecegini dusunmen gerekli: http://blog.guessornot.com/wooden-baby-spoon/wooden-baby-spoon-1
                    //gereken seyler: 1. permalink formati
                    thumbnailUrl =
                        blogUrl + "/wp-content/uploads/" + _ftpDir + "/" + imageName + "-" + imageIndex + "-150x150" + extension;
                           
                    imageUploads.Add(uploaded);
                    content.Append(
                        string.Format(
                            "<div style=\"width: 150px; float: left; margin-right: 15px; margin-bottom: 3px;\"><a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" title=\"{2}\" /></a></div>",
                            blogUrl + converterFunctions.SeoUrl(postTitle) + "/" + converterFunctions.SeoUrl(imageName + "-" + imageIndex), thumbnailUrl, item.Title));

                    imageIndex++;
                }
                if (_useMySqlFtpWay)
                {
                    var imageIds = imageDal.Insert(imagePosts, _ftpDir);
                    for (int i = 0; i < imageUploads.Count; i++)
                    {
                        imageUploads[i].Id = imageIds[i].ToString();
                    }
                }

                content.Append(string.Format("</div><h4>Price:${0}</h4>", item.Price));
                content.Append("<strong>Description: </strong>");
                content.Append(converterFunctions.ArrangeContent(item.Content));
                content.Append("<br><strong>Source:</strong> <a href=\"");
                content.Append(item.Url);
                content.Append("\" rel=\"nofollow\" target=\"_blank\">");
                content.Append(item.Site);
                content.Append(".com</a>");

                var post = new Post
                {
                    PostType = "post",
                    Title = postTitle,
                    Content = content.ToString(),
                    PublishDateTime = DateTime.Now,
                    Author = authorId.ToString(),
                    CommentStatus = "open",
                    Status = "draft",
                    BlogUrl = blogUrl,
                    CustomFields = new[]
                    {
                        new CustomField() {Key = "foreignkey", Value = id},
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
                    if (useFeatureImage)
                    {
                        post.FeaturedImageId = imageUploads[0].Id;
                    }
                    post.ImageIds = imageUploads.Select(i => i.Id).ToList();
                    post.CustomFields[4].Value = imageUploads[0].Id;
                }

                var terms = new List<Term>();
                foreach (var tag in item.Tags)
                {
                    if (useCache)
                    {
                        var tagOnBlog =
                            _blogCache.TagsPresent(blogUrl).FirstOrDefault(
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

                            if (useCache)
                            {
                                _blogCache.InsertTag(blogUrl, t);
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


                post.Terms = terms.ToArray();
                string newPost = "-1";
                var stopWatch = new Stopwatch();
                if (_useMySqlFtpWay)
                {
                    stopWatch.Start();
                    newPost = postDal.InsertPost(post).ToString();
                    stopWatch.Stop();
                    var mysqlTime = stopWatch.ElapsedMilliseconds;
                    Logger.LogProcess("mysqltime: " + mysqlTime);
                }
                else
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                    newPost = client.NewPost(post);
                    stopWatch.Stop();
                    var wordpressSharpTime = stopWatch.ElapsedMilliseconds;
                    Logger.LogProcess("wordpressSharpTime: " + wordpressSharpTime);
                }
                if (useCache)
                {
                    _blogCache.InsertId(blogUrl, id);
                }

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
            var rgx = new Regex("[^a-zA-Z0-9 ]");
            result= rgx.Replace(result, "").Trim();
            result = result.Replace("     ", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");
            return result;
        }
    }
}