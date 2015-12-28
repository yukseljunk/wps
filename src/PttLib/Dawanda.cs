using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;

namespace PttLib
{
    public class Dawanda : Site
    {
        public override string UrlKeywordFormat
        {
            get { return "http://en.dawanda.com/search?q={0}"; }
        }

        public override string Name
        {
            get
            {
                return "Dawanda";
            }
        }

        protected override string GetExtraInfo(HtmlNode itemNode)
        {
            if (itemNode == null) return null;
            if (itemNode.PreviousSibling == null) return null;
            if (itemNode.PreviousSibling.PreviousSibling == null) return null;
            if (itemNode.PreviousSibling.PreviousSibling.SelectSingleNode("img") == null) return null;

            return itemNode.PreviousSibling.PreviousSibling.SelectSingleNode("img").Attributes["src"].Value;
        }

        protected override void GetItemCount(out int totalItemCount, HtmlDocument htmlDoc)
        {
            var contentNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='category_tree']/ul/li[1]/a");
            totalItemCount = 0;

            if (contentNode == null) return;

            totalItemCount = Int32.Parse(contentNode.Attributes["data-count"].Value, NumberStyles.AllowThousands, new CultureInfo("en-US"));

        }

        /// <summary>
        /// gets title and link values for keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageCount"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public override IList<Tuple<string, string, string>> GetItems(string keyword, out int pageCount, out int totalItemCount, int page = 1)
        {
            Thread.Sleep(1000);
            return base.GetItems(keyword, out pageCount, out totalItemCount, page);
        }

        public override Item GetItem(string title, string url, string extraInfo)
        {
            Thread.Sleep(1000);
            var item = base.GetItem(title, url, extraInfo);
            if (item == null) return null;

            var regex = new Regex(@"\?(\d{8})");
            var match = regex.Match(extraInfo);
            if (match.Success)
            {
                DateTime dt;
                if (DateTime.TryParseExact(match.Groups[1].Value, "yyyyMMdd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dt))
                {
                    item.Created = dt;
                }
            }
            return item;
        }

        public override void GetPageCount(out int pageCount, HtmlDocument htmlDoc)
        {
            pageCount = 0;
            var pageNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='pagination-container']/div/a");
            if (pageNodes != null)
            {
                pageCount = int.Parse(pageNodes[pageNodes.Count - 2].InnerText);
            }
        }

        public override string TagsXPath
        {
            get
            {
                return "//aside[h4[text()='Tags']]/span";
            }
        }
        public override string ImagesXPath
        {
            get
            {
                return "//section[@id='product_gallery']//li/img";
            }
        }
        public override string ImagesAttribute
        {
            get
            {
                return "data-big";
            }
        }
        public override string PriceXPath
        {
            get
            {
                return "//meta[@property='og:price:amount']";
            }
        }
        public override string DescriptionXPath
        {
            get
            {
                return "//section[@class='description']";
            }
        }
        public override string IdRegex
        {
            get
            {
                return @"/product/(.*?)-";
            }
        }
    }
}