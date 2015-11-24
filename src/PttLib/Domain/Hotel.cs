using System;
using System.Collections.Generic;

namespace PttLib.Domain
{
    public class Hotel:IHotel
    {
        public string CommonName { get; set; }
        public Dictionary<IOperator, Tuple<string, string>> HotelValueForOperator { get; set; }
    }

    public class HotelFactory:IHotelFactory
    {
        public IHotel CreateHotel(string commonName)
        {
            if (string.IsNullOrEmpty(commonName)) return null;

            return new Hotel(){CommonName = commonName};
        }
        public IList<IHotel> CreateHotels(IList<string> hotelCommonNames)
        {
            if (hotelCommonNames == null) return null;
            var result = new List<IHotel>();
            foreach (var hotelCommonName in hotelCommonNames)
            {
                var hotelToAdd = CreateHotel(hotelCommonName);
                if(hotelToAdd!=null) result.Add(CreateHotel(hotelCommonName));
            }

            return result;
        } 
    }

    public interface IHotelFactory
    {
        IHotel CreateHotel(string commonName);
        IList<IHotel> CreateHotels(IList<string> hotelCommonNames);

    }
}