using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetTrends.Business_Layer;

namespace TweetTrends.Data_Access_Layer.Parsers
{
    class SentimentParser
    {

        public static Dictionary<string, List<Sentiment>> Parse(string filepath)
        {
            List<Sentiment> sentiments = new List<Sentiment>();

            using (StreamReader reader = new StreamReader(filepath))
            {
                String row;
                while ((row = reader.ReadLine()) != null)
                {
                    string[] words = row.Split(',');
                    sentiments.Add(new Sentiment(words[0], double.Parse(words[1].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture)));
                }
            }

            return formSentimentDictionary(sentiments);
        }

        private static Dictionary<string, List<Sentiment>> formSentimentDictionary(List<Sentiment> sentiments)
        {
            Dictionary<string, List<Sentiment>> sentimentsDictionary = new Dictionary<string, List<Sentiment>>();

            foreach(Sentiment s in sentiments)
            {
                if(!sentimentsDictionary.ContainsKey(s.Phrase.Substring(0, 1)))
                    sentimentsDictionary.Add(s.Phrase.Substring(0, 1), new List<Sentiment>());

                sentimentsDictionary[s.Phrase.Substring(0, 1)].Add(s);
                
            }

            return sentimentsDictionary;
        }
    }
}
