
namespace PttLib.CaptchaBreaker
{
    public interface ICaptchaBreaker
    {
        void Prepare();
        string Guess(string fileName, int tryCount);
        string Name { get; }
        bool IsManual { get; }
        int MaxNumberOfTry { get; }
    }
}
