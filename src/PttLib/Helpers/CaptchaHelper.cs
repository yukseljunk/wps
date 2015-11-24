using System;
using System.IO;
using System.Reflection;

namespace PttLib.Helpers
{
    public static class CaptchaHelper
    {
        public static object UsedCaptchaLock = new object();
        public static int UsedCaptchaCount;// { get; set; }
        public static int BrokenCaptchaCount;

        public static void DeleteCaptchasFolder()
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\captchas";
            if (!Directory.Exists(path)) return;

            Directory.Delete(path, true);
            
        }
        public static string NewCaptchaFileName()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + string.Format("\\captchas\\captcha{0}.jpg", Guid.NewGuid());

        }

        public  static void CreateCaptchasFolder()
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\captchas";
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);

        }

        public static int CaptchaFolderFileCount()
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\captchas";
            if (!Directory.Exists(path)) return 0;
            return Directory.GetFiles(path).Length;

        }
    }
}