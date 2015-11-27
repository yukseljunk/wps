using HtmlAgilityPack;

namespace PttLib
{
    public class Dawanda : Site
    {
        public override void GetPageCount(out int pageCount, HtmlDocument htmlDoc)
        {
            pageCount = 500;
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