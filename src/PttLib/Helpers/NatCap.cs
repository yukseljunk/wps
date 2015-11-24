using System.Text;

namespace PttLib.Helpers
{
    public class NatCap
    {
        private string _natcapExeFilePath = Helper.AssemblyDirectory + @"\natcap.exe";
        
        public string GrabForOperator(string url, string operatorName, string proxyUrl, string culture)
        {
            return Grab(url, proxyUrl, culture);
        }

        public string Grab(string url, string proxyUrl, string culture)
        {
            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
                                {
                                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized,
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    FileName = _natcapExeFilePath,
                                    Arguments = url + " " + (string.IsNullOrEmpty(proxyUrl) ? "" : proxyUrl),
                                    StandardOutputEncoding = Encoding.UTF8,
                                    CreateNoWindow = false
                                };

            process.StartInfo = startInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }
    }
}