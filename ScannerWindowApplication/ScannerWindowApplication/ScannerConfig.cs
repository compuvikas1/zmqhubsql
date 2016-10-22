using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ScannerWindowApplication
{
    public partial class ScannerConfig : Form
    {
        ScannerDashboard parentSD;

        public ScannerConfig(ScannerDashboard sd)
        {
            InitializeComponent();
            parentSD = sd;
        }
                
        private void ScannerConfig_Load(object sender, EventArgs e)
        {
            string fileName = "symbols1.txt";

            var fileLines = File.ReadAllLines(fileName);
            Array.Sort(fileLines); // alphabetically sorting the symbols

            foreach (var singleLine in fileLines)
            {
                cmbSymbol.Items.Add(singleLine);
            }

            var filterLines = File.ReadAllLines("filterconfig.txt");

            foreach (var singleLine in filterLines)
            {
                string[] filterArray = singleLine.Split(',');
                DataGridViewRow row = new DataGridViewRow();

                // new symbol, add it in the gridview
                DataGridViewTextBoxCell cellSymbol = new DataGridViewTextBoxCell();
                cellSymbol.Value = filterArray[0];
                row.Cells.Add(cellSymbol);

                DataGridViewTextBoxCell cellClosePrice = new DataGridViewTextBoxCell();
                cellClosePrice.Value = filterArray[1];
                row.Cells.Add(cellClosePrice);

                DataGridViewTextBoxCell cellLTP = new DataGridViewTextBoxCell();
                cellLTP.Value = filterArray[2];
                row.Cells.Add(cellLTP);

                DataGridViewTextBoxCell cellQuantity = new DataGridViewTextBoxCell();
                cellQuantity.Value = filterArray[3];
                row.Cells.Add(cellQuantity);

                DataGridViewCheckBoxCell cellApply = new DataGridViewCheckBoxCell();
                cellApply.Value = filterArray[4];
                row.Cells.Add(cellApply);

                filterGridView.Rows.Add(row);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbSymbol.SelectedItem != null)
            {
                string symbol = cmbSymbol.SelectedItem.ToString().ToUpper();
                Boolean symbolExists = false;
                foreach (DataGridViewRow row in filterGridView.Rows)
                {
                    //((DataGridViewCheckBoxCell)row.Cells[0]).Value = true;
                    string symbolCell = ((DataGridViewTextBoxCell)row.Cells[0]).Value.ToString();
                    if(symbol.Equals(symbolCell))
                    {
                        symbolExists = true;
                    }
                }

                if(symbolExists)
                {
                    // symbol already exists in the gridview, cannot add twice
                    MessageBox.Show("Symbol Already exists in Table");
                }
                else
                {
                    DataGridViewRow row = new DataGridViewRow();

                    //validate SpreadPrice and volume to be integer and non negative number's
                    double closePrice;
                    try
                    {
                        // if spreadprice is left blank then we assign zero to it
                        if (txtClosePrice.Text.Trim().Length == 0)
                            closePrice = 0;
                        else
                            closePrice = Convert.ToDouble(txtClosePrice.Text.Trim());
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Please enter valid ClosePrice (decimal numbers) ");
                        return;
                    }

                    double ltp;
                    try
                    {
                        // if volume is left blank then we assign zero to it
                        if (txtLtp.Text.Trim().Length == 0)
                            ltp = 0;
                        else
                            ltp = Convert.ToDouble(txtLtp.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please enter valid LTP (numbers) ");
                        return;
                    }

                    int Quantity;
                    try
                    {
                        // if bidsize is left blank then we assign zero to it
                        if (txtQuantity.Text.Trim().Length == 0)
                            Quantity = 0;
                        else
                            Quantity = Convert.ToInt32(txtQuantity.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please enter valid Quantity (numbers) ");
                        return;
                    }
                    
                    // new symbol, add it in the gridview
                    DataGridViewTextBoxCell cellSymbol = new DataGridViewTextBoxCell();
                    row.Cells.Add(cellSymbol);
                    cellSymbol.Value = symbol;
                                        
                    DataGridViewTextBoxCell cellClosePrice = new DataGridViewTextBoxCell();
                    cellClosePrice.Value = closePrice;
                    row.Cells.Add(cellClosePrice);
                    
                    DataGridViewTextBoxCell cellLtp = new DataGridViewTextBoxCell();
                    cellLtp.Value = ltp;
                    row.Cells.Add(cellLtp);

                    DataGridViewTextBoxCell cellQuantity = new DataGridViewTextBoxCell();
                    cellQuantity.Value = Quantity;
                    row.Cells.Add(cellQuantity);

                    DataGridViewCheckBoxCell cellApply = new DataGridViewCheckBoxCell();
                    cellApply.Value = chkApply.Checked;
                    row.Cells.Add(cellApply);

                    filterGridView.Rows.Add(row);

                    MessageBox.Show( symbol + " - Added Successfully");
                }
            }
        }

        private void btnSaveFilter_Click(object sender, EventArgs e)
        {
            // save all the setting in filterconfig.txt

            File.Delete("filterconfig.txt");

            foreach (DataGridViewRow row in filterGridView.Rows)
            {
                //((DataGridViewCheckBoxCell)row.Cells[0]).Value = true;
                string symbol = ((DataGridViewTextBoxCell)row.Cells[0]).Value.ToString();
                string closePrice = ((DataGridViewTextBoxCell)row.Cells[1]).Value.ToString();
                string ltp = ((DataGridViewTextBoxCell)row.Cells[2]).Value.ToString();
                string quantity = ((DataGridViewTextBoxCell)row.Cells[3]).Value.ToString();
                string apply = ((DataGridViewCheckBoxCell)row.Cells[5]).Value.ToString();

                string line = symbol + "," + closePrice + "," + ltp + "," + quantity + "," + apply + "\n";

                File.AppendAllText("filterconfig.txt", line);

                //Add this filter in the dictionary
                bool applyFlag = Convert.ToBoolean(apply);
                if (applyFlag == true)
                {
                    SymbolFilter symFilter = new SymbolFilter();
                    symFilter.closePrice = Convert.ToDouble(closePrice);
                    symFilter.ltp = Convert.ToDouble(ltp);
                    symFilter.quantity = Convert.ToInt32(quantity);

                    parentSD.dictFilters[symbol] = symFilter;
                }
                else
                {
                    if(parentSD.dictFilters.ContainsKey(symbol))
                        parentSD.dictFilters.Remove(symbol);
                }
            }

            MessageBox.Show("Filter's Saved");

            
        }
    }
}
