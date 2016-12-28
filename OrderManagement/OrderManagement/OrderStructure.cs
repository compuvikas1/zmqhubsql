using System;
using System.Runtime.InteropServices;

namespace OrderManagement
{
    public struct OrderFillStruct
    {
        public int OrderID { get; set; }
        public int FillID { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public float FilledQuantity { get; set; }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct OrderStruct
    {
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] OrderStatus;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] symbol;

        public int methodID;
        public int OrderID;
        public float price;
        public float quantity;
        public char direction;
        public int version;
        public int machineID;
        public int userID;

        public OrderStruct(int isym, int ios)
        {
            OrderStatus = new char[ios];
            symbol = new char[isym];
            methodID = 0;
            OrderID = 0;
            price = 0;
            quantity = 0;
            direction = '\0';
            version = 0;
            machineID = 0;
            userID = 0;
        }

        public void display()
        {
            Console.WriteLine("OrderStatus : {0}", new string(OrderStatus));
            Console.WriteLine("symbol : {0}", new string(symbol));
            Console.WriteLine("methodID : {0}", methodID);
            Console.WriteLine("OrderID : {0}", OrderID);
            Console.WriteLine("price : {0}", price);
            Console.WriteLine("quantity : {0}", quantity);
            Console.WriteLine("direction : {0}", direction);
            Console.WriteLine("machineID : {0}", machineID);
            Console.WriteLine("userID : {0}", userID);
        }
    };

    public enum RequestType
    {
        INS, AMD, CAN, UNK
    };
}
