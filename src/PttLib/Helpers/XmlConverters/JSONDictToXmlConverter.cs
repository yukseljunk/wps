using System.Text;
using System.Web.Script.Serialization;

namespace PttLib.Helpers.XmlConverters
{
    public class JSONDictToXmlConverter : IXmlConverter
    {
        public string ToXml(string dataBlockArray)
        {
            var htmlResult = new StringBuilder("<d><hs>");
            var jss = new JavaScriptSerializer();
            if (string.IsNullOrEmpty(dataBlockArray)) return null;
            var dict = jss.Deserialize<dynamic>(dataBlockArray);
            var hotels = dict["hotels"];
            foreach (var hotel in hotels)
            {
                htmlResult.Append("<h>");
                var hotelName = hotel["name"].ToString();
                var hotelId = hotel["hotelId"].ToString();
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