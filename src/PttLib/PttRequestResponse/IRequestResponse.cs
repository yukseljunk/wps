namespace PttLib.PttRequestResponse
{
    public interface IRequestResponse
    {
        string GetHtml(IPttRequest request, bool forHotelList);

        /// <summary>
        /// decorated gethml
        /// </summary>
        /// <param name="request"></param>
        /// <param name="forHotelList"></param>
        /// <param name="extensiveLoggingNeeded"></param>
        /// <returns></returns>
        string GetHtmlMaster(IPttRequest request, bool forHotelList, bool extensiveLoggingNeeded = false, string operatorName=null);

    }
}