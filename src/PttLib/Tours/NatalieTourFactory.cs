using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PttLib.Domain;
using PttLib.Helpers;
using PttLib.PttRequestResponse;

namespace PttLib.Tours
{
    public class NatalieTourFactory : TourFactory
    {
        public NatalieTourFactory()
            : base(null, null, null)
        {
        }

        protected override IList<Tour> GetToursFromHtml(IPttRequest pttRequest, IOperator op, HtmlDocument html, out TourFactoryStatus status)
        {
            status = new TourFactoryStatus { Success = true, MoveToNextHotel = false };

            if (html.DocumentNode.SelectNodes(op.TourInfo.XPath) == null)
            {
                status.MoveToNextHotel = true;
                return null;
            }

            var list = new List<Tour>();
            var hotelName = html.DocumentNode.SelectNodes("//div[@id='fs-ed-hHtl']")[0].InnerText;

            //kur degerini cek sayfadan
            //fm.rate.setRates([102,101,0.029542097489, den kuru cek, bu katsayi ile carpip dolar degerini bulmus olursun.
            double usdToRubRate = 1.0;// 0.029542097489;
            var curId = 101;
            if (op.QueryObject.Currency == "EUR") curId = 103;
            //102,101, usd rate
            //102,103, eur rate
            //var match = Regex.Match(html.DocumentNode.OuterHtml, @"setRates\(\[\d+\,\d+\,([\d\.]+)\s*", RegexOptions.IgnoreCase);
            var match = Regex.Match(html.DocumentNode.OuterHtml, @"102\," + curId + @"\,([\d\.]+)\s*", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                NumberStyles style;
                style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;

                string key = match.Groups[1].Value;
                double.TryParse(key, style, CultureInfo.CreateSpecificCulture("en-GB"), out usdToRubRate);
            }

            foreach (var node in html.DocumentNode.SelectNodes(op.TourInfo.XPath))
            {
                var startIndex = 0;
                var tourItem = new Tour();
                tourItem.Date = op.QueryObject.StartDate;
                var priceColIndex = 7;
                if (op.QueryObject.MinNights != op.QueryObject.MaxNights)
                {
                    priceColIndex = 9;
                }
                if (!node.ChildNodes[2 * startIndex + priceColIndex].InnerText.Contains("RUB"))//otel adi kolonu var
                {
                    startIndex = 1;
                }
                var night = node.ChildNodes[2 * startIndex + 7].InnerText;
                if (op.QueryObject.MinNights != op.QueryObject.MaxNights)
                {
                    match = Regex.Match(night, @"(\d+)\s*", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        tourItem.Night = match.Groups[1].Value;
                    }

                }
                else
                {
                    tourItem.Night = op.QueryObject.MinNights.ToString();
                }

                tourItem.Hotel = hotelName;

                var roomType = node.ChildNodes[2 * startIndex + 3].InnerText;
                tourItem.RoomType = roomType;

                var price = node.ChildNodes[2 * startIndex + priceColIndex].InnerText.Replace("RUB", "").Trim();
                tourItem.Price = 0;
                if (!string.IsNullOrEmpty(price))
                {
                    tourItem.Price = System.Convert.ToDouble(price, CultureInfo.GetCultureInfo("tr-TR"));
                }
                tourItem.Price = Math.Round(usdToRubRate * tourItem.Price, 0);

                var meal = node.ChildNodes[2 * startIndex + 5].InnerText;
                tourItem.Meal = meal;
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
         
                tourItem.City = Dictionnaries.Regions[op.QueryObject.OriginId];
                tourItem.ACC = node.ChildNodes[2 * startIndex + 1].InnerText; //"DBL + 1 CHD";

                tourItem.TO = op.Name;
                tourItem.SPONo = "";
                tourItem.IssueDate = DateTime.Now;

                if (tourItem.Price > op.QueryObject.MaxPrice)
                {
                    status.MoveToNextHotel = true;
                    break;
                }

                if (op.QueryObject != null && (op.QueryObject.MealType == mealTypeIndex || op.QueryObject.MealType == 0) && (tourItem.Price >= op.QueryObject.MinPrice))
                {
                    list.Add(tourItem);
                }
            }
            return list;
        }
    }
}