using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScannerWindowApplication
{
    public partial class ScannerDashboard : Form
    {
        public string topics { get; set; }
        //public Dictionary<string, SymbolFilter> dictFilters = new Dictionary<string, SymbolFilter>();
        public Dictionary<string, List<SymbolFilter>> dictFilters = new Dictionary<string, List<SymbolFilter>>();

        public ScannerDashboard()
        {
            InitializeComponent();
        }

        ScannerConfig config = null;
        ScannerBox scannerBox = null;

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (config == null || config.Text == "")
            {
                config = new ScannerConfig(this);
                config.MdiParent = this;
                config.Dock = DockStyle.Fill;
                config.Show();
            }
            else if (CheckOpened(config.Text))
            {
                config.WindowState = FormWindowState.Normal;
                config.Dock = DockStyle.Fill;
                config.Show();
                config.Focus();
            }
        }

        private void scannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (scannerBox == null || scannerBox.Text == "")
            {
                scannerBox = new ScannerBox(this);                
                scannerBox.MdiParent = this;
                scannerBox.Dock = DockStyle.Fill;
                scannerBox.Show();
            }
            else if (CheckOpened(scannerBox.Text))
            {
                scannerBox.WindowState = FormWindowState.Normal;
                scannerBox.Dock = DockStyle.Fill;
                scannerBox.Show();
                scannerBox.Focus();
            }
        }

        private bool CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
