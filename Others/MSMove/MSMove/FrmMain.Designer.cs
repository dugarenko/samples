
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
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.txtWindowName = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.noti = new System.Windows.Forms.NotifyIcon(this.components);
            this.chkInfo = new System.Windows.Forms.CheckBox();
            this.tmrInfo = new System.Windows.Forms.Timer(this.components);
            this.tmrHide = new System.Windows.Forms.Timer(this.components);
            this.chkHex = new System.Windows.Forms.CheckBox();
            this.udHandle = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.udHandle)).BeginInit();
            this.SuspendLayout();
            // 
            // chkMove
            // 
            this.chkMove.AutoSize = true;
            this.chkMove.Location = new System.Drawing.Point(12, 96);
            this.chkMove.Name = "chkMove";
            this.chkMove.Size = new System.Drawing.Size(64, 21);
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
            // txtClassName
            // 
            this.txtClassName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClassName.Location = new System.Drawing.Point(12, 40);
            this.txtClassName.MinimumSize = new System.Drawing.Size(358, 22);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(358, 22);
            this.txtClassName.TabIndex = 2;
            // 
            // txtWindowName
            // 
            this.txtWindowName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWindowName.Location = new System.Drawing.Point(12, 68);
            this.txtWindowName.MinimumSize = new System.Drawing.Size(358, 22);
            this.txtWindowName.Name = "txtWindowName";
            this.txtWindowName.Size = new System.Drawing.Size(358, 22);
            this.txtWindowName.TabIndex = 3;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(270, 96);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(100, 35);
            this.btnTest.TabIndex = 4;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
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
            this.chkInfo.Location = new System.Drawing.Point(82, 96);
            this.chkInfo.Name = "chkInfo";
            this.chkInfo.Size = new System.Drawing.Size(53, 21);
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
            this.chkHex.AutoSize = true;
            this.chkHex.Location = new System.Drawing.Point(13, 13);
            this.chkHex.Name = "chkHex";
            this.chkHex.Size = new System.Drawing.Size(54, 21);
            this.chkHex.TabIndex = 6;
            this.chkHex.Text = "Hex";
            this.chkHex.UseVisualStyleBackColor = true;
            this.chkHex.CheckedChanged += new System.EventHandler(this.chkHex_CheckedChanged);
            // 
            // udHandle
            // 
            this.udHandle.Location = new System.Drawing.Point(73, 12);
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
            this.udHandle.Size = new System.Drawing.Size(297, 22);
            this.udHandle.TabIndex = 7;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 144);
            this.Controls.Add(this.udHandle);
            this.Controls.Add(this.chkHex);
            this.Controls.Add(this.chkInfo);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtWindowName);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.chkMove);
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
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.TextBox txtWindowName;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.NotifyIcon noti;
        private System.Windows.Forms.CheckBox chkInfo;
        private System.Windows.Forms.Timer tmrInfo;
        private System.Windows.Forms.Timer tmrHide;
        private System.Windows.Forms.CheckBox chkHex;
        private System.Windows.Forms.NumericUpDown udHandle;
    }
}

