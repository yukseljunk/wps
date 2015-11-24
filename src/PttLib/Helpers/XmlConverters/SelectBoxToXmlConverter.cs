using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace PttLib.Helpers.XmlConverters
{
    public delegate string HotelNameNodeSelector(HtmlNode refNode, int nodeindex);
    public delegate string HotelIdNodeSelector(HtmlNode refNode, int nodeindex);


    public class SelectBoxToXmlConverter : IXmlConverter
    {
        private readonly string _xpath;
        private readonly int _optionItemStart;
        private readonly int _optionItemStep;
        private readonly HotelNameNodeSelector _nameNodeSelector;
        private readonly HotelIdNodeSelector _idNodeSelector;

        public SelectBoxToXmlConverter(string xpath=null,int optionItemStart=1, int optionItemStep=2, HotelIdNodeSelector idNodeSelector=null, HotelNameNodeSelector nameNodeSelector=null)
        {
            _xpath = xpath;
            _optionItemStart = optionItemStart;
            _optionItemStep = optionItemStep;
            _nameNodeSelector = nameNodeSelector ?? DefaultNameNodeSelector;
            _idNodeSelector = idNodeSelector ?? DefaultHotelIdNode;
        }

        private string DefaultNameNodeSelector(HtmlNode refnode, int nodeindex)
        {
            return HttpUtility.HtmlDecode(refnode.ChildNodes[nodeindex + 1].InnerText).Trim();
        }

        private string DefaultHotelIdNode(HtmlNode refnode, int nodeindex)
        {
            return refnode.ChildNodes[nodeindex].Attributes["value"].Value;
        }

        public string ToXml(string dataBlockArray)
        {
            var htmlResult = new StringBuilder("<d><hs>");
            var html = new HtmlDocument();
            html.LoadHtml(dataBlockArray);
            var refNode = html.DocumentNode;
            if (!string.IsNullOrEmpty(_xpath))
            {
                refNode = html.DocumentNode.SelectSingleNode(_xpath);
            }
            if (refNode == null) return null;
            if (refNode.ChildNodes == null || refNode.ChildNodes.Count == 0)
            {
                return null;
            }
            for (var nodeIndex = _optionItemStart; nodeIndex < refNode.ChildNodes.Count-1; nodeIndex += _optionItemStep)
            {
                htmlResult.Append("<h>");
                //var hotelName = HttpUtility.HtmlDecode(refNode.ChildNodes[nodeIndex + 1].InnerText).Trim();
                var hotelName = _nameNodeSelector(refNode, nodeIndex);
                //var hotelId = refNode.ChildNodes[nodeIndex].Attributes["value"].Value;
                var hotelId = _idNodeSelector(refNode, nodeIndex);
                htmlResult.Append("<i key='hotelName'>");
                htmlResult.Append(hotelName);
                htmlResult.Append("</i><i key='hotelId'>");
                htmlResult.Append(hotelId);
                htmlResult.Append("</i></h>");
            }

            htmlResult.Append("</hs></d>");
            return htmlResult.ToString();

        }
    }
}