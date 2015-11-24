using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace PttLib.Helpers.XmlConverters
{
    public class JSONArrayToXmlConverterRecursively : IXmlConverter
    {
        private Dictionary<int, string> _values;

        public string ToXml(string dataBlockArray)
        {
            var jss = new JavaScriptSerializer();
            
            var jsonValue = jss.Deserialize<dynamic>(dataBlockArray);
            _values = new Dictionary<int, string>();
            foreach (var item in jsonValue)
            {
                foreach (var subitem in item.Value)
                {
                    ParseHotelRecursively(subitem);
                }
            }

            var htmlResult = new StringBuilder("<d><hs>");
            foreach (KeyValuePair<int,string> subItemChildHotel in _values)
            {
                htmlResult.Append("<h>");

                htmlResult.Append("<i key='hotelName'>");
                htmlResult.Append(subItemChildHotel.Value);
                htmlResult.Append("</i>");
                
                htmlResult.Append("<i key='hotelId'>");
                htmlResult.Append(subItemChildHotel.Key);
                htmlResult.Append("</i>");

                htmlResult.Append("</h>");
            }

            htmlResult.Append("</hs></d>");
            return htmlResult.ToString();
        }

        
        private void ParseHotelRecursively(dynamic input)
        {
            if (input["HOTELS"] != null)
            {
                foreach (var hotelItem in input["HOTELS"])
                {
                    _values.Add(hotelItem["KEY"], hotelItem["VALUE"]);
                }
            }

            if (input["CHILDS"] == null) return;
            
            foreach (var childItem in input["CHILDS"])
            {
                ParseHotelRecursively(childItem);
            }
        }
    }
}