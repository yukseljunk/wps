using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
        private readonly BlogCache _blogCache;
        private readonly Dal _dal;
        private IList<int> _userIds; 

        public EtsyFactory(WordPressSiteConfig siteConfig, BlogCache blogCache, Dal dal)
        {
            _siteConfig = siteConfig;
            _blogCache = blogCache;
            _dal = dal;
            var userDal = new UserDal(_dal);
            _userIds = userDal.UserIds();

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
            var converterFunctions = new ConverterFunctions();
            using (var client = new WordPressClient(_siteConfig))
            {
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
                    
                    //validation
                    if (item.Images.Count==0||string.IsNullOrWhiteSpace(item.Title.Trim()) ||string.IsNullOrWhiteSpace(item.Content.Trim()))
                    {
                        return -2;
                    }

                    var content = new StringBuilder("<div style=\"width: 300px; margin-right: 10px;\">");
                    //new NetworkCredential("bloggon@nalgorithm.com", "U4E9TrT;5!)F")

                    
                    var imageIndex = 1;
                    IList<UploadResult> imageUploads = new List<UploadResult>();
                    
                    foreach (var imageUrl in item.Images)
                    {
                        //var imageData = Data.CreateFromUrl(imageUrl);
                        //imageData.Name = converterFunctions.SeoUrl(item.Title, 50) + "-" + imageIndex + Path.GetExtension(imageUrl);
                        //imageIndex++;

                        //UploadFileFtp(imageData, "ftp://ftp.nalgorithm.com", "bloggon@nalgorithm.com", "U4E9TrT;5!)F");
                   
                        /*
                        var uploaded = client.UploadFile(imageData);
                        imageUploads.Add(uploaded);
                        var thumbnailUrl =
                            Path.GetDirectoryName(uploaded.Url).Replace("http:\\", "http:\\\\").Replace("\\", "/") + "/" +
                            Path.GetFileNameWithoutExtension(uploaded.Url) + "-150x150" +
                            Path.GetExtension(uploaded.Url);

                        content.Append(
                            string.Format(
                                "<div style=\"width: 70px; float: left; margin-right: 15px; margin-bottom: 3px;\"><a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" width=\"70px\" height=\"70px\" title=\"{2}\" /></a></div>",
                                uploaded.Url, thumbnailUrl, item.Title));
                         */
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
                            new CustomField() {Key = "_aioseop_title", Value = item.Title},
                            new CustomField() {Key = "_aioseop_description", Value = item.MetaDescription},
                            new CustomField() {Key = "_aioseop_keywords", Value = string.Join(",", item.Tags)},
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
                                        HttpUtility.HtmlDecode(converterFunctions.RemoveDiacritics(tag)).Trim().ToLowerInvariant());
                            if (tagOnBlog == null)
                            {
                                var t = new Term
                                {
                                    Name = converterFunctions.RemoveDiacritics(tag),
                                    Description = tag,
                                    Slug = tag.Replace(" ", "_"),
                                    Taxonomy = "post_tag"
                                };

                                var tId = tagDal.InsertTag(t);
                                t.Id = tId.ToString();

                                //var termId = client.NewTerm(t);
                                //t.Id = termId;
                                
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
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    var newPost = postDal.InsertPost(post);
                    stopWatch.Stop();
                    var mysqlTime = stopWatch.ElapsedMilliseconds;
                    //var newPost = client.NewPost(post);
                    stopWatch.Reset();
                    stopWatch.Start();
                    post.Terms = null;
                    var newPost2 = client.NewPost(post);
                    stopWatch.Stop();
                    var wordpressSharpTime = stopWatch.ElapsedMilliseconds;
                    if (mysqlTime > wordpressSharpTime)
                    {
                        //no need to use mysql???
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
                    Logger.LogProcess("Author:"+authorId);
                    Logger.LogExceptions(exception);
                    return -1;

                }

            }
            return -1;

        }

        public void UploadFileFtp(Data file, string ftpAddress, string username, string password)
        {
            var request = (FtpWebRequest)WebRequest.Create(ftpAddress + "/" + Path.GetFileName(file.Name));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(file.Bits, 0, file.Bits.Length);
                reqStream.Close();
            }

            request.Abort();


        }

        public void UploadFileFtp(string filePath, string ftpAddress, string username, string password)
        {
            var request = (FtpWebRequest) WebRequest.Create(ftpAddress+ "/" + Path.GetFileName(filePath));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;
            byte[] buffer;
            using(FileStream stream = File.OpenRead(filePath))
            {
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();   
            }
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();
            }

            request.Abort();
            

        }

    }
}