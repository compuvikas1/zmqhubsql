﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerWindowApplication
{
    public class Feed
    {
        public string symbol;
        public string feedtime;
        public string expiry;
        public string callput;
        public string strike;
        public string bidSize;
        public string bidPrice;
        public string askPrice;
        public string askSize;
        public string volume;

        public Feed(string symbol, string feedtime, string expiry, string callput, string strike,
            string bidSize, string bidPrice, string askSize, string askPrice, string volume)
        {
            this.symbol = symbol;
            this.feedtime = feedtime;
            this.expiry = expiry;
            this.callput = callput;
            this.strike = strike;
            this.bidSize = bidSize;
            this.bidPrice = bidPrice;
            this.askPrice = askPrice;
            this.askSize = askSize;
            this.volume = volume;
        }
    }
}
