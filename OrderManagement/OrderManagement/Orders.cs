using QuickFix;
using QuickFix.Fields;
using System;

namespace OrderManagement
{
    enum state
    {
        NEW,
        SCRATCH,
        AMEND,
        CANCEL,
        REJECT,
        WORKING,
        COMPLETED
    }

    enum action
    {
        SAVE,
        CANCEL,
        PUBLISH
    }

    class Orders 
    {
        private bool isOrderAmendPossible(char [] orderStatus)
        {

            return false;
        }
        
        public Orders() { }
                
        public int addAck() {
            return 0;
        }

        public int addReject()
        {
            return 0;
        }

        public int addOrder(OrderStruct os)
        {
            OrderDAO odao = new OrderDAO();
            Console.WriteLine("Going to call DB");
            //os.display();
            int ordNo = odao.insertOrder(os);

            ClOrdID OrdId = new ClOrdID(Convert.ToString(ordNo));
            var order = new QuickFix.FIX42.NewOrderSingle(OrdId, new HandlInst('1'), new Symbol(os.symbol.ToString()), new Side(Side.BUY),
                new TransactTime(DateTime.Now.ToUniversalTime()), new OrdType(OrdType.LIMIT));

            //origOrdId = new OrigClOrdID(order.ClOrdID.ToString());
            order.Price = new Price((decimal)os.price);
            order.SetField(new OrderQty(1));

            Session.SendToTarget(order, FixClient.MySess);
            return ordNo;
        }

        public int amendOrder(ref OrderStruct os)
        {
            if (!isOrderAmendPossible(os.OrderStatus))
            {
                return -1;
            }
            OrderDAO ord = new OrderDAO();
            OrderStruct os1 = new OrderStruct();
            ord.getOrderFromDB(os.OrderID, ref os1);
            if (!os1.OrderStatus.Equals("COMPLETED"))
            {
                ord.amendOrder(ref os);
                return 0;
            }
            // check the conditons and then save the order.
            return -1;
        }
        
        public int cancelOrder(OrderStruct os) {//will add check in case of already filled, no cancel.
            OrderDAO ord = new OrderDAO();
            if (ord.cancelOrder(os) < 0) { return -1; }
            return 0;
        }

        public int addFills(OrderFillStruct ofs)
        {
            OrderDAO ord = new OrderDAO();
            ord.addOrderFills(ofs);
            return 0;
        }

        private int isPartillyFilled()
        {
            return 0;
        }

        private int isFullyFilled()
        {
            return 0;
        }

        private bool canCancelOrder()
        {
            return false;
        }
    }
}
