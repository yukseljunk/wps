using System;
using System.IO;
using System.Threading;
using HtmlAgilityPack;
using PttLib.Helpers;
using System.Web.Script.Serialization;
using System.Web;

namespace PttLib.PttRequestResponse.CaptchaRequestResponse
{
    public class TezCaptcha : ICaptchaRequestResponse
    {
        #region Implementation of ICaptchaRequestResponse
        private readonly bool _extensiveLoggingNeeded;

        public TezCaptcha(bool extensiveLoggingNeeded = false)
        {
            _extensiveLoggingNeeded = extensiveLoggingNeeded;
        }
        public bool CaptchaShown(string htmlSource)
        {
            var jss = new JavaScriptSerializer();
            try
            {
                var dict = jss.Deserialize<dynamic>(htmlSource);
                if (dict.ContainsKey("message"))
                {
                    if (dict["message"] == "captchaEnable")
                    {
                        if (_extensiveLoggingNeeded)
                        {
                            Logger.LogProcess("TezCaptcha captcha shown!");
                        }
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.LogProcess("TezCaptcha captcha shown exception:");
                Logger.LogExceptions(exception);
                return false;
            }
            if (_extensiveLoggingNeeded)
            {
                Logger.LogProcess("TezCaptcha captcha not shown!");
            }
            return false;
        }


        public IPttRequest RequestWithCaptchaValue(IPttRequest request, IPttCaptcha pttCaptcha)
        {
            if (_extensiveLoggingNeeded)
            {
                Logger.LogProcess("TezCaptcha RequestWithCaptchaValue captcha coming:" + pttCaptcha.ToString());
            }

            var pttRequestFactory = new PttRequestFactory(request);
            var newRequest = pttRequestFactory.Deserialize(
                string.Format("<Request><Url>{0}</Url><Referer>{1}</Referer><Method>POST</Method></Request>", 
                    "http://www.tez-tour.com/captcha/check.htm", 
                    "http://www.tez-tour.com/captcha/index.htm?locale=ru&pr=s&ref=http://www.tez-tour.com/search.html"
                    ));
            

            newRequest.WrappedRequest.AllowAutoRedirect = false;
            newRequest.WrappedRequest.Timeout = newRequest.WrappedRequest.Timeout * 4;
            newRequest.WrappedRequest.ReadWriteTimeout = newRequest.WrappedRequest.ReadWriteTimeout * 4;
            newRequest.WrappedRequest.ServicePoint.ConnectionLeaseTimeout = newRequest.WrappedRequest.Timeout;
            newRequest.WrappedRequest.ServicePoint.MaxIdleTime = newRequest.WrappedRequest.Timeout;

            var captchaImageUri = new Uri(pttCaptcha.Url);
            var captchaFileNameWithExtension = Path.GetFileNameWithoutExtension(captchaImageUri.AbsolutePath);

            var captchaUrl = pttCaptcha.FormUrl;
            var hashStart = captchaUrl.IndexOf("#");
            if (hashStart > -1) captchaUrl = captchaUrl.Substring(hashStart);
            captchaUrl = HttpUtility.UrlEncode(HttpUtility.UrlEncode(captchaUrl));

            if (_extensiveLoggingNeeded)
            {
                Logger.LogProcess("TezCaptcha RequestWithCaptchaValue captchaurl parsed:" + captchaUrl);
            }

            newRequest.PostValue = string.Format("image={0}&ref={3}&pr=s&hashRef={2}&answer={1}&submit=%D0%AF+%D0%BD%D0%B5+%D1%80%D0%BE%D0%B1%D0%BE%D1%82%21",
                captchaFileNameWithExtension,
                pttCaptcha.Value,
                captchaUrl,
                "http%3A%2F%2Fwww.tez-tour.com%2Fsearch.html");

            newRequest.WrappedRequest.ContentType = "application/x-www-form-urlencoded";
           
            return newRequest;
        }

        public IPttCaptcha CaptchaImage(IPttRequest request, string htmlSource)
        {
            if (_extensiveLoggingNeeded)
            {
                Logger.LogProcess("TezCaptcha CaptchaImage to be parsed from:" + htmlSource);
            }

            var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize<dynamic>(htmlSource);
            string captchaFormHtml = "";
            var captchaFormUrl = "";
            if (dict.ContainsKey("message"))
            {
                if (dict["message"] == "captchaEnable")
                {
                    if (_extensiveLoggingNeeded)
                    {
                        Logger.LogProcess("TezCaptcha CaptchaImage captchaEnable message came");
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                    captchaFormUrl = HttpUtility.UrlDecode((string)dict["ref"]);
                    if (_extensiveLoggingNeeded)
                    {
                        Logger.LogProcess("TezCaptcha CaptchaImage captcha form url:" + captchaFormUrl);
                    }
                    var pttRequestFactory = new PttRequestFactory(request, false);
                    var newReq = pttRequestFactory.Deserialize(string.Format("<Request><Url>{0}</Url></Request>",captchaFormUrl));

                    if (_extensiveLoggingNeeded)
                    {
                        Logger.LogProcess("TezCaptcha CaptchaImage newRequest created:" + newReq);
                    }
                    try
                    {
                        IPttResponse response = new PttResponse();
                        captchaFormHtml = response.GetResponse(newReq);
                        newReq.WrappedRequest = null;
                        GC.Collect();
                    }
                    catch (Exception exception)
                    {
                        Logger.LogProcess("TezCaptcha CaptchaImage newrequest response error:");
                        Logger.LogExceptions(exception);
                    }
                    if (_extensiveLoggingNeeded)
                    {
                        Logger.LogProcess("TezCaptcha CaptchaImage captcha form html:" + captchaFormHtml);
                    }

                }
            }

            var html = new HtmlDocument();
            html.LoadHtml(captchaFormHtml);
            var captchaValue = html.DocumentNode.SelectSingleNode("//input[@name='image']");

            if (captchaValue == null)
            {
                if (_extensiveLoggingNeeded)
                {
                    Logger.LogProcess("TezCaptcha CaptchaImage captcha image useing default value");
                }

                return new PttCaptcha() { Url = "http://www.tez-tour.com/captcha/images/19E9EE6B59814EF25AAC404E5336EA2E.png", FormUrl = captchaFormUrl };
            }
            if (_extensiveLoggingNeeded)
            {
                Logger.LogProcess("TezCaptcha CaptchaImage captcha image extracted:" + captchaValue.Attributes["value"].Value);
            }

            return new PttCaptcha() { Url = string.Format("http://www.tez-tour.com/captcha/images/{0}.png", captchaValue.Attributes["value"].Value), FormUrl = captchaFormUrl };

        }

        public bool RepeatFirstRequest
        {
            get
            {
                Thread.Sleep(3000);
                return true;
            }
        }

        #endregion
    }
}