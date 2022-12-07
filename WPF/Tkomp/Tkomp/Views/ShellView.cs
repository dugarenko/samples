using System.Windows;
using Tkomp.Interfaces;

namespace Tkomp.Views
{
    public class ShellView : Window, IMessageBox
    {
        public void Show(string messageBoxText)
        {
            MessageBox.Show(this, messageBoxText);
        }
    }
}
