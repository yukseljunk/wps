using System;
using System.Globalization;
using PttLib.CaptchaBreaker;
using PttLib.Helpers;
using PttLib.PttRequestResponse.CaptchaRequestResponse;

namespace PttLib.PttRequestResponse
{
    public class RequestResponseWithNatCap : RequestResponseWithCaptcha
    {
        private readonly string _name;

        public RequestResponseWithNatCap(string name, Func<string, bool> retryNeededForRequest, Tuple<int, int> tryAgainInException, ICaptchaRequestResponse captchaRequestResponse, ICaptchaBreaker captchaBreaker) :
            base(retryNeededForRequest, tryAgainInException, captchaRequestResponse, captchaBreaker)
        {
            _name = name;
        }

        public override string GetHtml(IPttRequest request, bool forHotelList)
        {
            var proxy = WebHelper.GetProxyForOperator(_name);

            var natCap = new NatCap();
            //phantomerror
            var html = natCap.GrabForOperator(request.Url, _name, proxy == null ? "" : proxy.Item1 + ":" + proxy.Item2, new CultureInfo("ru-RU").Name);
            if (_captchaRequestResponse.CaptchaShown(html))
            {
                Logger.LogProcess("Natalie Captcha shown...");
            }
            if (html.Contains("###MAXCAPTCHAEXCEEDED###"))
            {
                Logger.LogProcess("NATCAP: MAX CAPTCHA EXCEEDED");
                return null;
            }
            if (html.Contains("###404###"))
            {
                Logger.LogProcess("NATCAP: 404");
                return null;
            }

            if (html.Contains("###BLOCKED###"))
            {
                Logger.LogProcess("NATCAP: IP BLOCKED " + (proxy==null?"":proxy.Item1));
                return null;
            }
            if (html.Contains("###CAPTCHASHOWN###"))
            {
                Logger.LogProcess("NATCAP: captcha shown");
                return null;
            }
            return html;
        }

    }
}