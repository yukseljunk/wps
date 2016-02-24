namespace WpsLib.Item
{
    public class ItemImage
    {
        public string OriginalSource { get; set; }
        public string NewSource { get; set; }
        public string Link { get; set; }
        public Item ContainingItem { get; set; }
        public bool Primary { get; set; }

        public override string ToString()
        {
            return string.Format("Source:{0}, NewSource:{1}, Link:{2}", OriginalSource, NewSource, Link);
        }

    }
}