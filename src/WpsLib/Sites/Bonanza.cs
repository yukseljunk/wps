using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using WpsLib.Item;
using WpsLib.Sites;

namespace PttLib
{
    class Bonanza : Site
    {
        public override int BlockSize { get { return 5; } }

        private Dictionary<string, int> _totalPageCounts = new Dictionary<string, int>();
        private Dictionary<string, int> _totalItemCounts = new Dictionary<string, int>();

        public override string UrlKeywordFormat
        {
            get { return "http://www.bonanza.com/items/search_page?q[filter_category_id]=&q[shipping_in_price]=0&q[sort_by]=newness&q[translate_term]=true&q[search_term]={0}"; }
        }

        public override string Name
        {
            get
            {
                return "Bonanza";
            }
        }
        public override string PageNoQsParameter
        {
            get { return "q[page]"; }
        }

        public override void GetPageCount(out int pageCount, HtmlDocument htmlDoc, string keyword)
        {
            pageCount = 0;
            if (_totalPageCounts.ContainsKey(keyword))
            {
                pageCount = _totalPageCounts[keyword];
                return;
            }

            var mainUrl = string.Format("http://www.bonanza.com/items/search?q[filter_category_id]=&q[shipping_in_price]=0&q[sort_by]=newness&q[translate_term]=true&q[search_term]={0}", keyword);
            var mainPage = Get(mainUrl);
            if (mainPage == null) return;
            var htmlDoc2 = new HtmlDocument();
            htmlDoc2.LoadHtml(mainPage);

            var pageNode = htmlDoc2.DocumentNode.SelectSingleNode("//div[@id='search_pages_container']");
            if (pageNode != null)
            {
                pageCount = int.Parse(pageNode.Attributes["data-pageCount"].Value);
            }
            if (!_totalPageCounts.ContainsKey(keyword))
            {
                _totalPageCounts.Add(keyword, pageCount);
            }
            else
            {
                _totalPageCounts[keyword] = pageCount;
            }
            var itemCount = 0;
            var itemCountNode = htmlDoc2.DocumentNode.SelectSingleNode("//span[@id='listing_count_number']");
            if (itemCountNode != null)
            {
                itemCount = int.Parse(itemCountNode.InnerText.Trim().Replace(",", "").Replace(".", ""));
            }
            if (!_totalItemCounts.ContainsKey(keyword))
            {
                _totalItemCounts.Add(keyword, itemCount);
            }
            else
            {
                _totalItemCounts[keyword] = itemCount;
            }

        }

        protected override void GetItemCount(out int totalItemCount, HtmlDocument htmlDoc, string keyword)
        {

            totalItemCount = 0;
            if (_totalItemCounts.ContainsKey(keyword))
            {
                totalItemCount = _totalItemCounts[keyword];

            }

        }
        public override string ItemsXPath
        {
            get { return "//ul[contains(concat(' ', normalize-space(@class), ' '), ' browsable_item ')]/li[contains(concat(' ', normalize-space(@class), ' '), ' item_title ')]/a"; }
        }
        protected override string GetTitleFromSingleItem(HtmlNode itemNode)
        {
            return itemNode.InnerText;
        }

        public override string ImagesXPath
        {
            get { return "//div[@class='flag_image_container']/a"; }
        }
        public override string ImagesAttribute
        {
            get { return "href"; }
        }
        public override string PriceXPath
        {
            get { return "//meta[@property='product:price:amount']"; }
        }

        public override string DescriptionXPath
        {
            get { return "//div[@id='bonz_item_description']"; }
        }
        public override string IdRegex
        {
            get { return @"/listings/.*/(\d+)"; }
        }
        public override string DateTimeXPath
        {
            get { return "//div[@class='item_listing_detail']/div[text() = 'Posted for sale']"; }
        }
        public override string MetaDescriptionXPath
        {
            get { return "//meta[@name='Description']"; }
        }
        protected override void SetCreatedDate(HtmlDocument htmlDoc, Item item)
        {
            var dateTimeLabelNode = htmlDoc.DocumentNode.SelectSingleNode(DateTimeXPath);
            if (dateTimeLabelNode == null) return;
            var dateTimeNode = dateTimeLabelNode.NextSibling;
            if (dateTimeNode == null) return;
            item.Created = DateTime.Now;
        }
        protected override string RefineContent(string content)
        {
            var contentRefined = content.Replace("<h2>More about this item</h2>", "").Trim();
            return contentRefined;
        }
    }
}