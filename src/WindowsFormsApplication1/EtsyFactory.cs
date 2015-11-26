using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using PttLib;
using PttLib.TourInfo;
using WordPressSharp;
using WordPressSharp.Models;

namespace WindowsFormsApplication1
{
    public class EtsyFactory
    {
        private readonly WordPressSiteConfig _siteConfig;
        private readonly BlogCache _blogCache;

        public EtsyFactory(WordPressSiteConfig siteConfig, BlogCache blogCache)
        {
            _siteConfig = siteConfig;
            _blogCache = blogCache;
        }

        public int Create(Item item, string blogUrl, bool useCache = true, bool useFeatureImage = false)
        {
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

                    var content = new StringBuilder("<div style=\"width: 300px; margin-right: 10px;\">");
                    var imageIndex = 1;
                    IList<UploadResult> imageUploads = new List<UploadResult>();
                    foreach (var imageUrl in item.Images)
                    {
                        var imageData = Data.CreateFromUrl(imageUrl);
                        imageData.Name = converterFunctions.SeoUrl(item.Title, 50) + "-" + imageIndex + Path.GetExtension(imageUrl);
                        imageIndex++;
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
                        Author = "John Doe",
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

                                var termId = client.NewTerm(t);
                                t.Id = termId;
                                if (useCache)
                                {
                                    _blogCache.InsertTag(blogUrl, t);
                                }
                                terms.Add(t);
                            }
                            else
                            {
                                terms.Add(tagOnBlog);
                            }
                        }
                        else
                        {
                            //not cached solution
                        }
                    }
                    post.Terms = terms.ToArray();
                    var newPost = client.NewPost(post);


                    if (useCache)
                    {
                        _blogCache.InsertId(blogUrl, id);
                    }


                    return Convert.ToInt32(newPost);

                }
                catch (Exception exception)
                {
                    throw exception;
                }

            }
            return -1;

        }



    }
}