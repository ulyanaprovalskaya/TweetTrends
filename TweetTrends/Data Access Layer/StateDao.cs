using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsForms;
using TweetTrends.Business_Layer;
using TweetTrends.Data_Access_Layer.Parsers;

namespace TweetTrends.Data_Access_Layer
{
    class StateDao
    {
        Db db;

        public StateDao(string filepath)
        {
            db = Db.GetInstance(filepath);
            db.Tweets = TweetParser.Parse(filepath, db.Sentiments);
            StateParser.GroupTweetsByStates(db.States, db.Tweets);
        }

        public Dictionary<string, State> GetStates()
        {
            return db.States;
        }

        public List<GMapPolygon> GetPolygons()
        {
            return StateParser.GetPolygons(db.States);
        }

        

    }
}
