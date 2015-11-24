using System.Collections.Generic;
using HtmlAgilityPack;
using PttLib.CaptchaBreaker;
using PttLib.Helpers;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;
using PttLib.PttRequestResponse.CaptchaRequestResponse;

namespace PttLib.Operators.Operator
{
    class Coral : OperatorBase
    {
        private const string TOUR_OPERATOR = "CORAL";
        private const string HOTEL_LIST_START_TOKEN = "PackageSearch.jsHot,";
        private const string HOTEL_LIST_END_TOKEN = ");";
        
        private const string DATA_START = "Schema.ProductSearch.Fill(";
        private const string DATA_END = ");";
        private const string NOISE_IN_DATA = @"<font color=\'red\'>SVO&nbsp;<b>!</b></font>";
        
        public Coral()
        {
            AllHotelsXmlConverter = new JSONArrayToXmlConverter();
        }

        public override IRequestResponse RequestResponseBehavior
        {
            get
            {
                return new RequestResponseWithCaptcha(RetryNeededForRequest, TryAgainInException, new CoralCaptcha(), CaptchaBreaker);
            }
        }

        public override int DatePartitioningDays
        {
            get { return 7; }
        }

        public ICaptchaBreaker CaptchaBreaker
        {
            get
            {
                var captchaBreakerFactory = new CaptchaBreakerFactory();
                return captchaBreakerFactory.Create("Coral");
            }
        }

        #region Overrides of OperatorBase

        public override  List<List<string>> MealTypes
        {
            get
            {
                return new List<List<string>>()
                                              {
                                                 new List<string>(){ "ALL"},
                                                 new List<string>(){ "Без питания"},
                                                 new List<string>(){ "Завтрак","Bed & Breakfast"},
                                                 new List<string>(){ "Полупансион"},
                                                 new List<string>(){ "Полный Пансион"},
                                                 new List<string>(){ "Все Включено","Все Эксклюзивное Все включено"},
                                                 new List<string>(){ "Ультра Все Включено"}
                                              }; 
            }
        }

        public override string Refine(IPttRequest pttRequest, string htmlSource)
        {
            var dataBlock = htmlSource.TrimMiddle(DATA_START, DATA_END);
            dataBlock = dataBlock.Replace(NOISE_IN_DATA, "");

            var jsArrayToXmlConverter = new JavaScriptArrayToXmlConverter();
            return jsArrayToXmlConverter.ToXml(dataBlock);
        }

        protected override void ParseHotelNameAndIdFromNode(HtmlNode htmlNode, out string hotelName, out string hotelId)
        {
            hotelName = htmlNode.SelectNodes(".//i[@key='_2']")[0].InnerText;
            hotelId = htmlNode.SelectNodes(".//i[@key='_1']")[0].InnerText;
        }

        protected override string RefineHotelNames(string hotelsHtml)
        {
            return hotelsHtml.TrimMiddle(HOTEL_LIST_START_TOKEN, HOTEL_LIST_END_TOKEN);

        }


        public override string Name
        {
            get { return TOUR_OPERATOR; }
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
                if (QueryObject.MaxNights - QueryObject.MinNights > 7)
                {
                    return "Coral geceleme araligi 7 gunden fazlasi icin veri dondermiyor!";
                }
                if (QueryObject.MealType > 5)
                {
                    return "Coral Ultra All inclusive ve diger yemek tipi icin veri dondermiyor!";
                }
                return null;
            }
        }

        #endregion
    }
}
