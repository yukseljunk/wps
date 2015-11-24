using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace PttLib.TourInfo
{
    public class TourInfo:ITourInfo
    {
        public string Name { get; set; }
        public string XPath { get; set; }
        public IList<ITourInfoField> Fields { get; set; }
        public IList<ITourInfoConverter> PreHtmlConvertersForAllFields { get; set; }
        public IList<ITourInfoConverter> PreConvertersForAllFields { get; set; }
        public IList<ITourInfoConverter> PostConvertersForAllFields { get; set; }

        public ITourInfo Clone()
        {
            var clone = new TourInfo();
            clone.Name = this.Name;
            clone.XPath = this.XPath;
            clone.Fields=new List<ITourInfoField>();
            foreach (var tourInfoField in Fields)
            {
                clone.Fields.Add(tourInfoField.Clone());
            }
            clone.PreConvertersForAllFields= new List<ITourInfoConverter>();
            foreach (var convertersForAllField in PreConvertersForAllFields)
            {
                clone.PreConvertersForAllFields.Add(convertersForAllField.Clone());
            }
            clone.PreHtmlConvertersForAllFields = new List<ITourInfoConverter>();
            foreach (var convertersForAllField in PreHtmlConvertersForAllFields)
            {
                clone.PreHtmlConvertersForAllFields.Add(convertersForAllField.Clone());
            }
            clone.PostConvertersForAllFields = new List<ITourInfoConverter>();
            foreach (var convertersForAllField in PostConvertersForAllFields)
            {
                clone.PostConvertersForAllFields.Add(convertersForAllField.Clone());
            }
            return clone;
        }

        public T GetFieldValue<T>(HtmlNode node, string fieldName)
        {
            var tourInfoField = Fields.FirstOrDefault(f => f.Name == fieldName);
            if (tourInfoField == null) return default(T);

            var fieldNode = node.SelectSingleNode(tourInfoField.XPath);
            if (fieldNode == null) return default(T);
            var initialInnerHtml = fieldNode.InnerHtml;
            foreach (var converter in PreHtmlConvertersForAllFields)
            {
                fieldNode.InnerHtml = converter.Convert(fieldNode.InnerHtml) as string;
            }
            foreach (var converter in tourInfoField.PreHtmlConverters)
            {
                fieldNode.InnerHtml = converter.Convert(fieldNode.InnerHtml) as string;
            }

            object d = fieldNode.InnerText;
            fieldNode.InnerHtml = initialInnerHtml;
            foreach (var preConvertersForAllField in PreConvertersForAllFields)
            {
                d = preConvertersForAllField.Convert(d);
            }
            foreach (var preConverter in tourInfoField.PreConverters)
            {
                d = preConverter.Convert(d);
            }

            if (!string.IsNullOrEmpty(tourInfoField.Regex))
            {
                var m = Regex.Match(d.ToString(), tourInfoField.Regex, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    d = m.Groups[1].Value;
                }
            }
            foreach (var postConverter in tourInfoField.PostConverters)
            {
                d = postConverter.Convert(d);
            }
            foreach (var postConvertersForAllField in PostConvertersForAllFields)
            {
                d = postConvertersForAllField.Convert(d);
            }
            return (T)d;
        }

    }
}
