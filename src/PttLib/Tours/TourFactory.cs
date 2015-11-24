using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using PttLib.Domain;
using PttLib.Helpers;
using PttLib.PttRequestResponse;

namespace PttLib.Tours
{

    [Flags]
    public enum TourFactoryControlFlags
    {
        Pass=1,
        Break = 2,
        Return = 4,
        Continue = 8,
        MoveToNextHotel = 16
    }
    
    public class TourFactoryStatus {
        public bool Success { get; set; }
        public bool MoveToNextHotel { get; set; }
        public string NextPageUrl { get; set; }
    }

    public class TourFactory : ITourFactory
    {
        private readonly Func<IPttRequest, HtmlDocument, TourFactoryControlFlags> _beforeIteratingWhileGettingTours;
        private readonly Func<IPttRequest, HtmlNode, TourFactoryControlFlags> _iteratingWhileGettingToursBeforeTourItemSet;
        private readonly Func<IPttRequest, Tour, TourFactoryControlFlags> _iteratingWhileGettingToursAfterTourItemSet;


        public TourFactory(Func<IPttRequest, HtmlDocument, TourFactoryControlFlags> beforeIteratingWhileGettingTours,
                           Func<IPttRequest, HtmlNode, TourFactoryControlFlags> iteratingWhileGettingToursBeforeTourItemSet,
                           Func<IPttRequest, Tour, TourFactoryControlFlags> iteratingWhileGettingToursAfterTourItemSet)
        {
            _beforeIteratingWhileGettingTours = beforeIteratingWhileGettingTours;
            _iteratingWhileGettingToursBeforeTourItemSet = iteratingWhileGettingToursBeforeTourItemSet;
            _iteratingWhileGettingToursAfterTourItemSet = iteratingWhileGettingToursAfterTourItemSet;

        }

        public virtual IList<Tour> Create(IPttRequest request, IOperator op, out TourFactoryStatus status) 
        {
            if (op.ExtensiveLoggingNeeded)
            {
                Logger.LogProcess("TourFactory.Create trying to get html for " + request.Url + " with postdata:"+request.PostValue);
            }
            
            var htmlSource = op.RequestResponseBehavior.GetHtmlMaster(request, false, op.ExtensiveLoggingNeeded, op.Name);
            if(op.ExtensiveLoggingNeeded)
            {
                Logger.LogProcess("TourFactory.Create Html coming from htmlmaster");
                Logger.LogProcess(htmlSource ?? "");
            }
            if (string.IsNullOrEmpty(htmlSource))
            {
                status = new TourFactoryStatus { Success = false };
                return null;
            }

            var htmlSourceRefined = op.Refine(request, htmlSource);
            if (op.ExtensiveLoggingNeeded)
            {
                Logger.LogProcess("TourFactory.Create Html refined");
                Logger.LogProcess(htmlSourceRefined ?? "");
            }
            var html = new HtmlDocument();
            html.LoadHtml(htmlSourceRefined);
            var list = GetToursFromHtml(request, op, html, out status);
            return list;
        }


