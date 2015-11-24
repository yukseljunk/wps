using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PttLib.Domain;
using PttLib.Helpers;
using PttLib.Operators.Operator;
using PttLib.PttRequestResponse;

namespace PttLib.Tours
{
    public class BibloTourFactory : TourFactory
    {
        private const string BIBLO_URL = "http://www.bgoperator.ru";
        private const string NEXT_PAGE_LINK_XPATH = "//a[starts-with(@href,'/price.shtml?action=price')]";
        private const string NEXT_PAGE_LINK_ATTRIBUTE = "href";

        public BibloTourFactory() : base(null, null, null) { }

        protected override IList<Tour> GetToursFromHtml(IPttRequest pttRequest, IOperator op, HtmlDocument html, out TourFactoryStatus status)
        {
            status = new TourFactoryStatus { Success = true, MoveToNextHotel = false };

            if (html.DocumentNode.SelectNodes(op.TourInfo.XPath) == null)
            {
                status.MoveToNextHotel = true;
                return null;
            }

            var nextPageAnchorNode = html.DocumentNode.SelectNodes(NEXT_PAGE_LINK_XPATH);
            if (nextPageAnchorNode != null)
            {
                var nextPageAnchor = nextPageAnchorNode[0];
                var hrefAttr = nextPageAnchor.Attributes[NEXT_PAGE_LINK_ATTRIBUTE];
                if (hrefAttr != null)
                {
                    status.NextPageUrl = BIBLO_URL + hrefAttr.Value;
                }
            }
            else
            {
                status.MoveToNextHotel = true;
            }

            var list = new List<Tour>();
            foreach (HtmlNode node in html.DocumentNode.SelectNodes(op.TourInfo.XPath))
            {
                if (node.FirstChild.Name.ToLower() == "th") continue;

                var tourItem = new Tour();

                tourItem.City = Dictionnaries.Regions[op.QueryObject.OriginId];
                tourItem.TO = op.Name.Replace("_", "");//anex_ gibiler icin
                tourItem.IssueDate = DateTime.Now;

                tourItem.Date = op.TourInfo.GetFieldValue<DateTime>(node, "Date");
                tourItem.Night = op.TourInfo.GetFieldValue<string>(node, "Night");
                tourItem.Hotel = op.TourInfo.GetFieldValue<string>(node, "Hotel");
                tourItem.ACC = op.QueryObject.NumberOfAdults + " AD";
                if(op.QueryObject.ChildrenAges != null && op.QueryObject.ChildrenAges.Any())
                {
                    tourItem.ACC += op.QueryObject.ChildrenAges.Count().ToString() + " CHD";
                }

                var mealTypeIndex = 7;
                tourItem.Meal = "-";
                var roomType = node.ChildNodes[5].ChildNodes[0].InnerText.Trim();
                if (roomType.Length > 2)
                {

                    var innerMealIndex = 0;
                    foreach (var mealtypeAlternative in op.MealTypes)
                    {
                        foreach (var mealType in mealtypeAlternative.OrderByDescending(a => a.Length))
                            {
                                if (roomType.EndsWith(mealType))
                                {
                                    roomType = roomType.Substring(0, roomType.Length - mealType.Length).Trim();
                                    mealTypeIndex = innerMealIndex;
                                    break;
                                }
                            }
                           
                        innerMealIndex++;
                    }
                }
                tourItem.Meal = Dictionnaries.MealTypes[mealTypeIndex];
                tourItem.RoomType = roomType;


                tourItem.Price = 0;
                var price = node.ChildNodes[7].InnerText.Replace("*", "").Trim();//2 adult icin 2162&nbsp;&nbsp;2 ruscabiseyler geliyor bu
                if (!string.IsNullOrEmpty(price))
                {
                    if (op.QueryObject.ChildrenAges != null && op.QueryObject.ChildrenAges.Count > 0 &&
                        price.IndexOf("[") > -1)
                    {
                        //TODO: bunu degistirmisler ne idugu belirsiz...
                        price = price.Replace("&nbsp;", "");
                        var priceList = new List<string>();
                        var priceConverted = price.Replace("][", "$_$");
                        var priceConvertedPieces = priceConverted.Split(new[] { "]" },
                                                                        StringSplitOptions.RemoveEmptyEntries);
                        foreach (var priceConvertedPiece in priceConvertedPieces)
                        {
                            priceList.Add(priceConvertedPiece.Replace("$_$", "][") + "]");
                        }

                        var priceRangeValueFactory = new PriceRangeValueFactory();
                        var priceRangeValuesCalcd = priceRangeValueFactory.RangeValues(priceList);
                        var priceRangeCalculator = new PriceRangeValueCalculator(priceRangeValuesCalcd);

                        tourItem.Price = priceRangeCalculator.GetPrice(op.QueryObject.ChildrenAges);

                    }
                    else
                    {
                        var matchDouble = Regex.Match(price, @"(\d+)", RegexOptions.IgnoreCase);
                        if (matchDouble.Success)
                        {
                            price = matchDouble.Groups[1].Value;
                        }

                        tourItem.Price = Convert.ToDouble(price, CultureInfo.GetCultureInfo("tr-TR"));
                    }

                }

                if (op.QueryObject != null && tourItem.Price < op.QueryObject.MinPrice) continue;
                tourItem.SPONo = op.TourInfo.GetFieldValue<string>(node, "SPONo"); 
                if (op.QueryObject != null)
                {
                    if ((tourItem.Price >= op.QueryObject.MinPrice && tourItem.Price <= op.QueryObject.MaxPrice) && (tourItem.Date >= op.QueryObject.StartDate && tourItem.Date<=op.QueryObject.EndDate))
                    //                            &&(QueryObject.MealType == mealTypeIndex || QueryObject.MealType == 0)) //biblo da uyusmazliklar mevcut... ai sectiginde uai ler de geliyor....
                    {
                        list.Add(tourItem);
                    }
                }
            }
            return list;
        }

