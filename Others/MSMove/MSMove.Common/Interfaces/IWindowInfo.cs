using System;
using System.Drawing;
using System.Windows.Forms;

namespace MSMove.Common.Interfaces
{
    public interface IWindowInfo
    {
        /// <summary>
        /// Uchwyt okna.
        /// </summary>
        IntPtr Handle
        { get; }

        /// <summary>
        /// Identyfikator procesu.
        /// </summary>
        int ProcessId
        { get; }

        /// <summary>
        /// Identyfikator wątku.
        /// </summary>
        int ThreadId
        { get; }

        /// <summary>
        /// Podpis okna.
        /// </summary>
        string Caption
        { get; }

        /// <summary>
        /// Nazwa klasy.
        /// </summary>
        string ClassName
        { get; }

        /// <summary>
        /// Zapisuje zestaw czterech liczb, które reprezentują położenie i rozmiar prostokąta.
        /// </summary>
        Rectangle Rectangle
        { get; }

        /// <summary>
        /// Wymiary okna.
        /// </summary>
        Size Size
        { get; }

        /// <summary>
        /// Zapisuje zestaw czterech liczb, które reprezentują położenie i rozmiar prostokąta obszaru klienta.
        /// </summary>
        Rectangle ClientRectangle
        { get; }

        /// <summary>
        /// Wymiary okna klienta.
        /// </summary>
        Size ClientSize
        { get; }

        /// <summary>
        /// Zwraca wysokość tytułu.
        /// </summary>
        int TitleHeight
        { get; }

        /// <summary>
        /// Określa czy okno jest włączone.
        /// </summary>
        bool IsWindowEnabled
        { get; }

        /// <summary>
        /// Określa czy okno jest widoczne.
        /// </summary>
        bool IsWindowVisible
        { get; }

        int GWL_STYLE
        { get; }

        int GWL_EXSTYLE
        { get; }

        /// <summary>
        /// Zwraca informację czy okno jeszcze "żyje".
        /// </summary>
        bool IsLive
        { get; }

        /// <summary>
        /// Określa stan wizualny okna.
        /// </summary>
        FormWindowState WindowState
        { get; }

        /// <summary>
        /// Określa czy okno jest na pierwszym planie.
        /// </summary>
        bool IsForegroundWindow
        { get; }

        /// <summary>
        /// Określa czy okno jest aktywne.
        /// </summary>
        bool IsActiveWindow
        { get; }
    }
}
