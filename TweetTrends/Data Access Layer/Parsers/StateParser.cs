using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsForms;
using TweetTrends.Business_Layer;

namespace TweetTrends.Data_Access_Layer.Parsers
{
    class StateParser
    {
        public static Dictionary<string, State> Parse(string filepath)
        {

            string jsonString = new StreamReader(filepath).ReadToEnd();
            Dictionary<string, List<List<List<double>>>> states = JsonConvert.DeserializeObject<Dictionary<string, List<List<List<double>>>>>(jsonString);

            Dictionary<string, State> sts = new Dictionary<string, State>();
            foreach(var pair in states)
            {
                sts.Add(pair.Key, new State(pair.Value));
            }

            return sts;
        }

        public static void GroupTweetsByStates(Dictionary<string, State> states, List<Tweet> tweets)
        {
            ClearStatesTweets(states);

            List<GMapPolygon> polygons = GetPolygons(states);
            foreach (Tweet tweet in tweets)
            {
                if (tweet != null)
                {
                    foreach (GMapPolygon polygon in polygons)
                    {
                        if (polygon.IsInside(tweet.Location))
                        {
                            states[polygon.Name].Tweets.Add(tweet);
                        }
                    }
                }
            }

            SetStatesWeight(states);

        }

        public static List<GMapPolygon> GetPolygons(Dictionary<string, State> states)
        {
            List<GMapPolygon> polygons = new List<GMapPolygon>();
            foreach (var state in states)
            {
                foreach (var polygon in state.Value.Polygons)
                {
                    List<PointLatLng> gMapPolygon_coords = new List<PointLatLng>();
                    foreach (var coords in polygon)
                    {
                        double y = coords[0];
                        double x = coords[1];
                        PointLatLng point = new PointLatLng(x, y);
                        gMapPolygon_coords.Add(point);
                    }
                    GMapPolygon gMapPolygon = new GMapPolygon(gMapPolygon_coords, state.Key);
                    polygons.Add(gMapPolygon);
                }
            }
            return polygons;
        }

        public static void SetStatesWeight(Dictionary<string, State> states)
        {
            foreach(var state in states)
            {
                foreach(Tweet tweet in state.Value.Tweets)
                {
                    state.Value.Weight += tweet.Weight;
                }
                state.Value.Weight /= state.Value.Tweets.Count;
            }
        }

        private static void ClearStatesTweets(Dictionary<string, State> states)
        {
            foreach(var state in states)
            {
                state.Value.Tweets.Clear();
            }
        }
    }
}
