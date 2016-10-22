using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using NetMQ;
using NetMQ.Sockets;

namespace netMQScanner
{
    class Program
    {
        public static void testNetMQ() {
            using (var server = new ResponseSocket())
            using (var client = new RequestSocket())
            {
                server.Bind("tcp://*:5555");
                client.Connect("tcp://localhost:5555");

                Console.WriteLine("Sending Hello");
                client.SendFrame("Hello");

                var message = server.ReceiveFrameString();
                Console.WriteLine("Received {0}", message);

                Console.WriteLine("Sending World");
                server.SendFrame("World");

                message = client.ReceiveFrameString();
                Console.WriteLine("Received {0}", message);
            }
        }
        public static void Main(string[] args)
        {
            try
            {
                //string topic = args[0] == "All" ? "" : args[0];
                string _topic = "VGH.IDX , XFI.IDX, XIBX.IDX, XID.IDX, XII.IDX";
                Console.WriteLine("Subscriber started for Topic : {0}", _topic);

                using (var subSocket = new SubscriberSocket())
                {
                    subSocket.Options.ReceiveHighWatermark = 1000;
                    subSocket.Connect("tcp://127.0.0.1:5551");
                    //subSocket.Subscribe(_topic);
                    subSocket.Subscribe("VGH.IDX");
                    subSocket.Subscribe("XFI.IDX");
                    subSocket.Subscribe("XIBX.IDX");
                    subSocket.Subscribe("XID.IDX");
                    subSocket.Subscribe("XII.IDX");
                    Console.WriteLine("Subscriber socket connecting...");
                    while (true)
                    {
                        //if (ScannerBox.openedMainForm == false)
                          //  break;
                        string messageReceived = subSocket.ReceiveFrameString();
                        string[] arr = messageReceived.Split(',');
                        //Feed feed = new Feed(arr[0], arr[1], arr[2], arr[3]);
                        //ScannerBox.qfeed.Enqueue(feed);
                        Console.WriteLine(messageReceived);
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
