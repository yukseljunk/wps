using System;
using System.Collections.Generic;
using System.Globalization;

namespace PttLib.Operators.Query
{
    public  class Query
    {
        private const int DATE_PARTITION_DAYS = 1;

        public int NumberOfAdults { get; set; }
       
        public List<int> ChildrenAges { get; set; }
        public List<string> Operators { get; set; }
        public List<string> Hotels { get; set; }
 
        private CultureInfo _cultureInfo;
        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
        }

        public string Currency { get; set; }
        public bool Stop { get; set; }
        public List<string> ChildExpectedBirthDates {
            get
            {
                var result = new List<string>();
                if (ChildrenAges == null) return null;
                foreach (var childrenAge in ChildrenAges)
                {
                    var expectedBirthDay = DateTime.Today.AddYears(-1*childrenAge);
                    var expectedBirthDayConverted = _cultureInfo == null ? expectedBirthDay.ToString("d") : expectedBirthDay.ToString("d", _cultureInfo);
                    result.Add(expectedBirthDayConverted);
                }
                return result;
            }
        }


        public int MealType { get; set; }
        

        public List<int> HotelCategories { get; set; }
        /*
         
         2- 2*
         3- 3*
         4- 4*
         5- 5*
         6- any
         */
        public int OriginId { get; set; }
        public string Destination { get; set; }
        //for future use
        public string ExtraInfo { get; set; }



        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }

        public int MinNights { get; set; }
        public int MaxNights { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        private Dictionary<DateTime, DateTime> DatesPartitioned(int partitioningDays)
        {
                var result = new Dictionary<DateTime, DateTime>();
                var totalDays = (EndDate - StartDate).Days + 1;
                for (int dateIndex = 0; dateIndex < totalDays; dateIndex += partitioningDays)
                {
                    var partitionEndDate = StartDate.AddDays(dateIndex + partitioningDays - 1);
                    if (partitionEndDate > EndDate) partitionEndDate = EndDate;
                    result.Add(StartDate.AddDays(dateIndex), partitionEndDate);
                }
                return result;
        }
        
        public List<DateTime> StartDatesPartitioned(int partitioningDays)
        {
          
                var result = new List<DateTime>();
                foreach (KeyValuePair<DateTime, DateTime> dateTime in DatesPartitioned(partitioningDays))
                {
                    result.Add(dateTime.Key);
                }
                return result;
           
        }

        public List<DateTime> EndDatesPartitioned(int partitioningDays)
        {

            var result = new List<DateTime>();
            foreach (KeyValuePair<DateTime, DateTime> dateTime in DatesPartitioned(partitioningDays))
            {
                result.Add(dateTime.Value);
            }
            return result;

        }
    
        public string StartDateShort
        {
            get
            {
                return _cultureInfo == null ? StartDate.ToString("d") : StartDate.ToString("d", _cultureInfo);
            }
        }
        public string EndDateShort
        {
            get
            {
                return _cultureInfo == null ? EndDate.ToString("d") : EndDate.ToString("d", _cultureInfo);
            }
        }
        public string StartDate8
        {
            get
            {
                return _cultureInfo == null ? StartDate.ToString("yyyyMMdd") : StartDate.ToString("yyyyMMdd", _cultureInfo);
            }
        }
        public string EndDate8
        {
            get
            {
                return _cultureInfo == null ? EndDate.ToString("yyyyMMdd") : EndDate.ToString("yyyyMMdd", _cultureInfo);
            }
        }
        public string StartDateYmd
        {
            get
            {
                return _cultureInfo == null ? StartDate.ToString("yyyy-MM-dd") : StartDate.ToString("yyyy-MM-dd", _cultureInfo);
            }
        }
        public string EndDateYmd
        {
            get
            {
                return _cultureInfo == null ? EndDate.ToString("yyyy-MM-dd") : EndDate.ToString("yyyy-MM-dd", _cultureInfo);
            }
        }
        
        public string StartDateDay
        {

            get { return StartDate.Day < 10 ? "0" + StartDate.Day.ToString(_cultureInfo) : StartDate.Day.ToString(_cultureInfo); }
        }
        public string StartDateMonth
        {

            get { return StartDate.Month < 10 ? "0" + StartDate.Month.ToString(_cultureInfo) : StartDate.Month.ToString(_cultureInfo); }
        }
        public string StartDateYear
        {

            get { return StartDate.Year.ToString(_cultureInfo); }
        }

        public string EndDateDay
        {

            get { return EndDate.Day < 10 ? "0" + EndDate.Day.ToString(_cultureInfo) : EndDate.Day.ToString(_cultureInfo); }
        }
        public string EndDateMonth
        {

            get { return EndDate.Month < 10 ? "0" + EndDate.Month.ToString(_cultureInfo) : EndDate.Month.ToString(_cultureInfo); }
        }
        public string EndDateYear
        {

            get { return EndDate.Year.ToString(_cultureInfo); }
        }


        public Query Clone()
        {
            var query = new Query
                {
                    NumberOfAdults = this.NumberOfAdults,
                    ChildrenAges = this.ChildrenAges,
                    _cultureInfo = this._cultureInfo,
                    MealType = this.MealType,
                    HotelCategories = this.HotelCategories,
                    OriginId = this.OriginId,
                    Destination = this.Destination,
                    ExtraInfo = this.ExtraInfo,
                    MinPrice = this.MinPrice,
                    MaxPrice = this.MaxPrice,
                    MinNights = this.MinNights,
                    MaxNights = this.MaxNights,
                    StartDate = this.StartDate,
                    EndDate = this.EndDate,
                    Currency = this.Currency
                };
            return query;
        }

    }
}
