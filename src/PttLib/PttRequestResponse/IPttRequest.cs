using System.Collections.Concurrent;
using System.Net;

namespace PttLib.PttRequestResponse
{
    public interface IPttRequest
    {
        PttRequestType RequestType { get; set; }
        void CopySession(IPttRequest target);
        void CopySessionWithoutProxy(IPttRequest target);
        string PostValue { get; set; }
        bool Chunked { get; set; }
        CookieContainer CookieContainer { get; set; }
        IWebProxy Proxy { get; set; }
        string Url { get; set; }
        HttpWebRequest WrappedRequest { get; set; }
        IPttRequest Clone(PttWebRequest pttWebRequest);
        CookieCollection Cookies { get; }
        string Condition { get; set; }
        bool ConditionSatisfied { get; }
        string ViewStateValue { get; set; }
        string EventValidationValue { get; set; }

        //to carry some variables btwn successive requests
        ConcurrentDictionary<string,object> SessionJar { get; set; }

        string Response { get; set; }
    }
}