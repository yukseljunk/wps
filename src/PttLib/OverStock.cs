using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PttLib.Helpers;

namespace PttLib
{
    public class OverStock : Site
    {
        public override int BlockSize
        {
            get
            {
                return 3;
            }
        }
        public override string UrlKeywordFormat
        {
            get { return "http://www.overstock.com/search?keywords={0}&searchtype=Header&resultIndex={{0}}&resultsPerPage=50&sort=Relevance&infinite=true"; }
        }

        public override string Get(string url, int page = 1)
        {
            var urlToAsk = url.Replace(" ", "+");
            urlToAsk = string.Format(urlToAsk, (page - 1) * 50 + 1);
            var maxTry = 3;
            var tryCount = 0;
            var response = "";
            while (tryCount < maxTry)
            {
                if (_options.UseProxy)
                {
                    response = WebHelper.CurlSimple(urlToAsk, "application/json",
                        new WebProxy(_options.ProxyAddress + ":" + _options.ProxyPort));
                }
                else
                {
                    response = WebHelper.CurlSimple(urlToAsk, "application/json");
                }
                if (!string.IsNullOrEmpty(response))
                {
                    break;
                }
                Thread.Sleep(TimeSpan.FromSeconds(3));
                tryCount++;
            }

            if (!response.StartsWith("{")) return response;

            dynamic d = JObject.Parse(response);
            return string.Format(
                "<html><head></head><body><ul id='result-products'>{0}</ul><span class='results-count'><span>{1}</span></span><ul class='pagination'><li><div class='pagination-page-text'>X of {2}</div></li></ul></body></html>",
                d.html,
                d.totalResults,
                (d.totalResults / 50 + 1).ToString());
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

        public override string ImagesAttribute
        {
            get { return "data-max-img"; }
        }

        public override string ImagesXPath
        {
            get { return "//div[@class='thumb-section']//li"; }
        }

        public override string PriceXPath
        {
            get { return "//span[@itemprop='price']"; }
        }
        public override string DescriptionXPath
        {
            get { return "//div[@class='description toggle-content']"; }
        }
        public override string IdRegex
        {
            get
            {
                return @"/(\d+)/product.html";
            }
        }
    }
}