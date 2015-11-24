using System.Text;
using HtmlAgilityPack;

namespace PttLib.Helpers.XmlConverters
{
    public class BibloToXmlConverter : IXmlConverter
    {
        private readonly bool _includeOpenCloseTags;

        public BibloToXmlConverter():this(true)
        {
            
        }
        public BibloToXmlConverter(bool includeOpenCloseTags)
        {
            _includeOpenCloseTags = includeOpenCloseTags;
        }

        public string ToXml(string dataBlockArray)
        {
            if(dataBlockArray.StartsWith("<h>"))
            {
                return "<d><hs>" + dataBlockArray + "</hs></d>";
            }

            var htmlResult = new StringBuilder();
            if(_includeOpenCloseTags) htmlResult.Append("<d><hs>");
            var html = new HtmlDocument();
            html.LoadHtml(dataBlockArray);
            var hotelLabels = html.DocumentNode.SelectNodes("//div[@id='hot']//input[@type='checkbox' and @name='F4']");
            if (hotelLabels == null) return null;
            foreach (var node in hotelLabels)
            {
                
                var hotelId = node.Attributes["value"].Value;
                if (string.IsNullOrEmpty(hotelId)) continue;
                var hotelName = "";
                var labelNode = node.ParentNode.SelectNodes("label");
                if (labelNode != null)
                {
                    hotelName = labelNode[0].ChildNodes[0].InnerText.Replace("&nbsp;", "").Trim();
                }
                else
                {
                    var spanNodes = node.ParentNode.SelectNodes("span");
                    if (spanNodes != null)
                    {
                        foreach (var spanNode in spanNodes)
                        {
                            if (!string.IsNullOrEmpty(spanNode.InnerText))
                            {
                                hotelName = spanNode.InnerText.Replace("&nbsp;", "").Trim();
                            }
                        }
                    }
                }
                if (hotelName == "") continue;
                htmlResult.Append("<h>");
                htmlResult.Append("<i key='hotelName'>");
                htmlResult.Append(hotelName);
                htmlResult.Append("</i><i key='hotelId'>");
                htmlResult.Append(hotelId);
                htmlResult.Append("</i></h>");
            }
            if (_includeOpenCloseTags)  htmlResult.Append("</hs></d>");
            return htmlResult.ToString();

        }
    }
}