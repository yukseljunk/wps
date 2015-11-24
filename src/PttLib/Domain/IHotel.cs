using System;
using System.Collections.Generic;

namespace PttLib.Domain
{
    public interface IHotel
    {
        string CommonName { get; set; }
        Dictionary<IOperator, Tuple<string, string>> HotelValueForOperator { get; set; }
    }
}