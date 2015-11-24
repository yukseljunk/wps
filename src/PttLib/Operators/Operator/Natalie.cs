using System.Collections.Generic;
using HtmlAgilityPack;
using PttLib.CaptchaBreaker;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;
using PttLib.PttRequestResponse.CaptchaRequestResponse;
using PttLib.Tours;

namespace PttLib.Operators.Operator
{
    class Natalie : OperatorBase
    {
        private const string TOUR_OPERATOR = "NATALIE";
        private const string HOTELS_LIST_XPATH = "//select[@id='hHtl']";

        public override List<List<string>> MealTypes
        {
            get
            {
                return new List<List<string>>()
                                              {
                                                 new List<string>(){"ALL"},
                                                 new List<string>(){ "AO"},
                                                 new List<string>(){ "BB"},
                                                  new List<string>(){"HB"},
                                                  new List<string>(){"FB"},
                                                  new List<string>(){"AI"}
                                              };
            }
        }

        public Natalie()
        {
            AllHotelsXmlConverter = AllHotelsXmlConverter = new SelectBoxToXmlConverter(HOTELS_LIST_XPATH, 4, 3);
        }

        public override int DatePartitioningDays
        {
            get { return 1; }
        }

        public override ITourFactory TourFactory
        {
            get { 
                return new NatalieTourFactory();
            }
        }

        public override IRequestResponse RequestResponseBehavior
        {
            get
            {
                return new RequestResponseWithNatCap(TOUR_OPERATOR, RetryNeededForRequest, TryAgainInException, new NatalieCaptcha(), CaptchaBreaker);
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

        public override bool HarvestSingleThread
        {
            get
            {
                return true;
            }
        }


        public override string Name
        {
            get { return TOUR_OPERATOR; }
        }

        protected override void ParseHotelNameAndIdFromNode(HtmlNode htmlNode, out string hotelName, out string hotelId)
        {
            hotelName = htmlNode.SelectNodes(".//i[@key='hotelName']")[0].InnerText;
            hotelId = htmlNode.SelectNodes(".//i[@key='hotelId']")[0].InnerText;
        }

        public override string QueryNotValid
        {
            get
            {
                var baseQueryNotValid = base.QueryNotValid;
                if (baseQueryNotValid != null)
                {
                    return baseQueryNotValid;
                }
                if (QueryObject.MealType > 5)
                {
                    return "Natalie Ultra All inclusive ve diger yemek tipi icin veri dondermiyor!";
                }

                return null;
            }
        }

    }
}
