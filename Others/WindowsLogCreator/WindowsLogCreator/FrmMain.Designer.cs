namespace SkayTek.WindowsLogCreator
{
	partial class FrmMain
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
			this.btnCreateWinLog = new System.Windows.Forms.Button();
			this.txtLogName = new System.Windows.Forms.TextBox();
			this.tlpLogAndSource = new System.Windows.Forms.TableLayoutPanel();
			this.lblSourceName = new System.Windows.Forms.Label();
			this.lblLogName = new System.Windows.Forms.Label();
			this.btnDeleteSource = new System.Windows.Forms.Button();
			this.btnDeleteLog = new System.Windows.Forms.Button();
			this.cboSourceNames = new System.Windows.Forms.ComboBox();
			this.gbxLogAndSource = new System.Windows.Forms.GroupBox();
			this.tlpGeneral = new System.Windows.Forms.TableLayoutPanel();
			this.grpTest = new System.Windows.Forms.GroupBox();
			this.tlpTestLog = new System.Windows.Forms.TableLayoutPanel();
			this.label6 = new System.Windows.Forms.Label();
			this.txtRawData = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.numCategory = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.numEventID = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.cboEventLogEntryTypes = new System.Windows.Forms.ComboBox();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.btnSaveLog = new System.Windows.Forms.Button();
			this.cboSource = new System.Windows.Forms.ComboBox();
			this.tlpLogAndSource.SuspendLayout();
			this.gbxLogAndSource.SuspendLayout();
			this.tlpGeneral.SuspendLayout();
			this.grpTest.SuspendLayout();
			this.tlpTestLog.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numCategory)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numEventID)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCreateWinLog
			// 
			this.btnCreateWinLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreateWinLog.AutoSize = true;
			this.btnCreateWinLog.Location = new System.Drawing.Point(651, 116);
			this.btnCreateWinLog.Name = "btnCreateWinLog";
			this.btnCreateWinLog.Size = new System.Drawing.Size(130, 27);
			this.btnCreateWinLog.TabIndex = 1;
			this.btnCreateWinLog.Text = "Create Windows Log";
			this.btnCreateWinLog.UseVisualStyleBackColor = true;
			this.btnCreateWinLog.Click += new System.EventHandler(this.btnCreateWinLog_Click);
			// 
			// txtLogName
			// 
			this.txtLogName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLogName.Location = new System.Drawing.Point(9, 25);
			this.txtLogName.Name = "txtLogName";
			this.txtLogName.Size = new System.Drawing.Size(374, 21);
			this.txtLogName.TabIndex = 1;
			this.txtLogName.Text = "SkayTek Software";
			// 
			// tlpLogAndSource
			// 
			this.tlpLogAndSource.AutoSize = true;
			this.tlpLogAndSource.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpLogAndSource.ColumnCount = 2;
			this.tlpLogAndSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpLogAndSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpLogAndSource.Controls.Add(this.txtLogName, 0, 1);
			this.tlpLogAndSource.Controls.Add(this.lblSourceName, 1, 0);
			this.tlpLogAndSource.Controls.Add(this.lblLogName, 0, 0);
			this.tlpLogAndSource.Controls.Add(this.btnDeleteSource, 1, 2);
			this.tlpLogAndSource.Controls.Add(this.btnDeleteLog, 0, 2);
			this.tlpLogAndSource.Controls.Add(this.cboSourceNames, 1, 1);
			this.tlpLogAndSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpLogAndSource.Location = new System.Drawing.Point(3, 17);
			this.tlpLogAndSource.Name = "tlpLogAndSource";
			this.tlpLogAndSource.Padding = new System.Windows.Forms.Padding(6);
			this.tlpLogAndSource.RowCount = 3;
			this.tlpLogAndSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpLogAndSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpLogAndSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpLogAndSource.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpLogAndSource.Size = new System.Drawing.Size(772, 87);
			this.tlpLogAndSource.TabIndex = 0;
			// 
			// lblSourceName
			// 
			this.lblSourceName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSourceName.AutoSize = true;
			this.lblSourceName.Location = new System.Drawing.Point(389, 6);
			this.lblSourceName.Name = "lblSourceName";
			this.lblSourceName.Size = new System.Drawing.Size(81, 15);
			this.lblSourceName.TabIndex = 2;
			this.lblSourceName.Text = "Source name";
			// 
			// lblLogName
			// 
			this.lblLogName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblLogName.AutoSize = true;
			this.lblLogName.Location = new System.Drawing.Point(9, 6);
			this.lblLogName.Name = "lblLogName";
			this.lblLogName.Size = new System.Drawing.Size(63, 15);
			this.lblLogName.TabIndex = 0;
			this.lblLogName.Text = "Log name";
			// 
			// btnDeleteSource
			// 
			this.btnDeleteSource.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnDeleteSource.AutoSize = true;
			this.btnDeleteSource.Location = new System.Drawing.Point(668, 53);
			this.btnDeleteSource.Name = "btnDeleteSource";
			this.btnDeleteSource.Size = new System.Drawing.Size(95, 25);
			this.btnDeleteSource.TabIndex = 5;
			this.btnDeleteSource.Text = "Delete Source";
			this.btnDeleteSource.UseVisualStyleBackColor = true;
			this.btnDeleteSource.Click += new System.EventHandler(this.btnDeleteSource_Click);
			// 
			// btnDeleteLog
			// 
			this.btnDeleteLog.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnDeleteLog.AutoSize = true;
			this.btnDeleteLog.Location = new System.Drawing.Point(306, 53);
			this.btnDeleteLog.Name = "btnDeleteLog";
			this.btnDeleteLog.Size = new System.Drawing.Size(77, 25);
			this.btnDeleteLog.TabIndex = 4;
			this.btnDeleteLog.Text = "Delete Log";
			this.btnDeleteLog.UseVisualStyleBackColor = true;
			this.btnDeleteLog.Click += new System.EventHandler(this.btnDeleteLog_Click);
			// 
			// cboSourceNames
			// 
			this.cboSourceNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cboSourceNames.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SkayTek.WindowsLogCreator.Properties.Settings.Default, "CboSourceNamesText", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cboSourceNames.FormattingEnabled = true;
			this.cboSourceNames.Items.AddRange(new object[] {
            "Account Management",
            "Certificate Generator",
            "Certificate Service",
            "License Manager Service",
            "Portfolio",
            "Registration Service",
            "Web Site",
            "Web Site Domain",
            "Windows Log Reader"});
			this.cboSourceNames.Location = new System.Drawing.Point(389, 24);
			this.cboSourceNames.Name = "cboSourceNames";
			this.cboSourceNames.Size = new System.Drawing.Size(374, 23);
			this.cboSourceNames.TabIndex = 13;
			this.cboSourceNames.Text = global::SkayTek.WindowsLogCreator.Properties.Settings.Default.CboSourceNamesText;
			// 
			// gbxLogAndSource
			// 
			this.gbxLogAndSource.AutoSize = true;
			this.gbxLogAndSource.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.gbxLogAndSource.Controls.Add(this.tlpLogAndSource);
			this.gbxLogAndSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbxLogAndSource.Location = new System.Drawing.Point(3, 3);
			this.gbxLogAndSource.Name = "gbxLogAndSource";
			this.gbxLogAndSource.Size = new System.Drawing.Size(778, 107);
			this.gbxLogAndSource.TabIndex = 0;
			this.gbxLogAndSource.TabStop = false;
			this.gbxLogAndSource.Text = "Log and Source";
			// 
			// tlpGeneral
			// 
			this.tlpGeneral.AutoSize = true;
			this.tlpGeneral.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpGeneral.ColumnCount = 1;
			this.tlpGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpGeneral.Controls.Add(this.gbxLogAndSource, 0, 0);
			this.tlpGeneral.Controls.Add(this.btnCreateWinLog, 0, 1);
			this.tlpGeneral.Controls.Add(this.grpTest, 0, 2);
			this.tlpGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpGeneral.Location = new System.Drawing.Point(0, 0);
			this.tlpGeneral.Name = "tlpGeneral";
			this.tlpGeneral.RowCount = 3;
			this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpGeneral.Size = new System.Drawing.Size(784, 262);
			this.tlpGeneral.TabIndex = 0;
			// 
			// grpTest
			// 
			this.grpTest.AutoSize = true;
			this.grpTest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grpTest.Controls.Add(this.tlpTestLog);
			this.grpTest.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpTest.Location = new System.Drawing.Point(3, 149);
			this.grpTest.Name = "grpTest";
			this.grpTest.Size = new System.Drawing.Size(778, 110);
			this.grpTest.TabIndex = 2;
			this.grpTest.TabStop = false;
			this.grpTest.Text = "Test log";
			// 
			// tlpTestLog
			// 
			this.tlpTestLog.AutoSize = true;
			this.tlpTestLog.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpTestLog.ColumnCount = 6;
			this.tlpTestLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTestLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTestLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTestLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTestLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTestLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpTestLog.Controls.Add(this.label6, 4, 1);
			this.tlpTestLog.Controls.Add(this.txtRawData, 5, 1);
			this.tlpTestLog.Controls.Add(this.label5, 2, 1);
			this.tlpTestLog.Controls.Add(this.label4, 0, 1);
			this.tlpTestLog.Controls.Add(this.numCategory, 3, 1);
			this.tlpTestLog.Controls.Add(this.label3, 4, 0);
			this.tlpTestLog.Controls.Add(this.label2, 2, 0);
			this.tlpTestLog.Controls.Add(this.numEventID, 1, 1);
			this.tlpTestLog.Controls.Add(this.label1, 0, 0);
			this.tlpTestLog.Controls.Add(this.cboEventLogEntryTypes, 5, 0);
			this.tlpTestLog.Controls.Add(this.txtMessage, 3, 0);
			this.tlpTestLog.Controls.Add(this.btnSaveLog, 5, 2);
			this.tlpTestLog.Controls.Add(this.cboSource, 1, 0);
			this.tlpTestLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpTestLog.Location = new System.Drawing.Point(3, 17);
			this.tlpTestLog.Name = "tlpTestLog";
			this.tlpTestLog.RowCount = 3;
			this.tlpTestLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTestLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTestLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpTestLog.Size = new System.Drawing.Size(772, 90);
			this.tlpTestLog.TabIndex = 0;
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(487, 35);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(59, 15);
			this.label6.TabIndex = 10;
			this.label6.Text = "Raw data";
			// 
			// txtRawData
			// 
			this.txtRawData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRawData.Location = new System.Drawing.Point(552, 32);
			this.txtRawData.Name = "txtRawData";
			this.txtRawData.Size = new System.Drawing.Size(217, 21);
			this.txtRawData.TabIndex = 11;
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(242, 35);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 15);
			this.label5.TabIndex = 8;
			this.label5.Text = "Category";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 35);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(50, 15);
			this.label4.TabIndex = 6;
			this.label4.Text = "Event id";
			// 
			// numCategory
			// 
			this.numCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.numCategory.Location = new System.Drawing.Point(306, 32);
			this.numCategory.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.numCategory.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
			this.numCategory.Name = "numCategory";
			this.numCategory.Size = new System.Drawing.Size(75, 21);
			this.numCategory.TabIndex = 9;
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(487, 7);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 15);
			this.label3.TabIndex = 4;
			this.label3.Text = "Log type";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(242, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "Message";
			// 
			// numEventID
			// 
			this.numEventID.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.numEventID.Location = new System.Drawing.Point(59, 32);
			this.numEventID.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.numEventID.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
			this.numEventID.Name = "numEventID";
			this.numEventID.Size = new System.Drawing.Size(75, 21);
			this.numEventID.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Source";
			// 
			// cboEventLogEntryTypes
			// 
			this.cboEventLogEntryTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cboEventLogEntryTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEventLogEntryTypes.FormattingEnabled = true;
			this.cboEventLogEntryTypes.Location = new System.Drawing.Point(552, 3);
			this.cboEventLogEntryTypes.Name = "cboEventLogEntryTypes";
			this.cboEventLogEntryTypes.Size = new System.Drawing.Size(217, 23);
			this.cboEventLogEntryTypes.TabIndex = 5;
			// 
			// txtMessage
			// 
			this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMessage.Location = new System.Drawing.Point(306, 4);
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(175, 21);
			this.txtMessage.TabIndex = 3;
			this.txtMessage.Text = "Testowy komunikat";
			// 
			// btnSaveLog
			// 
			this.btnSaveLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveLog.AutoSize = true;
			this.btnSaveLog.Location = new System.Drawing.Point(701, 59);
			this.btnSaveLog.Name = "btnSaveLog";
			this.btnSaveLog.Size = new System.Drawing.Size(68, 25);
			this.btnSaveLog.TabIndex = 12;
			this.btnSaveLog.Text = "Save Log";
			this.btnSaveLog.UseVisualStyleBackColor = true;
			this.btnSaveLog.Click += new System.EventHandler(this.btnSaveLog_Click);
			// 
			// cboSource
			// 
			this.cboSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cboSource.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SkayTek.WindowsLogCreator.Properties.Settings.Default, "CboSourceText", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cboSource.FormattingEnabled = true;
			this.cboSource.Items.AddRange(new object[] {
            "Account Management",
            "Certificate Generator",
            "Certificate Service",
            "License Manager Service",
            "Registration Service",
            "Web Site",
            "Web Site Domain",
            "Windows Log Reader"});
			this.cboSource.Location = new System.Drawing.Point(59, 3);
			this.cboSource.Name = "cboSource";
			this.cboSource.Size = new System.Drawing.Size(177, 23);
			this.cboSource.TabIndex = 13;
			this.cboSource.Text = global::SkayTek.WindowsLogCreator.Properties.Settings.Default.CboSourceText;
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 262);
			this.Controls.Add(this.tlpGeneral);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmMain";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Create Windows Log";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
			this.tlpLogAndSource.ResumeLayout(false);
			this.tlpLogAndSource.PerformLayout();
			this.gbxLogAndSource.ResumeLayout(false);
			this.gbxLogAndSource.PerformLayout();
			this.tlpGeneral.ResumeLayout(false);
			this.tlpGeneral.PerformLayout();
			this.grpTest.ResumeLayout(false);
			this.grpTest.PerformLayout();
			this.tlpTestLog.ResumeLayout(false);
			this.tlpTestLog.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numCategory)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numEventID)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Button btnCreateWinLog;
		private System.Windows.Forms.TextBox txtLogName;
		private System.Windows.Forms.TableLayoutPanel tlpLogAndSource;
		private System.Windows.Forms.Label lblSourceName;
		private System.Windows.Forms.Label lblLogName;
		private System.Windows.Forms.Button btnDeleteSource;
		private System.Windows.Forms.Button btnDeleteLog;
		private System.Windows.Forms.GroupBox gbxLogAndSource;
		private System.Windows.Forms.TableLayoutPanel tlpGeneral;
		private System.Windows.Forms.GroupBox grpTest;
		private System.Windows.Forms.TableLayoutPanel tlpTestLog;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtRawData;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numCategory;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numEventID;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboEventLogEntryTypes;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.Button btnSaveLog;
        private System.Windows.Forms.ComboBox cboSourceNames;
        private System.Windows.Forms.ComboBox cboSource;
	}
}

