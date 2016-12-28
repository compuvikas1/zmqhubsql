using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace OMClient
{
    class Program
    {
        public static void stringToStruct(string clientString, ref OrderStruct ord)
        {
            string[] toks = clientString.Split(':');
            //foreach (string tok in toks) { Console.WriteLine("["+tok+"]"); }
            if (toks[0].Equals("INS") && toks.Length >= 7)
            {
                /*ord.methodID = 1;

                ord.symbol = "AAPL0000".ToCharArray();// toks[1].ToCharArray();
                ord.OrderStatus = "NEW00000".ToCharArray();
                ord.price = (float)Convert.ToDouble(toks[2].ToString());
                ord.quantity = (float)Convert.ToDouble(toks[3].ToString());
                ord.direction = toks[4][0];
                ord.machineID = Convert.ToInt16(toks[5].ToString());
                ord.userID = Convert.ToInt16(toks[5].ToString());*/


                ord.methodID = 1;
                ord.symbol = toks[1].ToCharArray();
                ord.OrderStatus = "NEW00000".ToCharArray();
                ord.price = (float)Convert.ToDouble(toks[2].ToString());
                ord.quantity = (float)Convert.ToDouble(toks[3].ToString());
                ord.direction = toks[4][0];
                ord.machineID = Convert.ToInt16(toks[5].ToString());
                ord.userID = Convert.ToInt16(toks[5].ToString());

            }
            if (toks[0].Equals("CAN") && toks.Length >= 3)
            {
                ord.methodID = 2;
                ord.symbol = toks[1].ToCharArray();
                ord.OrderID = (int)Convert.ToDecimal(toks[2].ToString());
            }
        }
        static byte[] getBytes(OrderStruct str)
        {
            try
            {
                int size = Marshal.SizeOf(typeof(OrderStruct));
                Console.WriteLine("Size of str : {0} ", size);

                byte[] arr = new byte[size];

                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(str, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
                Marshal.FreeHGlobal(ptr);
                return arr;
            } catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message);
                return null;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Format : INS:SYM:Price:Quantity:direction:machineID:userID\n");
            //INS:AAPL0000:111.90:1:B:1:1
            //CAN:AAPL0000:(ORDID):111.90:1:B:1:1

            //Console.WriteLine("Format : INS:NYM12:1023.456773:.345:B:VIK123:25\n Response : SUCCESS:INS:ORDID:");
            //Console.WriteLine("Format : AMD:2009:NYM12:1025.456773:.345:B:VIK123:25\n");
            int OrderNature = 1;
            Console.WriteLine("argumenst : "+args.Length);
            if(args.Length >= 1) {
                OrderNature = 2;
            }
            try
            {
                byte[] bytesFrom = new byte[65537];
                string reqType = null;
                Byte[] sendBytes = null;

                OrderStruct os = new OrderStruct(8,8);
                if(OrderNature == 1)
                {
                    stringToStruct("INS:AAPL0000:111.90:1:B:21:25", ref os);
                } else {
                    stringToStruct("CAN:AAPL0000:"+args[0]+ ":111.90:1:B:21:25", ref os);
                }                
                os.display();

                TcpClient clientSocket = new TcpClient();
                Console.WriteLine("Connecting.....");

                clientSocket.Connect("127.0.0.1", 5552);
                // use the ipaddress as in the server program

                Console.WriteLine("Connected ...");
                
                NetworkStream networkStream = clientSocket.GetStream();

                sendBytes = (getBytes(os));

                networkStream.Write(sendBytes, 0, sendBytes.Length);
                Console.WriteLine("written "+sendBytes.Length+" bytes.");
                networkStream.Flush();

                System.Threading.Thread.Sleep(200);

                networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                reqType = System.Text.Encoding.ASCII.GetString(bytesFrom);
                Console.WriteLine("Received : "+reqType);
                clientSocket.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
    }
}
