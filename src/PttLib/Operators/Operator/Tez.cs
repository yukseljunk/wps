using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using System;
using PttLib.CaptchaBreaker;
using PttLib.Helpers;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;
using PttLib.PttRequestResponse.CaptchaRequestResponse;
using PttLib.Tours;

namespace PttLib.Operators.Operator
{
    /// <summary>
    /// for data harvest
    /// </summary>
    class Tez : OperatorBase
    {
        private const string TOUR_OPERATOR = "TEZ";
        private int _sleepSecondsAfterRejection = 15;
        private int _maxTryAfterRejection = 15;
        private const int MaxNightsSupported = 7;


        public Tez()
        {
            AllHotelsXmlConverter = new JSONDictToXmlConverter();
        }

        public override List<List<string>> MealTypes
        {
            get
            {
                return new List<List<string>>()
                                              {
                                                 new List<string>(){ "ALL"},
                                                 new List<string>(){ "Размещение без питания"},
                                                 new List<string>(){ "Только завтраки"},
                                                 new List<string>(){ "Завтрак и ужин"},
                                                 new List<string>(){ "Завтрак, обед и ужин"},
                                                 new List<string>(){ "Все включено"},
                                                 new List<string>(){ "Ультра все включено"}
                                              };
            }
        }

        public override string QueryNotValid
        {
            get
            {
                if (QueryObject.MaxNights - QueryObject.MinNights > 8)
                {
                    return String.Format("For {0}, max nights should be maximum {1} days bigger than min nights!", Name, MaxNightsSupported);
                }
                return base.QueryNotValid;
            }
        }

        public override ITourFactory TourFactory
        {
            get
            {
                return new TourFactory(ReturnBeforeIteratingWhileGettingTours, null, SkipIteratingAfterTourItemSet);
            }
        }

        public override int DatePartitioningDays
        {
            get { return 20; }
        }

        private TourFactoryControlFlags SkipIteratingAfterTourItemSet(IPttRequest request, Tour tour)
        {
            var moveToNextHotel =(QueryObject != null && tour.Price >= QueryObject.MaxPrice+1);
            return moveToNextHotel ? TourFactoryControlFlags.MoveToNextHotel | TourFactoryControlFlags.Continue : TourFactoryControlFlags.Pass;
        }


        private TourFactoryControlFlags ReturnBeforeIteratingWhileGettingTours(IPttRequest request, HtmlDocument html)
            {
            var moveToNextHotel = html.DocumentNode.SelectNodes(TourInfo.XPath).Count < 100;
            return moveToNextHotel ? TourFactoryControlFlags.MoveToNextHotel : TourFactoryControlFlags.Pass;
            }

      

        public override IRequestResponse RequestResponseBehavior
        {
            get
            {
                return new RequestResponseWithCaptcha(RetryNeededForRequest, TryAgainInException, new TezCaptcha(ExtensiveLoggingNeeded), CaptchaBreaker, ExtensiveLoggingNeeded);
            }
        }
        
        public override string Name
        {
            get { return TOUR_OPERATOR; }
        }

        public override Tuple<int, int> TryAgainInException
        {
            get
            {
                var tezRetry = ConfigurationManager.AppSettings["tezRetry"];
                if (String.IsNullOrEmpty(tezRetry)) return new Tuple<int, int>(_maxTryAfterRejection, _sleepSecondsAfterRejection); 
                var tezRetrySplitted= tezRetry.Split(new []{","},StringSplitOptions.RemoveEmptyEntries);
                return new Tuple<int, int>(Int32.Parse(tezRetrySplitted[0]), Int32.Parse(tezRetrySplitted[1]));
            }
        }

      
        public ICaptchaBreaker CaptchaBreaker
        {
            get
            {
                var captchaBreakerFactory = new CaptchaBreakerFactory();
                return captchaBreakerFactory.Create("Tez");
            }
        }

        public override string Refine(IPttRequest pttRequest, string htmlSource)
        {
            var htmlResult = new StringBuilder();
            var jss = new JavaScriptSerializer();

            htmlResult.Append("<d>");
            var dict = jss.Deserialize<dynamic>(htmlSource);

            if (!dict.ContainsKey("data"))
            {
                htmlResult.Append("</d>");
                return htmlResult.ToString();
            }

            var data = dict["data"];

            if (pttRequest.Url.Contains("/toursearch/"))
            {
                foreach (var item in data)
                {
                    htmlResult.Append("<h>");
                    for (var indexor = 0; indexor < item.Length; indexor++)
                    {
                        htmlResult.Append("<p");
                        htmlResult.Append(indexor);
                        htmlResult.Append(">");
                        htmlResult.Append(item[indexor]);
                        htmlResult.Append("</p");
                        htmlResult.Append(indexor);
                        htmlResult.Append(">");
                    }
                    htmlResult.Append("</h>");
                }
                htmlResult.Append("</d>");
                return htmlResult.ToString();
            }

            var dataIndex = 0;
            foreach (var item in data)
            {
                dataIndex++;
                htmlResult.Append("<h>");

                ItemTag(htmlResult, 0, dataIndex.ToString()); //index
                ItemTag(htmlResult, 1, item[0]); //date
                ItemTag(htmlResult, 2, "");
                ItemTag(htmlResult, 3, "");
                ItemTag(htmlResult, 4, item[3].ToString()); //nights
                ItemTag(htmlResult, 5, "");
                ItemTag(htmlResult, 6, item[6][1]); //hotel
                ItemTag(htmlResult, 7, "");
                ItemTag(htmlResult, 8, item[8]); //roomtype
                ItemTag(htmlResult, 9, "");
                ItemTag(htmlResult, 10, "");
                ItemTag(htmlResult, 11, "");
                ItemTag(htmlResult, 12, "");
                ItemTag(htmlResult, 13, item[7][1]); //mealtype
                ItemTag(htmlResult, 14, "");

                var accomodation = "";
                foreach (dynamic subItem in item[9])
                {
                    var count = subItem[0];
                    var startAge = subItem[1];
                    var endAge = subItem[2];
                    if (startAge == 0 && endAge == 0)
                    {
                        accomodation += count.ToString() + " ADULTS ";
                    }
                    else
                    {
                        accomodation += count.ToString() + " Child " + startAge.ToString() + "-" +
                                        endAge.ToString();
                    }
                }
                ItemTag(htmlResult, 15, accomodation); //accomodation
                ItemTag(htmlResult, 16, item[10]["total"].ToString()); //price

                htmlResult.Append("</h>");
            }
            htmlResult.Append("</d>");
            return htmlResult.ToString();

        }
        private static void ItemTag(StringBuilder htmlResult, int indexor, string data)
        {
            htmlResult.Append("<p");
            htmlResult.Append(indexor);
            htmlResult.Append(">");


            htmlResult.Append(data);

            htmlResult.Append("</p");
            htmlResult.Append(indexor);
            htmlResult.Append(">");
        }


        protected override bool RetryNeededForRequest(string result)
        {
            try
            {
                var jss = new JavaScriptSerializer();
                var dict = jss.Deserialize<dynamic>(result);
                if (dict.ContainsKey("message"))
                {
                    Logger.LogProcess("Tez retry needed because of " + dict["message"]);
                    if (dict["message"] == "captchaEnable")
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception exc)
            {

                return false;
            }
        }
    }

}
