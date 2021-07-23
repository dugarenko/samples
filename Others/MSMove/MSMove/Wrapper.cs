using MSMove.Common.Interfaces;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MSMove
{
    internal static class NativeMethods
    {
        internal static HandleRef HWND_TOP = new HandleRef(null, (IntPtr)0);
        internal static HandleRef HWND_BOTTOM = new HandleRef(null, (IntPtr)1);
        internal static HandleRef HWND_TOPMOST = new HandleRef(null, new IntPtr(-1));
        internal static HandleRef HWND_NOTOPMOST = new HandleRef(null, new IntPtr(-2));
        internal static HandleRef HWND_MESSAGE = new HandleRef(null, new IntPtr(-3));

        internal const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        internal const uint MOUSEEVENTF_LEFTUP = 0x0004;
        internal const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        internal const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        internal const uint MOUSEEVENTF_MOVE = 0x0001;
        internal const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        internal const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        internal const uint MOUSEEVENTF_XDOWN = 0x0080;
        internal const uint MOUSEEVENTF_XUP = 0x0100;
        internal const uint MOUSEEVENTF_WHEEL = 0x0800;
        internal const uint MOUSEEVENTF_HWHEEL = 0x01000;
        internal const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

        internal const int SWP_NOSIZE = 0x0001;
        internal const int SWP_NOMOVE = 0x0002;
        internal const int SWP_NOZORDER = 0x0004;
        internal const int SWP_NOACTIVATE = 0x0010;
        internal const int SWP_SHOWWINDOW = 0x0040;
        internal const int SWP_HIDEWINDOW = 0x0080;
        internal const int SWP_DRAWFRAME = 0x0020;
        internal const int SWP_NOOWNERZORDER = 0x0200;

        internal const int SW_NORMAL = 1;
        internal const int SW_MAXIMIZE = 3;
        internal const int SW_MINIMIZE = 6;

        internal const int GWL_STYLE = (-16);
        internal const int GWL_EXSTYLE = (-20);

        internal const int WS_MAXIMIZE = 0x01000000;
        internal const int WS_MINIMIZE = 0x20000000;

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int X;
            public int Y;
        }

        //This marshals differently than NativeMethods.POINTSTRUCT 
        internal struct POINTSTRUCT
        {
            public int x;
            public int y;

            public POINTSTRUCT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public RECT(Rectangle r)
            {
                this.left = r.Left;
                this.top = r.Top;
                this.right = r.Right;
                this.bottom = r.Bottom;
            }

            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x, y, x + width, y + height);
            }

            public System.Drawing.Size Size
            {
                get
                {
                    return new System.Drawing.Size(this.right - this.left, this.bottom - this.top);
                }
            }
        }

        [Flags]
        internal enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            /// <summary>
            /// This value is not supported. If ES_USER_PRESENT is combined with other esFlags values,
            /// the call will fail and none of the specified states will be set.
            /// </summary>
            //ES_USER_PRESENT = 0x00000004
        }

        [DllImport("user32.dll")]
        internal static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        internal static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);
        [DllImport("User32", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindow(string className, string windowName);
        [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport("user32.dll", EntryPoint = "WindowFromPoint", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr _WindowFromPoint(POINTSTRUCT pt);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int GetWindowTextLength(HandleRef hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        internal static extern int GetClassName(HandleRef hwnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern bool IsWindow(HandleRef hWnd);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern bool GetWindowRect(HandleRef hWnd, [In, Out] ref RECT rect);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern bool GetClientRect(HandleRef hWnd, [In, Out] ref RECT rect);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern bool IsWindowEnabled(HandleRef hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern bool IsWindowVisible(HandleRef hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern int GetWindowThreadProcessId(HandleRef hWnd, out int lpdwProcessId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern bool ShowWindow(HandleRef hWnd, int nCmdShow);
        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
        internal static extern IntPtr GetWindowLong32(HandleRef hWnd, int nIndex);
        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtr")]
        internal static extern IntPtr GetWindowLongPtr64(HandleRef hWnd, int nIndex);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern bool SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SetActiveWindow(HandleRef hWnd);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        internal static IntPtr WindowFromPoint(Point p)
        {
            POINTSTRUCT ps = new POINTSTRUCT(p.X, p.Y);
            return _WindowFromPoint(ps);
        }

        // GetWindowLong won't work correctly for 64-bit: we should use GetWindowLongPtr instead. On 
        // 32-bit, GetWindowLongPtr is just #defined as GetWindowLong.  GetWindowLong really should
        // take/return int instead of IntPtr/HandleRef, but since we're running this only for 32-bit 
        // it'll be OK.
        internal static IntPtr GetWindowLong(HandleRef hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return GetWindowLong32(hWnd, nIndex);
            }
            return GetWindowLongPtr64(hWnd, nIndex);
        }
    }

    internal class WindowInfo : IWindowInfo
    {
        internal WindowInfo(IntPtr handle)
        {
            if (handle == IntPtr.Zero || !NativeMethods.IsWindow(new HandleRef(null, handle)))
            {
                return;
            }
            Handle = handle;
            Refresh();
        }

        internal WindowInfo(string className, string windowName)
        {
            IntPtr handle = NativeMethods.FindWindow(className, windowName);
            if (handle == IntPtr.Zero)
            {
                handle = NativeMethods.FindWindow(className, "*" + windowName);
                if (handle == IntPtr.Zero)
                {
                    return;
                }
            }

            Handle = handle;
            Refresh();
        }

        internal void Refresh()
        {
            int processId = 0;
            ThreadId = NativeMethods.GetWindowThreadProcessId(new HandleRef(null, Handle), out processId);
            ProcessId = processId;

            int len = NativeMethods.GetWindowTextLength(new HandleRef(null, Handle));
            // Check to see if the system supports DBCS character if so, double the length of the buffer.
            if (System.Windows.Forms.SystemInformation.DbcsEnabled)
            {
                len = (len * 2) + 1;
            }
            StringBuilder sb = new StringBuilder(len + 1);
            NativeMethods.GetWindowText(new HandleRef(null, Handle), sb, sb.Capacity);
            Caption = sb.ToString();

            sb = new StringBuilder(256);
            NativeMethods.GetClassName(new HandleRef(null, Handle), sb, sb.Capacity);
            ClassName = sb.ToString();

            NativeMethods.RECT rect = new NativeMethods.RECT();
            NativeMethods.GetWindowRect(new HandleRef(null, Handle), ref rect);
            Rectangle = new Rectangle(rect.left, rect.top, rect.Size.Width, rect.Size.Height);
            Size = rect.Size;

            NativeMethods.RECT rectClient = new NativeMethods.RECT();
            NativeMethods.GetClientRect(new HandleRef(null, Handle), ref rectClient);
            ClientRectangle = new Rectangle(rectClient.left, rectClient.top, rectClient.Size.Width, rectClient.Size.Height);
            ClientSize = rectClient.Size;

            IsWindowEnabled = NativeMethods.IsWindowEnabled(new HandleRef(null, Handle));
            IsWindowVisible = NativeMethods.IsWindowVisible(new HandleRef(null, Handle));

            GWL_STYLE = unchecked((int)(long)NativeMethods.GetWindowLong(
                new HandleRef(null, Handle), NativeMethods.GWL_STYLE));
            GWL_EXSTYLE = unchecked((int)(long)NativeMethods.GetWindowLong(
                new HandleRef(null, this.Handle), NativeMethods.GWL_EXSTYLE));
        }

        /// <summary>
        /// Uchwyt okna.
        /// </summary>
        public IntPtr Handle
        { get; private set; }

        /// <summary>
        /// Identyfikator procesu.
        /// </summary>
        public int ProcessId
        { get; private set; }

        /// <summary>
        /// Identyfikator wątku.
        /// </summary>
        public int ThreadId
        { get; private set; }

        /// <summary>
        /// Podpis okna.
        /// </summary>
        public string Caption
        { get; private set; }

        /// <summary>
        /// Nazwa klasy.
        /// </summary>
        public string ClassName
        { get; private set; }

        /// <summary>
        /// Zapisuje zestaw czterech liczb, które reprezentują położenie i rozmiar prostokąta.
        /// </summary>
        public Rectangle Rectangle
        { get; private set; }

        /// <summary>
        /// Wymiary okna.
        /// </summary>
        public Size Size
        { get; private set; }

        /// <summary>
        /// Zapisuje zestaw czterech liczb, które reprezentują położenie i rozmiar prostokąta obszaru klienta.
        /// </summary>
        public Rectangle ClientRectangle
        { get; private set; }

        /// <summary>
        /// Wymiary okna klienta.
        /// </summary>
        public Size ClientSize
        { get; private set; }

        /// <summary>
        /// Zwraca wysokość tytułu.
        /// </summary>
        public int TitleHeight
        {
            get
            {
                return Rectangle.Height - ClientRectangle.Height;
            }
        }

        /// <summary>
        /// Określa czy okno jest włączone.
        /// </summary>
        public bool IsWindowEnabled
        { get; private set; }

        /// <summary>
        /// Określa czy okno jest widoczne.
        /// </summary>
        public bool IsWindowVisible
        { get; private set; }

        public int GWL_STYLE
        { get; private set; }

        public int GWL_EXSTYLE
        { get; private set; }

        /// <summary>
        /// Zwraca informację czy okno jeszcze "żyje".
        /// </summary>
        public bool IsLive
        {
            get
            {
                return NativeMethods.IsWindow(new HandleRef(null, Handle));
            }
        }

        /// <summary>
        /// Określa stan wizualny okna.
        /// </summary>
        public FormWindowState WindowState
        {
            get
            {
                if ((GWL_STYLE & NativeMethods.WS_MAXIMIZE) == NativeMethods.WS_MAXIMIZE)
                {
                    return FormWindowState.Maximized;
                }
                else if ((GWL_STYLE & NativeMethods.WS_MINIMIZE) == NativeMethods.WS_MINIMIZE)
                {
                    return FormWindowState.Minimized;
                }
                return FormWindowState.Normal;
            }
        }

        /// <summary>
        /// Określa czy okno jest na pierwszym planie.
        /// </summary>
        public bool IsForegroundWindow
        {
            get
            {
                var res = NativeMethods.GetForegroundWindow();
                return (NativeMethods.GetForegroundWindow() == Handle);
            }
        }

        /// <summary>
        /// Określa czy okno jest aktywne.
        /// </summary>
        public bool IsActiveWindow
        {
            get
            {
                return (NativeMethods.GetActiveWindow() == Handle);
            }
        }
    }

    internal class WindowExecutor : WindowInfo
    {
        internal WindowExecutor(IntPtr handle)
            : base(handle)
        { }

        internal WindowExecutor(string className, string windowName)
            : base(className, windowName)
        { }

        #region Internal methods - static.

        /// <summary>
        /// Pobiera uchwyt do okna na pierwszym planie (okna, z którym aktualnie pracuje użytkownik).
        /// System przypisuje nieco wyższy priorytet wątkowi, który tworzy okno pierwszego planu, niż innym wątkom.
        /// </summary>
        internal static IntPtr GetForegroundWindow()
        {
            return NativeMethods.GetForegroundWindow();
        }

        /// <summary>
        /// Ustawia okno na pierwszy plan.
        /// </summary>
        /// <param name="handle">Uchwyt okna.</param>
        /// <returns>true jeśli okno zostało ustawione na pierwszy plan, w przeciwnym razie false.</returns>
        internal static bool SetForegroundWindow(IntPtr handle)
        {
            return NativeMethods.SetForegroundWindow(handle);
        }

        /// <summary>
        /// Ostawia okno jako aktywne.
        /// </summary>
        /// <returns>true jeśli okno zostało ustawione jako aktywne, w przeciwnym razie false.</returns>
        internal bool SetActiveWindow()
        {
            // Jeśli funkcja się powiedzie, wartością zwracaną jest uchwyt do okna, które było wcześniej aktywne.
            return SetActiveWindow(Handle);
        }

        /// <summary>
        /// Zwraca bieżącą pozycję kursora myszy.
        /// </summary>
        internal static Point GetCursorPos()
        {
            NativeMethods.POINT lpPoint;
            if (NativeMethods.GetCursorPos(out lpPoint))
            {
                return new Point(lpPoint.X, lpPoint.Y);
            }
            return new Point(-1, -1);
        }

        /// <summary>
        /// Ustawia pozycję kursora myszy.
        /// </summary>
        /// <param name="point">Współrzędne docelowe.</param>
        /// <returns>true, jeśli współrzędne myszy zostały ustawione, w przeciwnym razie false.</returns>
        internal static bool SetCursorPos(Point point)
        {
            return NativeMethods.SetCursorPos(point.X, point.Y);
        }

        /// <summary>
        /// Ustawia pozycję kursora myszy na wskazanym ekranie i wywołuje kliknięcie lewego przycisku myszy.
        /// </summary>
        /// <param name="point">Współrzędne docelowe.</param>
        /// <param name="screen">Ekran.</param>
        /// <returns>true, jeśli współrzędne myszy zostały ustawione, w przeciwnym razie false.</returns>
        internal static void SetCursorPosMouseLeftButtonClick(Point point, Screen screen)
        {
            uint dwFlags = NativeMethods.MOUSEEVENTF_ABSOLUTE + NativeMethods.MOUSEEVENTF_MOVE +
                NativeMethods.MOUSEEVENTF_LEFTDOWN + NativeMethods.MOUSEEVENTF_LEFTUP;
            int dx = (point.X * 65535 / screen.Bounds.Width);
            int dy = (point.Y * 65535 / screen.Bounds.Height);
            NativeMethods.mouse_event(dwFlags, dx, dy, 0, 0);
        }

        /// <summary>
        /// Wywołuje kliknięcie lewego przycisku myszy.
        /// </summary>
        internal static void MouseLeftButtonClick()
        {
            NativeMethods.mouse_event(NativeMethods.MOUSEEVENTF_LEFTDOWN | NativeMethods.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        /// <summary>
        /// Zwraca uchwyt aktywnego okna.
        /// </summary>
        internal static IntPtr GetActiveWindow()
        {
            return NativeMethods.GetActiveWindow();
        }

        /// <summary>
        /// Ustawia okno jako aktywne na podstawie wskazanego uchwytu.
        /// </summary>
        /// <param name="handle">Uchwyt okna.</param>
        /// <returns>true jeśli okno zostało ustawione jako aktywne, w przeciwnym razie false.</returns>
        internal static bool SetActiveWindow(IntPtr handle)
        {
            // Jeśli funkcja się powiedzie, wartością zwracaną jest uchwyt do okna, które było wcześniej aktywne.
            return (NativeMethods.SetActiveWindow(new HandleRef(null, handle)) != IntPtr.Zero);
        }

        #endregion

        #region Internal methods.

        /// <summary>
        /// Minimalizuje okno.
        /// </summary>
        internal void MinimizeWindow()
        {
            NativeMethods.ShowWindow(new HandleRef(null, Handle), NativeMethods.SW_MINIMIZE);
            Refresh();
        }

        /// <summary>
        /// Maksymalizuje okno.
        /// </summary>
        internal void MaximizeWindow()
        {
            NativeMethods.ShowWindow(new HandleRef(null, Handle), NativeMethods.SW_MAXIMIZE);
            Refresh();
        }

        /// <summary>
        /// Pokazuje okno normalnie.
        /// </summary>
        internal void NormalWindow()
        {
            NativeMethods.ShowWindow(new HandleRef(null, Handle), NativeMethods.SW_NORMAL);
            Refresh();
        }

        /// <summary>
        /// Wysuwa okno na pierwszy plan. Okno nie jest na trwałe utrzymywane na pierwszym planie.
        /// </summary>
        internal void BringToFrontWindow()
        {
            NativeMethods.SetWindowPos(new HandleRef(null, Handle), NativeMethods.HWND_TOP, 0, 0, 0, 0,
                NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE | NativeMethods.SWP_SHOWWINDOW);
            Refresh();
        }

        /// <summary>
        /// Ustawia okno na pierwszy plan.
        /// </summary>
        /// <returns>true jeśli okno zostało ustawione na pierwszy plan, w przeciwnym razie false.</returns>
        internal bool SetForegroundWindow()
        {
            return SetForegroundWindow(Handle);
        }

        #endregion
    }

    internal class Wrapper : IWrapper
    {
        internal Wrapper(IntPtr handle, string className, string windowName, bool rollbackState)
        {
            Handle = handle;
            ClassName = className;
            WindowName = windowName;
            RollbackState = rollbackState;
        }

        /// <summary>
        /// Umożliwia aplikacji informowanie systemu, że jest w użyciu, zapobiegając w ten sposób
        /// przejściu systemu w stan uśpienia lub wyłączeniu wyświetlacza podczas działania aplikacji.
        /// </summary>
        /// <param name="noSleepOrTurnOff">Wratość 'true' zapobiega przejściu systemu w stan uśpienia lub wyłączeniu wyświetlacza.</param>
        public void Display(bool noSleepOrTurnOff)
        {
            if (noSleepOrTurnOff)
            {
                NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_DISPLAY_REQUIRED | NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
            }
            else
            {
                NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
            }
        }

        public bool Move()
        {
            WindowExecutor we = null;
            if (Handle != IntPtr.Zero)
            {
                we = new WindowExecutor(Handle);
            }
            else
            {
                we = new WindowExecutor(ClassName, WindowName);
            }
            if (we.Handle == IntPtr.Zero)
            {
                Handle = IntPtr.Zero;
                return false;
            }
            Debug.WriteLine(string.Format("ClassName:{0}   Caption:{1}   Rectangle:{2}", we.ClassName, we.Caption, we.Rectangle));

            // Zapisujem stany.
            FormWindowState saveWindowState = we.WindowState;
            IntPtr saveActiveWindowHandle = WindowExecutor.GetForegroundWindow();
            Point saveCursorPos = WindowExecutor.GetCursorPos();

            if (we.WindowState == FormWindowState.Minimized)
            {
                // Pokazanie okna w stanie normalnym.
                we.NormalWindow();
                // Czekamy na pokazanie okna.
                Thread.Sleep(500);
            }

            if (we.Handle != saveActiveWindowHandle)
            {
                // Ustawienie okna na pierwszym planie.
                bool result = we.SetForegroundWindow();
            }

            // Pobranie docelowych współrzednych kliknięcia myszy.
            Point point = new Point(we.Rectangle.X + (we.Size.Width / 2), we.Rectangle.Y + (we.TitleHeight / 2));

            // Czy ustawiono kusor myszy na żądanej pozycji?
            if (WindowExecutor.SetCursorPos(point))
            {
                // Kliknięcie w tytuł okna.
                WindowExecutor.MouseLeftButtonClick();

                if (RollbackState)
                {
                    // Ewentualne przywrócenie stanów.
                    if (we.WindowState != saveWindowState)
                    {
                        if (saveWindowState == FormWindowState.Maximized)
                        {
                            we.MaximizeWindow();
                        }
                        else if (saveWindowState == FormWindowState.Minimized)
                        {
                            we.MinimizeWindow();
                        }
                        else if (saveWindowState == FormWindowState.Normal)
                        {
                            we.NormalWindow();
                        }
                    }
                    if (saveActiveWindowHandle != IntPtr.Zero)
                    {
                        // Ustawienie okna na pierwszym planie.
                        WindowExecutor.SetForegroundWindow(saveActiveWindowHandle);
                        // Ustawienie okna jako aktywne.
                        WindowExecutor.SetActiveWindow(saveActiveWindowHandle);
                    }
                }
                WindowExecutor.SetCursorPos(saveCursorPos);
            }

            return true;
        }

        /// <summary>
        /// Zwraca informacje o oknie z pod bieżącej pozycji kursora myszy.
        /// </summary>
        public static IWindowInfo GetWindowInfoFromPoint()
        {
            NativeMethods.POINT lpPoint;
            if (NativeMethods.GetCursorPos(out lpPoint))
            {
                Point point = new Point(lpPoint.X, lpPoint.Y);
                IntPtr handle = NativeMethods.WindowFromPoint(point);

                if (handle != IntPtr.Zero && NativeMethods.IsWindow(new HandleRef(null, handle)))
                {
                    WindowInfo wi = new WindowInfo(handle);
                    return wi;
                }
            }
            return null;
        }

        /// <summary>
        /// Uchwyt okna.
        /// </summary>
        public IntPtr Handle
        { get; private set; }

        /// <summary>
        /// Nazwa klasy.
        /// </summary>
        public string ClassName
        { get; private set; }

        /// <summary>
		/// Podpis okna.
		/// </summary>
		public string WindowName
        { get; private set; }

        /// <summary>
        /// Przywraca stany, np. jeśli okno było zminimalizowane to do takiego stanu zostanie przywrócone.
        /// </summary>
        public bool RollbackState
        { get; private set; }
    }
}
