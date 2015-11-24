using System;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using ExpressionEvaluator;
using PttLib.Helpers;

namespace PttLib.PttRequestResponse
{
    public enum PttRequestType
    {
        None,
        TourInfo,
        HotelInfo,
        Init
    }


    /// <summary>
    /// ptt request, newly created to replace request.cs class
    /// dynamical replace supported properties: Url, Postvalue and Condition
    /// </summary>
    public class PttRequest : IPttRequest
    {
        private CookieContainer _cookieContainer;
        private IWebProxy _proxy;
        private HttpWebRequest _request;
        private string _url;
        private string _postValue;
        private string _condition;

        /// <summary>
        /// copies session type props to target, i.e. proxy, cookiecontainer, viewstate, eventvalidation
        /// </summary>
        /// <param name="target"></param>
        public void CopySession(IPttRequest target)
        {
            target.Proxy = Proxy;
            CopySessionWithoutProxy(target);
        }

        /// <summary>
        /// like copysession, except proxy not kept
        /// </summary>
        /// <param name="target"></param>
        public void CopySessionWithoutProxy(IPttRequest target)
        {
            target.CookieContainer = CookieContainer;
            target.ViewStateValue = ViewStateValue;
            target.EventValidationValue = EventValidationValue;
            target.SessionJar = SessionJar;
        }

        public string PostValue
        {
            get
            {
                if (_postValue == null) return null;
                return TypeHelper.FillDynamicProperties(_postValue, this);
            }
            set { _postValue = value; }
        }

        public bool Chunked { get; set; }
        public string Condition
        {
            get
            {
                if (_condition == null) return null;
                return TypeHelper.FillDynamicProperties(_condition, this);
            }
            set { _condition = value; }
        }

        public PttRequestType RequestType { get; set; }
        public bool ConditionSatisfied
        {
            get
            {
                //if condition not specified, it is true by default
                if (string.IsNullOrEmpty(Condition)) return true;

                var expression = new CompiledExpression(Condition);
                var result = expression.Eval();
                return (bool)result;
            }
        }

        public HttpWebRequest WrappedRequest
        {
            get
            {
                return _request;
            }
            set
            {
                _request = value;
            }
        }

        public PttRequest(string url, bool createWebRequest = true)
        {
            _url = url;
            if (url == null) throw new ArgumentException("No null values", "url");
            SessionJar = new ConcurrentDictionary<string, object>();

            if (!createWebRequest) return;

            int temp = ServicePointManager.DefaultConnectionLimit;
            _request = (HttpWebRequest)WebRequest.Create(Url);
            var sp = _request.ServicePoint;
            var prop = sp.GetType().GetProperty("HttpBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
            prop.SetValue(sp, (byte)0, null);
        }

        public string Url
        {
            get
            {
                return TypeHelper.FillDynamicProperties(_url, this);
            }
            set
            {
                if (_url == value) return;
                _url = value;
                Response = "";
            }
        }

       
        public IPttRequest Clone(PttWebRequest pttWebRequest)
        {
            var result = new PttRequest(Url);
            result.PostValue = PostValue;
            result.Chunked = Chunked;
            result.CookieContainer = CookieContainer;
            result.Proxy = Proxy;
            result.WrappedRequest = pttWebRequest.Convert();
            GC.Collect();//onceki request i memory den ucur
            result.RequestType = RequestType;
            result.EventValidationValue = EventValidationValue;
            result.ViewStateValue = ViewStateValue;
            result.SessionJar = SessionJar;
            return result;
        }

        public CookieCollection Cookies
        {
            get
            {
                return WebHelper.GetAllCookies(CookieContainer);
            }
        }


        public CookieContainer CookieContainer
        {
            get
            {
                return _cookieContainer;
            }
            set
            {
                _cookieContainer = value;
                if (_request == null) return;
                _request.CookieContainer = value;
            }
        }

        public IWebProxy Proxy
        {
            get { return _proxy; }
            set
            {
                _proxy = value;
                if (_request == null) return;
                _request.Proxy = value;
                if (value == null) return;
                _request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }
        }


        public override string ToString()
        {
            var proxyDefinition = "Proxy " + ProxyDefinition;
            if (_request == null) return proxyDefinition;

            var cookies = "";
            if (Cookies != null)
            {
                foreach (Cookie cookie in Cookies)
                {
                    cookies+=string.Format("Cookie:{0}, value:{1}", cookie.Name, cookie.Value);
                }
            }

            return string.Format("Request to url: {0}, postdata: {1}, chunked: {2},{3}, with cookies:{4} ,keepAlive:{5}", Url, PostValue, Chunked, proxyDefinition, cookies, (WrappedRequest==null?"":WrappedRequest.KeepAlive.ToString()));
        }

        private string ProxyDefinition
        {
            get
            {
                if (_proxy == null)
                {
                    return "null proxy";
                }
                var webProxy = _proxy as WebProxy;
                if (webProxy == null || webProxy.Address == null) return "user proxy settings";
                return webProxy.Address.AbsoluteUri;
            }
        }

        public string ViewStateValue { get; set; }
        public string EventValidationValue { get; set; }
        public ConcurrentDictionary<string, object> SessionJar { get; set; }
        public string Response { get; set; }
    }
}