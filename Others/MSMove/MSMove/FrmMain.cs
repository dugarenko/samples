using MSMove.Properties;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MSMove
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            // Szyfrujemy tą klasę.
            // Ustaw właściwość klasy Wrapper 'Build Action = None', wówczas klasa będzie pobierana z zasobów i kompilowana w locie.
            //string encryptedWrapper = MSMove.Common.WrapperHelper.Encrypt(Resources.WrapperBase64String);
            //Debug.WriteLine(encryptedWrapper);
            //string sourceCode = MSMove.Common.WrapperHelper.Decrypt(Resources.WrapperBase64String);
            //Debug.WriteLine(sourceCode);

            chkHex.Checked = Settings.Default.Hex;
            txtClassName.Text = Settings.Default.ClassName;
            cboWindowName.Text = Settings.Default.WindowName;
        }

        private IntPtr GetHandle()
        {
            IntPtr handle = IntPtr.Zero;
            try
            {
                long hwnd = Convert.ToInt64(udHandle.Value);
                handle = new IntPtr(hwnd);
            }
            catch { }
            return handle;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                Icon = Resources.app;
                tmrMove.Interval = Settings.Default.MoveInterval;

                noti.Icon = Icon;
                noti.ContextMenu = new ContextMenu();
                noti.ContextMenu.MenuItems.Add(new MenuItem("Exit", notiMenu_Exit));
                noti.Click += Noti_Click;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Noti_Click(object sender, EventArgs e)
        {
            try
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void notiMenu_Exit(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                tmrInfo.Enabled = false;
                tmrInfo.Dispose();

                tmrMove.Enabled = false;
                tmrMove.Dispose();

                noti.Icon = null;
                noti.Dispose();

                MSMove.Common.WrapperHelper.Display(false, GetHandle(), txtClassName.Text, cboWindowName.Text, Settings.Default.RollbackState);

                Settings.Default.Hex = chkHex.Checked;
                Settings.Default.ClassName = txtClassName.Text;
                Settings.Default.WindowName = cboWindowName.Text;
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkMove_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var chk = sender as CheckBox;
                if (chk == null)
                {
                    return;
                }
                
                tmrInfo.Enabled = false;

                if (chk.Checked && txtClassName.Visible)
                {
                    chkHex.Enabled = false;
                    udHandle.Enabled = false;
                    txtClassName.Enabled = false;
                    cboWindowName.Enabled = false;
                    chkInfo.Enabled = false;
                    chkInfo.Checked = false;

                    tmrHide.Enabled = true;
                    MSMove.Common.WrapperHelper.Display(true, GetHandle(), txtClassName.Text, cboWindowName.Text, Settings.Default.RollbackState);
                }
                else
                {
                    lblHandle.Visible = true;
                    lblClassName.Visible = true;
                    lblWindowName.Visible= true;

                    chkHex.Visible = true;
                    udHandle.Visible = true;
                    txtClassName.Visible = true;
                    cboWindowName.Visible = true;

                    chkHex.Enabled = true;
                    udHandle.Enabled = true;
                    txtClassName.Enabled = true;
                    cboWindowName.Enabled = true;

                    chkInfo.Checked = false;
                    chkInfo.Enabled = true;

                    chk.Checked = false;

                    MSMove.Common.WrapperHelper.Display(false, GetHandle(), txtClassName.Text, cboWindowName.Text, Settings.Default.RollbackState);
                }

                tmrMove.Enabled = chk.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tmrMove_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!MSMove.Common.WrapperHelper.Move(GetHandle(), txtClassName.Text, cboWindowName.Text, Settings.Default.RollbackState))
                {
                    tmrMove.Enabled = false;
                    chkMove.Checked = false;

                    lblHandle.Visible = false;
                    lblClassName.Visible = false;
                    lblWindowName.Visible = false;

                    chkHex.Visible = false;
                    udHandle.Visible = false;
                    txtClassName.Visible = false;
                    cboWindowName.Visible = false;

                    chkInfo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tmrHide_Tick(object sender, EventArgs e)
        {
            try
            {
                lblHandle.Visible = false;
                lblClassName.Visible = false;
                lblWindowName.Visible = false;

                chkHex.Visible = false;
                udHandle.Visible = false;
                txtClassName.Visible = false;
                cboWindowName.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                tmrHide.Enabled = false;
            }
        }

        private void chkInfo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var chk = sender as CheckBox;
                if (chk == null)
                {
                    return;
                }

                if (chk.Checked)
                {
                    chkMove.Enabled = false;
                }
                else
                {
                    chkMove.Enabled = true;
                }

                tmrInfo.Enabled = chk.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tmrInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                var wi = MSMove.Common.WrapperHelper.GetWindowInfoFromPoint();
                if (wi != null)
                {
                    udHandle.Value = wi.Handle.ToInt64();
                    txtClassName.Text = wi.ClassName;
                    cboWindowName.Text = wi.Caption;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            try
            {
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkHex_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var chk = sender as CheckBox;
                if (chk == null)
                {
                    return;
                }

                udHandle.Hexadecimal = chk.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