        public override IList<Tour> Create(IPttRequest request, IOperator op, out TourFactoryStatus status)
        {
            if(!Biblo.TourDates.Contains(op.QueryObject.StartDate))
            {
                status = new TourFactoryStatus { Success = true, MoveToNextHotel = true};
                return null;
            }
            return base.Create(request, op, out status);
        }
    }
    #region PRICERANGECALC
    class PriceRangeValueCalculator
    {
        private readonly IList<PriceRangeValue> _priceRanges;

        public PriceRangeValueCalculator(IList<PriceRangeValue> priceRanges)
        {
            _priceRanges = priceRanges;
        }

        public int GetPrice(IList<int> points)
        {
            foreach (var point in points)
            {
                foreach (var priceRangeValue in _priceRanges)
                {
                    foreach (var priceRange in priceRangeValue.Ranges.Where(r => !r.Hit))
                    {
                        if (priceRange.IsInRange(point))
                        {
                            priceRange.Hit = true;
                            break;
                        }
                    }
                }
            }
            foreach (var priceRangeValue in _priceRanges)
            {
                var allHit = priceRangeValue.Ranges.All(priceRange => priceRange.Hit);
                if (allHit)
                {
                    return priceRangeValue.Value;
                }
            }

            return 0;
        }
    }
    class PriceRangeValueFactory
    {
        public IList<PriceRangeValue> RangeValues(IList<string> values)
        {
            var result = new List<PriceRangeValue>();
            foreach (var value in values)
            {
                var matchDouble = Regex.Match(value, @"(\d+)\[(\d+)\-(\d+)\]\[(\d+)\-(\d+)\]",
                       RegexOptions.IgnoreCase);
                if (matchDouble.Success)
                {
                    var rangeValue = matchDouble.Groups[1].Value;
                    var start1 = matchDouble.Groups[2].Value;
                    var end1 = matchDouble.Groups[3].Value;
                    var start2 = matchDouble.Groups[4].Value;
                    var end2 = matchDouble.Groups[5].Value;
                    int rangeValueConverted;
                    int startConverted1;
                    int endConverted1;
                    int startConverted2;
                    int endConverted2;
                    if (Int32.TryParse(rangeValue, out rangeValueConverted) &&
                        Int32.TryParse(start1, out startConverted1) &&
                        Int32.TryParse(end1, out endConverted1) &&
                        Int32.TryParse(start2, out startConverted2) &&
                        Int32.TryParse(end2, out endConverted2))
                    {
                        result.Add(new PriceRangeValue()
                        {
                            Value = rangeValueConverted,
                            Ranges =
                                new List<PriceRange>() { 
                                    new PriceRange() { Start = startConverted1, End = endConverted1 },
                                    new PriceRange() { Start = startConverted2, End = endConverted2 }
                                }
                        });
                    }
                }
                else
                {
                    var matchSingle = Regex.Match(value, @"(\d+)\[(\d+)\-(\d+)\]",
                                                  RegexOptions.IgnoreCase);

                    // Here we check the Match instance.
                    if (matchSingle.Success)
                    {
                        // Finally, we get the Group value and display it.
                        var rangeValue = matchSingle.Groups[1].Value;
                        var start = matchSingle.Groups[2].Value;
                        var end = matchSingle.Groups[3].Value;
                        int rangeValueConverted;
                        int startConverted;
                        int endConverted;
                        if (Int32.TryParse(rangeValue, out rangeValueConverted) &&
                            Int32.TryParse(start, out startConverted) && Int32.TryParse(end, out endConverted))
                        {
                            result.Add(new PriceRangeValue()
                            {
                                Value = rangeValueConverted,
                                Ranges =
                                    new List<PriceRange>() { new PriceRange() { Start = startConverted, End = endConverted } }
                            });
                        }

                    }
                }

            }

            return result;
        }
    }
    class PriceRangeValue
    {
        public int Value { get; set; }
        public IList<PriceRange> Ranges { get; set; }
    }
    class PriceRange
    {
        public int Start { get; set; }
        public int End { get; set; }
        public bool IsInRange(int point)
        {
            return point >= Start && point < End;
        }

        public bool Hit { get; set; }
    }
    #endregion
}