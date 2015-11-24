using System.Collections.Generic;
using HtmlAgilityPack;
using PttLib.CaptchaBreaker;
using PttLib.Helpers;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;
using PttLib.PttRequestResponse.CaptchaRequestResponse;

namespace PttLib.Operators.Operator
{
    class Sunmar : OperatorBase {

        private const string TOUR_OPERATOR = "SUNMAR";
        private const string HOTEL_LIST_START_TOKEN = "PackageSearch.jsHot,";
        private const string HOTEL_LIST_END_TOKEN = ");";
        private const string HOTEL_DATA_START_TOKEN = "Schema.ProductSearch.Fill(";
        private const string HOTEL_DATA_END_TOKEN = ");";

        public Sunmar()
        {
            AllHotelsXmlConverter = new JavaScriptArrayToXmlConverter(itemSeparator:", ");
        }

        public override IRequestResponse RequestResponseBehavior
        {
            get
            {
                return new RequestResponseWithCaptcha(RetryNeededForRequest, TryAgainInException, new CoralCaptcha(), CaptchaBreaker);
            }
        }

        public override List<List<string>> MealTypes
        {
            get
            {
                return new List<List<string>>()
                                              {
                                                  new List<string>(){"ALL"},
                                                  new List<string>(){"Без питания"},
                                                  new List<string>(){"Завтрак"},
                                                  new List<string>(){"Полупансион"},
                                                  new List<string>(){"Полный Пансион"},
                                                  new List<string>(){"Все Включено"},
                                                  new List<string>(){"Ультра Все Включено"}
                                              }; 
            }
        }

        public override int DatePartitioningDays
        {
            get { return 7; }
        }


        public override string Name
        {
            get { return TOUR_OPERATOR; }
        }

        public ICaptchaBreaker CaptchaBreaker
        {
            get
            {
                var captchaBreakerFactory = new CaptchaBreakerFactory();
                return captchaBreakerFactory.Create("Coral");
            }
        }

        protected override void ParseHotelNameAndIdFromNode(HtmlNode htmlNode, out string hotelName, out string hotelId)
        {
            hotelName = htmlNode.SelectNodes(".//i")[1].InnerText;
            hotelId = htmlNode.SelectNodes(".//i")[0].InnerText;
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
                    return "Sunmar Ultra All inclusive ve diger yemek tipi icin veri dondermiyor!";
                }
                return null;
            }
        }

        protected override string RefineHotelNames(string hotelsHtml)
        {
            return hotelsHtml.TrimMiddle(HOTEL_LIST_START_TOKEN, HOTEL_LIST_END_TOKEN);

        }


        public override string Refine(IPttRequest pttRequest, string htmlSource)
        {

            var html = htmlSource.TrimMiddle(HOTEL_DATA_START_TOKEN, HOTEL_DATA_END_TOKEN);
            var parser = new JavaScriptArrayToXmlConverter(itemSeparator: ",");
            var parsedData = parser.ToXml(html);
            return parsedData;
        }
    }

}
