using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PttLib.Helpers;

namespace PttLib
{
    public class Site
    {
        public virtual string PageNoQsParameter
        {
            get { return "page"; }
        }

        public string Get(string url, int page = 1)
        {
            var urlToAsk = url.Replace(" ", "+");
            if (urlToAsk.Contains("?"))
            {
                urlToAsk += "&";
            }
            else
            {
                urlToAsk += "?";
            }
            urlToAsk += PageNoQsParameter + "=" + page;
            return WebHelper.CurlSimple(urlToAsk);

        }

        public virtual string ItemsXPath
        {
            get { return "//div[@class='product-listview-wrapper']//article/a[2]"; }
        }

        public IList<Tuple<string, string>> GetItems(string url, out int pageCount, int page = 1)
        {
            var result = new List<Tuple<string, string>>();
            var html = Get(url, page);
            var uri = new Uri(url);
            var mainUrl = uri.Scheme + "://" + uri.Host;
            pageCount = 0;
            if (html == null) return null;
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(html);

            if (htmlDoc.DocumentNode != null)
            {
                GetPageCount(out pageCount, htmlDoc);

                var itemNodes = htmlDoc.DocumentNode.SelectNodes(ItemsXPath);

                if (itemNodes != null)
                {
                    foreach (var itemNode in itemNodes)
                    {
                        var link = itemNode.Attributes["href"].Value;
                        if (!link.StartsWith("http"))
                        {
                            if (!link.StartsWith("/"))
                            {
                                link = "/" + link;
                            }
                            link = mainUrl + link;
                        }
                        result.Add(new Tuple<string, string>(itemNode.Attributes["title"].Value, link));
                    }
                }
            }
            return result;
        }

        public virtual void GetPageCount(out int pageCount, HtmlDocument htmlDoc)
        {
            pageCount = 0;
            var pageNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='pagination btn-group clearfix mt-xs-3']/a");
            if (pageNodes != null)
            {
                pageCount = int.Parse(pageNodes[pageNodes.Count - 2].InnerText);
            }
        }

        public virtual string TagsXPath
        {
            get { return "//ul[@id='listing-tag-list']/li/a"; }
        }

        public virtual string ImagesXPath
        {
            get { return "//div[@id='image-main']//li"; }
        }

        public virtual string ImagesAttribute
        {
            get { return "data-full-image-href"; }
        }

        public virtual string PriceXPath
        {
            get { return "//meta[@itemprop='price']"; }
        }

        public virtual string DescriptionXPath
        {
            get { return "//div[@id='description-text']"; }
        }

        public virtual string IdRegex
        {
            get { return @"/listing/(.*?)/"; }
        }


        
        public virtual Item GetItem(string title, string url)
        {
            var item = new Item()
                           {
                               Tags = new List<string>(),
                               Images = new List<string>(),
                               Url = url
                           };

            var itemHtml = WebHelper.CurlSimple(url);
            if (itemHtml == null) return null;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(itemHtml);

            var metaDescription = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='description']");
            if (metaDescription != null)
            {
                item.MetaDescription = metaDescription.Attributes["content"].Value;
            }

            var tags = htmlDoc.DocumentNode.SelectNodes(TagsXPath);
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    item.Tags.Add(tag.InnerText);
                }
            }

            var images = htmlDoc.DocumentNode.SelectNodes(ImagesXPath);
            if (images != null)
            {
                foreach (var image in images)
                {
                    item.Images.Add(image.Attributes[ImagesAttribute].Value);
                }
            }

            var metaPrice = htmlDoc.DocumentNode.SelectSingleNode(PriceXPath);
            if (metaPrice != null)
            {
                item.Price = Double.Parse(metaPrice.Attributes["content"].Value, CultureInfo.InvariantCulture);
            }

            var content = htmlDoc.DocumentNode.SelectSingleNode(DescriptionXPath);
            if (content != null)
            {
                item.Content = Regex.Replace(content.InnerHtml, "<a.*>.*</a>", "");
            }
            item.Title = title;

            var regex = new Regex(IdRegex);
            var match = regex.Match(url);
            if (match.Success)
            {
                item.Id = Int32.Parse(match.Groups[1].Value);
            }

            return item;
        }


    }
}