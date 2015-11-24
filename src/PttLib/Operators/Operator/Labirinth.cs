using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using PttLib.CaptchaBreaker;
using PttLib.Helpers;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;
using PttLib.PttRequestResponse.CaptchaRequestResponse;
using PttLib.Tours;

namespace PttLib.Operators.Operator
{
    class Labirinth : OperatorBase
    {
        private const string DEFAULT_CAPTCHA_URL = "http://online.labirint.travel/vendor/kcaptcha/reg.php?SAMO=mo3stscrjhipabs6rn4tp5pn74&_=1402922469";
        private const string TOUR_OPERATOR = "LABIRINTH";
        private const string HOTEL_LIST_START_TOKEN = "add_hotels(";
        private const string HOTEL_LIST_END_TOKEN = ");";
        private const string DATA_START_TOKEN = "jQuery(samo.controls.resultset).ehtml('";
        private const string DATA_END_TOKEN = "');";

        public Labirinth()
        {
            AllHotelsXmlConverter = new JSONArrayToXmlConverter();
        }

        public override List<List<string>> MealTypes
        {
            get
            {
                return new List<List<string>>()
                                              {
                                                  new List<string>(){"ALL"},
                                                  new List<string>(){"RR"},
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

        private ICaptchaBreaker CaptchaBreaker
        {
            get
            {
                var captchaBreakerFactory = new CaptchaBreakerFactory();
                return captchaBreakerFactory.Create("Dbc");
            }
        }

        public override Tuple<int, int> TryAgainInException
        {
            get
            {
                return new Tuple<int, int>(3,15);
            }
        }

        public override ITourFactory TourFactory
        {
            get { return new TourFactory(null, null, IteratingWhileGettingToursAfterTourItemSet); }
        }

        private TourFactoryControlFlags IteratingWhileGettingToursAfterTourItemSet(IPttRequest request, Tour tourItem)
        {
            return tourItem.Price <= QueryObject.MaxPrice
                ? TourFactoryControlFlags.Pass
                : TourFactoryControlFlags.Continue;
        }

        public override string Name
        {
            get { return TOUR_OPERATOR; }
        }

        protected override void ParseHotelNameAndIdFromNode(HtmlNode htmlNode, out string hotelName, out string hotelId)
        {
            hotelName = htmlNode.SelectNodes(".//i[@key='name']")[0].InnerText;
            hotelId = htmlNode.SelectNodes(".//i[@key='id']")[0].InnerText;
        }

        protected override string RefineHotelNames(string hotelsHtml)
        {
            return hotelsHtml.TrimMiddle(HOTEL_LIST_START_TOKEN, HOTEL_LIST_END_TOKEN);
        }

        public override string Refine(IPttRequest pttRequest, string htmlSource)
        {
            return htmlSource.TrimMiddle(DATA_START_TOKEN, DATA_END_TOKEN).Replace("\\\"", "");
        }
    }
}