namespace PttLib.PttRequestResponse.CaptchaRequestResponse
{
    public interface ICaptchaRequestResponse
    {
        bool CaptchaShown(string htmlSource);

        IPttRequest RequestWithCaptchaValue(IPttRequest request, IPttCaptcha pttCaptcha);

        IPttCaptcha CaptchaImage(IPttRequest request, string htmlSource);

        bool RepeatFirstRequest { get; }
    }
}