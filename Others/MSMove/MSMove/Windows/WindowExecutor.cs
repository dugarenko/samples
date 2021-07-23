using MSMove.Win32;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MSMove.Windows
{
    /// <summary>
	/// Wykonuje egzekucje na oknie.
	/// </summary>
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
}
