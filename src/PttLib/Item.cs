using System.Collections.Generic;

namespace PttLib
{
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string Content { get; set; }
        public IList<string> Tags { get; set; }
        public IList<string> Images { get; set; }
        public double Price { get; set; }
        public string Site { get; set; }
        public string Url { get; set; }
        public int WordCount { get; set; }
        public int PostId { get; set; }
        public int Order { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "Item Site:{8}\nId:{0}\nTitle:{1}\nMeta Description: {2}\nContent: {3}\nTags: {4}\nImages:{5}\nPrice:{6}\nUrl:{7}\n",
                Id,
                Title,
                MetaDescription,
                Content.Substring(0, Content.Length > 100 ? 100 : Content.Length - 1) + "...",
                string.Join(",", Tags),
                string.Join(",", Images),
                Price,
                Url,
                Site);
        }
    }
}