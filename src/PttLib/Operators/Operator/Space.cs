using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;
using PttLib.Tours;

namespace PttLib.Operators.Operator
{
    class Space : OperatorBase
    {
        private const string TOUR_OPERATOR = "SPACE";

        public override string Name
        {
            get { return TOUR_OPERATOR; }
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
                                                  new List<string>(){"UAI"},
                                                  new List<string>(){"OTHER"}
                                              };
            }
        }

        public Space()
        {
            AllHotelsXmlConverter = new SpaceToXmlConverter();
        }

        protected override void ParseHotelNameAndIdFromNode(HtmlNode htmlNode, out string hotelName, out string hotelId)
        {
            hotelName = htmlNode.SelectNodes(".//i[@key='hotelName']")[0].InnerText;
            hotelId = htmlNode.SelectNodes(".//i[@key='hotelId']")[0].InnerText;
        }

        public override ITourFactory TourFactory
        {
            get
            {
                return new TourFactory(BeforeIteratingWhileGettingTours, IteratingWhileGettingToursBeforeTourItemSet, IteratingWhileGettingToursAfterTourItemSet);
            }
        }

        private TourFactoryControlFlags BeforeIteratingWhileGettingTours(IPttRequest request, HtmlDocument htmlDocument)
        {
            var totalPageCountNode = htmlDocument.DocumentNode.SelectNodes("//span['_Pager2_LbHeader' = substring(@id, string-length(@id) - 15)]");
            var totalPageCount = 1;
            if (totalPageCountNode != null && totalPageCountNode.Any())
            {
                var match = Regex.Match(totalPageCountNode[0].InnerText, @"(\d)", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string pageCount = match.Groups[1].Value;
                    Int32.TryParse(pageCount, out totalPageCount);
                }
            }
            return totalPageCount == PageIndex - 1 ? TourFactoryControlFlags.MoveToNextHotel : TourFactoryControlFlags.Pass;
        }

        private TourFactoryControlFlags IteratingWhileGettingToursBeforeTourItemSet(IPttRequest request, HtmlNode node)
        {
            if(node.ParentNode.SelectNodes("tr").FirstOrDefault() == node) return TourFactoryControlFlags.Continue;
            return node.ChildNodes.Count() < 12 ? TourFactoryControlFlags.Continue : TourFactoryControlFlags.Pass;
        }

        private TourFactoryControlFlags IteratingWhileGettingToursAfterTourItemSet(IPttRequest request, Tour tourItem)
        {
            return (tourItem.Price >= QueryObject.MinPrice && tourItem.Price <= QueryObject.MaxPrice) &&
                   (Int32.Parse(tourItem.Night) >= QueryObject.MinNights &&
                    Int32.Parse(tourItem.Night) <= QueryObject.MaxNights)
                ? TourFactoryControlFlags.Pass
                : TourFactoryControlFlags.Continue;
        }
    }
}