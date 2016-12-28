using System;
using System.Data;
using System.Data.SqlClient;
//TODO: check if conn is not valid, again open teh connection.
namespace OrderManagement
{
    class OrderDAO
    {
        private SqlConnection conn;

        public OrderDAO()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "SONY-VAIO";
            builder.InitialCatalog = "THOM";
            builder.UserID = "zmq1";
            builder.Password = "test123";
            conn = new SqlConnection(builder.ToString());
            conn.Open();
        }

        private void refreshConnection()
        {
            if(conn.State != ConnectionState.Open)
            {
                conn.Close();
                conn.Open();
            }
        }

        private int getNextSeq()
        {
            try
            {
                //refreshConnection();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "getNextVal";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("value", ""));

                    var returnParameter = cmd.Parameters.Add("@value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();
                    var tnp = returnParameter.Value;
                    return Convert.ToInt32(tnp);
                }
            }
            catch (Exception ex)
            {
                Console.Write("Some exception : " + ex.Message);
            }
            return -1;
        }

        public int insertOrder(OrderStruct os)
        {
            try
            {
                using (var cmd = new SqlCommand())
                {
                    //refreshConnection();
                    cmd.Connection = conn;
                    int nextSeq = getNextSeq();
                    Console.WriteLine("Next Seq : {0}",nextSeq);
                    string symbol = new string(os.symbol);
                    //string ordStatus = new string(os.OrderStatus);
                    string query = "INSERT INTO ORDERS (OrderID,OrderStatus,symbol,price, quantity,direction,version,machineID,userID) VALUES ("
                        + nextSeq + "," + "'NEW'" + "," + "'"+ symbol +"'" + "," + os.price + "," + os.quantity + ",'" + os.direction + "',1,"
                        + os.machineID + "," + os.userID + ");";
                    cmd.CommandText = query;
                    Console.WriteLine("query : {0}", query);
                    cmd.ExecuteNonQuery();
                    return nextSeq;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Exception (Insert) : " + ex.Message);
            }
            return -1;
        }

        public int amendOrder(ref OrderStruct os)//Update the previouss and add new
        {
            try
            {
                using (var cmd = new SqlCommand())
                {
                    //refreshConnection();
                    cmd.Connection = conn;
                    int nextSeq = getNextSeq();
                    string symbol = new string(os.symbol);
                    string query = "INSERT INTO ORDERS (OrderID,OrderStatus,symbol,price, quantity,direction,version,machineID,userID) VALUES ("
                        + nextSeq + "," + "'NEW'" + ",'" + symbol + "'," + os.price + "," + os.quantity + "," + os.direction + ","
                        + "1" + "," + os.machineID + "," + os.userID + ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    query = "UPDATE ORDERS SET OrderStatus = CANCELED, LinkedOrderID = "+ nextSeq +" WHERE OrderID = " + os.OrderID;
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    os.OrderID = nextSeq;
                    return nextSeq;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Exception(Amend) : " + ex.Message);
            }
            return -1;
        }

        public int cancelOrder(OrderStruct os)//Update the previouss and add new
        {
            try
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    string query = "UPDATE ORDERS SET OrderStatus = 'CANCELED' WHERE OrderID = " + os.OrderID;
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Exception (Cancel) : " + ex.Message);
            }
            return -1;
        }

        public int addOrderFills(OrderFillStruct ofs)
        {
            try
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    int nextSeq = getNextSeq();
                    string query = "INSERT INTO FILLS (OrderID,FillID,Quantity,Price, FilledQuantity) VALUES ("
                    + ofs.OrderID + "," + ofs.FillID + "," + ofs.Quantity + "," + ofs.Price + "," + ofs.FilledQuantity +  ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Exception(addOrderFills) : " + ex.Message);
            }
            return -1;
        }

        private void getOrderStatusFromDB(int OrderID, ref char[] OrderStatus)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT OrderStatus FROM ORDERS WHERE OrderID = " + OrderID;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tmp = reader["OrderStatus"];
                        if (tmp != DBNull.Value)
                        {
                            OrderStatus = ((string)tmp).ToCharArray();
                        }                        
                    }
                }
            }
        }

        public void getOrderFromDB(int OrderID, ref OrderStruct os)
        {
            //TODO: Add the fills from fills tble to struct;
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT OrderID,OrderStatus,symbol,price, quantity,direction,version,machineID,userID,insertTS FROM ORDERS WHERE OrderID = " + OrderID;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tmp = reader["OrderStatus"]; if (tmp != DBNull.Value) { os.OrderStatus = ((string)tmp).ToCharArray(); }
                        tmp = reader["symbol"]; if (tmp != DBNull.Value) { os.symbol = ((string)tmp).ToCharArray();  }
                        tmp = reader["price"]; if (tmp != DBNull.Value) { os.price = (float)tmp; }
                        tmp = reader["quantity"]; if (tmp != DBNull.Value) { os.quantity = (float)tmp; }
                        tmp = reader["direction"]; if (tmp != DBNull.Value) { os.direction = (char)tmp; }
                        tmp = reader["version"]; if (tmp != DBNull.Value) { os.version = (int)tmp; }
                        tmp = reader["machineID"]; if (tmp != DBNull.Value) { os.machineID = (int)tmp; }
                        tmp = reader["userID"]; if (tmp != DBNull.Value) { os.userID = (int)tmp; }
                    }
                }
            }
        }
    }
}
