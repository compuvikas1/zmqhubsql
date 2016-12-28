using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

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
                double spreadPrice = Convert.ToDouble(feed.askPrice) - Convert.ToDouble(feed.bidPrice);
                int volume = Convert.ToInt32(feed.volume);
                double bidsize = Convert.ToDouble(feed.bidSize);
                double asksize = Convert.ToDouble(feed.askSize);                

                bool flagSpreadPriceCondition = true;
                bool flagVolumeCondition = true;
                bool flagBidSizeCondition = true;
                bool flagAskSizeCondition = true;

                SymbolFilter symbolfilter;
                if (parentSD.dictFilters.TryGetValue(feed.symbol.Trim(), out symbolfilter))
                {                
                    if (symbolfilter.spreadPrice != 0 && spreadPrice < symbolfilter.spreadPrice)
                    {
                        flagSpreadPriceCondition = false;
                    }
                    if (symbolfilter.volume != 0 && volume < symbolfilter.volume)
                    {
                        flagVolumeCondition = false;
                    }
                    if (symbolfilter.bidSize != 0 && bidsize < symbolfilter.bidSize)
                    {
                        flagBidSizeCondition = false;
                    }
                    if (symbolfilter.askSize != 0 && asksize < symbolfilter.askSize)
                    {
                        flagAskSizeCondition = false;
                    }
                }

                foreach (DataGridViewRow dgvRow in dataGridView1.Rows)
                {
                    if (dgvRow.Cells[0].FormattedValue.ToString() == feed.symbol &&
                        dgvRow.Cells[2].Value.ToString() == feed.expiry &&
                        dgvRow.Cells[3].Value.ToString().Substring(0,1) == feed.callput.Trim().Substring(0,1) &&
                    dgvRow.Cells[4].Value.ToString() == round(feed.strike, 2).ToString())
                    {
                        if (flagSpreadPriceCondition && flagVolumeCondition 
                            && flagBidSizeCondition && flagAskSizeCondition)
                        {
                            dgvRow.Cells[1].Value = feed.feedtime.Substring(11, 8);
                            dgvRow.Cells[2].Value = feed.expiry;
                            string callput = "PUT";
                            if (feed.callput.Trim().Substring(0, 1) == "C")
                                callput = "CALL";
                            dgvRow.Cells[3].Value = callput;
                            dgvRow.Cells[4].Value = round(feed.strike, 2);
                            dgvRow.Cells[5].Value = round(feed.bidSize, 2);
                            dgvRow.Cells[6].Value = round(feed.bidPrice, 2);
                            dgvRow.Cells[7].Value = round(feed.askPrice, 2);
                            dgvRow.Cells[8].Value = round(feed.askSize, 2);
                            dgvRow.Cells[9].Value = feed.volume;
                        }
                        
                        foundRow = true;
                        break;
                    }
                    rowIndex++;
                }
                if (foundRow == false)
                {
                    if (flagSpreadPriceCondition && flagVolumeCondition
                        && flagBidSizeCondition && flagAskSizeCondition)
                    {
                        string callput = "PUT";
                        if (feed.callput.Trim().Substring(0, 1) == "C")
                            callput = "CALL";

                        dataGridView1.Rows.Insert(0, feed.symbol, feed.feedtime.Substring(11, 8), feed.expiry,
                            callput,
                            round(feed.strike, 2), round(feed.bidSize, 2), round(feed.bidPrice, 2), round(feed.askPrice, 2), round(feed.askSize, 2), feed.volume);
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
