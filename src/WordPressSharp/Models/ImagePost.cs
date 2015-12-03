namespace WordPressSharp.Models
{
    public class ImagePost : Post
    {
        private const string DefaultMimeType = "image/jpeg";
        private string _mimeType;

        public string MimeType
        {
            get
            {
                if (string.IsNullOrEmpty(_mimeType))
                {
                    _mimeType = DefaultMimeType;
                }
                return _mimeType;
            }
            set { _mimeType = value; }
        }

        public string Alt { get; set; }

    }
}