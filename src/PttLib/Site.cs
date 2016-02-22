using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using PttLib.Helpers;
using PttLib.TourInfo;

namespace PttLib
{
    public class Site
    {
        public virtual int BlockSize { get { return 5; } }

        protected ProgramOptions _options = null;

        public Site()
        {
            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();
        }

        public virtual string Name { get; private set; }
        public virtual string PageNoQsParameter
        {
            get { return "page"; }
        }

        public virtual string UrlKeywordFormat
        {
            get
            {
                return string.Format("https://www.{0}.com/search?q={{0}}&order=date_desc", Name);
            }
        }

        public virtual string Get(string url, int page = 1)
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

            if (_options.UseProxy)
            {
                return WebHelper.CurlSimple(urlToAsk, "text/html", new WebProxy(_options.ProxyAddress + ":" + _options.ProxyPort));
            }
            return WebHelper.CurlSimple(urlToAsk);

        }

        public virtual string ItemsXPath
        {
            get { return "//div[@class='product-listview-wrapper']//article/a[2]"; }
        }

        public string UrlFromKey(string key)
        {
            return string.Format(UrlKeywordFormat, HttpUtility.UrlEncode(key));
        }

        private int _lastPageCount = 0;
        /// <summary>
        /// gets title and link values for keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageCount"></param>
        /// <param name="totalItemCount"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public virtual IList<Tuple<string, string, string>> GetItems(string keyword, out int pageCount, out int totalItemCount, int page = 1)
        {
            var result = new List<Tuple<string, string, string>>();
            var url = UrlFromKey(keyword);
            pageCount = 0;
            totalItemCount = 0;
            var uri = new Uri(url);
            var mainUrl = uri.Scheme + "://" + uri.Host;

            var htmlDoc = GetHtmlDocument(page, url);

            if (htmlDoc == null)
            {
                return null;
            }
            var tryCount = 0;
            while (tryCount < 3)
            {
                if (htmlDoc.DocumentNode != null)
                {
                    GetPageCount(out pageCount, htmlDoc, keyword);
                    GetItemCount(out totalItemCount, htmlDoc, keyword);

                    if (pageCount == 0)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(3));
                        htmlDoc = GetHtmlDocument(page, url);
                        if (htmlDoc == null)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        if (Math.Abs(_lastPageCount - pageCount)>10 && _lastPageCount > 0)
                        {
                            Logger.LogProcess("Last page count difference!");

                        }
                        _lastPageCount = pageCount;
                        break;
                    }

                }
                else
                {
                    return result;
                }
                tryCount++;
            }

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
                    result.Add(new Tuple<string, string, string>(GetTitleFromSingleItem(itemNode), link, GetExtraInfo(itemNode)));
                }
            }

            return result;
        }

        private HtmlDocument GetHtmlDocument(int page, string url)
        {
            var html = Get(url, page);
            if (html == null) return null;
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc;
        }

        protected virtual string GetTitleFromSingleItem(HtmlNode itemNode)
        {
            return itemNode.Attributes["title"].Value;
        }

        protected virtual void GetItemCount(out int totalItemCount, HtmlDocument htmlDoc, string keyword)
        {
            var contentNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='content']");
            totalItemCount = 0;

            if (contentNode == null) return;

            var regex = new Regex(@"\((.*) Results\)");
            var match = regex.Match(contentNode.InnerHtml);
            if (match.Success)
            {
                totalItemCount = Int32.Parse(match.Groups[1].Value, NumberStyles.AllowThousands, new CultureInfo("en-US"));
            }

        }

        protected virtual string GetExtraInfo(HtmlNode itemNode)
        {
            return null;
        }

        public virtual void GetPageCount(out int pageCount, HtmlDocument htmlDoc, string keyword)
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
        public virtual string AlternativeImagesAttribute
        {
            get { return "src"; }
        }

        public virtual string PriceXPath
        {
            get { return "//meta[@itemprop='price']"; }
        }

        public virtual string DescriptionXPath
        {
            get { return "//div[@id='description-text']"; }
        }
        public virtual string MetaDescriptionXPath
        {
            get { return "//meta[@name='description']"; }
        }
        public virtual string IdRegex
        {
            get { return @"/listing/(.*?)/"; }
        }

        public virtual string DateTimeXPath
        {
            get { return "//div[@id='fineprint']/ul/li[1]"; }
        }

        public virtual string DateTimeRegex
        {
            get { return @"Listed on (.*)"; }
        }

        public int GetId(string url)
        {
            var regex = new Regex(IdRegex);
            var match = regex.Match(url);
            if (match.Success)
            {
                return Int32.Parse(match.Groups[1].Value);
            }
            return 0;
        }


        public virtual Item GetItem(string title, string url, string extraInfo)
        {
            var item = new Item()
                           {
                               Tags = new List<string>(),
                               ItemImages = new List<ItemImage>(),
                               Url = url//,
                               //Keyword = keyword
                           };

            string itemHtml = "";
            var maxTry = 3;
            var tryCount = 0;
            while (tryCount < maxTry)
            {
                if (_options.UseProxy)
                {
                    itemHtml = WebHelper.CurlSimple(url, "text/html",
                        new WebProxy(_options.ProxyAddress + ":" + _options.ProxyPort));
                }
                else
                {
                    itemHtml = WebHelper.CurlSimple(url);
                }
                if (!string.IsNullOrEmpty(itemHtml))
                {
                    break;
                }
                Thread.Sleep(TimeSpan.FromSeconds(3));
                tryCount++;
            }

            if (itemHtml == null) return null;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(itemHtml);

            var metaDescription = htmlDoc.DocumentNode.SelectSingleNode(MetaDescriptionXPath);
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
                    var imageUrl = image.Attributes[ImagesAttribute].Value;
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        imageUrl = image.Attributes[AlternativeImagesAttribute].Value;
                        if (string.IsNullOrEmpty(imageUrl))
                        {
                            continue;
                        }
                    }
                    if (!imageUrl.StartsWith("http://") && !imageUrl.StartsWith("https://"))
                    {
                        imageUrl = "http:" + (imageUrl.StartsWith("//") ? "" : "//") + imageUrl;
                    }
                    item.ItemImages.Add(new ItemImage() { OriginalSource = imageUrl, Primary = true });
                }
            }

            var metaPrice = htmlDoc.DocumentNode.SelectSingleNode(PriceXPath);
            if (metaPrice != null)
            {
                item.Price = GetPriceValue(metaPrice);
            }

            var content = htmlDoc.DocumentNode.SelectSingleNode(DescriptionXPath);
            if (content != null)
            {
                item.Content = Regex.Replace(content.InnerHtml, "<a.*>.*</a>", "");
                item.Content = RefineContent(item.Content);
                var converterFunctions = new ConverterFunctions();
                item.WordCount = converterFunctions.StripTags(content.InnerHtml, new List<string>()).WordCount();

            }
            item.Title = title;
            var regex = new Regex(IdRegex);
            var match = regex.Match(url);
            if (match.Success)
            {
                item.Id = Int32.Parse(match.Groups[1].Value);
            }
            item.Site = Name;
            SetCreatedDate(htmlDoc, item);
            return item;
        }

        protected virtual string RefineContent(string content)
        {
            return content;
        }

        protected virtual void SetCreatedDate(HtmlDocument htmlDoc, Item item)
        {
            var dateTimeNode = htmlDoc.DocumentNode.SelectSingleNode(DateTimeXPath);
            if (dateTimeNode != null)
            {
                var regex = new Regex(DateTimeRegex);
                var match = regex.Match(dateTimeNode.InnerText);
                if (match.Success)
                {
                    item.Created = DateTime.Parse(match.Groups[1].Value);
                }
            }
        }

        protected virtual double GetPriceValue(HtmlNode metaPrice)
        {
            return Double.Parse(metaPrice.Attributes["content"].Value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture);
        }
    }
}