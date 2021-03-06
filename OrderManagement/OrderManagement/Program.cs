﻿using System;
using System.Data.SqlClient;
using System.Text;

using NetMQ;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;
using QuickFix;

namespace OrderManagement
{
    class fixApp
    {
        QuickFix.Transport.SocketInitiator initiator;
        QuickFix.Application myApp;
        QuickFix.SessionSettings settings;
        private int g_nCounter;
        private string traderAccount;

        [StructLayout(LayoutKind.Sequential)]
        public class SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime([In, Out] SYSTEMTIME st);

        public void initFix()
        {
            settings = new QuickFix.SessionSettings(@"tradeclient.cfg");
            myApp = new FixClient();
            MessageStoreFactory storeFactory = new FileStoreFactory(settings);
            LogFactory logFactory = new FileLogFactory(settings);

            initiator = new QuickFix.Transport.SocketInitiator(myApp, storeFactory, settings, logFactory);
            initiator.Start();
        }

        public void closeFix()
        {
            initiator.Stop();
        }
    }

    class Program
    {        
        static void Main(string[] args)
        {
            //Fix part

            fixApp fx = new fixApp();
            fx.initFix();

            IPAddress ipaddr = IPAddress.Parse("127.0.0.1");
            TcpListener serverSocket = new TcpListener(ipaddr, 5551);
            TcpClient clientSocket = default(TcpClient);
            Console.WriteLine("Going to receive Order");
            try
            {
                int counter = 0;
                serverSocket.Start();
                Console.WriteLine(" >> " + "Server Started\n >> Format : ORD:3:[INS|UPT|CAN]:struct body\n");
                counter = 0;
                while (true)
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();
                    Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                    clientHandler client = new clientHandler();
                    client.startClient(clientSocket, Convert.ToString(counter));
                }                            
            }
            finally
            {
                NetMQConfig.Cleanup();
                clientSocket.Close();
                serverSocket.Stop();
                Console.WriteLine(" >> " + "exit");
                Console.ReadLine();
            }            
        }

        public class clientHandler
        {
            TcpClient clientSocket;
            string clNo;
            
            public void startClient(TcpClient inClientSocket, string clineNo)
            {
                this.clientSocket = inClientSocket;
                this.clNo = clineNo;
                Thread ctThread = new Thread(doProcessing);
                ctThread.Start();
            }

            private RequestType getReqType(int req)
            {
                if (req == 1)
                {
                    return RequestType.INS;
                }
                else if (req == 2)
                {
                    return RequestType.CAN;
                }
                else if (req == 3)
                {
                    return RequestType.AMD;
                }
                else
                {
                    return RequestType.UNK;
                }
            }

            private OrderStruct getStructFromBytes(byte [] bytesFrom)
            {
                IntPtr ptPoit = Marshal.AllocHGlobal(bytesFrom.Length);
                Marshal.Copy(bytesFrom, 0, ptPoit, bytesFrom.Length);
                OrderStruct os = (OrderStruct)Marshal.PtrToStructure(ptPoit, typeof(OrderStruct));
                //Console.WriteLine("Order : "+os.methodID);
                os.display();
                return os;
            }

            private void doProcessing()
            {
                byte[] bytesFrom = new byte[10025];
                string reqType = null;
                Byte[] sendBytes = null;
                string serverResponse = null;
                try
                {
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    reqType = System.Text.Encoding.ASCII.GetString(bytesFrom, 0, 10);
                    Console.WriteLine(" >> " + "From client-" + clNo + reqType);
                    OrderStruct os = getStructFromBytes(bytesFrom);
                    RequestType rt = getReqType(os.methodID);
                    
                    switch (rt)
                    {
                        case RequestType.INS:
                            //DO inert activity
                            //OrderStruct osIns = getStructFromBytes(bytesFrom);
                            Orders ord = new Orders();
                            int OrderNo = -1;
                            if((OrderNo = ord.addOrder(os)) < 0)
                            {
                                serverResponse = "FAILURE:INS:ORDID:" + Convert.ToString(OrderNo);
                            } else
                            {
                                serverResponse = "SUCCESS:INS:ORDID:" + Convert.ToString(OrderNo);
                            }
                            break;
                        case RequestType.AMD:
                            //Do amendment activity, cancel the previous and boo new.
                            //OrderStruct osAmd = getStructFromBytes(bytesFrom);
                            Orders AmdOrd = new Orders();
                            int newOrderNo = -1;
                            if((newOrderNo = AmdOrd.amendOrder(ref os)) < 0) {
                                serverResponse = "FAILURE:AMD:OLD-ORDID:" + Convert.ToString(os.OrderID);
                            } else
                            {
                                serverResponse = "SUCCESS:AMD:OLD-ORDID:" + Convert.ToString(os.OrderID)+":NEW-ORDID:"+newOrderNo;
                            }                            
                            break;
                        case RequestType.CAN:
                            //Do cancel activity
                            Orders ordCan = new Orders();
                            if (ordCan.cancelOrder(os) < 0)
                            {
                                serverResponse = "FAILURE:CAN:ORDID:" + Convert.ToString(os.OrderID);
                            }
                            else
                            {
                                serverResponse = "SUCCESS:CAN:ORDID:" + Convert.ToString(os.OrderID);
                            }
                            break;
                        default:
                            //Retrn generic error
                            reqType = System.Text.Encoding.ASCII.GetString(bytesFrom);
                            serverResponse = "ERROR:UNK:ORDID:[" + reqType + "]";
                            break;
                    }
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    this.clientSocket.Close();
                    Console.WriteLine(" >> Closing connection : " + serverResponse);                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception :  >> " + ex.ToString());
                }
            }
        }
    }
}
