using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PttLib.Helpers;

namespace PttLib
{
    public class Etsy
    {
        public string Get(string url, int page = 1)
        {
            return WebHelper.CurlSimple(url.Replace(" ", "+") + "&page=" + page);

        }

        public IList<Tuple<string, string>> GetItems(string url, out int pageCount, int page = 1)
        {
            var result = new List<Tuple<string, string>>();
            var html = Get(url, page);
            pageCount = 0;
            if (html == null) return null;
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(html);

            if (htmlDoc.DocumentNode != null)
            {
                var pageNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='pagination btn-group clearfix mt-xs-3']/a");
                if (pageNodes != null)
                {
                    pageCount =int.Parse(pageNodes[pageNodes.Count-2].InnerText);
                }

                var itemNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='buyer-card card']/a");

                if (itemNodes != null)
                {
                    foreach (var itemNode in itemNodes)
                    {
                        result.Add(new Tuple<string, string>(itemNode.Attributes["title"].Value, itemNode.Attributes["href"].Value));
                    }
                    // Do something with bodyNode
                }
            }


            return result;

        }

        public Item GetItem(string title, string url)
        {
            var item = new Item()
            {
                Tags = new List<string>(),
                Images = new List<string>(),
                Url = url
            };

            var itemHtml = WebHelper.CurlSimple(url);
            if (itemHtml == null) return null;

            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(itemHtml);
            var metaDescription = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='description']");
            if (metaDescription != null)
            {
                item.MetaDescription = metaDescription.Attributes["content"].Value;
            }
            var tags = htmlDoc.DocumentNode.SelectNodes("//ul[@id='listing-tag-list']/li/a");
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    item.Tags.Add(tag.InnerText);
                }
            }
            var images = htmlDoc.DocumentNode.SelectNodes("//div[@id='image-main']//li");
            if (images != null)
            {
                foreach (var image in images)
                {
                    item.Images.Add(image.Attributes["data-full-image-href"].Value);
                }
            }
            var metaPrice = htmlDoc.DocumentNode.SelectSingleNode("//meta[@itemprop='price']");
            if (metaPrice != null)
            {
                item.Price = Double.Parse(metaPrice.Attributes["content"].Value);
            }
            var content = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='description-text']");
            if (content != null)
            {
                item.Content = Regex.Replace(content.InnerHtml, "<a.*>.*</a>", "");
            }
            item.Title = title;

            Regex regex = new Regex(@"/listing/(.*?)/");
            Match match = regex.Match(url);
            if (match.Success)
            {
                item.Id = Int32.Parse(match.Groups[1].Value);
            }

            return item;
        }

    }
}