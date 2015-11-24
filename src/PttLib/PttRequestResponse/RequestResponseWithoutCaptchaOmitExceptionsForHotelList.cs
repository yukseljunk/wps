using System;

namespace PttLib.PttRequestResponse
{
    public class RequestResponseWithoutCaptchaOmitExceptionsForHotelList : RequestResponseBase
    {
        #region Overrides of RequestResponseBase

        public RequestResponseWithoutCaptchaOmitExceptionsForHotelList(Func<string, bool> retryNeededForRequest, Tuple<int, int> tryAgainInException)
            : base(retryNeededForRequest, tryAgainInException)
        {
        }

        /// <summary>
        /// the god-wrapper for gethml functions
        /// </summary>
        public override string GetHtmlMaster(IPttRequest request, bool forHotelList, bool extensiveLoggingNeeded = false,
            string operatorName = null)
        {
            if (forHotelList && request.RequestType == PttRequestType.Init)
            {
                try
                {
                    return base.GetHtmlMaster(request, forHotelList, extensiveLoggingNeeded, operatorName);
                }
                catch
                {
                    return null;
                }
            }
            return base.GetHtmlMaster(request, forHotelList, extensiveLoggingNeeded, operatorName);
        }

        #endregion
    }
}