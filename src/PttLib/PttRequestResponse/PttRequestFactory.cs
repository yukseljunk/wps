using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using HtmlAgilityPack;
using PttLib.Helpers;

namespace PttLib.PttRequestResponse
{
    public delegate void FillSessionJarDelegate(IPttRequest request, int requestCounter);

    class PttRequestFactory:IPttRequestFactory
    {
        private readonly bool _keepAliveDefaultValue=true;
        private readonly IWebProxy _proxy;
        private readonly CookieContainer _cookieContainer;
        private readonly string _viewStateValue;
        private readonly string _eventValidationValue;
    
        private const string SimplestRequestFormat = "<Request><Url>{0}</Url></Request>";

        /// <summary>
        /// default ctor, proxy either changed or nulled, cookieContainer new
        /// </summary>
        public PttRequestFactory()
        {
            IWebProxy proxy = new WebProxy();
            WebHelper.ArrangeProxy(ref proxy);
            _proxy = proxy;
            _cookieContainer= new CookieContainer();
            _viewStateValue = "";
            _eventValidationValue = "";

        }

        /// <summary>
        /// proxy arranged wrto config file for operator, cookiecontainer new
        /// </summary>
        /// <param name="operatorName"></param>
        /// <param name="keepAliveDefaultValue"></param>
        public PttRequestFactory(string operatorName, bool keepAliveDefaultValue)
        {
            _keepAliveDefaultValue = keepAliveDefaultValue;
            IWebProxy proxy = new WebProxy();
            WebHelper.ArrangeProxy(ref proxy, operatorName);
            _proxy = proxy;
            _cookieContainer = new CookieContainer();
            _viewStateValue = "";
            _eventValidationValue = "";
        }

        /// <summary>
        /// new but proxy and cookie, eventvalidation and cookiecontainer  taken by request
        /// </summary>
        /// <param name="request"></param>
        public PttRequestFactory(IPttRequest request, bool keepAliveDefaultValue=true)
        {
            _proxy = request.Proxy;
            _cookieContainer = request.CookieContainer;
            _viewStateValue = request.ViewStateValue;
            _eventValidationValue = request.EventValidationValue;
            _keepAliveDefaultValue = keepAliveDefaultValue;
            
        }

        /// <summary>
        /// new, proxy and cookie passed explicitly
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="cookieContainer"></param>
        public PttRequestFactory(IWebProxy proxy, CookieContainer cookieContainer = null)
        {
            _proxy = proxy;
            _cookieContainer = cookieContainer ?? new CookieContainer();
            _viewStateValue = "";
            _eventValidationValue = "";
        }

        /// <summary>
        /// all values are default
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IPttRequest SimpleRequest(string url)
        {
            var serialized = string.Format(SimplestRequestFormat, url);
            return Deserialize(serialized);
        }

        /// <summary>
        /// deserialize from single <request></request> node
        /// </summary>
        /// <param name="serialized"></param>
        /// <returns></returns>
        public IPttRequest Deserialize(string serialized)
        {
            var html = new HtmlDocument();
            html.LoadHtml(serialized.Replace("\r\n", ""));
            var requestNodes = html.DocumentNode.SelectNodes("request");
            if (requestNodes == null) return null;
            IPttRequest pttRequest;
            return GetSinglePttRequest(requestNodes.FirstOrDefault(), out pttRequest) ? pttRequest : null;
        }


        /// <summary>
        ///  deserialize from multi <request> nodes wrapped by <requests> node
        /// </summary>
        /// <param name="serialized"></param>
        /// <param name="lastRequest">request to transfer the state from</param>
        /// <param name="fillSessionJar"></param>
        /// <returns></returns>
        public IEnumerable<IPttRequest> DeserializeList(string serialized, IPttRequest lastRequest, FillSessionJarDelegate fillSessionJar)
        {
            var requestsSerialized = serialized.Replace("\r\n", "");

            var html = new HtmlDocument();
            html.LoadHtml(requestsSerialized);
            var requestNodes = html.DocumentNode.SelectNodes("requests/request");
            if (requestNodes == null) yield break;

            var requestCounter = 0;
            var _lastRequest = lastRequest;
            foreach (var requestNode in requestNodes)
            {
                var singleRequestSerialized = requestNode.OuterHtml;
                //singleRequestSerialized dynamic replacements to be done here...
                if (_lastRequest != null)
                {
                    if (fillSessionJar != null)
                    {
                        fillSessionJar(_lastRequest, requestCounter);
                    }
                    singleRequestSerialized = TypeHelper.FillDynamicProperties(singleRequestSerialized, (PttRequest)_lastRequest);
                }

                var pttRequest = Deserialize(singleRequestSerialized);
                if (pttRequest == null) continue;

                requestCounter++;
                if (_lastRequest != null)
                {
                    _lastRequest.CopySessionWithoutProxy(pttRequest);//transfer cookie, sessionjar, eventvalidation, viewstate etc
                }
                _lastRequest = pttRequest;
                yield return pttRequest;
            }
        }
     
