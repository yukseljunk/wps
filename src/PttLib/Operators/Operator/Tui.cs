using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PttLib.Helpers;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;
using PttLib.Tours;

namespace PttLib.Operators.Operator
{
    public class Tui : OperatorBase
    {
        private string DATA_START_TOKEN = "<table class=\"searchResultTable\"";
        private string DATA_END_TOKEN = "<table id=\"searchLabel\"";

        private string HOTEL_LIST_DATA_START_TOKEN = "<table id=\"ctl00_generalContent_QuotedDynamicControl_DynamicOffersFilter_chklHotel\"";
        private string HOTEL_LIST_DATA_END_TOKEN = "</table>";

        private const string TOUR_OPERATOR = "TUI";

        public Tui()
        {
            AllHotelsXmlConverter = new SelectBoxToXmlConverter("table", 0, 1, TuiHotelIdNode, TuiHotelNameNode);
        }

        private string TuiHotelNameNode(HtmlNode refnode, int nodeindex)
        {
            return refnode.ChildNodes[nodeindex].InnerText.Trim();
        }

        protected string TuiHotelIdNode(HtmlNode refnode, int nodeindex)
        {
            var input = refnode.ChildNodes[nodeindex].SelectSingleNode(".//input");
            if (input == null) return "";
            var idAttr = input.Attributes["id"];
            if (idAttr == null) return "";
            var partsByUnderscore = idAttr.Value.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var idValue = partsByUnderscore.Last();
            int idInt;
            var conversion = int.TryParse(idValue, out idInt);
            return conversion ? (idInt + 1).ToString() : idValue;
        }

        protected override void ParseHotelNameAndIdFromNode(HtmlNode htmlNode, out string hotelName, out string hotelId)
        {
            hotelName = htmlNode.SelectNodes(".//i[@key='hotelName']")[0].InnerText;
            hotelId = htmlNode.SelectNodes(".//i[@key='hotelId']")[0].InnerText;
        }

        public override List<List<string>> MealTypes
        {
            get
            {
                return new List<List<string>>()
                                              {
                                                  new List<string>(){"ULTRA ALL INCLUSIVE"},
                                                  new List<string>(){"ROOM ONLY"},
                                                  new List<string>(){"BED AND BREAKFAST"},
                                                  new List<string>(){"HALFBOARD"},
                                                  new List<string>(){"FULLBOARD"},
                                                  new List<string>(){"ALL INCLUSIVE"},
                                                  new List<string>(){"UAI"},
                                                  new List<string>(){"OTHER"}
                                              };
            }
        }

        public override string Name
        {
            get { return TOUR_OPERATOR; }
        }

        public override string Refine(IPttRequest pttRequest, string htmlSource)
        {
            return htmlSource.TrimMiddle(DATA_START_TOKEN, DATA_END_TOKEN, true).Replace("\r", "").Replace("\n", "").Replace("\t", "");
        }

        protected override string RefineHotelNames(string hotelsHtml)
        {
            return hotelsHtml.TrimMiddle(HOTEL_LIST_DATA_START_TOKEN, HOTEL_LIST_DATA_END_TOKEN, true).Replace("\r", "").Replace("\n", "").Replace("\t", "");
        }

        public override ITourFactory TourFactory
        {
            get
            {
                return new TourFactory(null, IteratingWhileGettingToursBeforeTourItemSet, IteratingWhileGettingToursAfterTourItemSet);
            }
        }

        private TourFactoryControlFlags IteratingWhileGettingToursAfterTourItemSet(IPttRequest request, Tour tourItem)
        {
            if (!request.SessionJar.ContainsKey("EurRate")) return TourFactoryControlFlags.Pass;
            var rate = (double)request.SessionJar["EurRate"];
            tourItem.Price = Math.Round(tourItem.Price / (double)rate, MidpointRounding.AwayFromZero);

            return tourItem.Price > QueryObject.MaxPrice ? TourFactoryControlFlags.Continue : TourFactoryControlFlags.Pass;
        }

        protected override void FillSessionJar(IPttRequest request, int requestCounter)
        {
            if (requestCounter != 2) return;
            if (string.IsNullOrEmpty(request.Response)) return;

            double rate = 0.0;

            var html = new HtmlDocument();
            html.LoadHtml(request.Response);
            var curr = html.DocumentNode.SelectSingleNode("//span[@class='eurRate']");
            if (curr == null) return;

            var match = Regex.Match(curr.InnerText, @"(\d+),(\d+)", RegexOptions.IgnoreCase);
            if (!match.Success) return;

            var integerPart = match.Groups[1].Value;
            var floatingPart = match.Groups[2].Value;
            Double.TryParse(integerPart + "." + floatingPart, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out rate);

            request.SessionJar.AddOrUpdate("EurRate", rate, (k, v) => rate);
        }

        private TourFactoryControlFlags IteratingWhileGettingToursBeforeTourItemSet(IPttRequest request, HtmlNode node)
        {
            if (node.ParentNode.SelectNodes("tr").FirstOrDefault() == node) return TourFactoryControlFlags.Continue;
            if (node.ChildNodes.Count() < 12) return TourFactoryControlFlags.Continue;
            var hiddenNodes = node.SelectNodes(".//input[@type='hidden']");
            var hiddenPrices = new StringBuilder();
            if (hiddenNodes != null)
            {
                foreach (var hiddenNode in hiddenNodes)
                {
                    var nameAttr = hiddenNode.Attributes["name"];
                    var valueAttr = hiddenNode.Attributes["value"];
                    if (nameAttr != null && valueAttr != null)
                    {
                        hiddenPrices.Append("&");
                        hiddenPrices.Append(nameAttr.Value);
                        hiddenPrices.Append("=");
                        hiddenPrices.Append(valueAttr.Value);
                    }
                }
            }
            request.SessionJar.AddOrUpdate("HiddenPrices", hiddenPrices.ToString(), (k, v) => v + hiddenPrices.ToString());
            return TourFactoryControlFlags.Pass;
        }
    }
}