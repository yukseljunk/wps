using System.Text;
using HtmlAgilityPack;

namespace PttLib.Helpers.XmlConverters
{
    public class VersaToXmlConverter : IXmlConverter
    {
        public string ToXml(string dataBlockArray)
        {
            var htmlResult = new StringBuilder("<d><hs>");
            var html = new HtmlDocument();
            html.LoadHtml(dataBlockArray);
            
            var hotelLabels = html.DocumentNode.SelectNodes("//div[@name='HOTELS']//label");
            if (hotelLabels == null) return null;
            foreach (var node in hotelLabels)
            {
                var hotelId = node.ChildNodes[0].Attributes["value"].Value;
                if (string.IsNullOrEmpty(hotelId)) continue;
                htmlResult.Append("<h>");
                var hotelName = node.ChildNodes[1].InnerText;
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