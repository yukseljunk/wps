using HtmlAgilityPack;
using PttLib.Helpers;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;

namespace PttLib.Operators.Operator
{
    class Polar : OperatorBase
    {
        const string OperatorName = "POLAR";
        private const string HOTEL_LIST_START_TOKEN = "add_hotels(";
        private const string HOTEL_LIST_END_TOKEN = ");";

        public Polar()
        {
            AllHotelsXmlConverter = new JSONArrayToXmlConverter();
        }

        public override IRequestResponse RequestResponseBehavior
        {
            get { return new RequestResponseWithoutCaptcha(RetryNeededForRequest, TryAgainInException); }

        }

        #region Overrides of OperatorBase

        protected override string RefineHotelNames(string hotelsHtml)
        {
            return hotelsHtml.TrimMiddle(HOTEL_LIST_START_TOKEN, HOTEL_LIST_END_TOKEN);

        }

        public override string Refine(IPttRequest pttRequest, string htmlSource)
        {
            return htmlSource.TrimMiddle("samo.jQuery(samo.controls.resultset).ehtml('", "');samo.notify").Replace("\\", "");
        }

        protected override void ParseHotelNameAndIdFromNode(HtmlNode htmlNode, out string hotelName, out string hotelId)
        {
            hotelName = htmlNode.SelectNodes(".//i[@key='name']")[0].InnerText;
            hotelId = htmlNode.SelectNodes(".//i[@key='id']")[0].InnerText;
        }

        public override string Name
        {
            get
            {
                return OperatorName;
            }
        }

        #endregion
    }
}