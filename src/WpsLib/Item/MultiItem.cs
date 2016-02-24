using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpsLib.Item
{
    public class MultiItem : Item
    {
        public override int Id
        {
            get
            {
                return Items[0].Id;

            }
            set { _id = value; }
        }

        public override int Order
        {
            get { return Items[0].Order; }
            set { _order = value; }
        }

        private readonly IList<Item> _items;
        private int _id;
        private int _order;

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
                for (int i = 0; i < _items.Count; i++)
                {
                    if (i == 0)
                    {
                        foreach (var itemImage in _items[i].ItemImages)
                        {
                            itemImage.Primary = true;
                        }
                    }
                    else
                    {
                        foreach (var itemImage in _items[i].ItemImages)
                        {
                            itemImage.Primary = false;
                        }
                    }
                    combinedImages.AddRange(_items[i].ItemImages);
                }
                return combinedImages;
            }
        }
        public override string Site
        {
            get
            {
                if (_items.Count == 0) return null;
                return Items[0].Site;
            }
        }
        public override string Url
        {
            get
            {
                if (_items.Count == 0) return null;
                return Items[0].Url;
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

        public override string PostBody(int thumbnailSize, bool includePriceAndSource = true, bool tagsAsText = true)
        {
            var result = new StringBuilder();
            var itemIndex = 0;
            var sourceStatement = "";
            foreach (var item in _items)
            {
                if (itemIndex > 0)
                {
                    result.Append("<div style='clear:both;width:500px;'></div>");
                }
                else
                {
                    sourceStatement = item.SourceStatement();
                }
                result.Append(item.PostBody(thumbnailSize, false));
                itemIndex++;
            }
            result.Append(sourceStatement);

            if (Tags != null && Tags.Count > 0 && tagsAsText)
            {
                result.Append("<br/>Tags: ");
                result.Append(string.Join(", ", Tags));
            }

            return result.ToString();
        }
    }
}