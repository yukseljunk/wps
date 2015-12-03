using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using PttLib;
using PttLib.Helpers;
using PttLib.TourInfo;
using WordPressSharp;
using WordPressSharp.Models;
using WordpressScraper.Dal;

namespace WindowsFormsApplication1
{
    public class EtsyFactory
    {
        private readonly WordPressSiteConfig _siteConfig;
        private readonly FtpConfig _ftpConfiguration;
        private readonly BlogCache _blogCache;
        private readonly Dal _dal;
        private readonly bool _useMySqlFtpWay;
        private IList<int> _userIds;
        private string _ftpDir;
 
        public EtsyFactory(WordPressSiteConfig siteConfig, FtpConfig ftpConfiguration, BlogCache blogCache, Dal dal, bool useMySqlFtpWay = true)
        {
            _siteConfig = siteConfig;
            _ftpConfiguration = ftpConfiguration;
            _blogCache = blogCache;
            _dal = dal;
            _useMySqlFtpWay = useMySqlFtpWay;
            var userDal = new UserDal(_dal);
            _userIds = userDal.UserIds();

            if (useMySqlFtpWay)
            {
                var ftp = new Ftp();
                _ftpDir = DateTime.Now.Year + "/" + DateTime.Now.Month;
                ftp.MakeFtpDir(_ftpConfiguration.Url, _ftpDir, _ftpConfiguration.UserName, _ftpConfiguration.Password);
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
            var imageDal= new ImageDal(_dal);
            var converterFunctions = new ConverterFunctions();
            WordPressClient client = null;

            if (!_useMySqlFtpWay)
            {
                client = new WordPressClient(_siteConfig);
            }

            try
            {
                var id = "etsy_" + item.Id;
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

                var content = new StringBuilder("<div style=\"width: 300px; margin-right: 10px;\">");

                var imageIndex = 1;
                IList<UploadResult> imageUploads = new List<UploadResult>();

                foreach (var imageUrl in item.Images)
                {
                    var imageData = Data.CreateFromUrl(imageUrl);
                    imageData.Name = converterFunctions.SeoUrl(item.Title, 50) + "-" + imageIndex +
                                     Path.GetExtension(imageUrl);
                    imageIndex++;
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
                                       }; //TODO:should go to mysql to insert this as post

                        var imageId=imageDal.Insert(new ImagePost()
                        {
                            Url = uploaded.Url, 
                            Author = authorId.ToString(),
                            Alt= item.Title
                        });
                        uploaded.Id = imageId.ToString();
                        thumbnailUrl = uploaded.Url; //TODO:Will be thinking about this
                    }
                    else
                    {
                        uploaded = client.UploadFile(imageData);
                        thumbnailUrl =
                            Path.GetDirectoryName(uploaded.Url).Replace("http:\\", "http:\\\\").Replace("\\", "/") + "/" +
                            Path.GetFileNameWithoutExtension(uploaded.Url) + "-150x150" +
                            Path.GetExtension(uploaded.Url);
                    }

                    imageUploads.Add(uploaded);
                    content.Append(
                        string.Format(
                            "<div style=\"width: 70px; float: left; margin-right: 15px; margin-bottom: 3px;\"><a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" width=\"70px\" height=\"70px\" title=\"{2}\" /></a></div>",
                            uploaded.Url, thumbnailUrl, item.Title));
                }
                content.Append(string.Format("</div><h4>Price:${0}</h4>", item.Price));
                content.Append("<strong>Description: </strong>");
                content.Append(converterFunctions.ArrangeContent(item.Content));

                var post = new Post
                               {
                                   PostType = "post",
                                   Title = converterFunctions.FirstNWords(item.Title, 65, true),
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
    }
}