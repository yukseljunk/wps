using PttLib.Helpers;

namespace PttLib
{
    public class OverStock : Site
    {
        public override string UrlKeywordFormat
        {
            get { return "http://www.overstock.com/search?keywords={0}&searchtype=Header&resultIndex={{0}}&resultsPerPage=25&sort=Relevance&infinite=true"; }
        }

        public override string Get(string url, int page = 1)
        {
            var urlToAsk = url.Replace(" ", "+");
            urlToAsk = string.Format(urlToAsk, (page-1)*25+1);

            return WebHelper.CurlSimple(urlToAsk,"application/json; charset=utf-8");
        }

        public override string Name
        {
            get
            {
                return "OverStock";
            }
        }

    }
}