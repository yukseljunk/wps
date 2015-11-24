using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using PttLib.Helpers;

namespace PttLib.TourInfo
{
    public class TourInfoFactory:ITourInfoFactory
    {
        public ITourInfo Deserialize(string serialized)
        {
            var html = new HtmlDocument();
            html.LoadHtml(serialized.Replace("\r\n", ""));
            var requestNodes = html.DocumentNode.SelectNodes("tourinfo");
            if (requestNodes == null) return null;
            ITourInfo tourInfo;
            return GetSingleTourInfo(requestNodes.FirstOrDefault(), out tourInfo) ? tourInfo : null;
        }

        private static bool GetSingleTourInfo(HtmlNode tourInfoNode, out ITourInfo tourInfo)
        {
            var name = XmlParse.GetStringNodeValue(tourInfoNode, "name", "");
            var xpath = XmlParse.GetStringNodeValue(tourInfoNode, "xpath", "");
            IList<ITourInfoConverter> tourInfoPreConventersForAll = new List<ITourInfoConverter>();
            var convertersForAll = XmlParse.GetStringListNodeValue(tourInfoNode, "allfields/preconverters/converter");
            if (convertersForAll != null)
            {
                foreach (var converter in convertersForAll)
                {
                    tourInfoPreConventersForAll.Add(new TourInfoConverter(converter));
                }
            }

            IList<ITourInfoConverter> tourInfoPreHtmlConventersForAll = new List<ITourInfoConverter>();
            convertersForAll = XmlParse.GetStringListNodeValue(tourInfoNode, "allfields/prehtmlconverters/converter");
            if (convertersForAll != null)
            {
                foreach (var converter in convertersForAll)
                {
                    tourInfoPreHtmlConventersForAll.Add(new TourInfoConverter(converter));
                }
            }

            IList<ITourInfoConverter> tourInfoPostConventersForAll = new List<ITourInfoConverter>();
            convertersForAll = XmlParse.GetStringListNodeValue(tourInfoNode, "allfields/postconverters/converter");
            if (convertersForAll != null)
            {
                foreach (var converter in convertersForAll)
                {
                    tourInfoPostConventersForAll.Add(new TourInfoConverter(converter));
                }
            }
            IList<ITourInfoField> fields = new List<ITourInfoField>();
            var fieldNodes = tourInfoNode.SelectNodes("fields/field");
            if (fieldNodes != null)
            {
                foreach (var fieldNode in fieldNodes)
                {
                    var field = new TourInfoField();
                    field.Name = XmlParse.GetStringNodeValue(fieldNode, "name", "");
                    field.XPath = XmlParse.GetStringNodeValue(fieldNode, "xpath", "");
                    field.Regex = XmlParse.GetStringNodeValue(fieldNode, "regex", "");
                    var preconverters = XmlParse.GetStringListNodeValue(fieldNode, "preconverters/converter");
                    var tourInfoPreConventers = new List<ITourInfoConverter>();
                    if (preconverters != null)
                    {
                        foreach (var converter in preconverters)
                        {
                            tourInfoPreConventers.Add(new TourInfoConverter(converter));
                        }
                    }

                    var preHtmlconverters = XmlParse.GetStringListNodeValue(fieldNode, "prehtmlconverters/converter");
                    var tourInfoPreHtmlConverters = new List<ITourInfoConverter>();
                    if (preHtmlconverters != null)
                    {
                        foreach (var converter in preHtmlconverters)
                        {
                            tourInfoPreHtmlConverters.Add(new TourInfoConverter(converter));
                        }
                    }

                    var postconverters = XmlParse.GetStringListNodeValue(fieldNode, "postconverters/converter");
                    var tourInfoPostConventers = new List<ITourInfoConverter>();
                    if (postconverters != null)
                    {
                        foreach (var converter in postconverters)
                        {
                            tourInfoPostConventers.Add(new TourInfoConverter(converter));
                        }
                    }
                    field.PreHtmlConverters = tourInfoPreHtmlConverters;
                    field.PreConverters = tourInfoPreConventers;
                    field.PostConverters = tourInfoPostConventers;
                    fields.Add(field);
                }

            }
            tourInfo = new TourInfo() { Name = name, 
                    XPath = xpath, 
                    PreConvertersForAllFields = tourInfoPreConventersForAll, 
                    PreHtmlConvertersForAllFields=tourInfoPreHtmlConventersForAll, 
                    PostConvertersForAllFields = tourInfoPostConventersForAll, 
                    Fields = fields };
            return true;
        }
    }
}