        private bool GetSinglePttRequest(HtmlNode requestNode, out IPttRequest pttRequest)
        {

            pttRequest = null;
            var url =HttpUtility.HtmlDecode(XmlParse.GetStringNodeValue(requestNode, "url", ""));
            if (string.IsNullOrEmpty(url)) return false;

            //request bagimsiz olanlar>bunlardan dolayi bu request biraz daha kapsamli olacak
            var postValue = HttpUtility.HtmlDecode(XmlParse.GetStringNodeValue(requestNode, "postvalue", ""));
            var chunked = XmlParse.GetBooleanNodeValue(requestNode, "chunked", false);
            var condition = HttpUtility.HtmlDecode(XmlParse.GetStringNodeValue(requestNode, "condition", ""));

            //request ile ilgili olanlar
            
            pttRequest = new PttRequest(url, false);
            pttRequest.RequestType = XmlParse.GetEnumNodeValue<PttRequestType>(requestNode, "type", PttRequestType.None);

            if (pttRequest.ConditionSatisfied)//satisfy etmeyenleri loglama, webrequest ini doldurma
            {
                FillWebRequest(requestNode, pttRequest, url);
            }

            pttRequest.Chunked = chunked;
            pttRequest.PostValue = postValue;
            pttRequest.Condition = condition;
            pttRequest.Proxy = _proxy;
            pttRequest.CookieContainer = _cookieContainer;
            pttRequest.EventValidationValue = _eventValidationValue;
            pttRequest.ViewStateValue = _viewStateValue;

            if(pttRequest.ConditionSatisfied)//satisfy etmeyenleri loglama, webrequest ini doldurma
            {
                Logger.LogProcess(pttRequest.ToString());
            }
            return true;
        }

      

        private void FillWebRequest(HtmlNode requestNode, IPttRequest pttRequest, string url)
        {
            pttRequest.WrappedRequest = (HttpWebRequest)WebRequest.Create(url);
            var uri = new Uri(url);

            pttRequest.WrappedRequest.Method = XmlParse.GetStringNodeValue(requestNode, "method", "GET", true);
            pttRequest.WrappedRequest.ContentType = XmlParse.GetStringNodeValue(requestNode, "contenttype", "text/html", true);
            pttRequest.WrappedRequest.Referer = XmlParse.GetStringNodeValue(requestNode, "referer",
                uri.GetLeftPart(UriPartial.Authority), true);
            pttRequest.WrappedRequest.Host = XmlParse.GetStringNodeValue(requestNode, "host", uri.Host, true);
            pttRequest.WrappedRequest.UserAgent = XmlParse.GetStringNodeValue(requestNode, "useragent",
                "Mozilla/5.0 (Windows NT 5.2; rv:9.0.1) Gecko/20100101 Firefox/9.0.1",
                true);

            pttRequest.WrappedRequest.Accept = XmlParse.GetStringNodeValue(requestNode, "accept",
                "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8", true);
            pttRequest.WrappedRequest.KeepAlive = XmlParse.GetBooleanNodeValue(requestNode, "keepalive", _keepAliveDefaultValue);
            pttRequest.WrappedRequest.AllowAutoRedirect = XmlParse.GetBooleanNodeValue(requestNode, "allowautoredirect", true);
            pttRequest.WrappedRequest.ProtocolVersion = HttpVersion.Version11;
            pttRequest.WrappedRequest.Timeout = XmlParse.GetIntegerNodeValue(requestNode, "timeout", 100000);
            pttRequest.WrappedRequest.ReadWriteTimeout = XmlParse.GetIntegerNodeValue(requestNode, "readwritetimeout", 300000);
            pttRequest.WrappedRequest.ServicePoint.ConnectionLeaseTimeout = 5000;
            pttRequest.WrappedRequest.ServicePoint.MaxIdleTime = 5000;
            pttRequest.WrappedRequest.ServicePoint.Expect100Continue = false;

            var decompressionMethods = XmlParse.GetEnumListNodeValue<DecompressionMethods>(requestNode,
                "automaticdecompression/value");
            if (null != decompressionMethods)
            {
                foreach (var decompressionMethod in decompressionMethods)
                {
                    pttRequest.WrappedRequest.AutomaticDecompression = pttRequest.WrappedRequest.AutomaticDecompression |
                                                                       decompressionMethod;
                }
            }
            var headers = XmlParse.GetKeyValueListNodeValue(requestNode, "headers/add");
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    pttRequest.WrappedRequest.Headers.Add(header.Key, header.Value);
                }
            }

            var cookies = XmlParse.GetKeyValueListNodeValue(requestNode, "cookies/add");
            if (cookies != null)
            {
                if (pttRequest.WrappedRequest.CookieContainer == null)
                {
                    pttRequest.WrappedRequest.CookieContainer = new CookieContainer();
                }

                foreach (var cookie in cookies)
                {
                    pttRequest.WrappedRequest.CookieContainer.Add(new Cookie(cookie.Key, cookie.Value) { Domain = uri.Host });
                }
            }
        }

    }
}
