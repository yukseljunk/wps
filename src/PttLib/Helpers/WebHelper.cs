using System;
using System.Collections;
using System.Configuration;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.RegularExpressions;
using PttLib.PttRequestResponse;

namespace PttLib.Helpers
{
    public static class WebHelper
    {
        public static object UsedRequestLock = new object();
        public static int UsedRequestCount;// { get; set; }
        public static DateTime LastRequestTime;// { get; set; }

        public static string CurlSimple(string url)
        {
            try
            {
                var requestFactory = new PttRequestFactory();
                var pttRequest = requestFactory.SimpleRequest(url);
                var response = new PttResponse();
                return response.GetResponse(pttRequest);                
            }
            catch(Exception exception)
            {
                Logger.LogExceptions(exception);
            }
            return null;
        }

        public static string CurlSimplePost(string url, string postData, string host, bool chunked=false)
        {
            try
            {
                var pttRequestFactory = new PttRequestFactory();
                var pttRequest = pttRequestFactory.SimpleRequest(url);
                pttRequest.Chunked = chunked;
                pttRequest.WrappedRequest.Host = host;
                pttRequest.WrappedRequest.Method = "POST";
                pttRequest.WrappedRequest.ContentType = "application/x-www-form-urlencoded";
                pttRequest.PostValue = postData;
                var pttResponse = new PttResponse();
                return pttResponse.GetResponse(pttRequest);
            }
            catch(Exception exception)
            {
                Logger.LogExceptions(exception);
            }
            return null;
        }

        public static string ExtractViewState(string str)
        {
            var viewState = "";
            var pattern = "(?<=__VIEWSTATE\" value=\")(?<val>.*?)(?=\")";
            var match = Regex.Match(str, pattern);
            if (match.Success)
            {
                viewState = match.Groups["val"].Value;
            }
            if (viewState == "")
            {
                pattern = "(?<=viewStateHidenFieldId\" value=\")(?<val>.*?)(?=\")";
                match = Regex.Match(str, pattern);
                if (match.Success)
                {
                    viewState = match.Groups["val"].Value;
                }
                
            }
            return viewState;
        }

        public static string ExtractEventValidation(string str)
        {
            string eventValidation = "";
            string pattern = "(?<=__EVENTVALIDATION\" value=\")(?<val>.*?)(?=\")";

            Match match = Regex.Match(str, pattern);

            if (match.Success)
            {
                eventValidation = match.Groups["val"].Value;
            }

            return eventValidation;
        }

        public static void ServicePointManagerSetup()
        {
            ServicePointManager.UseNagleAlgorithm = true;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.DefaultConnectionLimit = 10;
            ServicePointManager.MaxServicePointIdleTime = 10000;
        }

        public static Tuple<string,int> GetProxyForOperator(string operatorNameForProxyCheck)
        {   
            if (String.IsNullOrEmpty(operatorNameForProxyCheck)) return null;
            //operator proxy listesinde var mi kontrol et ona gore devam

            var proxyOperators = ConfigurationManager.AppSettings["proxyOperators"];
            if (String.IsNullOrEmpty(proxyOperators)) return null;
            if (!("," + proxyOperators.ToUpper() + ",").Contains("," + operatorNameForProxyCheck.ToUpper() + ",")) return null;
            return ProxyRotator.GetProxy(operatorNameForProxyCheck);

        }

        public static void ArrangeProxy(ref IWebProxy proxy, string operatorName=null)
        {
            var proxyForOperator = GetProxyForOperator(operatorName);
            if (proxyForOperator != null)
            {
                proxy = new WebProxy(proxyForOperator.Item1, proxyForOperator.Item2);
                return;
            }

            var useProxyOfClient = ConfigurationManager.AppSettings["useUserProxySettings"];
            if (useProxyOfClient == "1")
            {
                proxy = null;
            }

        }

        public static CookieCollection GetAllCookies(CookieContainer cookieJar)
        {
            if (cookieJar == null) return null;
            var cookieCollection = new CookieCollection();

            var table = (Hashtable)cookieJar.GetType().InvokeMember("m_domainTable",
                                                                    BindingFlags.NonPublic |
                                                                    BindingFlags.GetField |
                                                                    BindingFlags.Instance,
                                                                    null,
                                                                    cookieJar,
                                                                    new object[] { });

            foreach (var tableKey in table.Keys)
            {
                var str_tableKey = (string)tableKey;

                if (str_tableKey[0] == '.')
                {
                    str_tableKey = str_tableKey.Substring(1);
                }

                var list = (SortedList)table[tableKey].GetType().InvokeMember("m_list",
                                                                              BindingFlags.NonPublic |
                                                                              BindingFlags.GetField |
                                                                              BindingFlags.Instance,
                                                                              null,
                                                                              table[tableKey],
                                                                              new object[] { });

                foreach (var listKey in list.Keys)
                {
                    var url = "https://" + str_tableKey + (string)listKey;
                    cookieCollection.Add(cookieJar.GetCookies(new Uri(url)));
                }
            }

            return cookieCollection;
        }

        public static string GetMac()
        {
            try
            {

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        return nic.GetPhysicalAddress().ToString();
                    }
                }
            }
            catch(Exception exception)
            {
                

            }
            return null;
        }
    }
}