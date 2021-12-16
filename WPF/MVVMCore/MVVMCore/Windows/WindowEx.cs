using MVVMCore.Reflection;
using MVVMCore.Win32;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace MVVMCore.Windows
{
    /// <summary>
    /// Rozszerzenie klasy Window.
    /// </summary>
    public static class WindowEx
    {
        /// <summary>
        /// Zwraca wartość wskazującą, czy okno zostało usunięte.
        /// </summary>
        /// <param name="window">Okno.</param>
        public static bool IsDisposed(this Window window)
        {
            return PropertyInfoEx.GetValue<Window, bool>(window, "IsDisposed");
        }

        /// <summary>
        /// Zwraca wartość wskazującą, czy okno jest modalne (ShowDialog).
        /// </summary>
        /// <param name="window">Okno.</param>
        public static bool IsModal(this Window window)
        {
            return FieldInfoEx.GetValue<Window, bool>(window, "_showingAsDialog");
        }

        /// <summary>
        /// Zwraca główne okno aplikacji.
        /// </summary>
        public static Window GetMainWindow()
        {
            // ===================================
            // Patrz jeszcze w DependencyObjectEx.
            // ===================================

            Window window = null;

            if (Application.Current.Dispatcher.CheckAccess())
            {
                try
                {
                    window = Application.Current.MainWindow;
                }
                catch
                {
                    window = Application.Current.Windows.OfType<Window>().FirstOrDefault();
                }
            }
            else
            {
                window = Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        return Application.Current.MainWindow;
                    }
                    catch
                    {
                        return Application.Current.Windows.OfType<Window>().FirstOrDefault();
                    }
                });
            }

            return window;
        }

        /// <summary>
        /// Zwraca aktywne okno aplikacji.
        /// </summary>
        public static Window GetActiveWindow()
        {
            Window window = null;

            if (Application.Current.Dispatcher.CheckAccess())
            {
                window = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
            }
            else
            {
                window = Application.Current.Dispatcher.Invoke(() =>
                {
                    return Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
                });
            }

            if (window != null)
            {
                return window;
            }

            IntPtr activeWindowHandle = NativeMethods.GetActiveWindow();
            if (activeWindowHandle != IntPtr.Zero)
            {
                if (NativeMethods.IsWindow(new HandleRef(null, activeWindowHandle)))
                {
                    window = HwndSource.FromHwnd(activeWindowHandle).RootVisual as Window;
                }
            }

            return window;
        }

        /// <summary>
        /// Centruje okno na ekranie.
        /// </summary>
        /// <param name="window">Okno.</param>
        public static void CenterToScreen(this Window window)
        {
            if (window.WindowState != WindowState.Normal)
            {
                return;
            }

            Rect workingArea = SystemParameters.WorkArea;
            Point point = new Point(
                (workingArea.Width - window.Width) / 2,
                (workingArea.Height - window.Height) / 2);

            window.Left = point.X;
            window.Top = point.Y;
        }

        /// <summary>
        /// Wyświetla okno na całym obszarze roboczym ekranu. Jeśli okno jest większe zostanie zmniejszone.
        /// </summary>
        /// <param name="window">Okno.</param>
        public static void ToWorkingArea(this Window window)
        {
            if (window.WindowState != WindowState.Normal)
            {
                return;
            }

            Rect workingArea = SystemParameters.WorkArea;
            window.Width = workingArea.Width;
            window.Height = workingArea.Height;

            CorectWindowPosition(window);
        }

        /// <summary>
        /// Koryguje pozycję i rozmiar okna na ekranie. Rozmiar okna nigdy większy niż obszar roboczy ekranu,
        /// lokalizacja okna zawsze w obszarze roboczym ekranu, okno zawsze w całości na ekranie roboczym.
        /// </summary>
        /// <param name="window">Okno.</param>
        public static void CorectWindowPosition(this Window window)
        {
            if (window.WindowState != WindowState.Normal)
            {
                return;
            }

            Rect workingArea = SystemParameters.WorkArea;
            Point saveLocation = new Point(window.Left, window.Top);
            Size saveSize = new Size(window.Width, window.Height);

            // Rozmiar okna nigdy większy niż obszar roboczy ekranu.
            if (saveSize.Width > workingArea.Width || saveSize.Height > workingArea.Height)
            {
                if (saveSize.Width > workingArea.Width)
                {
                    window.Width = workingArea.Size.Width;
                }
                else
                {
                    window.Width = saveSize.Width;
                }

                if (saveSize.Height > workingArea.Height)
                {
                    window.Height = workingArea.Height;
                }
                else
                {
                    window.Height = saveSize.Height;
                }
            }

            // Lokalizacja okna zawsze w obszarze roboczym ekranu.
            if (saveLocation.X < workingArea.X || saveLocation.Y < workingArea.Y)
            {
                if (saveLocation.X < workingArea.X)
                {
                    window.Left = workingArea.Left;
                }
                else
                {
                    window.Left = saveLocation.X;
                }

                if (saveLocation.Y < workingArea.Y || saveLocation.Y > workingArea.Bottom)
                {
                    window.Top = workingArea.Y;
                }
                else
                {
                    window.Top = saveLocation.Y;
                }
            }

            // Okno zawsze w całości na ekranie roboczym.
            if (window.Top + window.Height > workingArea.Bottom)
            {
                window.Top = workingArea.Bottom - window.Height;
            }

            if (window.Left + window.Width > workingArea.Right)
            {
                window.Left = workingArea.Right - window.Width;
            }
        }

        /// <summary>
        /// Koryguje pozycję i rozmiar okna na ekranie. Rozmiar okna nigdy większy niż obszar roboczy ekranu,
        /// lokalizacja okna zawsze w obszarze roboczym ekranu, okno zawsze w całości na ekranie roboczym.
        /// </summary>
        /// <param name="window">Okno.</param>
        /// <param name="centerScreen">Określa czy okno wyświetlić na środku ekranu.</param>
        public static void CorectWindowPosition(this Window window, bool centerScreen)
        {
            if (window.WindowState != WindowState.Normal)
            {
                return;
            }

            CorectWindowPosition(window);

            if (centerScreen)
            {
                CenterToScreen(window);
            }
        }
    }
}
