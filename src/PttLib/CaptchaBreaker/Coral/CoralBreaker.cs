using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading;
using PttLib.CaptchaBreaker.Coral.Guesser;
using PttLib.CaptchaBreaker.Fx;
using PttLib.CaptchaBreaker.Info;
using PttLib.Helpers;

namespace PttLib.CaptchaBreaker.Coral
{
    /// <summary>
    /// just for CORAL captchas
    /// </summary>
    class CoralBreaker:ICaptchaBreaker
    {
        public static object RequestWaitLock = new object();

        private readonly CaptchaBreakerFactory _captchaBreakerFactory;

        public CoralBreaker(CaptchaBreakerFactory captchaBreakerFactory)
        {
            _captchaBreakerFactory = captchaBreakerFactory;
        }

        private static Bitmap Prepare(Bitmap bitmap)
        {
            var result = (Bitmap)bitmap.Clone();
            result = NegateFx.Negate(result);
            result = ChannelFx.RemoveGreen(result, Histogram.BgColor(result));
            result = NoiseFx.RemoveDots(result, Histogram.BgColor(result));
            result = ColorFx.RemoveColor(result, Histogram.BgColor(result), "ff838380");
            result = ColorFx.RemoveColor(result, Histogram.BgColor(result), "ffc1c1c0");
            return result;
        }
        public bool IsManual
        {
            get { return false; }
        }

        public int MaxNumberOfTry { get { return 4; } }

        public static string Guess(Bitmap bitmap)
        {
            if (bitmap == null) return null;
            var img = (Bitmap) bitmap.Clone();
            img = Prepare(img);
            var bgColor = Histogram.BgColor(img);
            var segments = Segments.GetSegments(img, bgColor, true);
            
            if (segments == null) return null;
            if (!segments.Any()) return null;
            if (segments.Count!=5) return null;

            var combinedGuess = "";
            CaptchaGuess captchaGuess= new CaptchaGuessByFillPercentageNormalizedVectorProduct();
            foreach (var segment in segments)
            {
                var guess = captchaGuess.GuessCaptcha(segment, bgColor);
                combinedGuess += guess;
            }
            return combinedGuess;
        }

        public void Prepare()
        {
            var captchaGuess = new CaptchaGuessByFillPercentageNormalizedVectorProduct();
            captchaGuess.Prepare();
        }


        public string Guess(string fileName, int tryCount)
        {
            //Logger.LogProcess(String.Format("{1} captcha try {0} for {2}", tryCount, this.Name, fileName));
            if (tryCount > MaxNumberOfTry)
            {
                return _captchaBreakerFactory.ManualBreaker().Guess(fileName, tryCount - MaxNumberOfTry);
            }
            WaitBetweenRequests();
            var guess = Guess(new Bitmap(fileName));
            //Logger.LogProcess(String.Format("{2} captcha guess {1} for try {0} for {3}", tryCount, guess ?? "", this.Name,fileName));
            return guess;
        }

        public string Name
        {
            get { return "Coral"; }
        }

        private static void WaitBetweenRequests()
        {
            //her req arasi bekleme 
            var coralSuccessiveRequestWaitSec = ConfigurationManager.AppSettings["coralSuccessiveRequestWaitSec"];
            if (String.IsNullOrEmpty(coralSuccessiveRequestWaitSec)) return;

            int successiveRequestWaitSec;
            if (!Int32.TryParse(coralSuccessiveRequestWaitSec, out successiveRequestWaitSec)) return;

            if (successiveRequestWaitSec <= 0) return;

            lock (RequestWaitLock)
            {
                if (WebHelper.LastRequestTime != DateTime.MinValue) //set before
                {
                    while (true)
                    {
                        var diff = DateTime.Now.Subtract(WebHelper.LastRequestTime);
                        if (diff.TotalSeconds > successiveRequestWaitSec)
                        {
                            WebHelper.LastRequestTime = DateTime.Now;
                            break;
                        }
                        else
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                        }
                    }
                }
                else //first time set
                {
                    WebHelper.LastRequestTime = DateTime.Now;
                }
            }
        }
    }
}
