using System;
using HtmlAgilityPack;
using PttLib.Helpers;

namespace PttLib.Operators.Query
{
    public class QueryFactory : IQueryFactory
    {
        public Query Create(string queryOuterXml)
        {
            var query = new Query();

            var html = new HtmlDocument();
            html.LoadHtml(queryOuterXml);

            query.MinPrice = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/minprice", 0);
            query.MaxPrice = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/maxprice", 10000);
            query.MinNights = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/minnights", 9);
            query.MaxNights = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/maxnights", 14);
            query.NumberOfAdults = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/numberofadults", 2);
            query.MealType = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/mealtype", 0); //UserDefaultMealType()
            query.OriginId = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/originid", 0);
            if (query.OriginId == 0) query.OriginId = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/originıd", 0);
            query.ChildrenAges = XmlParse.GetIntegerListNodeValue(html.DocumentNode, "/query/childrenages/int32");
            var stopValue = XmlParse.GetStringNodeValue(html.DocumentNode, "/query/stop", "false");
            query.Stop = stopValue.ToLower() == "true";

            var startDateDay = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/startdateday", 0);
            var startDateMonth = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/startdatemonth", 0);
            var startDateYear = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/startdateyear", 0);

            var endDateDay = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/enddateday", 0);
            var endDateMonth = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/enddatemonth", 0);
            var endDateYear = XmlParse.GetIntegerNodeValue(html.DocumentNode, "/query/enddateyear", 0);

            query.StartDate = new DateTime(startDateYear, startDateMonth, startDateDay);
            query.EndDate = new DateTime(endDateYear, endDateMonth, endDateDay);

            query.Operators = XmlParse.GetStringListNodeValue(html.DocumentNode, "/query/operators/string");
            query.HotelCategories = XmlParse.GetIntegerListNodeValue(html.DocumentNode, "/query/hotelcategories/int32");
            query.Destination = XmlParse.GetStringNodeValue(html.DocumentNode, "/query/destination", "");
            query.Hotels = XmlParse.GetStringListNodeValue(html.DocumentNode, "/query/hotels/string");
            return query;
        }
    }
}
