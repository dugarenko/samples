using SkayTek.WindowsLogCreator.Properties;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace SkayTek.WindowsLogCreator
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            cboEventLogEntryTypes.DataSource = Enum.GetNames(typeof(EventLogEntryType));
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLogName.Text == "")
                    throw new ArgumentException("Podaj nazwę dziennika.");

                if (EventLog.Exists(txtLogName.Text))
                {
                    EventLog.Delete(txtLogName.Text);
                    MessageBox.Show("Dziennik został usunięty.");
                }
                else
                {
                    MessageBox.Show("Dziennik nie istnieje.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteSource_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboSourceNames.Text == "")
                    throw new ArgumentException("Podaj nazwę źródła.");

                if (EventLog.SourceExists(cboSourceNames.Text))
                {
                    EventLog.DeleteEventSource(cboSourceNames.Text);
                    MessageBox.Show("Źródło zostało usnięte.");
                }
                else
                {
                    MessageBox.Show("Źródło nie istnieje.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCreateWinLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboSourceNames.Text == "")
                    throw new ArgumentException("Podaj nazwę źródła.");
                if (txtLogName.Text == "")
                    throw new ArgumentException("Podaj nazwę dziennika.");

                EventLog.CreateEventSource(cboSourceNames.Text, txtLogName.Text);
                MessageBox.Show("Dziennik został utworzony.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboSource.Text == "")
                    throw new ArgumentException("Podaj nazwę źródła.");

                byte[] rawData = Encoding.UTF8.GetBytes(txtRawData.Text);

                // Jeśli podana nazwa żródła nie istnieje wpis zostanie dodany do dziennika systemu Windows (Aplikacja).
                EventLog.WriteEntry(cboSource.Text, txtMessage.Text,
                    (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), cboEventLogEntryTypes.Text, false),
                    (int)numEventID.Value, (short)numCategory.Value, rawData);

                MessageBox.Show("Dodano wpis do dziennika.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
