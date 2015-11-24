using System.Collections.Generic;

namespace PttLib.TourInfo
{
    public class TourInfoField:ITourInfoField
    {
        public string Name { get; set; }
        public string XPath { get; set; }
        public string Regex { get; set; }
        public IList<ITourInfoConverter> PreHtmlConverters { get; set; }
        public IList<ITourInfoConverter> PreConverters { get; set; }
        public IList<ITourInfoConverter> PostConverters { get; set; }

        public ITourInfoField Clone()
        {
            var clone = new TourInfoField();
            clone.Name = this.Name;
            clone.XPath = this.XPath;
            clone.Regex = this.Regex;
            clone.PreConverters=new List<ITourInfoConverter>();
            foreach (var tourInfoConverter in PreConverters)
            {
                clone.PreConverters.Add(tourInfoConverter.Clone());
            }
            clone.PreHtmlConverters = new List<ITourInfoConverter>();
            foreach (var tourInfoConverter in PreHtmlConverters)
            {
                clone.PreHtmlConverters.Add(tourInfoConverter.Clone());
            }
            clone.PostConverters = new List<ITourInfoConverter>();
            foreach (var tourInfoConverter in PostConverters)
            {
                clone.PostConverters.Add(tourInfoConverter.Clone());
            }
            return clone;
        }

    }
}