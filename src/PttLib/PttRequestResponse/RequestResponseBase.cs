using System;
using System.Net;
using System.Threading;
using PttLib.Helpers;

namespace PttLib.PttRequestResponse
{
    public abstract class RequestResponseBase : IRequestResponse
    {
        private readonly Func<string, bool> _retryNeededForRequest;
        private readonly Tuple<int, int> _tryAgainInException;

        public RequestResponseBase(Func<string, bool> retryNeededForRequest, Tuple<int, int> tryAgainInException)
        {
            _retryNeededForRequest = retryNeededForRequest;
            _tryAgainInException = tryAgainInException;
        }

        /// <summary>
        /// the god-wrapper for gethml functions
        /// </summary>
        public virtual string GetHtmlMaster(IPttRequest request, bool forHotelList, bool extensiveLoggingNeeded = false, string operatorName=null)
        {
            PttWebRequest pttWebRequest = null;
            var unsuccesfullTries = 0;
            while (true)
            {
                try
                {
                    string result = "";
                    if (extensiveLoggingNeeded)
                    {
                        Logger.LogProcess("GetHtmlMaster try #" + (unsuccesfullTries + 1) + " for request:" + request);
                    }
                    if (pttWebRequest==null)
                    {
                        pttWebRequest = PttWebRequest.Create(request.WrappedRequest);                        
                    }
                    var requestToUse = request;
                    if (unsuccesfullTries > 0)
                    {
                        requestToUse = request.Clone(pttWebRequest);
                        IWebProxy proxy = new WebProxy();
                        WebHelper.ArrangeProxy(ref proxy, operatorName);
                        requestToUse.Proxy = proxy;
                    }

                    result = GetHtml(requestToUse, forHotelList);

                    if (_retryNeededForRequest(result))
                    {
                        if (extensiveLoggingNeeded)
                        {
                            Logger.LogProcess("Retry needed for try #" + (unsuccesfullTries + 1) + " for request:" + request);
                        }
                        throw new Exception("Retry needed!");
                    }
                    if (extensiveLoggingNeeded)
                    {
                        Logger.LogProcess("GetHtmlMaster no more retry needed for request:" + request);
                    }
                    return result;
                }
                catch (Exception exc)
                {
                    if (_tryAgainInException.Item1 == 0)
                    {
                        throw exc;
                    }
                    unsuccesfullTries++;
                    Thread.Sleep(TimeSpan.FromSeconds(_tryAgainInException.Item2));
                    if (unsuccesfullTries == _tryAgainInException.Item1)
                    {
                        throw exc;
                    }
                }
            }
        }

        public virtual string GetHtml(IPttRequest request, bool forHotelList)
        {
            IPttResponse response = new PttResponse();
            var htmlSource = response.GetResponse(request);
            return htmlSource;
        }

    }
}