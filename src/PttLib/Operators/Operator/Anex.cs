using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PttLib.CaptchaBreaker.Dbc;
using PttLib.Helpers;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;
using PttLib.PttRequestResponse.CaptchaRequestResponse;

namespace PttLib.Operators.Operator
{
    abstract class AnexBase:OperatorBase
    {
        protected const string TOUR_LIST_START_TOKEN = ".ehtml('";
        protected const string TOUR_LIST_END_TOKEN = "');";

        #region Overrides of OperatorBase

        public abstract override string Name { get; }

        public override IRequestResponse RequestResponseBehavior
        {
            get
            {
                return new RequestResponseWithCaptcha(RetryNeededForRequest, TryAgainInException, new AnexCaptcha(), new DeathByCaptchaBreaker(), ExtensiveLoggingNeeded);
            }
        }

        public override Tuple<int, int> TryAgainInException
        {
            get { return new Tuple<int, int>(3, 5); }
        }

        public override string Refine(IPttRequest pttRequest, string htmlSource)
        {
            return htmlSource.TrimMiddle(TOUR_LIST_START_TOKEN, TOUR_LIST_END_TOKEN).Replace("\\\"", "\"").Replace("\\n\\", "");
        }


        #endregion
    }

    class Anex_ : AnexBase
    {
        const string OperatorName = "ANEX_";

        public Anex_()
        {
            AllHotelsXmlConverter = new JSONArrayToXmlConverter();
        }

        #region Overrides of OperatorBase

        public override bool HarvestSingleThread
        {
            get
            {
                return true;
            }
        }

        protected override bool RetryNeededForRequest(string result)
        {

            if(result.Contains("Page 1 of"))
            {
                Logger.LogProcess("ANEX_ Most Probably IP BLOCKED");
                return true;
            }
            return false;
        }
        public override List<List<string>> MealTypes
        {
            get
            {
                return new List<List<string>>(){
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

        public override string Name
        {
            get
            {
                return OperatorName;
            }
        }

        protected override string RefineHotelNames(string hotelsHtml)
        {
            return hotelsHtml.TrimMiddle("{\"d\":\"", "\"}").Replace("\\\"", "\"").Replace("\\\\", "\\");
        }


        protected override void ParseHotelNameAndIdFromNode(HtmlNode htmlNode, out string hotelName, out string hotelId)
        {
            hotelName = htmlNode.SelectNodes(".//i[@key='Name']")[0].InnerText;
            hotelId = htmlNode.SelectNodes(".//i[@key='HotelCode']")[0].InnerText;
        }

        protected override void FillSessionJar(IPttRequest request, int requestCounter)
        {
            if (requestCounter != 1)
            {
                if (ExtensiveLoggingNeeded)
                {
                    Logger.LogProcess("FillSessionJar requestCounter less than 2");
                }
                return;
            }
            if (string.IsNullOrEmpty(request.Response))
            {
                if (ExtensiveLoggingNeeded)
                {
                    Logger.LogProcess("FillSessionJar request.Response empty or null");
                }

                return;
            }
            var match = Regex.Match(request.Response, @"url\:[ ]*""Price\/(GetPrice\d+\.aspx)", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                if (ExtensiveLoggingNeeded)
                {
                    Logger.LogProcess("FillSessionJar regex do not match");
                }
                return;
            }
            if (ExtensiveLoggingNeeded)
            {
                Logger.LogProcess("FillSessionJar parsed price page name: " + match.Groups[1].Value);
            }
            request.SessionJar.AddOrUpdate("PricePage", match.Groups[1].Value, (k, v) => match.Groups[1].Value);
        }
        
        #endregion
        }

    class Anex : AnexBase
        {
        const string OperatorName = "ANEX";

        public Anex()
            {
            AllHotelsXmlConverter = new SelectBoxToXmlConverter("//div[@name='HOTELS']", 0, 2, AnexHotelIdNode);
        }

        protected string AnexHotelIdNode(HtmlNode refnode, int nodeindex)
            {
            return refnode.ChildNodes[nodeindex + 1].ChildNodes[0].Attributes["value"].Value;
    }

        #region Overrides of OperatorBase


        public override string Name
        {
            get
            {
                return OperatorName;
                }
                }


        protected override void ParseHotelNameAndIdFromNode(HtmlNode htmlNode, out string hotelName, out string hotelId)
                {
            hotelName = htmlNode.SelectNodes(".//i[@key='hotelName']")[0].InnerText;
            hotelId = htmlNode.SelectNodes(".//i[@key='hotelId']")[0].InnerText;
                }

        #endregion
    }

}