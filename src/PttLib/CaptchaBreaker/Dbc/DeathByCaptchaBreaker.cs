using System;
using System.Configuration;
using PttLib.Helpers;
using DeathByCaptcha;

namespace PttLib.CaptchaBreaker.Dbc
{
    internal class DeathByCaptchaBreaker : ICaptchaBreaker
    {
        private  string _captchaUserName;
        private string _captchaPassword;
        public int MaxNumberOfTry
        {
            get { return 2; }
        }
     
        public DeathByCaptchaBreaker()
        {
            var captchaUserName = ConfigurationManager.AppSettings["captchaUserName"];
            _captchaUserName = captchaUserName;
            var captchaPassword = ConfigurationManager.AppSettings["captchaPassword"];
            _captchaPassword = captchaPassword;
        
        }
        public void Prepare()
        {

        }

        
        public string Guess(string fileName, int tryCount)
        {
            //Logger.LogProcess(string.Format("{1} captcha try {0}", tryCount, this.Name));

            if (tryCount > MaxNumberOfTry) throw new ApplicationException("DBC max captcha try count exceeded!");
            lock (CaptchaHelper.UsedCaptchaLock)
            {
                CaptchaHelper.UsedCaptchaCount++;
            }
            var client = (Client)new HttpClient(_captchaUserName, _captchaPassword);
            var captcha = client.Decode(fileName, Client.DefaultTimeout);
            if (null != captcha)
            {
                //Logger.LogProcess(string.Format("{2} captcha guess {1} for try {0}", tryCount, captcha.Text ?? "", this.Name));
                return captcha.Text;

            }
            return null;
        }


        public string Name
        {
            get { return "Dbc"; }
        }

        public bool IsManual
        {
            get { return true; }
        }
    }

   

}