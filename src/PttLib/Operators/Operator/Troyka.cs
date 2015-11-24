using System;
using HtmlAgilityPack;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;

namespace PttLib.Operators.Operator
{
    internal class Troyka : OperatorBase
    {
        public override string Name
        {
            get { return "TROYKA"; }
        }

        public override Tuple<int, int> TryAgainInException
        {
            get { return new Tuple<int, int>(3, 5); }
        }

        public Troyka()
        {   
            AllHotelsXmlConverter= new JSONArrayToXmlConverter();
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
                var dayCount = (QueryObject.EndDate - QueryObject.StartDate).Days;
                if (dayCount >= 90)
                {
                    return "Troyka 90 gun ve ustu veri yollamiyor!";
                }
                return null;
            }
        }

        protected override void ParseHotelNameAndIdFromNode(HtmlNode htmlNode, out string hotelName, out string hotelId)
        {
            hotelName = htmlNode.SelectNodes(".//i[@key='name']")[0].InnerText + " " + htmlNode.SelectNodes(".//i[@key='hotelCategory']")[0].InnerText;
            hotelId = htmlNode.SelectNodes(".//i[@key='id']")[0].InnerText;
        }

        public override string Refine(IPttRequest pttRequest, string htmlSource)
        {
            var arrayToXmlConverter = new JSONArrayToXmlConverter();
            return arrayToXmlConverter.ToXml(htmlSource);
        }
    }
}