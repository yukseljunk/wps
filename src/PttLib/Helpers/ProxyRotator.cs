using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using HtmlAgilityPack;
using PttLib.PttRequestResponse;

namespace PttLib.Helpers
{
    class ProxyRotator
    {
        private static List<string> _proxies;
        private static List<string> _proxyOperators;
        private static Dictionary<string, int> _operatorsProxyTimeout;
        private static Dictionary<string, List<string>> _operatorsBlockedProxies;
        private static Dictionary<string, Dictionary<string, DateTime>> _operatorsProxyTable;
        private static Dictionary<string, object> _operatorsLockers;
        private static int PROXY_TABLE_POLL_INTERVAL = 3;


        static ProxyRotator()
        {
            SetupProxy();
            FillProxies(GetProxyListFromInternet());
            ReadOperatorsFromConfig();
            SetProxyTimeOutForOperators();
            PrepareBlockedProxiesList();
            PrepareOperatorProxyTable();
            PrepareOperatorLockers();
        }

        private static void PrepareOperatorLockers()
        {
            _operatorsLockers = new Dictionary<string, object>();
            if (!_proxyOperators.Any()) return;
            foreach (var proxyOperator in _proxyOperators)
            {
                _operatorsLockers.Add(proxyOperator, new object());
            }
        }

        private static void PrepareOperatorProxyTable()
        {
            _operatorsProxyTable = new Dictionary<string, Dictionary<string, DateTime>>();
            if (!_proxyOperators.Any()) return;
            foreach (var proxyOperator in _proxyOperators)
            {
                var proxyDict = new Dictionary<string, DateTime>();
                foreach (var proxy in _proxies)
                {
                    proxyDict.Add(proxy, DateTime.MinValue);
                }
                _operatorsProxyTable.Add(proxyOperator, proxyDict);
            }


        }

        private static void PrepareBlockedProxiesList()
        {
            _operatorsBlockedProxies = new Dictionary<string, List<string>>();
            if (!_proxyOperators.Any()) return;
            foreach (var proxyOperator in _proxyOperators)
            {
                _operatorsBlockedProxies.Add(proxyOperator, new List<string>());
            }
        }

        private static void ReadOperatorsFromConfig()
        {
            _proxyOperators = new List<string>();
            var proxyOperators = ConfigurationManager.AppSettings["proxyOperators"];
            if (string.IsNullOrEmpty(proxyOperators)) return;
            var proxyOpsSplitted = proxyOperators.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var op in proxyOpsSplitted)
            {
                _proxyOperators.Add(op);
            }
        }

        private static void SetProxyTimeOutForOperators()
        {
            _operatorsProxyTimeout = new Dictionary<string, int>();
            var proxyOperatorsTimeoutValues = ConfigurationManager.AppSettings["proxyOperatorsTimeoutValues"];
            if (string.IsNullOrEmpty(proxyOperatorsTimeoutValues)) return;
            if (!_proxyOperators.Any()) return;
            var proxyOpTimeoutSplitted = proxyOperatorsTimeoutValues.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (proxyOpTimeoutSplitted.Length != _proxyOperators.Count) throw new ConfigurationErrorsException("Config Error: Incorrect operator proxy timeout values");

            for (int opIndex = 0; opIndex < _proxyOperators.Count; opIndex++)
            {
                int opTimeOut;
                if (Int32.TryParse(proxyOpTimeoutSplitted[opIndex], out opTimeOut))
                {
                    _operatorsProxyTimeout[_proxyOperators[opIndex]] = opTimeOut;
                }
            }
        }

        private static void SetupProxy()
        {
            var proxyUserName = ConfigurationManager.AppSettings["proxyUserName"];
            var proxyPassword = ConfigurationManager.AppSettings["proxyPassword"];

            if (string.IsNullOrEmpty(proxyUserName) || string.IsNullOrEmpty(proxyPassword)) return;
            try
            {
                var pttRequestFactory = new PttRequestFactory();
                var pttRequest = pttRequestFactory.SimpleRequest("http://vip.squidproxies.com/valid.php");
                pttRequest.WrappedRequest.Host = "vip.squidproxies.com";
                pttRequest.WrappedRequest.Method = "POST";
                pttRequest.WrappedRequest.ContentType = "application/x-www-form-urlencoded";
                pttRequest.PostValue = "username=" + proxyUserName + "&password=" + proxyPassword + "&button=Sign+In";
                var pttResponse = new PttResponse();
                pttResponse.GetResponse(pttRequest);

                var secondPttRequest = pttRequestFactory.SimpleRequest("http://vip.squidproxies.com/index.php?action=authips");
                secondPttRequest.WrappedRequest.Host = "vip.squidproxies.com";
                secondPttRequest.WrappedRequest.Referer = "http://vip.squidproxies.com/index.php?action=assignedproxies";
                pttRequest.CopySession(secondPttRequest);
                pttResponse.GetResponse(secondPttRequest);

                //check if ip is in the list
                var document = new HtmlDocument();
                document.LoadHtml(secondPttRequest.Response);
                var ipNode = document.DocumentNode.SelectSingleNode("//div[@class='bcont']");
                if (ipNode == null || ipNode.FirstChild == null) return;
                var ipText = ipNode.FirstChild.InnerText.Trim(); //Your IP: 88.103.5.212;
                var words = ipText.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var ipaddress = words.Last();
                Logger.LogProcess("IP specified in squid proxy page: "+ipaddress);
                var ipListNode = ipNode.SelectSingleNode("//textarea[@name='authips']");
                if (ipListNode == null) return;
                var ipList = ipListNode.InnerText;
                var ips = ipList.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (ips.Contains(ipaddress)) return;

                var thirdPttRequest = pttRequestFactory.SimpleRequest("http://vip.squidproxies.com/index.php?action=addauthip");
                thirdPttRequest.WrappedRequest.Host = "vip.squidproxies.com";
                thirdPttRequest.WrappedRequest.Referer = "http://vip.squidproxies.com/index.php?action=addauthip";
                secondPttRequest.CopySession(thirdPttRequest);
                pttResponse.GetResponse(thirdPttRequest);
                Thread.Sleep(60000);

            }
            catch (Exception exception)
            {
                Logger.LogExceptions(exception);
            }
        }

