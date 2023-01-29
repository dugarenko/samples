
namespace MSMove
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
            this.components = new System.ComponentModel.Container();
            this.chkMove = new System.Windows.Forms.CheckBox();
            this.tmrMove = new System.Windows.Forms.Timer(this.components);
            this.btnHide = new System.Windows.Forms.Button();
            this.noti = new System.Windows.Forms.NotifyIcon(this.components);
            this.chkInfo = new System.Windows.Forms.CheckBox();
            this.tmrInfo = new System.Windows.Forms.Timer(this.components);
            this.tmrHide = new System.Windows.Forms.Timer(this.components);
            this.chkHex = new System.Windows.Forms.CheckBox();
            this.udHandle = new System.Windows.Forms.NumericUpDown();
            this.lblClassName = new System.Windows.Forms.Label();
            this.lblWindowName = new System.Windows.Forms.Label();
            this.lblHandle = new System.Windows.Forms.Label();
            this.cboWindowName = new System.Windows.Forms.ComboBox();
            this.txtClassName = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.udHandle)).BeginInit();
            this.SuspendLayout();
            // 
            // chkMove
            // 
            this.chkMove.AutoSize = true;
            this.chkMove.Location = new System.Drawing.Point(124, 107);
            this.chkMove.Name = "chkMove";
            this.chkMove.Size = new System.Drawing.Size(67, 22);
            this.chkMove.TabIndex = 1;
            this.chkMove.Text = "Move";
            this.chkMove.UseVisualStyleBackColor = true;
            this.chkMove.CheckedChanged += new System.EventHandler(this.chkMove_CheckedChanged);
            // 
            // tmrMove
            // 
            this.tmrMove.Interval = 60000;
            this.tmrMove.Tick += new System.EventHandler(this.tmrMove_Tick);
            // 
            // btnHide
            // 
            this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHide.Location = new System.Drawing.Point(556, 108);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(112, 39);
            this.btnHide.TabIndex = 4;
            this.btnHide.Text = "Hide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // noti
            // 
            this.noti.BalloonTipText = "MS Move";
            this.noti.BalloonTipTitle = "MS Move";
            this.noti.Text = "MS Move";
            this.noti.Visible = true;
            // 
            // chkInfo
            // 
            this.chkInfo.AutoSize = true;
            this.chkInfo.Location = new System.Drawing.Point(197, 107);
            this.chkInfo.Name = "chkInfo";
            this.chkInfo.Size = new System.Drawing.Size(54, 22);
            this.chkInfo.TabIndex = 5;
            this.chkInfo.Text = "Info";
            this.chkInfo.UseVisualStyleBackColor = true;
            this.chkInfo.CheckedChanged += new System.EventHandler(this.chkInfo_CheckedChanged);
            // 
            // tmrInfo
            // 
            this.tmrInfo.Interval = 6000;
            this.tmrInfo.Tick += new System.EventHandler(this.tmrInfo_Tick);
            // 
            // tmrHide
            // 
            this.tmrHide.Interval = 6000;
            this.tmrHide.Tick += new System.EventHandler(this.tmrHide_Tick);
            // 
            // chkHex
            // 
            this.chkHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkHex.AutoSize = true;
            this.chkHex.Location = new System.Drawing.Point(612, 15);
            this.chkHex.Name = "chkHex";
            this.chkHex.Size = new System.Drawing.Size(56, 22);
            this.chkHex.TabIndex = 6;
            this.chkHex.Text = "Hex";
            this.chkHex.UseVisualStyleBackColor = true;
            this.chkHex.CheckedChanged += new System.EventHandler(this.chkHex_CheckedChanged);
            // 
            // udHandle
            // 
            this.udHandle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.udHandle.Location = new System.Drawing.Point(124, 14);
            this.udHandle.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.udHandle.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.udHandle.Name = "udHandle";
            this.udHandle.Size = new System.Drawing.Size(482, 24);
            this.udHandle.TabIndex = 7;
            // 
            // lblClassName
            // 
            this.lblClassName.AutoSize = true;
            this.lblClassName.Location = new System.Drawing.Point(11, 48);
            this.lblClassName.Name = "lblClassName";
            this.lblClassName.Size = new System.Drawing.Size(91, 18);
            this.lblClassName.TabIndex = 8;
            this.lblClassName.Text = "Class name:";
            // 
            // lblWindowName
            // 
            this.lblWindowName.AutoSize = true;
            this.lblWindowName.Location = new System.Drawing.Point(11, 79);
            this.lblWindowName.Name = "lblWindowName";
            this.lblWindowName.Size = new System.Drawing.Size(107, 18);
            this.lblWindowName.TabIndex = 9;
            this.lblWindowName.Text = "Window name:";
            // 
            // lblHandle
            // 
            this.lblHandle.AutoSize = true;
            this.lblHandle.Location = new System.Drawing.Point(12, 16);
            this.lblHandle.Name = "lblHandle";
            this.lblHandle.Size = new System.Drawing.Size(58, 18);
            this.lblHandle.TabIndex = 10;
            this.lblHandle.Text = "Handle:";
            // 
            // cboWindowName
            // 
            this.cboWindowName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboWindowName.FormattingEnabled = true;
            this.cboWindowName.Items.AddRange(new object[] {
            "*| Microsoft Teams"});
            this.cboWindowName.Location = new System.Drawing.Point(124, 75);
            this.cboWindowName.Name = "cboWindowName";
            this.cboWindowName.Size = new System.Drawing.Size(544, 26);
            this.cboWindowName.TabIndex = 11;
            // 
            // txtClassName
            // 
            this.txtClassName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClassName.FormattingEnabled = true;
            this.txtClassName.Items.AddRange(new object[] {
            "*| Microsoft Teams"});
            this.txtClassName.Location = new System.Drawing.Point(124, 44);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(544, 26);
            this.txtClassName.TabIndex = 12;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 163);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.cboWindowName);
            this.Controls.Add(this.lblHandle);
            this.Controls.Add(this.lblWindowName);
            this.Controls.Add(this.lblClassName);
            this.Controls.Add(this.udHandle);
            this.Controls.Add(this.chkHex);
            this.Controls.Add(this.chkInfo);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.chkMove);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MS Move";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.udHandle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkMove;
        private System.Windows.Forms.Timer tmrMove;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.NotifyIcon noti;
        private System.Windows.Forms.CheckBox chkInfo;
        private System.Windows.Forms.Timer tmrInfo;
        private System.Windows.Forms.Timer tmrHide;
        private System.Windows.Forms.CheckBox chkHex;
        private System.Windows.Forms.NumericUpDown udHandle;
        private System.Windows.Forms.Label lblClassName;
        private System.Windows.Forms.Label lblWindowName;
        private System.Windows.Forms.Label lblHandle;
        private System.Windows.Forms.ComboBox cboWindowName;
        private System.Windows.Forms.ComboBox txtClassName;
    }
}

