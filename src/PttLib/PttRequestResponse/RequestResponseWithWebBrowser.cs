using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using PttLib.CaptchaBreaker;
using PttLib.Helpers;
using PttLib.PttRequestResponse.CaptchaRequestResponse;
using System.Windows.Forms;

namespace PttLib.PttRequestResponse
{
    public class RequestResponseWithWebBrowser : RequestResponseWithCaptcha
    {
        private readonly string _name;
        public RequestResponseWithWebBrowser(string name, Func<string, bool> retryNeededForRequest, Tuple<int, int> tryAgainInException, ICaptchaRequestResponse captchaRequestResponse, ICaptchaBreaker captchaBreaker)
            : base(retryNeededForRequest, tryAgainInException, captchaRequestResponse, captchaBreaker)
        {
            _name = name;
            DisableClickSounds();
        }

        public override string GetHtml(IPttRequest request, bool forHotelList)
        {
            var proxy = WebHelper.GetProxyForOperator(_name);//proxy si ayarli geliyor olmasi lazim aslinda
            var lastCaptchaUrl = "";
            var result = "";
            var th = new Thread(() =>
            {
                if (proxy != null)
                {
                    SetSessionProxy(proxy.Item1 + ":" + proxy.Item2, "");
                }

                using (WebBrowser wb = new WebBrowser())
                {
                    var redirected = false;
                    wb.DocumentCompleted += (sndr, e) =>
                    {
                         
                        if (redirected)
                        {
                            Logger.LogProcess("document completed for " + e.Url + ", redirected");

                            result = wb.DocumentText;
                            Logger.LogProcess("result for "+ request.Url + ":" + result);
                            Application.ExitThread();
                        }
                        else//captcha?
                        {
                            Logger.LogProcess("document completed for " + e.Url + " not redirected:");

                           
                            if (_captchaRequestResponse.CaptchaShown(wb.DocumentText))
                            {
                                EventHandler handler2 = (sender, eventArgs) =>
                                {
                                    HtmlElement div = wb.Document.GetElementById("recaptcha_widget_div");
                                    if (div == null) return;
                                    var contentLoaded = div.InnerHtml; // get the content loaded via ajax
                                    if (string.IsNullOrEmpty(contentLoaded)) return;
                                    var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                                    htmlDoc.LoadHtml(contentLoaded);
                                    var captchaImage=htmlDoc.DocumentNode.SelectSingleNode("//img[@id='recaptcha_challenge_image']");
                                    if (captchaImage == null) return;
                                    var captchaUrl = captchaImage.Attributes["src"].Value;
                                    if (lastCaptchaUrl == captchaUrl) return;
                                    lastCaptchaUrl = captchaUrl;
                                    var newRequest = new PttRequest(captchaUrl) { WrappedRequest = { ContentType = "text/html", Method = "GET" } };
                                    var pttCaptcha = new PttCaptcha() { Url = captchaUrl, RequestUsed = newRequest };

                                    var captchaResolve = ResolveCaptcha(pttCaptcha.RequestUsed, pttCaptcha, 1);
                                    pttCaptcha.Value = captchaResolve.Item1;
                                    pttCaptcha.FileNameInTempDir = captchaResolve.Item2;

                                    var captchaBox = wb.Document.GetElementsByTagName("input")["recaptcha_response_field"];
                                    if (captchaBox == null) return;
                                    captchaBox.SetAttribute("value", pttCaptcha.Value);
                                    wb.Navigate("javascript:document.forms[0].submit()");
                                    
                                };


                                Logger.LogProcess("captcha asked for " + request.Url);
 
                                result = wb.DocumentText;

                                HtmlElement target = wb.Document.GetElementById("recaptcha_widget_div");

                                if (target != null)
                                {
                                    target.AttachEventHandler("onpropertychange", handler2);
                                }

/*
                                var pttCaptcha = _captchaRequestResponse.CaptchaImage(null, result);

                                try
                                {
                                    var captchaResolve = ResolveCaptcha(pttCaptcha.RequestUsed, pttCaptcha, 1);
                                    pttCaptcha.Value = captchaResolve.Item1;
                                    pttCaptcha.FileNameInTempDir = captchaResolve.Item2;
                                    Logger.LogProcess("captcha value for " + pttCaptcha.FileNameInTempDir+ " is " + pttCaptcha.Value);

                                }
                                catch (Exception exc)
                                {
                                    Logger.LogExceptions(exc);
                                    Application.ExitThread();
                                }
                                
                                var captchaBox = wb.Document.GetElementsByTagName("input")["recaptcha_response_field"];
                                captchaBox.SetAttribute("value", pttCaptcha.Value);
                                Logger.LogProcess("submitting captcha "+ pttCaptcha.Value);

                                wb.Navigate("javascript:document.forms[0].submit()");

                                Application.ExitThread();*/
                            }
                        }
                    };
                  
                    wb.Navigating += (sndr, e) =>
                    {
                        Logger.LogProcess("webbrowser navigating to "+  e.Url);
                        if (e.Url.ToString().ToLower().Contains("iswait"))
                        {
                            Logger.LogProcess("redirection done for " + e.Url);
                            redirected = true;
                        }
                    };
                    Logger.LogProcess("navigating to " +request.Url);
                    wb.Navigate(request.Url);

                    Application.Run();
                }
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            th.Join();

            return result;

        }

      

        private void SetSessionProxy(string ProxyAddress, string BypassList)
        {
            var proxyInfo = new INTERNET_PROXY_INFO
            {
                dwAccessType = 0x3,
                lpszProxy = ProxyAddress,
                lpszProxyBypass = BypassList
            };
            uint structSize = (uint)Marshal.SizeOf(proxyInfo);
            const uint SetProxy = 0x26;

            if (UrlMkSetSessionOption(SetProxy, proxyInfo, structSize, 0) != 0)
                throw new Win32Exception();
        }

        [StructLayout(LayoutKind.Sequential)]
        private class INTERNET_PROXY_INFO
        {
            public uint dwAccessType;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpszProxy;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpszProxyBypass;
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int UrlMkSetSessionOption(uint dwOption, INTERNET_PROXY_INFO structNewProxy, uint dwLen, uint dwZero);


        const int FEATURE_DISABLE_NAVIGATION_SOUNDS = 21;
        const int SET_FEATURE_ON_PROCESS = 0x00000002;

        [DllImport("urlmon.dll")]
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        static extern int CoInternetSetFeatureEnabled(
            int FeatureEntry,
            [MarshalAs(UnmanagedType.U4)] int dwFlags,
            bool fEnable);

        static void DisableClickSounds()
        {
            CoInternetSetFeatureEnabled(
                FEATURE_DISABLE_NAVIGATION_SOUNDS,
                SET_FEATURE_ON_PROCESS,
                true);
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

   

    }
}