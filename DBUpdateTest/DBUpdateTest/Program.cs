using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdateTest
{
    class Program
    {
        private static SqlConnection conn;
        public static void feedUpdateConn()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "SONY-VAIO";
            builder.InitialCatalog = "THOM";
            builder.UserID = "zmq1";
            builder.Password = "test123";
            SqlConnection con = new SqlConnection(builder.ToString());            
            conn = con;
        }

        public static int updateFeeds(string scriptNo)//Update the previouss and add new
        {
            try
            {
                using (var cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    string lr = null, tq = null;
                    double lr1 = 0.0;
                    double tq1 = 0.0;
                    string querySelect = "select LastRate, TotalQty FROM [LPIntraDay].[dbo].[TouchLine] WHERE  ScripNo = " + scriptNo;
                    cmd.CommandText = querySelect;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lr = reader["LastRate"].ToString();
                            tq = reader["TotalQty"].ToString();
                        }
                    }
                    if (lr != null && tq != null)
                    {
                        Random rnd = new Random();
                        int num = rnd.Next(-5, 5);

                        lr1 = Convert.ToDouble(lr) + num;
                        tq1 = Convert.ToDouble(tq) + num;
                        Console.WriteLine("nm = ["+num+"] ScriptNo : "+ scriptNo +" OLD: ["+lr+"] ["+tq+"] NEW: ["+lr1+"]["+tq1+"]");
                        string query = "UPDATE [LPIntraDay].[dbo].[TouchLine] SET UpdateTime = SYSDATETIME(), LastRate = " + lr1
                        + ", TotalQty = " + tq1 + " WHERE  ScripNo = " + scriptNo;
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Exception : " + ex.Message);
            }
            return -1;
        }

        static void Main(string[] args)
        {
            feedUpdateConn();
            Console.WriteLine("Enter no of scripts follwed by scriptNo (one per line)\n5 5 5 -> 5 scripts, 5 sec break, 5 times loop\\n script no");
            string line1 = Console.ReadLine();
            string[] toks = line1.Split(' ');
            int count = Convert.ToInt16(toks[0]);
            int sleepCount = Convert.ToInt16(toks[1]);
            int loopCount = Convert.ToInt16(toks[2]);
            string[] scripts = new string[count];
            for (int i = 0; i < count; i++)
            {
                scripts[i] = Console.ReadLine();                
            }
            for (int i = 0; i < loopCount; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    updateFeeds(scripts[j]);
                }
                System.Threading.Thread.Sleep(sleepCount);
            }
        }
    }
}
