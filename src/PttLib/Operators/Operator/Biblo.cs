using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using PttLib.Helpers;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;
using PttLib.Tours;

namespace PttLib.Operators.Operator
{
    class Biblo : OperatorBase
    {
        private const string TOUR_OPERATOR = "BIBLO";
        private const string jsFilePathSeparator = "?_=";

        public override List<List<string>> MealTypes
        {
            get
            {
                return new List<List<string>>(){
                                                  new List<string>(){"ALL"},
                                                  new List<string>(){"AO"},
                                                  new List<string>(){"BB"},
                                                  new List<string>(){"HB"},
                                                  new List<string>(){"FB"},
                                                  new List<string>(){"AI","AI Soft"},
                                                  new List<string>(){"ULTRA ALL"}
                                              };
            }
        }

        public override string Name
        {
            get
            {
                return TOUR_OPERATOR;
            }
        }

        public Biblo()
        {
            AllHotelsXmlConverter = new BibloToXmlConverter();
        }

        public override IRequestResponse RequestResponseBehavior
        {
            get
            {
                return new RequestResponseWithoutCaptchaOmitExceptionsForHotelList(RetryNeededForRequest, TryAgainInException);
            }
        }

        public override int DatePartitioningDays
        {
            get { return 1; }
        }

        public override ITourFactory TourFactory
        {
            get { return new BibloTourFactory(); }
        }

        public static List<DateTime> TourDates { get; set; }

        protected override void FillSessionJar(IPttRequest request, int requestCounter)
        {
            if (request.RequestType != PttRequestType.Init) return;
            var dateToSelectStr = DatesToSelect(QueryObject.StartDate, request.Url, request.Response);
            TourDates = dateToSelectStr;
            var firstDate = DateTime.Today.ToString("d", new CultureInfo("ru-RU"));
            var lastDate = DateTime.Today.ToString("d", new CultureInfo("ru-RU"));
            if (dateToSelectStr.Any())
            {
                firstDate = dateToSelectStr.First().ToString("d", new CultureInfo("ru-RU"));
                lastDate = dateToSelectStr.Last().ToString("d", new CultureInfo("ru-RU"));
            }
            request.SessionJar.AddOrUpdate("firstDate", firstDate, (k, v) => firstDate);
            request.SessionJar.AddOrUpdate("lastDate", lastDate, (k, v) => lastDate);

        }

        private List<DateTime> DatesToSelect(DateTime dateToSelect, string jsFileName, string jsFileContent)
        {
            var jss = new JavaScriptSerializer();
            var pathVals = new List<string>();
            var pathValues = jsFileName.Split(new[] { jsFilePathSeparator }, StringSplitOptions.RemoveEmptyEntries);
            var city = "";
            if (pathValues.Length > 1)
            {
                city = pathValues[1];
            }

            if (city == "")
            {
                return new List<DateTime>() { dateToSelect };
            }

            var wayData = jsFileContent.TrimMiddle("waysArray=", "];") + "]";
            var wayDataDeserialized = jss.Deserialize<dynamic>(wayData);
            foreach (Dictionary<string, object> wayInfo in wayDataDeserialized)
            {
                var destination = wayInfo["option"].ToString();
                if (string.Compare("ТУРЫ на " + city, destination, new CultureInfo("ru-RU"), CompareOptions.IgnoreCase) *
                    string.Compare("ТУРЫ В " + city, destination, new CultureInfo("ru-RU"), CompareOptions.IgnoreCase) *
                    string.Compare(city, destination, new CultureInfo("ru-RU"), CompareOptions.IgnoreCase) == 0)
                {
                    dynamic targets = wayInfo["t"];
                    foreach (var target in targets)
                    {
                        pathVals.Add(target.ToString());
                    }
                }
            }

            var result = new List<DateTime>();
            var dateData = jsFileContent.TrimMiddle("matrix=", ";");
            var deserialized = jss.Deserialize<dynamic>(dateData);
            foreach (Dictionary<string, object> dateInfo in deserialized)
            {
                var dateValue = Helper.FormatDateTime(dateInfo["d"].ToString());
                if (dateValue >= dateToSelect)
                {
                    dynamic shValues = dateInfo["sh"];
                    foreach (Dictionary<string, object> shInfo in shValues)
                    {
                        if (pathVals.Contains(shInfo["s"].ToString()))
                        {
                            result.Add(dateValue);
                        }
                    }
                }
            }
            return result;
        }
    }

}
