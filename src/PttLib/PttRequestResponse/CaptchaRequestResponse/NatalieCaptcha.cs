namespace PttLib.PttRequestResponse.CaptchaRequestResponse
{
    class NatalieCaptcha : ICaptchaRequestResponse
    {
        const string googleHost = "www.google.com";

        public bool CaptchaShown(string htmlSource)
        {
            return htmlSource.Contains("www.google.com/recaptcha/api");
        }

        public IPttRequest RequestWithCaptchaValue(IPttRequest request, IPttCaptcha pttCaptcha)
        {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(request.Response);

            var challengeField = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='recaptcha_challenge_field']");
            if (challengeField == null)
            {
                return null;
            }
            var challengeFieldValue = challengeField.Attributes["value"].Value;

            var pttRequestFactory = new PttRequestFactory(request);
            var newReq = pttRequestFactory.Deserialize(string.Format("<Request><Url>{0}</Url><Method>POST</Method></Request>", request.Url));

            newReq.PostValue = string.Format("recaptcha_challenge_field={0}&recaptcha_response_field={1}&submit=I%27m+a+human", challengeFieldValue, pttCaptcha.Value ?? "");
            newReq.WrappedRequest.ContentType = "application/x-www-form-urlencoded";
            
            var pttResponse = new PttResponse();
            //bu gelen htmlsource dan textarea degerini oku
            htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(pttResponse.GetResponse(newReq));
            var valueToPasteNode = htmlDoc.DocumentNode.SelectSingleNode("//textarea");
            if (valueToPasteNode == null)
            {
                return null;
            }


            return null;
        }

        public IPttCaptcha CaptchaImage(IPttRequest request, string htmlSource)
        {
            
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(htmlSource);
            var iframe = htmlDoc.DocumentNode.SelectSingleNode("//iframe");
            if (iframe == null)
            {
                return null;
            }
            var iframeSrc = iframe.Attributes["src"].Value;
            
            var newRequest = new PttRequest(iframeSrc) {WrappedRequest = {ContentType = "text/html", Method = "GET"}};
            IPttResponse response = new PttResponse();
            var captchaFormHtml = response.GetResponse(newRequest);

            htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(captchaFormHtml);
            var captchaImg = htmlDoc.DocumentNode.SelectSingleNode("//img");
            if (captchaImg == null)
            {
                return null;
            }
            var captchaSrc = string.Format("http://{0}/recaptcha/api/{1}", googleHost, captchaImg.Attributes["src"].Value);
            
            return new PttCaptcha() { Url = captchaSrc, RequestUsed = newRequest, FormUrl = iframeSrc};
        }

        public bool RepeatFirstRequest { get { return false; } }

        /*
        protected Tuple<string, string> Recaptcha(string htmlSource, string url, string postContentType, CookieContainer cookieContainer, IWebProxy proxyToUse)
        {
            const string googleHost = "www.google.com";
            //google recaptcha, gelen html de bir tane iframe var src sini al, request ettir, gelen dokumanda tek imaj var captcha imaji, onun url sini bul
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(htmlSource);
            var iframe = htmlDoc.DocumentNode.SelectSingleNode("//iframe");
            if (iframe == null)
            {
                return null;
            }
            var iframeSrc = iframe.Attributes["src"].Value;
            var req = new Request(iframeSrc, cookieContainer, googleHost, url);
            var captchaHtml = req.GetResponseAsString(null, cookieContainer, postContentType);
            htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(captchaHtml);
            var captchaImg = htmlDoc.DocumentNode.SelectSingleNode("//img");
            if (captchaImg == null)
            {
                return null;
            }
            var challengeField = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='recaptcha_challenge_field']");
            if (challengeField == null)
            {
                return null;
            }
            var challengeFieldValue = challengeField.Attributes["value"].Value;
            var captchaSrc = string.Format("http://{0}/recaptcha/api/{1}", googleHost, captchaImg.Attributes["src"].Value);

            var captchaFileName = DownloadCaptchaImage(captchaSrc, cookieContainer, googleHost, iframeSrc);
            var captchaValue = CaptchaBreaker.Guess(captchaFileName, 1);


            var postData2 = string.Format("recaptcha_challenge_field={0}&recaptcha_response_field={1}&submit=I%27m+a+human", challengeFieldValue, captchaValue ?? "");

            req = new Request(iframeSrc, cookieContainer, googleHost, iframeSrc, null, proxyToUse);
            htmlSource = req.GetResponseAsString(postData2, cookieContainer, postContentType);
            //bu gelen htmlsource dan textarea degerini oku
            htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(htmlSource);
            var valueToPasteNode = htmlDoc.DocumentNode.SelectSingleNode("//textarea");
            if (valueToPasteNode == null)
            {
                return null;
            }
            return new Tuple<string, string>(captchaValue, valueToPasteNode.InnerText);
        }*/

    }
}