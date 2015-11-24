namespace PttLib.PttRequestResponse
{
    public interface IPttCaptcha
    {
        string Value { get; set; }
        string Url { get; set; }
        string FormUrl { get; set; }
        string FileNameInTempDir { get; set; }
        bool IsBase64 { get; set; }
        string Base64Content { get; set; }
        IPttRequest RequestUsed { get; set; }
    }
}