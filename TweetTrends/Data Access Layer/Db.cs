using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetTrends.Business_Layer;
using TweetTrends.Data_Access_Layer.Parsers;

namespace TweetTrends.Data_Access_Layer
{
    class Db
    {
        string sentimentsFilepath = @"Data/sentiments.csv";
        string statesFilepath = @"Data/states.json";

        private List<Tweet> tweets;
        private Dictionary<string, State> states;
        private Dictionary<string, List<Sentiment>> sentiments;

        private static Db db;


        internal Dictionary<string, State> States { get => states;}
        internal Dictionary<string, List<Sentiment>> Sentiments { get => sentiments;}
        internal List<Tweet> Tweets { get => tweets; set => tweets = value; }

        public static Db GetInstance(string tweetsFilepath)
        {
            if (db == null)
            {
                db = new Db(tweetsFilepath);
                return db;
            }
            else return db;
        }

        private Db(String tweetsFilepath)
        {
            sentiments = SentimentParser.Parse(sentimentsFilepath);
            states = StateParser.Parse(statesFilepath);
        }
        
    }
}
