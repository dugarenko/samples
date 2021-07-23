using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace MSMove.Win32
{
    internal static class NativeMethods
    {
        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        internal static HandleRef HWND_TOP = new HandleRef(null, (IntPtr)0);
        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        internal static HandleRef HWND_BOTTOM = new HandleRef(null, (IntPtr)1);
        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        internal static HandleRef HWND_TOPMOST = new HandleRef(null, new IntPtr(-1));
        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        internal static HandleRef HWND_NOTOPMOST = new HandleRef(null, new IntPtr(-2));
        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
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
        private struct POINTSTRUCT
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
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable")]
        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
        internal static extern IntPtr GetWindowLong32(HandleRef hWnd, int nIndex);
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
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
}
