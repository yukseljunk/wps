using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PttLib.Helpers;
using PttLib.TourInfo;

namespace PttLib
{
    public class Item
    {
        public int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string MetaDescription { get; set; }
        public virtual string Content { get; set; }
        public virtual IList<string> Tags { get; set; }
        public virtual IList<ItemImage> ItemImages { get; set; }
        public virtual double Price { get; set; }
        public virtual string Site { get; set; }
        public virtual string Url { get; set; }
        public int WordCount { get; set; }
        public int PostId { get; set; }
        public int Order { get; set; }
        public DateTime Created { get; set; }
        public int Relevance { get; set; }
        public virtual string ForeignKey
        {
            get { return Site + "_" + Id; }
        }

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
                string.Join(",", ItemImages),
                Price,
                Url,
                Site);
        }

        public virtual string PostBody(int thumbnailSize, bool includePriceAndSource = true, bool tagsAsText = true)
        {
            var converterFunctions = new ConverterFunctions();
            var content = new StringBuilder("");
            if (ItemImages.Count > 0)
            {
                content.Append(string.Format("<div style=\"width: {0}px; margin-right: 10px;\">", (2 * thumbnailSize + 30)));
                foreach (var itemImage in ItemImages)
                {
                    content.Append(string.Format(
                        "<div style=\"width: {3}px; height: {3}px; float: left; margin-right: 15px; margin-bottom: 3px;\"><a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" title=\"{2}\" /></a></div>",
                        itemImage.Link, itemImage.NewSource, Title, thumbnailSize));

                }
                content.Append("</div>");

            }

            if (((int)(Price * 100)) > 0 && includePriceAndSource)
            {
                content.Append(string.Format("<h4>Price:${0}</h4>", Price));
            }
            content.Append(string.Format("<h2>{0}</h2>", Title));
            content.Append(converterFunctions.ArrangeContent(Content));
            if (!string.IsNullOrEmpty(Url) && includePriceAndSource)
            {
                content.Append(SourceStatement());
            }

            if (Tags != null && Tags.Count > 0 && tagsAsText && includePriceAndSource)
            {
                content.Append("<br/>Tags: ");
                content.Append(string.Join(", ", Tags));
            }
            return content.ToString();

        }

        public string SourceStatement()
        {
            var content = new StringBuilder();
            content.Append("<br><strong>Source:</strong> <a href=\"");
            content.Append(Url);
            content.Append("\" rel=\"nofollow\" target=\"_blank\">");
            var randomWordCount = Helper.GetRandomNumber(3, 8);
            var titleWordCount = Title.WordCount();
            if (titleWordCount < randomWordCount)
            {
                randomWordCount = titleWordCount;
            }
            var firstWords = string.Join(" ", Title.Split().Take(randomWordCount));
            content.Append(firstWords);
            content.Append("</a>");
            return content.ToString();
        }

        public bool IsInvalid
        {
            get
            {
                return ItemImages.Count == 0 || string.IsNullOrWhiteSpace(Title.Trim()) ||
                       string.IsNullOrWhiteSpace(Content.Trim());
            }
        }

    }
}