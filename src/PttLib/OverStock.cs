using System;
using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PttLib.Helpers;

namespace PttLib
{
    public class OverStock : Site
    {
        public override string UrlKeywordFormat
        {
            get { return "http://www.overstock.com/search?keywords={0}&searchtype=Header&resultIndex={{0}}&resultsPerPage=50&sort=Relevance&infinite=true"; }
        }

        public override string Get(string url, int page = 1)
        {
            var urlToAsk = url.Replace(" ", "+");
            urlToAsk = string.Format(urlToAsk, (page - 1) * 50 + 1);

            var response = WebHelper.CurlSimple(urlToAsk, "application/json");
            if (!response.StartsWith("{")) return response;

            dynamic d = JObject.Parse(response);
            return string.Format(
                "<html><head></head><body><ul id='result-products'>{0}</ul><span class='results-count'><span>{1}</span></span><ul class='pagination'><li><div class='pagination-page-text'>X of {2}</div></li></ul></body></html>", 
                d.html, 
                d.totalResults,
                (d.totalResults/50+1).ToString());
        }

        public override string Name
        {
            get
            {
                return "OverStock";
            }
        }

        public override string ItemsXPath
        {
            get { return "//ul[@id='result-products']//a[@class='pro-thumb']"; }
        }

        public override void GetPageCount(out int pageCount, HtmlDocument htmlDoc)
        {
            pageCount = 0;
            var pageNode = htmlDoc.DocumentNode.SelectSingleNode("//ul[@class='pagination']//div[@class='pagination-page-text']");
            if (pageNode != null)
            {
                var paginationText = pageNode.InnerText;
                var regex = new Regex(@"of (\d+)");
                var match = regex.Match(paginationText);
                if (match.Success)
                {
                    pageCount = Int32.Parse(match.Groups[1].Value, NumberStyles.AllowThousands, new CultureInfo("en-US"));
                }
            }
        }

        protected override void GetItemCount(out int totalItemCount, HtmlDocument htmlDoc)
        {
            totalItemCount = 0;
            var countNode = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='results-count']/span");
            if (countNode != null)
            {
                totalItemCount = int.Parse(countNode.InnerText);
            }
        }


    }
}