using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerWindowApplication
{
    public class Feed
    {
        public string feedtime;
        public string symbol;        
        public string expiry;
        public string strike;
        public string callput;        
        public string exch;
        public string closePrice;
        public string ltp;
        public string quantity;

        public Feed(string symbol, string feedtime, string expiry, string callput, string strike,
            string exch, string closePrice, string ltp, string quantity)
        {
            this.symbol = symbol;
            this.feedtime = feedtime;
            this.expiry = expiry;
            this.callput = callput;
            this.strike = strike;
            this.exch = exch;
            this.closePrice = closePrice;
            this.ltp = ltp;
            this.quantity = quantity;            
        }
    }
}
