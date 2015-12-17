using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PttLib
{
    public class MultiItem : Item
    {
        private readonly IList<Item> _items;

        public MultiItem(IList<Item> items)
        {
            _items = items;
        }
        public void AddItem(Item item)
        {
            _items.Add(item);
        }
        public IList<Item> Items { get { return _items; } }

        public override string Title
        {
            get
            {
                if (_items.Count == 0) return null;
                return Items[0].Title;
            }
        }

        public override double Price
        {
            get
            {
                return 0.00;
            }
        }
        public override string MetaDescription
        {
            get
            {
                return _items.Aggregate("", (current, item) => current + (item.MetaDescription + " "));
            }
        }

        public override IList<ItemImage> ItemImages
        {
            get
            {
                var combinedImages = new List<ItemImage>();
                foreach (var item in _items)
                {
                    combinedImages.AddRange(item.ItemImages);
                }
                return combinedImages;
            }
        }
        public override string Site
        {
            get
            {
                return "";
            }
        }
        public override string Url
        {
            get
            {
                return "";
            }
        }

        public override IList<string> Tags
        {
            get
            {
                var combinedTags = new List<string>();
                foreach (var item in _items)
                {
                    combinedTags.AddRange(item.Tags);
                }
                return combinedTags;
            }
        }
        public override string ForeignKey
        {
            get
            {
                var idCombined = "";
                foreach (var item in _items)
                {
                    if (idCombined != "") idCombined += ",";
                    idCombined += item.ForeignKey;
                }
                return idCombined;
            }
        }

        public override string Content
        {
            get
            {
                var contentCombined = "";
                foreach (var item in _items)
                {
                    if (contentCombined != "") contentCombined += "<div class='next_content'></div>";
                    contentCombined += item.Content;
                }
                return contentCombined;
            }
        }

        public override string PostBody(bool includePriceAndSource = true)
        {
            var result = new StringBuilder();
            var itemIndex = 0;
            foreach (var item in _items)
            {
                if(itemIndex>0)
                {
                    result.Append("<div style='clear:both;width:500px;'></div>");
                    //result.Append(string.Format("<h2>{0}</h2><br/>",item.Title));
                }
                result.Append(item.PostBody(false));
                itemIndex++;
            }

            return result.ToString();
        }
    }
}