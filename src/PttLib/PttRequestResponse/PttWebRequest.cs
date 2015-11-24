using System;
using System.Net;

namespace PttLib.PttRequestResponse
{
    /// <summary>
    /// proxy class for shitty webrequest class
    /// </summary>
    public class PttWebRequest
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string ContentType { get; set; }
        public string Referer { get; set; }
        public string Host { get; set; }
        public string UserAgent { get; set; }
        public string Accept { get; set; }
        public bool KeepAlive { get; set; }
        public bool AllowAutoRedirect { get; set; }
        public Version ProtocolVersion { get; set; }
        public int Timeout { get; set; }
        public int ReadWriteTimeout { get; set; }
        public DecompressionMethods AutomaticDecompression { get; set; }
        public WebHeaderCollection Headers { get; set; }
        public CookieContainer CookieContainer { get; set; }
        public IWebProxy Proxy { get; set; }
        public long ContentLength { get; set; }

        public HttpWebRequest Convert()
        {
            var request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = Method;
            request.ContentType = ContentType;
            request.Referer = Referer;
            request.Host = Host;
            request.UserAgent = UserAgent;
            request.Accept = Accept;
            request.KeepAlive = KeepAlive;
            request.AllowAutoRedirect = AllowAutoRedirect;
            request.ProtocolVersion = ProtocolVersion;
            request.Timeout = Timeout;
            request.ReadWriteTimeout = ReadWriteTimeout;
            request.AutomaticDecompression = AutomaticDecompression;

            foreach (var headerKey in Headers.AllKeys)
            {
                try
                {
                    request.Headers.Add(headerKey, Headers[headerKey]);
                }
                catch (ArgumentException exception)
                {
                    //this is for the headers that should not be set thru here
                }
            }
            request.CookieContainer = CookieContainer;
            request.Proxy = Proxy;
            return request;
        }

        public static PttWebRequest Create(HttpWebRequest webRequest)
        {
            var pttWebRequest = new PttWebRequest
            {
                Url = webRequest.Address.ToString(),
                Method = webRequest.Method,
                ContentType = webRequest.ContentType,
                Referer = webRequest.Referer,
                Host = webRequest.Host,
                UserAgent = webRequest.UserAgent,
                Accept = webRequest.Accept,
                KeepAlive = webRequest.KeepAlive,
                AllowAutoRedirect = webRequest.AllowAutoRedirect,
                ProtocolVersion = webRequest.ProtocolVersion,
                Timeout = webRequest.Timeout,
                ReadWriteTimeout = webRequest.ReadWriteTimeout,
                AutomaticDecompression = webRequest.AutomaticDecompression
            };
            if (webRequest.Headers != null)
            {
                pttWebRequest.Headers= new WebHeaderCollection();
                foreach (var headerKey in webRequest.Headers.AllKeys)
                {
                    try
                    {
                        pttWebRequest.Headers.Add(headerKey, webRequest.Headers[headerKey]);
                    }
                    catch (ArgumentException exception)
                    {
                        //this is for the headers that should not be set thru here
                    }
                }
            }
            pttWebRequest.CookieContainer = webRequest.CookieContainer;
            pttWebRequest.Proxy = webRequest.Proxy;
            return pttWebRequest;
        }

    }   
}