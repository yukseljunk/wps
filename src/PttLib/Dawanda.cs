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