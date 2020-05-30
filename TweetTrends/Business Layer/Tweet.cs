using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;

namespace TweetTrends.Business_Layer
{
    class Tweet
    {
        private PointLatLng location;
        private DateTime date;
        private string text;
        private double weight;


        public DateTime Date { get => date; set => date = value; }
        public string Text { get => text; set => text = value; }
        internal PointLatLng Location { get => location; set => location = value; }
        public double Weight { get => weight; set => weight = value; }

        public Tweet()
        {
            this.weight = 0;
            this.location = new PointLatLng();
        }
    }
}
