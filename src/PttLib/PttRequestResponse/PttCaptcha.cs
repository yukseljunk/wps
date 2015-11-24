namespace PttLib.PttRequestResponse
{
    public class PttCaptcha:IPttCaptcha
    {
        #region Implementation of IPttCaptcha

        public string Value { get; set; }
        public string Url { get; set; }
        public string FormUrl { get; set; }
        public string FileNameInTempDir { get; set; }
        public bool IsBase64 { get; set; }
        public string Base64Content { get; set; }
        public IPttRequest RequestUsed { get; set; }

        public override string ToString()
        {
            var result = "Captcha ";
            if(!string.IsNullOrEmpty(Url))
            {
                result += "Url:" + Url;
            }
            if (!string.IsNullOrEmpty(FileNameInTempDir))
            {
                result += " FileNameInTempDir:" + FileNameInTempDir;
            }
            if (!string.IsNullOrEmpty(FormUrl))
            {
                result += " FormUrl:" + FormUrl;
            }
            if (!string.IsNullOrEmpty(Value))
            {
                result += " Value:" + Value;
            }
            result += " IsBase64:" + IsBase64;
            if (!string.IsNullOrEmpty(Base64Content))
            {
                result += " Base64Content:" + Base64Content;
            }

            return result;
        }
        #endregion
    }
}