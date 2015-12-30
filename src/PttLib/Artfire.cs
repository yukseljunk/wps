using System;
using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace PttLib
{
    public class Artfire : Site
    {
        public override string UrlKeywordFormat
        {
            get { return "https://www.artfire.com/ext/discover/?search_term={0}&sort_by=newest&json=yup"; }
        }

        public override string Name
        {
            get
            {
                return "Artfire";
            }
        }

        protected override void GetItemCount(out int totalItemCount, HtmlDocument htmlDoc)
        {
            totalItemCount = 0;
            var totalFoundNode = htmlDoc.DocumentNode.SelectSingleNode("//input[@name='total_found']");
            if (totalFoundNode != null)
            {
                totalItemCount = int.Parse(totalFoundNode.Attributes["value"].Value);
            }
        }

        public override Item GetItem(string title, string url, string extraInfo)
        {
            var item=base.GetItem(title, url, extraInfo);
            if (item.Tags.Contains("All Products"))
            {
                item.Tags.Remove("All Products");
            }
            return item;
        }

        public override string ItemsXPath
        {
            get { return "//div[@class='product']/div/a[1]"; }
        }

        protected override string GetTitleFromSingleItem(HtmlNode itemNode)
        {
            return itemNode.SelectSingleNode("img").Attributes["title"].Value;
        }

        public override void GetPageCount(out int pageCount, HtmlDocument htmlDoc)
        {
            int totalItemCount;
            GetItemCount(out totalItemCount, htmlDoc);

            pageCount = totalItemCount / 24 + 1;
        }

        protected override double GetPriceValue(HtmlNode metaPrice)
        {
            return double.Parse(metaPrice.InnerText.Replace("$","").Trim(),NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture);
        }

        public override string TagsXPath
        {
            get
            {
                return "//ul[contains(concat(' ', normalize-space(@class), ' '), ' product-tags ')]/li";
            }
        }
        public override string ImagesXPath
        {
            get
            {
                return "//ul[contains(concat(' ', normalize-space(@class), ' '), ' slides ')]/li/img";
            }
        }
        public override string ImagesAttribute
        {
            get
            {
                return "data-x-large";
            }
        }

        public override string DateTimeXPath
        {
            get { return "//img[@class='img-thumbnail']"; }
        }

        public override string DateTimeRegex
        {
            get { return @"/products/(\d{4}/\d{2}/\d{2})/"; }
        }

        private string DateTimeAltXPath
        {
            get { return "//meta[@name='twitter:image:src']"; }
        }
        protected override void SetCreatedDate(HtmlDocument htmlDoc, Item item)
        {
            var dateTimeNode = htmlDoc.DocumentNode.SelectSingleNode(DateTimeXPath);
            if (dateTimeNode == null)
            {
                dateTimeNode = htmlDoc.DocumentNode.SelectSingleNode(DateTimeAltXPath);
            }
            if (dateTimeNode != null)
            {
                var regex = new Regex(DateTimeRegex);
                var match = regex.Match(dateTimeNode.OuterHtml);
                if (match.Success)
                {
                    item.Created = DateTime.Parse(match.Groups[1].Value);
                }
            }
        }

        public override string PriceXPath
        {
            get
            {
                return "//div[@itemprop='price']";
            }
        }
        public override string DescriptionXPath
        {
            get
            {
                return "//div[@itemprop='description']";
            }
        }
        public override string IdRegex
        {
            get
            {
                return @"/product_view/.*/(\d+)/";
            }
        }
    }
}