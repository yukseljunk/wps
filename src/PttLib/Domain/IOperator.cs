using System;
using System.Collections.Generic;
using System.Globalization;
using PttLib.Helpers.XmlConverters;
using PttLib.Operators.Query;
using PttLib.PttRequestResponse;
using PttLib.TourInfo;
using PttLib.Tours;

namespace PttLib.Domain
{
    public interface IOperator
    {
        string Name { get; }
        IList<Tour> GetTours(string hotelId, int pageNo);
        
        int PageIndex { get; set; }
        IList<string> HotelIds { get; set; }
        IList<string> HotelCommonNames { get; set; }
        IList<string> HotelNames { get; set; }

        IXmlConverter AllHotelsXmlConverter { get; set; }
        Dictionary<string, string> GetAllHotelNamesAndIds();
        string SerializedHotelListRequests { get; set; }
        string AllHotelsTemplateFile { get; }
        
        string SerializedTourDataRequests { get; set; }
        string QueryTemplateFile { get; }
        
        string TourInfoFile { get; }
        ITourInfo TourInfo { get; set; }

        CultureInfo CultureInfo { get; }
        Query QueryObject { get; set; }
        string QueryNotValid { get; }
        int DatePartitioningDays { get; }
        IOperator Clone();
        bool HarvestSingleThread { get; }

        Tuple<int, int> TryAgainInException { get; }
        IRequestResponse RequestResponseBehavior { get; }

        string Refine(IPttRequest pttRequest, string htmlSource);
        List<List<string>> MealTypes { get; }

        bool ExtensiveLoggingNeeded { get; set; }
    }
}
