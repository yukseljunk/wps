using System;
using System.IO;
using PttLib.CaptchaBreaker;
using PttLib.Helpers;
using PttLib.PttRequestResponse.CaptchaRequestResponse;

namespace PttLib.PttRequestResponse
{
    public class RequestResponseWithCaptcha : RequestResponseBase
    {
        protected readonly ICaptchaRequestResponse _captchaRequestResponse;
        private readonly bool _extensiveLoggingNeeded;
        private readonly ICaptchaBreaker _captchaBreaker;

        #region Implementation of ICaptchaRequestResponse

        public RequestResponseWithCaptcha(Func<string, bool> retryNeededForRequest, Tuple<int, int> tryAgainInException,
                                          ICaptchaRequestResponse captchaRequestResponse, ICaptchaBreaker captchaBreaker, bool extensiveLoggingNeeded=false)
            : base(retryNeededForRequest, tryAgainInException)
        {
            _captchaRequestResponse = captchaRequestResponse;
            _extensiveLoggingNeeded = extensiveLoggingNeeded;
            _captchaBreaker = captchaBreaker ?? DefaultCaptchaBreaker;
        }

        #endregion

        #region Overrides of RequestResponseBase

        public override string GetHtml(IPttRequest request, bool forHotelList)
        {
            if (_extensiveLoggingNeeded)
            {
                Logger.LogProcess("RequestResponseWithCaptcha gethtml coming request: " + request);
            }
            IPttResponse response = new PttResponse();
            var htmlSource = response.GetResponse(request);
            if(_extensiveLoggingNeeded)
            {
                Logger.LogProcess("RequestResponseWithCaptcha gethtml: "+ htmlSource);
            }
            if (!_captchaRequestResponse.CaptchaShown(htmlSource))
            {
                if (_extensiveLoggingNeeded)
                {
                    Logger.LogProcess("RequestResponseWithCaptcha captcha not shown, returning");
                }

                return htmlSource;
            }

            if (_extensiveLoggingNeeded)
            {
                Logger.LogProcess("RequestResponseWithCaptcha captcha shown");
            }
            return CaptchaPassedResult(request, htmlSource);
        }

        protected string CaptchaPassedResult(IPttRequest request, string htmlSource)
        {
            IPttResponse response = new PttResponse();
            var html = htmlSource;
            var captchaTries = 1;
            while (true)
            {
                var pttCaptcha = _captchaRequestResponse.CaptchaImage(request, html);

                if (_extensiveLoggingNeeded)
                {
                    Logger.LogProcess("RequestResponseWithCaptcha CaptchaPassedResult coming captcha:"+ pttCaptcha);
                }

                try
                {
                    var captchaResolve = ResolveCaptcha(request, pttCaptcha, captchaTries);
                    pttCaptcha.Value = captchaResolve.Item1;
                    pttCaptcha.FileNameInTempDir = captchaResolve.Item2;

                    if (_extensiveLoggingNeeded)
                    {
                        Logger.LogProcess("RequestResponseWithCaptcha CaptchaPassedResult after resolving:" + pttCaptcha);
                    }

                }
                catch (Exception exc)
                {
                    Logger.LogExceptions(exc);
                    return null;
                }
                IPttRequest captchaAddedRequest = _captchaRequestResponse.RequestWithCaptchaValue(request, pttCaptcha);
                if(_extensiveLoggingNeeded)
                {
                    Logger.LogProcess("RequestResponseWithCaptcha CaptchaPassedResult captchaAddedRequest:"+captchaAddedRequest);
                }
                var captchaPassedResult = response.GetResponse(captchaAddedRequest);
                captchaAddedRequest.WrappedRequest = null;
                GC.Collect();

                if (_extensiveLoggingNeeded)
                {
                    Logger.LogProcess("RequestResponseWithCaptcha CaptchaPassedResult captchaPassedResult:" + captchaPassedResult);
                }
                if (_captchaRequestResponse.RepeatFirstRequest)
                {
                    var pttRequestFactory = new PttRequestFactory(request);
                    var firstRequest = pttRequestFactory.Deserialize(string.Format("<Request><Url>{0}</Url><Host>www.tez-tour.com</Host><Referer> <![CDATA[http://www.tez-tour.com]]></Referer></Request>", request.Url));

                    if (_extensiveLoggingNeeded)
                    {
                        Logger.LogProcess("RequestResponseWithCaptcha CaptchaPassedResult RepeatFirstRequest firstrequest:" + firstRequest);
                    }
                    captchaPassedResult = response.GetResponse(firstRequest);
                }

                if (_captchaRequestResponse.CaptchaShown(captchaPassedResult))
                {
                    Logger.LogProcess(string.Format("Incorrect captcha for {0}", pttCaptcha.FileNameInTempDir));
                    html = captchaPassedResult;
                    captchaTries++;
                    continue;
                }
                Logger.LogProcess(string.Format("Correct captcha for {0}", pttCaptcha.FileNameInTempDir));

                return captchaPassedResult;
            }
        }

        #endregion


        protected Tuple<string, string> ResolveCaptcha(IPttRequest request, IPttCaptcha captcha, int captchaTries)
        {
            var captchaFileName = DownloadCaptchaImage(request, captcha);
            var captchaValue = _captchaBreaker.Guess(captchaFileName, captchaTries);
            return new Tuple<string, string>(captchaValue, captchaFileName);
        }


        protected string DownloadCaptchaImage(IPttRequest request, IPttCaptcha captcha)
        {
            CaptchaHelper.CreateCaptchasFolder();
            var captchaFileName = CaptchaHelper.NewCaptchaFileName();
            //Logger.LogProcess("gelen captcha dosya adi:" + captchaFileName);
            var captchaImageBytes = captcha.IsBase64 ? Convert.FromBase64String(captcha.Base64Content) : CaptchaImageBytes(request, captcha.Url);

            var fs = new FileStream(captchaFileName, FileMode.Create);
            var bw = new BinaryWriter(fs);
            bw.Write(captchaImageBytes);
            fs.Close();
            bw.Close();
            return captchaFileName;
        }

        protected byte[] CaptchaImageBytes(IPttRequest request, string imageSource)
        {
            IPttRequestFactory pttRequestFactory = new PttRequestFactory(request, false);
            IPttResponse response = new PttResponse();
            return response.GetResponseBytes(pttRequestFactory.SimpleRequest(imageSource));
        }


        public ICaptchaBreaker DefaultCaptchaBreaker
        {
            get
            {
                var captchaBreakerFactory = new CaptchaBreakerFactory();
                return captchaBreakerFactory.ManualBreaker();
            }
        }
    }
}