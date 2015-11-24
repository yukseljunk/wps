using System;
using System.Text;
using System.Web.Script.Serialization;

namespace PttLib.Helpers.XmlConverters
{
    public class JSONArrayToXmlConverter : IXmlConverter
    {
        public string ToXml(string dataBlockArray)
        {
            var jss = new JavaScriptSerializer();
            var htmlResult = new StringBuilder("<d><hs>");
            var jsonValue = jss.Deserialize<dynamic>(dataBlockArray);

            foreach (var item in jsonValue)
            {
                htmlResult.Append("<h>");
                var subItemIndex = 0;
                foreach (var subitem in item)
                {
                    subItemIndex++;
                    if (subitem is string)
                    {
                        htmlResult.Append("<i key='_" + subItemIndex + "'>");
                        htmlResult.Append(subitem);
                        htmlResult.Append("</i>");

                        continue;
                    }

                    htmlResult.Append("<i key='" + subitem.Key + "'>");
                    try
                    {
                        if (subitem.Value == null)
                        {
                            htmlResult.Append("</i>");
                            continue;
                        }

                        Type valueType = subitem.Value.GetType();
                        if (valueType.IsGenericType && valueType.Name.StartsWith("Dictionary"))
                        {
                            foreach (var subsubitem in subitem.Value)
                            {
                                htmlResult.Append("<s key='" + subsubitem.Key + "'>");
                                if (subsubitem.Value != null) htmlResult.Append(subsubitem.Value);
                                htmlResult.Append("</s>");
                            }
                        }
                        else
                        {
                            htmlResult.Append(subitem.Value);
                        }

                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }

                    htmlResult.Append("</i>");

                }
                htmlResult.Append("</h>");
            }

            htmlResult.Append("</hs></d>");
            return htmlResult.ToString();


        }
    }
}