using System;

namespace PttLib.Tours
{
    public class Tour
    {
        public DateTime Date { get; set; }
        public string Night { get; set; }
        public string Hotel { get; set; }
        public string HotelCommonName { get; set; }
        public double Price { get; set; }
        public string Meal { get; set; }
        public string RoomType { get; set; }
        public string ACC { get; set; }
        public string SPONo { get; set; }
        public string TO { get; set; }
        public string City { get; set; }
        public DateTime IssueDate { get; set; }

        public override string ToString()
        {
            return string.Format("Date:{0},Night:{1},Hotel:{2},Price:{3},Meal:{4},RoomType:{5},Acc:{6}", Date.ToShortDateString(),Night, Hotel, Price, Meal, RoomType, ACC);
        }
    }
}
