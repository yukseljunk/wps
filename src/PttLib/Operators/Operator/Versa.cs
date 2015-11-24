using System.Collections.Generic;
using PttLib.CaptchaBreaker;
using PttLib.Helpers;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;
using PttLib.PttRequestResponse.CaptchaRequestResponse;

namespace PttLib.Operators.Operator
{
    class Versa : OperatorBase
    {
        private const string TOUR_OPERATOR = "VERSA";
        private const string TOUR_LIST_START_TOKEN = ".ehtml('";
        private const string TOUR_LIST_END_TOKEN = "');";
        private const string DEFAULT_CAPTCHA_URL = "http://samo.versa.travel/vendor/kcaptcha/reg.php?SAMO=hjmt0ke21ma8dc9rjmtp9cvih7&_=1379686988";

        public Versa()
        {
            AllHotelsXmlConverter = new VersaToXmlConverter();
        }

        public override List<List<string>> MealTypes
        {
            get
            {
                return new List<List<string>>()
                                              {
                                                  new List<string>(){"ALL"},
                                                  new List<string>(){"RO"},
                                                  new List<string>(){"BB"},
                                                  new List<string>(){"HB"},
                                                  new List<string>(){"FB"},
                                                  new List<string>(){"AI"},
                                                  new List<string>(){"UAL"},
                                                  new List<string>(){"OTHER"}
                                              };
            }
        }

        public override IRequestResponse RequestResponseBehavior
        {
            get
            {
                return new RequestResponseWithCaptcha(RetryNeededForRequest, TryAgainInException, new AnexCaptcha(DEFAULT_CAPTCHA_URL), CaptchaBreaker);
            }
        }

        public ICaptchaBreaker CaptchaBreaker
        {
            get
            {
                var captchaBreakerFactory = new CaptchaBreakerFactory();
                return captchaBreakerFactory.Create("Dbc");
            }
        }

        public override string Refine(IPttRequest pttRequest, string htmlSource)
        {
            return htmlSource.TrimMiddle(TOUR_LIST_START_TOKEN, TOUR_LIST_END_TOKEN).Replace("\\\"", "\"").Replace("\\n\\", "");
        }

        public override string Name
        {
            get { return TOUR_OPERATOR; }
        }
    }
}