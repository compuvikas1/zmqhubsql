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

                DataGridViewTextBoxCell cellSpreadPrice = new DataGridViewTextBoxCell();
                cellSpreadPrice.Value = filterArray[1];
                row.Cells.Add(cellSpreadPrice);

                DataGridViewTextBoxCell cellVolume = new DataGridViewTextBoxCell();
                cellVolume.Value = filterArray[2];
                row.Cells.Add(cellVolume);

                DataGridViewTextBoxCell cellBidSize = new DataGridViewTextBoxCell();
                cellBidSize.Value = filterArray[3];
                row.Cells.Add(cellBidSize);

                DataGridViewTextBoxCell cellAskSize = new DataGridViewTextBoxCell();
                cellAskSize.Value = filterArray[4];
                row.Cells.Add(cellAskSize);

                DataGridViewCheckBoxCell cellApply = new DataGridViewCheckBoxCell();
                cellApply.Value = filterArray[5];
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
                    double spreadPrice;
                    try
                    {
                        // if spreadprice is left blank then we assign zero to it
                        if (txtSpreadPrice.Text.Trim().Length == 0)
                            spreadPrice = 0;
                        else
                            spreadPrice = Convert.ToDouble(txtSpreadPrice.Text.Trim());
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Please enter valid SpreadPrice (decimal numbers) ");
                        return;
                    }

                    int volume;
                    try
                    {
                        // if volume is left blank then we assign zero to it
                        if (txtVolume.Text.Trim().Length == 0)
                            volume = 0;
                        else
                            volume = Convert.ToInt32(txtVolume.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please enter valid Volume (numbers) ");
                        return;
                    }

                    int bidSize;
                    try
                    {
                        // if bidsize is left blank then we assign zero to it
                        if (txtBidSize.Text.Trim().Length == 0)
                            bidSize = 0;
                        else
                            bidSize = Convert.ToInt32(txtBidSize.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please enter valid BidSize (numbers) ");
                        return;
                    }

                    int askSize;
                    try
                    {
                        // if asksize is left blank then we assign zero to it
                        if (txtAskSize.Text.Trim().Length == 0)
                            askSize = 0;
                        else
                            askSize = Convert.ToInt32(txtAskSize.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please enter valid AskSize (numbers) ");
                        return;
                    }

                    // new symbol, add it in the gridview
                    DataGridViewTextBoxCell cellSymbol = new DataGridViewTextBoxCell();
                    row.Cells.Add(cellSymbol);
                    cellSymbol.Value = symbol;
                                        
                    DataGridViewTextBoxCell cellSpreadPrice = new DataGridViewTextBoxCell();
                    cellSpreadPrice.Value = spreadPrice;
                    row.Cells.Add(cellSpreadPrice);
                    
                    DataGridViewTextBoxCell cellVolume = new DataGridViewTextBoxCell();
                    cellVolume.Value = volume;
                    row.Cells.Add(cellVolume);

                    DataGridViewTextBoxCell cellBidSize = new DataGridViewTextBoxCell();
                    cellBidSize.Value = bidSize;
                    row.Cells.Add(cellBidSize);

                    DataGridViewTextBoxCell cellAskSize = new DataGridViewTextBoxCell();
                    cellAskSize.Value = askSize;
                    row.Cells.Add(cellAskSize);

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
                string spreadPrice = ((DataGridViewTextBoxCell)row.Cells[1]).Value.ToString();
                string volume = ((DataGridViewTextBoxCell)row.Cells[2]).Value.ToString();
                string bidsize = ((DataGridViewTextBoxCell)row.Cells[3]).Value.ToString();
                string asksize = ((DataGridViewTextBoxCell)row.Cells[4]).Value.ToString();
                string apply = ((DataGridViewCheckBoxCell)row.Cells[5]).Value.ToString();

                string line = symbol + "," + spreadPrice + "," + volume + "," + bidsize + "," + asksize + "," + apply + "\n";

                File.AppendAllText("filterconfig.txt", line);

                //Add this filter in the dictionary
                bool applyFlag = Convert.ToBoolean(apply);
                if (applyFlag == true)
                {
                    SymbolFilter symFilter = new SymbolFilter();
                    symFilter.spreadPrice = Convert.ToDouble(spreadPrice);
                    symFilter.volume = Convert.ToInt32(volume);
                    symFilter.bidSize = Convert.ToInt32(bidsize);
                    symFilter.askSize = Convert.ToInt32(asksize);

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
