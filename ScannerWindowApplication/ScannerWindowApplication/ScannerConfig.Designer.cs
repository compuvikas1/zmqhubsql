namespace ScannerWindowApplication
{
    partial class ScannerConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnSaveFilter = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.chkApply = new System.Windows.Forms.CheckBox();
            this.txtVolume = new System.Windows.Forms.TextBox();
            this.txtSpreadPrice = new System.Windows.Forms.TextBox();
            this.cmbSymbol = new System.Windows.Forms.ComboBox();
            this.chkVolume = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.filterGridView = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSymbolType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbExpiry = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbCallPut = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtStrike = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBidSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAskSize = new System.Windows.Forms.TextBox();
            this.Symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpreadPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Volume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BidSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AskSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Apply = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filterGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtAskSize);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.txtBidSize);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.txtStrike);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.cmbCallPut);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.cmbExpiry);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.cmbSymbolType);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.btnSaveFilter);
            this.splitContainer1.Panel1.Controls.Add(this.btnAdd);
            this.splitContainer1.Panel1.Controls.Add(this.chkApply);
            this.splitContainer1.Panel1.Controls.Add(this.txtVolume);
            this.splitContainer1.Panel1.Controls.Add(this.txtSpreadPrice);
            this.splitContainer1.Panel1.Controls.Add(this.cmbSymbol);
            this.splitContainer1.Panel1.Controls.Add(this.chkVolume);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.filterGridView);
            this.splitContainer1.Size = new System.Drawing.Size(996, 433);
            this.splitContainer1.SplitterDistance = 110;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnSaveFilter
            // 
            this.btnSaveFilter.Location = new System.Drawing.Point(896, 56);
            this.btnSaveFilter.Name = "btnSaveFilter";
            this.btnSaveFilter.Size = new System.Drawing.Size(75, 23);
            this.btnSaveFilter.TabIndex = 9;
            this.btnSaveFilter.Text = "Save Filters";
            this.btnSaveFilter.UseVisualStyleBackColor = true;
            this.btnSaveFilter.Click += new System.EventHandler(this.btnSaveFilter_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(787, 56);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // chkApply
            // 
            this.chkApply.AutoSize = true;
            this.chkApply.Location = new System.Drawing.Point(622, 56);
            this.chkApply.Name = "chkApply";
            this.chkApply.Size = new System.Drawing.Size(52, 17);
            this.chkApply.TabIndex = 7;
            this.chkApply.Text = "Apply";
            this.chkApply.UseVisualStyleBackColor = true;
            // 
            // txtVolume
            // 
            this.txtVolume.Location = new System.Drawing.Point(194, 60);
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.Size = new System.Drawing.Size(68, 20);
            this.txtVolume.TabIndex = 6;
            // 
            // txtSpreadPrice
            // 
            this.txtSpreadPrice.Location = new System.Drawing.Point(59, 62);
            this.txtSpreadPrice.Name = "txtSpreadPrice";
            this.txtSpreadPrice.Size = new System.Drawing.Size(62, 20);
            this.txtSpreadPrice.TabIndex = 5;
            // 
            // cmbSymbol
            // 
            this.cmbSymbol.FormattingEnabled = true;
            this.cmbSymbol.Location = new System.Drawing.Point(59, 6);
            this.cmbSymbol.Name = "cmbSymbol";
            this.cmbSymbol.Size = new System.Drawing.Size(62, 21);
            this.cmbSymbol.TabIndex = 4;
            // 
            // chkVolume
            // 
            this.chkVolume.AutoSize = true;
            this.chkVolume.Location = new System.Drawing.Point(135, 65);
            this.chkVolume.Name = "chkVolume";
            this.chkVolume.Size = new System.Drawing.Size(42, 13);
            this.chkVolume.TabIndex = 2;
            this.chkVolume.Text = "Volume";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Price";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Symbol";
            // 
            // filterGridView
            // 
            this.filterGridView.AllowUserToAddRows = false;
            this.filterGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.filterGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Symbol,
            this.SpreadPrice,
            this.Volume,
            this.BidSize,
            this.AskSize,
            this.Apply});
            this.filterGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterGridView.Location = new System.Drawing.Point(0, 0);
            this.filterGridView.Name = "filterGridView";
            this.filterGridView.Size = new System.Drawing.Size(996, 319);
            this.filterGridView.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(146, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Type";
            // 
            // cmbSymbolType
            // 
            this.cmbSymbolType.FormattingEnabled = true;
            this.cmbSymbolType.Items.AddRange(new object[] {
            "Cash",
            "Future",
            "Option"});
            this.cmbSymbolType.Location = new System.Drawing.Point(194, 6);
            this.cmbSymbolType.Name = "cmbSymbolType";
            this.cmbSymbolType.Size = new System.Drawing.Size(68, 21);
            this.cmbSymbolType.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(284, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Expiry";
            // 
            // cmbExpiry
            // 
            this.cmbExpiry.FormattingEnabled = true;
            this.cmbExpiry.Items.AddRange(new object[] {
            "27-10-2016",
            "24-11-2016",
            "29-12-2016"});
            this.cmbExpiry.Location = new System.Drawing.Point(338, 6);
            this.cmbExpiry.Name = "cmbExpiry";
            this.cmbExpiry.Size = new System.Drawing.Size(95, 21);
            this.cmbExpiry.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(460, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Call / Put";
            // 
            // cmbCallPut
            // 
            this.cmbCallPut.FormattingEnabled = true;
            this.cmbCallPut.Items.AddRange(new object[] {
            "Call",
            "Put"});
            this.cmbCallPut.Location = new System.Drawing.Point(527, 4);
            this.cmbCallPut.Name = "cmbCallPut";
            this.cmbCallPut.Size = new System.Drawing.Size(68, 21);
            this.cmbCallPut.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(619, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Strike";
            // 
            // txtStrike
            // 
            this.txtStrike.Location = new System.Drawing.Point(677, 4);
            this.txtStrike.Name = "txtStrike";
            this.txtStrike.Size = new System.Drawing.Size(100, 20);
            this.txtStrike.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(284, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "BidSz";
            // 
            // txtBidSize
            // 
            this.txtBidSize.Location = new System.Drawing.Point(338, 58);
            this.txtBidSize.Name = "txtBidSize";
            this.txtBidSize.Size = new System.Drawing.Size(95, 20);
            this.txtBidSize.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(474, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "AskSz";
            // 
            // txtAskSize
            // 
            this.txtAskSize.Location = new System.Drawing.Point(525, 59);
            this.txtAskSize.Name = "txtAskSize";
            this.txtAskSize.Size = new System.Drawing.Size(68, 20);
            this.txtAskSize.TabIndex = 21;
            // 
            // Symbol
            // 
            this.Symbol.HeaderText = "Symbol";
            this.Symbol.Name = "Symbol";
            this.Symbol.ReadOnly = true;
            // 
            // SpreadPrice
            // 
            this.SpreadPrice.HeaderText = "Spread Price";
            this.SpreadPrice.Name = "SpreadPrice";
            // 
            // Volume
            // 
            this.Volume.HeaderText = "Volume";
            this.Volume.Name = "Volume";
            // 
            // BidSize
            // 
            this.BidSize.HeaderText = "BidSz";
            this.BidSize.Name = "BidSize";
            // 
            // AskSize
            // 
            this.AskSize.HeaderText = "AskSz";
            this.AskSize.Name = "AskSize";
            // 
            // Apply
            // 
            this.Apply.HeaderText = "Apply";
            this.Apply.Name = "Apply";
            // 
            // ScannerConfig
            // 
            this.ClientSize = new System.Drawing.Size(996, 433);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ScannerConfig";
            this.Text = "Scanner Filter\'s";
            this.Load += new System.EventHandler(this.ScannerConfig_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.filterGridView)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnSaveFilter;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.CheckBox chkApply;
        private System.Windows.Forms.TextBox txtVolume;
        private System.Windows.Forms.TextBox txtSpreadPrice;
        private System.Windows.Forms.ComboBox cmbSymbol;
        private System.Windows.Forms.Label chkVolume;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView filterGridView;
        private System.Windows.Forms.ComboBox cmbExpiry;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbSymbolType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbCallPut;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtStrike;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBidSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAskSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn Symbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpreadPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Volume;
        private System.Windows.Forms.DataGridViewTextBoxColumn BidSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn AskSize;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Apply;
    }
}