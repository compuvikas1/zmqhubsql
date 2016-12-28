using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScannerDashBoard
{
    public partial class ScannerDashboard : Form
    {
        public string topics { get; set; }

        public ScannerDashboard()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScannerBox sb = new ScannerBox(this);
            sb.topic = this.topics;
            sb.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this.IsMdiContainer = true;
            //Form sc = new ScannerConfig();
            //sc.MdiParent = this;
            textBox1.Text = this.topics;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = this.topics;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = this.topics;
        }
    }
}
