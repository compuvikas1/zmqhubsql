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

namespace ScannerWindowApp
{
    
    public partial class ScannerBox : Form
    {
        public string topic { get; set; }
        public static Queue<Feed> qfeed = new Queue<Feed>();
        public delegate void AddListItem();
        public AddListItem myDelegate;
        private Thread myThread;
        public static Boolean openedMainForm = true;

        public ScannerBox(ScannerDashboard1 sd)
        {
            InitializeComponent();
            topic = sd.topics;
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
                foreach (DataGridViewRow dgvRow in dataGridView1.Rows)
                {
                    if (dgvRow.Cells[0].FormattedValue.ToString() == feed.symbol &&
                        dgvRow.Cells[2].Value.ToString() == feed.expiry &&
                        dgvRow.Cells[3].Value.ToString() == feed.callput &&
                    dgvRow.Cells[4].Value.ToString() == feed.strike)
                    {
                        dgvRow.Cells[1].Value = feed.feedtime;
                        dgvRow.Cells[2].Value = feed.expiry;
                        dgvRow.Cells[3].Value = feed.callput;
                        dgvRow.Cells[4].Value = feed.strike;
                        dgvRow.Cells[5].Value = feed.bidSize;
                        dgvRow.Cells[6].Value = feed.bidPrice;
                        dgvRow.Cells[7].Value = feed.askPrice;
                        dgvRow.Cells[8].Value = feed.askSize;
                        dgvRow.Cells[9].Value = feed.volume;
                        foundRow = true;
                        break;
                    }
                    rowIndex++;
                }
                if(foundRow == false)
                    dataGridView1.Rows.Insert(0, feed.symbol, feed.feedtime, feed.expiry, feed.callput, feed.strike, feed.bidSize, feed.bidPrice, feed.askPrice, feed.askSize, feed.volume);
                dataGridView1.Update();
            }
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            subscriber sc = new subscriber(topic);
            Thread th = new Thread(new ThreadStart(sc.ThreadB));
            Console.WriteLine("Threads started :");
            // Start thread B
            th.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            openedMainForm = false;
        }

        private void Restart_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
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
