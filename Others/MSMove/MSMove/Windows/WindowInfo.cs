using MSMove.Win32;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MSMove.Windows
{
    internal class WindowInfo
    {
		internal WindowInfo(IntPtr handle)
		{
			if (handle == IntPtr.Zero || NativeMethods.IsWindow(new HandleRef(null, handle)) == false)
			{
				throw new NullReferenceException();
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
		internal IntPtr Handle
		{ get; private set; }

		/// <summary>
		/// Identyfikator procesu.
		/// </summary>
		internal int ProcessId
		{ get; private set; }

		/// <summary>
		/// Identyfikator wątku.
		/// </summary>
		internal int ThreadId
		{ get; private set; }

		/// <summary>
		/// Podpis okna.
		/// </summary>
		internal string Caption
		{ get; private set; }

		/// <summary>
		/// Nazwa klasy.
		/// </summary>
		internal string ClassName
		{ get; private set; }

		/// <summary>
		/// Zapisuje zestaw czterech liczb, które reprezentują położenie i rozmiar prostokąta.
		/// </summary>
		internal Rectangle Rectangle
		{ get; private set; }

		/// <summary>
		/// Wymiary okna.
		/// </summary>
		internal Size Size
		{ get; private set; }

		/// <summary>
		/// Określa czy okno jest włączone.
		/// </summary>
		internal bool IsWindowEnabled
		{ get; private set; }

		/// <summary>
		/// Określa czy okno jest widoczne.
		/// </summary>
		internal bool IsWindowVisible
		{ get; private set; }

		internal int GWL_STYLE
		{ get; private set; }

		internal int GWL_EXSTYLE
		{ get; private set; }

		/// <summary>
		/// Zwraca informację czy okno jeszcze "żyje".
		/// </summary>
		internal bool IsLive
		{
			get
			{
				return NativeMethods.IsWindow(new HandleRef(null, Handle));
			}
		}

		/// <summary>
		/// Określa stan wizualny okna.
		/// </summary>
		internal FormWindowState WindowState
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
		internal bool IsForegroundWindow
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
		internal bool IsActiveWindow
		{
			get
			{
				return (NativeMethods.GetActiveWindow() == Handle);
			}
		}
	}
}
