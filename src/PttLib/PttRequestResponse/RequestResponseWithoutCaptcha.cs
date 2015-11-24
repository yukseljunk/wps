using System;

namespace PttLib.PttRequestResponse
{
    public class RequestResponseWithoutCaptcha : RequestResponseBase
    {
        #region Overrides of RequestResponseBase

        public RequestResponseWithoutCaptcha(Func<string, bool> retryNeededForRequest, Tuple<int, int> tryAgainInException) : base(retryNeededForRequest, tryAgainInException)
        {
        }


        #endregion
    }
}