        protected virtual IList<Tour> GetToursFromHtml(IPttRequest request, IOperator op, HtmlDocument html, out TourFactoryStatus status)
        {
            status = new TourFactoryStatus { Success = true, MoveToNextHotel = false };
            
            if (html.DocumentNode.SelectNodes(op.TourInfo.XPath) == null)
            {
                if (op.ExtensiveLoggingNeeded)
                {
                    Logger.LogProcess("TourFactory.GetToursFromHtml xpath node selection returns no nodes");
                }
                status.MoveToNextHotel = true;
                return null;
            }
           
            if (_beforeIteratingWhileGettingTours != null)
            {
                var ret = _beforeIteratingWhileGettingTours(request, html);
                if (op.ExtensiveLoggingNeeded)
                {
                    Logger.LogProcess("TourFactory.GetToursFromHtml _beforeIteratingWhileGettingTours  flags:"+ret.ToString());
                }
                status.MoveToNextHotel = status.MoveToNextHotel || ret.HasFlag(TourFactoryControlFlags.MoveToNextHotel);
                if (ret.HasFlag(TourFactoryControlFlags.Return))
                {
                    return null;
                }
            }

            var list = new List<Tour>();
            foreach (HtmlNode node in html.DocumentNode.SelectNodes(op.TourInfo.XPath))
            {
                if (_iteratingWhileGettingToursBeforeTourItemSet != null)
                {
                    var ret = _iteratingWhileGettingToursBeforeTourItemSet(request, node);
                    if (op.ExtensiveLoggingNeeded)
                    {
                        Logger.LogProcess("TourFactory.GetToursFromHtml _iteratingWhileGettingToursBeforeTourItemSet  flags:" + ret.ToString());
                    }
                    status.MoveToNextHotel = status.MoveToNextHotel || ret.HasFlag(TourFactoryControlFlags.MoveToNextHotel);
                    if (ret.HasFlag(TourFactoryControlFlags.Continue))
                    {
                        continue;
                    }
                    if (ret.HasFlag(TourFactoryControlFlags.Break))
                    {
                        break;
                    }
                    if (ret.HasFlag(TourFactoryControlFlags.Return))
                    {
                        return null;
                    }
                }

                var tourItem = new Tour();

                tourItem.City = Dictionnaries.Regions[op.QueryObject.OriginId];
                tourItem.TO = op.Name.Replace("_", "");//anex_ gibiler icin
                tourItem.IssueDate = DateTime.Now;

                tourItem.Date = op.TourInfo.GetFieldValue<DateTime>(node, "Date");
                tourItem.Night = op.TourInfo.GetFieldValue<string>(node, "Night");
                tourItem.Hotel = op.TourInfo.GetFieldValue<string>(node, "Hotel");
                tourItem.RoomType = op.TourInfo.GetFieldValue<string>(node, "RoomType");
                tourItem.ACC = op.TourInfo.GetFieldValue<string>(node, "Accomodation");
                tourItem.Price = op.TourInfo.GetFieldValue<Double>(node, "Price");


                var meal = op.TourInfo.GetFieldValue<string>(node, "Meal");
                var mealTypeIndex = 7;
                var innerMealIndex = 0;
                foreach (var mealtypeAlternative in op.MealTypes)
                {
                    if (mealtypeAlternative.Contains(meal))
                    {
                        mealTypeIndex = innerMealIndex;
                        break;
                    }
                    innerMealIndex++;
                }
                tourItem.Meal = Dictionnaries.MealTypes[mealTypeIndex];
                tourItem.SPONo = op.TourInfo.GetFieldValue<string>(node, "SPONo"); 

                if (_iteratingWhileGettingToursAfterTourItemSet != null)
                {
                    var ret = _iteratingWhileGettingToursAfterTourItemSet(request, tourItem);
                    if (op.ExtensiveLoggingNeeded)
                    {
                        Logger.LogProcess("TourFactory.GetToursFromHtml _iteratingWhileGettingToursAfterTourItemSet  flags:" + ret.ToString());
                    }

                    status.MoveToNextHotel = status.MoveToNextHotel || ret.HasFlag(TourFactoryControlFlags.MoveToNextHotel);
                    if (ret.HasFlag(TourFactoryControlFlags.Continue))
                    {
                        continue;
                    }
                    if (ret.HasFlag(TourFactoryControlFlags.Break))
                    {
                        break;
                    }
                    if (ret.HasFlag(TourFactoryControlFlags.Return))
                    {
                        return null;
                    }
                }
                if (op.QueryObject != null && tourItem.Price < op.QueryObject.MinPrice)
                {
                    if (op.ExtensiveLoggingNeeded)
                    {
                        Logger.LogProcess("TourFactory.GetToursFromHtml tour info found but less than minprice:" + tourItem.Price);
                    }
                    continue;
                }

                if (op.QueryObject != null && (op.QueryObject.MealType == mealTypeIndex || op.QueryObject.MealType == 0))
                {
                    if (op.ExtensiveLoggingNeeded)
                    {
                        Logger.LogProcess("TourFactory.GetToursFromHtml tour info added:" + tourItem.ToString());
                    }

                    list.Add(tourItem);
                }
                else
                {
                    if (op.ExtensiveLoggingNeeded)
                    {
                        Logger.LogProcess("TourFactory.GetToursFromHtml tour info found but excluded bcz of meal type:" + mealTypeIndex);
                    }
 
                }
            }
            return list;
        }
    }
}