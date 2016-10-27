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
        public ScannerBox(ScannerDashboard sd)
        {
            InitializeComponent();
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
                int rowIndex = 0;
                Boolean foundRow = false;

                //place condition for feedPrice
                double closePrice = Convert.ToDouble(feed.closePrice);
                double ltp = Convert.ToDouble(feed.ltp);
                int quantity = Convert.ToInt32(feed.quantity);

                bool flagSymbolCondition = true;
                bool flagExpiryCondition = true;
                bool flagStrikeCondition = true;

                bool flagClosePriceCondition = true;
                bool flagLtpCondition = true;
                bool flagQuantityCondition = true;

                List<SymbolFilter> listSymbolFilter;
                if (parentSD.dictFilters.TryGetValue(feed.symbol.Trim(), out listSymbolFilter))
                {
                    foreach (var symbolfilter in listSymbolFilter)
                    {
                        if (symbolfilter.symbol != null && feed.symbol != symbolfilter.symbol)                        
                            flagSymbolCondition = false;                        
                        else
                            flagSymbolCondition = true;

                        if (symbolfilter.expiry != null && symbolfilter.expiry != "")
                        {
                            // MessageBox.Show(feed.expiry + " not equal " + symbolfilter.expiry);
                            Console.WriteLine(feed.expiry + " and " + symbolfilter.expiry);
                            //DateTime dt1 = DateTime.ParseExact(symbolfilter.expiry, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                            if (feed.expiry != symbolfilter.expiry)                            
                                flagExpiryCondition = false;                            
                            else
                                flagExpiryCondition = true;
                        }

                        if (symbolfilter.strike != null && symbolfilter.strike != "" && feed.strike != symbolfilter.strike)                        
                            flagStrikeCondition = false;
                        else
                            flagStrikeCondition = true;

                        if (symbolfilter.closePrice != 0 && closePrice < symbolfilter.closePrice)                        
                            flagClosePriceCondition = false;
                        else
                            flagClosePriceCondition = true;

                        if (symbolfilter.ltp != 0 && ltp < symbolfilter.ltp)                        
                            flagLtpCondition = false;
                        else
                            flagLtpCondition = true;

                        if (symbolfilter.quantity != 0 && quantity < symbolfilter.quantity)                        
                            flagQuantityCondition = false;
                        else
                            flagQuantityCondition = true;

                        if (flagSymbolCondition && flagExpiryCondition &&
                            flagStrikeCondition && flagClosePriceCondition &&
                            flagLtpCondition && flagQuantityCondition)
                        {
                            break;
                        }
                    }
                }

                foreach (DataGridViewRow dgvRow in dataGridView1.Rows)
                {
                    if (dgvRow.Cells[1].FormattedValue.ToString() == feed.symbol &&
                        dgvRow.Cells[2].Value.ToString() == feed.expiry.Substring(0,10) &&
                        dgvRow.Cells[3].Value.ToString() == feed.strike &&
                        dgvRow.Cells[4].Value.ToString() == feed.callput)
                    {
                        if (flagSymbolCondition && flagExpiryCondition &&
                            flagStrikeCondition && flagClosePriceCondition && 
                            flagLtpCondition && flagQuantityCondition)
                        {
                            dgvRow.Cells[0].Value = feed.feedtime.Substring(11, 8);
                            dgvRow.Cells[2].Value = feed.expiry.Substring(0,10);
                            dgvRow.Cells[3].Value = feed.strike;
                            dgvRow.Cells[4].Value = feed.callput;
                            dgvRow.Cells[5].Value = round(feed.exch, 2);
                            dgvRow.Cells[6].Value = round(feed.closePrice, 2);
                            dgvRow.Cells[7].Value = round(feed.ltp, 2);
                            dgvRow.Cells[8].Value = round(feed.quantity, 2);                            
                        }
                        //Console.WriteLine("Existed Row is added");
                        foundRow = true;
                        break;
                    }
                    rowIndex++;
                }
                if (foundRow == false)
                {
                    if (flagSymbolCondition && flagExpiryCondition &&
                            flagStrikeCondition && flagClosePriceCondition &&
                            flagLtpCondition && flagQuantityCondition)
                    {
                        //Console.WriteLine("new Row Added");                        
                        dataGridView1.Rows.Insert(0, feed.feedtime.Substring(11, 8), feed.symbol, feed.expiry.Substring(0,10),
                            feed.strike, feed.callput, feed.exch, 
                             round(feed.closePrice, 2), round(feed.ltp, 2), feed.quantity);
                    }
                }
                dataGridView1.Update();
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
