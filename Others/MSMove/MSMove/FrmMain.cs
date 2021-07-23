using MSMove.Properties;
using System;
using System.Windows.Forms;

namespace MSMove
{
    public partial class FrmMain : Form
    {
        // Parametry.
        private string _className = "Notepad";
        private string _windowName = "Bez tytułu — Notatnik";
        private bool _rollbackState = false;
        private int _interval = 60000;

        public FrmMain()
        {
            InitializeComponent();
            //string encryptedWrapper = MSMove.Common.WrapperHelper.Encrypt(Resources.WrapperBase64String);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                Icon = Resources.app;
                tmrMove.Interval = _interval;

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

                MSMove.Common.WrapperHelper.Display(false, _className, _windowName, _rollbackState);
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

                chkInfo.Checked = false;
                chkInfo.Enabled = false;
                tmrInfo.Enabled = false;

                if (chk.Checked && txtClassName.Visible)
                {
                    _className = txtClassName.Text;
                    _windowName = txtWindowName.Text;

                    tmrHide.Enabled = true;

                    MSMove.Common.WrapperHelper.Display(true, _className, _windowName, _rollbackState);
                }
                else
                {
                    txtClassName.Visible = true;
                    txtWindowName.Visible = true;
                    chk.Checked = false;

                    chkInfo.Checked = false;
                    chkInfo.Enabled = true;

                    MSMove.Common.WrapperHelper.Display(false, _className, _windowName, _rollbackState);
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
                if (!MSMove.Common.WrapperHelper.Move(_className, _windowName, _rollbackState))
                {
                    tmrMove.Enabled = false;
                    chkMove.Checked = false;
                    txtClassName.Visible = false;
                    txtWindowName.Visible = false;
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
                txtClassName.Visible = false;
                txtWindowName.Visible = false;
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
                var winInfo = MSMove.Common.WrapperHelper.GetWindowInfoFromPoint();
                if (winInfo != null)
                {
                    txtClassName.Text = winInfo.ClassName;
                    txtWindowName.Text = winInfo.Caption;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
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
    }
}
