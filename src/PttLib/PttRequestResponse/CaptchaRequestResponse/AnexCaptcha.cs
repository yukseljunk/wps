using HtmlAgilityPack;

namespace PttLib.PttRequestResponse.CaptchaRequestResponse
{
    public class AnexCaptcha:ICaptchaRequestResponse
    {
        private readonly string _defaultCaptchUrl;
        private const string DEFAULT_CAPTCHA_URL = "http://online3.anextour.com/vendor/kcaptcha/reg.php?SAMO=ucrf8vrptc15ksc24o8u0k7kd4&_=1377176006";

        public AnexCaptcha(string defaultCaptchUrl = DEFAULT_CAPTCHA_URL)
        {
            _defaultCaptchUrl = defaultCaptchUrl;
        }

        #region Implementation of ICaptchaRequestResponse

        public bool CaptchaShown(string htmlSource)
        {
            return htmlSource.Contains("captchaForm");
        }

        public IPttRequest RequestWithCaptchaValue(IPttRequest request, IPttCaptcha pttCaptcha)
        {
            var pttRequestFactory = new PttRequestFactory(request);
            var newReq = pttRequestFactory.Deserialize(string.Format("<Request><Url>{0}</Url><Method>POST</Method></Request>", request.Url));

            newReq.PostValue = "samo_action=antibot&are_you_human=" + pttCaptcha.Value;
            newReq.WrappedRequest.ContentType = "application/x-www-form-urlencoded";
            return newReq;
        }

        public IPttCaptcha CaptchaImage(IPttRequest request, string htmlSource)
        {
            var html = new HtmlDocument();
            html.LoadHtml(htmlSource);
            var captchaImage = html.DocumentNode.SelectSingleNode("//img[@id='icaptcha']");
            if (captchaImage == null)
            {
                return new PttCaptcha() { Url = _defaultCaptchUrl };

            }
            return new PttCaptcha() {Url = captchaImage.Attributes["src"].Value};
        }

        public bool RepeatFirstRequest { get { return false; } }

        #endregion
    }
}