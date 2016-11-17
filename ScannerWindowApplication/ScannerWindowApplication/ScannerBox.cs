﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace ScannerWindowApplication
{
    public partial class ScannerBox : Form
    {
        public static Queue<Feed> qfeed = new Queue<Feed>();
        public delegate void AddListItem();
        public AddListItem myDelegate;
        private Thread myThread;
        public static Boolean openedMainForm = true;
        ScannerDashboard parentSD;

        public static DataTable dtFeed = new DataTable();        

        public ScannerBox(ScannerDashboard sd)
        {
            InitializeComponent();
            if (dtFeed.Columns.Contains("Time") == false)
            {
                var colTime = dtFeed.Columns.Add("Time");
                var colSymbol = dtFeed.Columns.Add("Symbol");
                var colExpiry = dtFeed.Columns.Add("Expiry");
                var colStrike = dtFeed.Columns.Add("Strike");
                var colPC = dtFeed.Columns.Add("PC");
                var colExch = dtFeed.Columns.Add("Exch");
                var colClosePrice = dtFeed.Columns.Add("ClosePrice");
                var colLTP = dtFeed.Columns.Add("LTP");
                var colQuantity = dtFeed.Columns.Add("Quantity");

                // set primary key constain so we can search for specific rows
                dtFeed.PrimaryKey = new[] { colSymbol, colExpiry, colStrike, colPC, colExch };
            }

            dataGridView1.DataSource = dtFeed;

            parentSD = sd;            
            ScannerBox.openedMainForm = true;
            myDelegate = new AddListItem(AddListItemMethod);

            myThread = new Thread(new ThreadStart(ThreadFunction));
            myThread.Start();
        }

        private void ThreadFunction()
        {
            MyThreadClass myThreadClassObject = new MyThreadClass(this);
            myThreadClassObject.Run();
        }

        public void AddListItemMethod()
        {
            String myItem;
            if (ScannerBox.qfeed.Count > 0)
            {
                Feed feed = ScannerBox.qfeed.Dequeue();

                var exisiting = dtFeed.Rows.Find(new Object[] { feed.symbol, feed.expiry, feed.strike, feed.callput, feed.exch });
                if (exisiting != null)
                    exisiting.ItemArray = new object[] { feed.feedtime, feed.symbol, feed.expiry, feed.strike, feed.callput, feed.exch, feed.closePrice, feed.ltp, feed.quantity };                
                else
                    dtFeed.Rows.Add(new Object[] { feed.feedtime, feed.symbol, feed.expiry, feed.strike, feed.callput, feed.exch, feed.closePrice, feed.ltp, feed.quantity });

                //int rowIndex = 0;
                //Boolean foundRow = false;

                ////place condition for feedPrice                
                
                //foreach (DataGridViewRow dgvRow in dataGridView1.Rows)
                //{
                //    if (string.Compare(dgvRow.Cells[1].FormattedValue.ToString(),feed.symbol)== 0 &&
                //        string.Compare(dgvRow.Cells[2].Value.ToString(),feed.expiry.Substring(0,10))== 0 &&
                //        string.Compare(dgvRow.Cells[3].Value.ToString(),feed.strike) == 0 &&
                //        string.Compare(dgvRow.Cells[4].Value.ToString(),feed.callput)==0)
                //    {
                //        dgvRow.Cells[0].Value = feed.feedtime.Substring(11, 9);
                //        dgvRow.Cells[2].Value = feed.expiry.Substring(0,10);
                //        dgvRow.Cells[3].Value = feed.strike;
                //        dgvRow.Cells[4].Value = feed.callput;
                //        dgvRow.Cells[5].Value = feed.exch;
                //        dgvRow.Cells[6].Value = round(feed.closePrice, 2);
                //        dgvRow.Cells[7].Value = round(feed.ltp, 2);
                //        dgvRow.Cells[8].Value = feed.quantity;                            
                        
                //        //Console.WriteLine("Existed Row is updated");
                //        foundRow = true;
                //        break;
                //    }
                //    rowIndex++;
                //}
                //if (foundRow == false)
                //{
                //    {
                //        //Console.WriteLine("new Row Added");                        
                //        dataGridView1.Rows.Insert(0, feed.feedtime.Substring(11, 9), feed.symbol, feed.expiry.Substring(0,10),
                //            feed.strike, feed.callput, feed.exch, 
                //             round(feed.closePrice, 2), round(feed.ltp, 2), feed.quantity);
                //    }
                //}
                //dataGridView1.Update();
            }
        }

        private double round(string value, int decNumber)
        {
            double val = Convert.ToDouble(value);
            val = Math.Round(val, decNumber);
            return val;
        }

        private void ScannerBox_Load(object sender, EventArgs e)
        {
            Subscriber sc = new Subscriber(parentSD);
            Thread th = new Thread(new ThreadStart(sc.ThreadB));
            Console.WriteLine("Threads started :");
            // Start thread B
            th.Start();
        }

        private void ScannerBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            openedMainForm = false;
        }


    }

    public class MyThreadClass
    {
        ScannerBox myFormControl1;
        public MyThreadClass(ScannerBox myForm)
        {
            myFormControl1 = myForm;
        }

        public void Run()
        {
            // Execute the specified delegate on the thread that owns
            // 'myFormControl1' control's underlying window handle.

            while (ScannerBox.openedMainForm)
            {
                try
                {
                    myFormControl1.Invoke(myFormControl1.myDelegate);

                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
