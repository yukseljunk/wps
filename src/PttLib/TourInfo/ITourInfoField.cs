using System.Collections.Generic;

namespace PttLib.TourInfo
{
    public interface ITourInfoField
    {
        string Name { get; set; }
        string XPath { get; set; }
        string Regex { get; set; }
        IList<ITourInfoConverter> PreHtmlConverters { get; set; }
        IList<ITourInfoConverter> PreConverters { get; set; }
        IList<ITourInfoConverter> PostConverters { get; set; }
        ITourInfoField Clone();
    }
}