        private static void FillProxies(string proxyList)
        {
            if (proxyList == null) return;
            var proxyLines = proxyList.Split(new[] { "\n\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            _proxies = new List<string>();
            foreach (var proxyLine in proxyLines)
            {
                _proxies.Add(proxyLine);
            }
            //natalie icin simdilik 10 ar 10 ar gelsin, su anda ilk 50 bloklu, 55-65 arasi dubai 65-75 yunanistan 75-85 ispanya olacak
            //_proxies = _proxies.Skip(50).Take(10).ToList();
        }

        //su an benim server da duruyor, bunu squid den cekme ihtimalimiz de var
        private static string GetProxyListFromInternet()
        {
            var proxyListUrl = ConfigurationManager.AppSettings["proxyListUrl"];
            if (proxyListUrl == null) throw new ConfigurationErrorsException("Proxy listesi Url si bulunamadi!");

            var client = new WebClient();
            var proxies = client.DownloadString(proxyListUrl);
            if (string.IsNullOrEmpty(proxies)) return null;
            return proxies;
            //return  Crypto.DecryptStringAES(proxies, Crypto.SharedKey);
        }


        public static Tuple<string, int> GetProxy(string operatorName)
        {
            if (!_proxies.Any()) return null;
            var couple = GetProxyForOperator(operatorName);
            if (string.IsNullOrEmpty(couple)) return null;
            var uri = new Uri("http://" + couple);
            return new Tuple<string, int>(uri.Host, uri.Port);
            //return new Tuple<string, int>(uri.Host, 80);

        }

        private static string GetProxyForOperator(string operatorName)
        {
            if (AllProxiesBlocked(operatorName))
            {
                return null;
            }
            lock (_operatorsLockers[operatorName])
            {
                while (true)
                {
                    var proxyFound = FindEmptySlotInProxyOperatorTable(operatorName);
                    if (!string.IsNullOrEmpty(proxyFound))
                    {
                        return proxyFound;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(PROXY_TABLE_POLL_INTERVAL));
                    CheckTimeoutsOfProxyOperatorTable(operatorName);
                }
            }
        }

        public static void OperatorBlockedProxy(string operatorName, string proxy)
        {
            lock (_operatorsLockers[operatorName])
            {
                if (!_operatorsBlockedProxies.ContainsKey(operatorName))
                {
                    _operatorsBlockedProxies.Add(operatorName, new List<string>());
                }
                var proxyFull = proxy;
                if (proxyFull.StartsWith("http"))
                {
                    proxyFull = proxy.Substring(7);
                }
                if (proxyFull.EndsWith("/"))
                {
                    proxyFull = proxyFull.Replace("/", "");
                }
                if (!_operatorsBlockedProxies[operatorName].Contains(proxyFull))
                {
                    _operatorsBlockedProxies[operatorName].Add(proxyFull);
                }
            }
        }

        private static void CheckTimeoutsOfProxyOperatorTable(string operatorName)
        {
            foreach (var proxy in _proxies)
            {
                if (_operatorsProxyTable.ContainsKey(operatorName) && _operatorsProxyTable[operatorName].ContainsKey(proxy))
                {
                    var dateCell = _operatorsProxyTable[operatorName][proxy];
                    var diff = DateTime.Now.Subtract(dateCell);
                    if (diff.TotalSeconds > _operatorsProxyTimeout[operatorName])
                    {
                        _operatorsProxyTable[operatorName][proxy] = DateTime.MinValue;
                    }
                }

            }
        }

        public static bool AllProxiesBlocked(string operatorName)
        {
            lock (_operatorsLockers[operatorName])
            {

                if (_operatorsBlockedProxies.ContainsKey(operatorName))
                {
                    foreach (var proxy in _proxies)
                    {
                        if (!_operatorsBlockedProxies[operatorName].Contains(proxy))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private static string FindEmptySlotInProxyOperatorTable(string operatorName)
        {
            foreach (var proxy in _proxies)
            {
                if (_operatorsBlockedProxies.ContainsKey(operatorName))
                {
                    if (_operatorsBlockedProxies[operatorName].Contains(proxy))
                    {
                        continue;
                    }
                }
                if (_operatorsProxyTable.ContainsKey(operatorName) && _operatorsProxyTable[operatorName].ContainsKey(proxy))
                {
                    var dateCell = _operatorsProxyTable[operatorName][proxy];
                    if (dateCell == DateTime.MinValue)
                    {
                        _operatorsProxyTable[operatorName][proxy] = DateTime.Now;
                        return proxy;
                    }
                }
            }
            return null;
        }
    }
}
