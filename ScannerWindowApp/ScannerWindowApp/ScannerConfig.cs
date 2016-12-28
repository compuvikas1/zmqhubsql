using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScannerWindowApp
{
    public partial class ScannerConfig : Form
    {
        ScannerDashboard1 parentSD;
        public ScannerConfig(ScannerDashboard1 sd)
        {
            InitializeComponent();
            parentSD = sd;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string fileName = "D:\\Rahul\\zmqhub\\symbols1.txt";
            var fileLines = System.IO.File.ReadAllLines(fileName);

            foreach (var singleLine in fileLines)
            {
                listBox2.Items.Add(singleLine);
            }        
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(listBox2.Items);
            listBox2.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.AddRange(listBox1.Items);
            listBox1.Items.Clear();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void ListBox1_MouseDoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {                
                listBox2.Items.Add(listBox1.SelectedItem);
                listBox1.Items.Remove(listBox1.SelectedItem);
            }

        }

        private void ListBox2_MouseDoubleClick(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                listBox1.Items.Add(listBox2.SelectedItem);
                listBox2.Items.Remove(listBox2.SelectedItem);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {//Moving slected from list2 to list1 (selection)
            if (listBox2.SelectedItem != null)
            {                
                listBox1.Items.Add(listBox2.SelectedItem);
                listBox2.Items.Remove(listBox2.SelectedItem);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//Moving list1 to list2 unselection)
            if (listBox1.SelectedItem != null)
            {                
                listBox2.Items.Add(listBox1.SelectedItem);
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach(object obj in listBox1.Items) {
                if (parentSD.topics == null)
                {
                    parentSD.topics = obj.ToString();
                }
                else
                {
                    parentSD.topics = parentSD.topics + "," + obj.ToString();
                }
            }
            //parentSD.topics = "Test me";
            this.Close();
        }
    }
}
