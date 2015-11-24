using System.Text.RegularExpressions;
using PttLib.Helpers;

namespace PttLib.PttRequestResponse.CaptchaRequestResponse
{
    class CoralCaptcha : ICaptchaRequestResponse
    {
        #region Implementation of ICaptchaRequestResponse

        public bool CaptchaShown(string htmlSource)
        {
            return htmlSource.Contains("PackageSearch.CaptchaImageBuffer=");
        }

        public IPttRequest RequestWithCaptchaValue(IPttRequest request, IPttCaptcha pttCaptcha)
        { 
            var pttRequestFactory = new PttRequestFactory(request);
            var newReq = pttRequestFactory.Deserialize(string.Format("<Request><Url>{0}</Url><Method>POST</Method><Referer>{0}</Referer></Request>", request.Url));
            newReq.WrappedRequest.ContentType = "application/x-www-form-urlencoded";
            newReq.PostValue = Regex.Replace(request.PostValue, @"packageSearchCaptcha=[^&]*&", "packageSearchCaptcha=" + pttCaptcha.Value + "&");
            return newReq;
        }

        public IPttCaptcha CaptchaImage(IPttRequest request, string htmlSource)
        {
            return new PttCaptcha()
                       {
                           Base64Content = htmlSource.TrimMiddleWithLeftCalc("PackageSearch.CaptchaImageBuffer=\"", "\"").Replace(
                                   "data:image/png;base64,", ""),
                                   IsBase64 = true
                       };
        }

        public bool RepeatFirstRequest { get { return false; } }

        #endregion
    }
}