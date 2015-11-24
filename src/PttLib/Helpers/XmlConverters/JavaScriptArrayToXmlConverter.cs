namespace PttLib.Helpers.XmlConverters
{
    public class JavaScriptArrayToXmlConverter:IXmlConverter
    {
        private readonly string _wrapper;
        private readonly string _itemSeparator;

        public JavaScriptArrayToXmlConverter(string wrapper="\"", string itemSeparator=",")
        {
            _wrapper = wrapper;
            _itemSeparator = itemSeparator;
        }

        #region Implementation of IXmlConverter

        public string ToXml(string dataBlockArray)
        {
            var dataBlock = dataBlockArray;
            dataBlock = dataBlock.Replace("[[", "<hs><h>");
            dataBlock = dataBlock.Replace("]]", "</h></hs>");
            dataBlock = dataBlock.Replace("],[", "</h><h>");

            dataBlock = dataBlock.Replace("<h>" + _wrapper, "<h><i>");
            dataBlock = dataBlock.Replace(_wrapper + "</h>", "</i></h>");
            dataBlock = dataBlock.Replace(_wrapper + _itemSeparator + _wrapper, "</i><i>");
            dataBlock = "<d>" + dataBlock + "</d>";
            return dataBlock;
        }

        #endregion
    }
}