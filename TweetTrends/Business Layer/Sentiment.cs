using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetTrends.Business_Layer
{
    class Sentiment
    {
        private string phrase;
        private double value;

        public string Phrase { get => phrase; set => phrase = value; }
        public double Value { get => value; set => this.value = value; }

        public Sentiment()
        {

        }

        public Sentiment(string phrase, double value)
        {
            this.phrase = phrase;
            this.value = value;
        }
    }
}
