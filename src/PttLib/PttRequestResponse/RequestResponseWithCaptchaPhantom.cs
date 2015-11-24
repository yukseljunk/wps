using System;
using System.Globalization;
using PttLib.CaptchaBreaker;
using PttLib.Helpers;
using PttLib.PttRequestResponse.CaptchaRequestResponse;

namespace PttLib.PttRequestResponse
{
    public class RequestResponseWithCaptchaPhantom : RequestResponseWithCaptcha
    {
        private readonly string _name;

        public RequestResponseWithCaptchaPhantom(string name, Func<string, bool> retryNeededForRequest, Tuple<int, int> tryAgainInException, ICaptchaRequestResponse captchaRequestResponse, ICaptchaBreaker captchaBreaker):
            base(retryNeededForRequest, tryAgainInException, captchaRequestResponse, captchaBreaker)
        {
            _name = name;
        }

        public override string GetHtml(IPttRequest request, bool forHotelList)
        {
            var proxy = WebHelper.GetProxyForOperator(_name);
            
            var phantomJs = new PhantomJs();
            //phantomerror
            var html = phantomJs.GrabForOperator(request.Url, _name, proxy == null ? "" : proxy.Item1 + ":" + proxy.Item2, new CultureInfo("ru-RU").Name);
            if (_captchaRequestResponse.CaptchaShown(html))
            {
                Logger.LogProcess("Natalie Captcha shown...");
            }
            if ("phantomerror" == html)
            {
                Logger.LogProcess("Phantom error...");
                return null;
            }
            return html;
        }

    }
}