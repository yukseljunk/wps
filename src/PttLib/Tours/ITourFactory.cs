using System.Collections.Generic;
using PttLib.Domain;
using PttLib.PttRequestResponse;

namespace PttLib.Tours
{
    public interface ITourFactory
    {
        IList<Tour> Create(IPttRequest request, IOperator op, out TourFactoryStatus status);
    }
}