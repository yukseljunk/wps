namespace PttLib.PttRequestResponse
{
    internal interface IPttResponse
    {
        string GetResponse(IPttRequest request);
        byte[] GetResponseBytes(IPttRequest request);
    }

   
}