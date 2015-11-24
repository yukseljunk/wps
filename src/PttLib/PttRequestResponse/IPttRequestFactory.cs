using System.Collections.Generic;

namespace PttLib.PttRequestResponse
{
    internal interface IPttRequestFactory
    {

        IPttRequest Deserialize(string serialized);

        IEnumerable<IPttRequest> DeserializeList(string serialized, IPttRequest lastRequest, FillSessionJarDelegate fillSessionJar);

        IPttRequest SimpleRequest(string url);
    }
}