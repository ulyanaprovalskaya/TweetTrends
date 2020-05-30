using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GMap.NET;
using TweetTrends.Business_Layer;

namespace TweetTrends.Data_Access_Layer.Parsers
{
    class TweetParser
    {
        private static Regex locationRegex = new Regex(@"[-]?[0-9]{1,3}[.][0-9]{1,15}");
        private static Regex dateRegex = new Regex(@"[0-9]{4}([-][0-9]{2}){2}.([0-9][0-9][:]){2}[0-9][0-9]"); 
        private static Regex textRegex = new Regex(@"[a-z0-9]+", RegexOptions.IgnoreCase);
        private static String commonRegex = @"([-]?[0-9]{1,3}[.][0-9]{1,15}[,]?|[0-9]{4}([-][0-9]{2}){2}.([0-9][0-9][:]){2}[0-9][0-9]|\[|\]|[_]|\t|\@[A-Za-z0-9]*|http://t.co/[A-Za-z0-9]*|[#])";

        static Db db;

        public TweetParser(string tweetsFilePath)
        {
            db = Db.GetInstance(tweetsFilePath);
        }

        public static List<Tweet> Parse(String filepath, Dictionary<string, List<Sentiment>> sentiments)
        {
            List<Tweet> tweets = new List<Tweet>();

            using (StreamReader reader = new StreamReader(filepath))
            {
                String row;
                while ((row = reader.ReadLine()) != null)
                {
                    Tweet tweet = GetTweet(row, locationRegex.Matches(row), dateRegex.Matches(row), textRegex.Matches(row));
                    tweets.Add(tweet);
                }
            }

            return GetAnalyzedTweets(tweets, sentiments);
        }

        private static Tweet GetTweet(String row, MatchCollection locationMatch, MatchCollection dateMatch, MatchCollection textMatch)
        {
            Tweet tweet = new Tweet();

            if (locationMatch.Count > 0)
            {
                tweet.Location = new PointLatLng(double.Parse(locationMatch[0].Value.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture),
                    double.Parse(locationMatch[1].Value.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture));
            }

            if(dateMatch.Count > 0)
            {
                tweet.Date = DateTime.Parse(dateMatch[0].Value);
            }

            tweet.Text = Regex.Replace(row, commonRegex, String.Empty).Trim().ToLower();

            return tweet;
        }

        public static List<Tweet> GetAnalyzedTweets(List<Tweet> tweets, Dictionary<string, List<Sentiment>> sentiments)
        {
            List<Tweet> analyzedTweets = new List<Tweet>();
            foreach (Tweet tweet in tweets)
            {
                if (AnalyzeTweetSentiment(tweet, sentiments))
                    analyzedTweets.Add(tweet);
            }

            return analyzedTweets;
        }

        public static bool AnalyzeTweetSentiment(Tweet tweet, Dictionary<string, List<Sentiment>> sentiments)
        {
            String[] words = tweet.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int wordsCount = 0;

            for (int i = 0; i < words.GetLength(0); i++)
            {
                if (words[i].Length > 1 && sentiments.ContainsKey(words[i].Substring(0, 1)))
                {
                    for (int j = i; j < words.GetLength(0) && j < i + 5; j++)
                    {
                        string phrase = ConcatWords(words, i, j);

                        bool phraseWasFound = false;
                        List<Sentiment> suitableSentiments = sentiments[words[i].Substring(0, 1)];

                        foreach (Sentiment s in suitableSentiments)
                        {
                            if (s.Phrase.Equals(words[i]))
                            {
                                phraseWasFound = true;
                                tweet.Weight += s.Value;
                                wordsCount++;
                                break;
                            }
                        }

                        if (phraseWasFound)
                        {
                            phraseWasFound = false;
                            i = j;
                            break;
                        }
                    }
                }
            }

            if (wordsCount == 0) return false;
            tweet.Weight /= wordsCount;
            return true;
        }

        private static string ConcatWords(string[] words, int firstWord, int lastWord)
        {
            string result = null;
            if (firstWord.Equals(lastWord)) return words[firstWord];
            for (int i = firstWord; i <= lastWord; i++)
            {
                result += words[i] + " ";
            }

            return result.Trim();
        }
    }
}
