using System.Collections.Generic;
using HtmlAgilityPack;

namespace PttLib.TourInfo
{
    public interface ITourInfo
    {
        string Name { get; set; }
        string XPath { get; set; }
        IList<ITourInfoField> Fields { get; set; }
        IList<ITourInfoConverter> PreHtmlConvertersForAllFields { get; set; }
        IList<ITourInfoConverter> PreConvertersForAllFields { get; set; }
        IList<ITourInfoConverter> PostConvertersForAllFields { get; set; }

        ITourInfo Clone();

        T GetFieldValue<T>(HtmlNode node, string fieldName);

    }
}