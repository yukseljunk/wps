using System.Text;
using HtmlAgilityPack;

namespace PttLib.Helpers.XmlConverters
{
    public class SpaceToXmlConverter : IXmlConverter
    {
        public string ToXml(string dataBlockArray)
        {
            var htmlResult = new StringBuilder("<d><hs>");
            var html = new HtmlDocument();
            html.LoadHtml(dataBlockArray);

            var hotelLabels = html.DocumentNode.SelectNodes("//div[@id='hotelDivScroll']//label");
            if (hotelLabels == null) return null;
            var hotelIndex = 1;//0 dan baslatinca match etmemis diyor, int default deger oldugu icin
            foreach (var node in hotelLabels)
            {
                htmlResult.Append("<h>");
                var hotelName = node.InnerText;
                htmlResult.Append("<i key='hotelName'>");
                htmlResult.Append(hotelName);
                htmlResult.Append("</i><i key='hotelId'>");
                htmlResult.Append(hotelIndex++);
                htmlResult.Append("</i></h>");
            }
            htmlResult.Append("</hs></d>");
            return htmlResult.ToString();

        }
    }
}