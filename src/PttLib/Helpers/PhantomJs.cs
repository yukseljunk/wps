using System.Text;

namespace PttLib.Helpers
{
    public class PhantomJs
    {
        private string _phantomJsExeFilePath = Helper.AssemblyDirectory + @"\phantomjs\phantomjs.exe";
        private string _phantomJsFilePath = Helper.AssemblyDirectory + @"\operators\phantomjs\{0}.js";
        private const string PhantomArguments = "--output-encoding={0} {1} \"{2}\"";
        private const string PhantomProxyArgument = "--proxy={0} ";
        
        public string GrabForOperator(string url, string operatorName, string proxyUrl, string culture)
        {
            return Grab(url, string.Format(_phantomJsFilePath, operatorName),proxyUrl,culture);
        }
 
        public string Grab(string url, string jsFilePath, string proxyUrl, string culture)
        {
            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
                                {
                                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized,
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    FileName = _phantomJsExeFilePath,
                                    Arguments =(string.IsNullOrEmpty(proxyUrl)?"": string.Format(PhantomProxyArgument,proxyUrl)) +string.Format(PhantomArguments, culture, jsFilePath ,url),
                                    StandardOutputEncoding = Encoding.UTF8,
                                    CreateNoWindow = true
                                };

            process.StartInfo = startInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }
    }
}