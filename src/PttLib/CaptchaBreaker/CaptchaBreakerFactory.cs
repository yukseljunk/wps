using System.Collections.Generic;
using System.Linq;
using PttLib.CaptchaBreaker.Coral;
using PttLib.CaptchaBreaker.Dbc;
using PttLib.CaptchaBreaker.Null;
using PttLib.CaptchaBreaker.Tez;

namespace PttLib.CaptchaBreaker
{
    public class CaptchaBreakerFactory
    {
        private List<ICaptchaBreaker> _breakers;
        public CaptchaBreakerFactory()
        {

            _breakers= new List<ICaptchaBreaker>()
                           {
                               new CoralBreaker(this),
                               new TezBreaker(this),
                               new DeathByCaptchaBreaker(),
                               new NullBreaker()
                           };
            
        }
        public ICaptchaBreaker Create(string name)
        {
            return _breakers.FirstOrDefault(b => b.Name == name);
        }

        public void Prepare()
        {
            foreach (var captchaBreaker in _breakers)
            {
                captchaBreaker.Prepare();
            }
        }

        public ICaptchaBreaker ManualBreaker()
        {
            var manualService= _breakers.FirstOrDefault(b => b.IsManual);
            if (manualService == null) return Create("Null");
            return manualService;
        }
    }

    

    
}