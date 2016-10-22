using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScannerWindowApplication
{
    class Subscriber
    {
        ScannerDashboard parentSD;
        public Subscriber(ScannerDashboard sd) { parentSD = sd; }
        public void ThreadB()
        {
            try
            {
                //string topic = args[0] == "All" ? "" : args[0];
                //string topic = "";
                //Console.WriteLine("Subscriber started for Topic : {0}", _topic);

                using (var subSocket = new SubscriberSocket())
                {
                    subSocket.Options.ReceiveHighWatermark = 1000;
                    subSocket.Connect("tcp://127.0.0.1:5551");
                    //subSocket.SubscribeToAnyTopic();
                    if (parentSD.dictFilters.Count == 0)
                    {
                        subSocket.SubscribeToAnyTopic();
                    }
                    else
                    {
                        foreach(var filters in parentSD.dictFilters)
                        {                            
                            Console.WriteLine("Subscribing Socket for Symbol : " + filters.Key);
                            subSocket.Subscribe(filters.Key);
                        }
                    }

                    Console.WriteLine("Subscriber socket connecting...");
                    while (true)
                    {
                        try
                        {
                            if (ScannerBox.openedMainForm == false)
                                break;
                            string messageReceived;
                            if (subSocket.TryReceiveFrameString(out messageReceived))
                            {
                                //string messageReceived = subSocket.ReceiveFrameString();
                                string[] arr = messageReceived.Split(',');
                                Feed feed = new Feed(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8]);
                                ScannerBox.qfeed.Enqueue(feed);
                                Console.WriteLine(messageReceived);
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
            finally
            {
                NetMQConfig.Cleanup();
            }
        }
    }
}
