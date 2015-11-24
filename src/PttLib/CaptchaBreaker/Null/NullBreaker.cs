namespace PttLib.CaptchaBreaker.Null
{
    internal class NullBreaker : ICaptchaBreaker
    {
        public void Prepare()
        {
        }

        public string Guess(string fileName, int tryCount)
        {
            return null;
        }

        public string Name { get { return "Null"; } }
        public bool IsManual { get { return false; } }
        public int MaxNumberOfTry { get { return 0; } }
    }
}