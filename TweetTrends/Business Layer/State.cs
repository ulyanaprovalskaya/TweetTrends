using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetTrends.Business_Layer
{
    class State
    {
        private List<Tweet> tweets;
        private double weight;
        private List<List<List<double>>> polygons;

        internal List<Tweet> Tweets { get => tweets; set => tweets = value; }
        public double Weight { get => weight; set => weight = value; }

        public List<List<List<double>>> Polygons { get => polygons; set => polygons = value; }

        public State()
        {
            tweets = new List<Tweet>();
            polygons = new List<List<List<double>>>();
            weight = 0;
        }

        public State(List<List<List<double>>> polygons)
        {
            this.polygons = polygons;
            weight = 0;
            tweets = new List<Tweet>();
        }
    }
}
