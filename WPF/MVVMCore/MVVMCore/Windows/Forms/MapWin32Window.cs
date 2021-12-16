using System;

namespace MVVMCore.Windows.Forms
{
    /// <summary>
    /// Zmapowanie uchwytu okna WPF na interfejs Windows Forms (System.Windows.Forms.IWin32Window).
    /// </summary>
    internal class MapWin32Window : System.Windows.Forms.IWin32Window
    {
        /// <summary>
        /// Inicjuje nową instancję klasy.
        /// </summary>
        /// <param name="wpfWindow">Okno WPF.</param>
        public MapWin32Window(System.Windows.Window wpfWindow)
        {
            if (wpfWindow == null)
            {
                wpfWindow = WindowEx.GetActiveWindow();
            }
            Handle = new System.Windows.Interop.WindowInteropHelper(wpfWindow).Handle;
        }

        #region System.Windows.Forms.IWin32Window Members.

        /// <summary>
        /// Pobiera uchwyt okna.
        /// </summary>
        public IntPtr Handle
        { get; private set; }

        #endregion
    }
}
