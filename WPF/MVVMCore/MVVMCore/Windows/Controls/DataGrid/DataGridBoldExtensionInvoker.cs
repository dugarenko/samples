using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MVVMCore.Windows.Controls
{
    /// <summary>
    /// Zapewnia funkcjonalność klasie DataGridBoldExtensionInvoker.
    /// </summary>
    public interface IDataGridBoldExtensionInvoker
    {
        /// <summary>
        /// Reprezentuje metodę, która będzie obsługiwać zdarzenie, które usuwa zaznaczenie wszystkich komórek w DataGridBold.
        /// </summary>
        event EventHandler<EventArgs> UnselectAllCellsHandler;
        /// <summary></summary>
        event Func<double> GetVerticalOffsetHandler;
        /// <summary></summary>
        event Func<double> GetHorizontalOffsetHandler;
        /// <summary></summary>
        event Action<object> ScrollIntoViewHandler;
        /// <summary></summary>
        event Action<object, double, double> ScrollIntoViewAndOffsetsHandler;
        /// <summary></summary>
        event Func<object, double, double, Task> ScrollIntoViewAndOffsetsAsyncHandler;
        /// <summary></summary>
        event Action<double, double> ScrollToOffsetsHandler;
        /// <summary></summary>
        event EventHandler<EventArgs> CopyCellContentToClipboardHandler;
        /// <summary></summary>
        event EventHandler<EventArgs> CopySelectedContentToClipboardHandler;
        /// <summary></summary>
        event Func<bool> FocusHandler;
        /// <summary></summary>
        event Func<ListSortDirection?> GetSortDirectionHandler;

        /// <summary>
        /// Usuwa zaznaczenie wszystkich komórek w DataGridBold.
        /// </summary>
        void UnselectAllCells();

        /// <summary>
        /// Gets a value that contains the vertical offset of the scrolled content.
        /// </summary>
        double GetVerticalOffset();

        /// <summary>
        /// Gets a value that contains the horizontal offset of the scrolled content.
        /// </summary>
        double GetHorizontalOffset();

        /// <summary>
        /// Przewija DataGridBold w pionie, aby wyświetlić wiersz dla określonego elementu danych.
        /// </summary>
        /// <param name="item">Element danych do pokazania.</param>
        void ScrollIntoView(object item);

        /// <summary>
        /// Przewija DataGridBold w pionie, aby wyświetlić wiersz dla określonego elementu danych i do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="item">Element danych do pokazania.</param>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        void ScrollIntoViewAndOffsets(object item, double verticalOffset, double horizontalOffset);

        /// <summary>
        /// Przewija DataGridBold w pionie, aby wyświetlić wiersz dla określonego elementu danych i do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="item">Element danych do pokazania.</param>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        Task ScrollIntoViewAndOffsetsAsync(object item, double verticalOffset, double horizontalOffset);

        /// <summary>
        /// Przewija zawartość w DataGridBold do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        void ScrollToOffsets(double verticalOffset, double horizontalOffset);

        /// <summary>
        /// Kopiuje pojedynczą komórkę do schowka.
        /// </summary>
        void CopyCellContentToClipboard();

        /// <summary>
        /// Kopiuje zaznaczoną zawartość do schowka.
        /// </summary>
        void CopySelectedContentToClipboard();

        /// <summary>
        /// Próbuje ustawić fokus na ten element.
        /// </summary>
        /// <returns>True, jeśli fokus klawiatury i fokus logiczny zostały ustawione na ten element; false, jeśli tylko logiczny fokus został ustawiony na ten element lub jeśli wywołanie tej metody nie wymusiło jego zmiany.</returns>
        bool Focus();

        /// <summary>
        /// Pobiera kierunek sortowania (rosnąco lub malejąco) kolumny. Zarejestrowana wartość domyślna to null.
        /// </summary>
        ListSortDirection? GetSortDirection();
    }

    /// <summary></summary>
    public class DataGridExtensionInvoker : IDataGridBoldExtensionInvoker
    {
        /// <summary>
        /// Reprezentuje metodę, która będzie obsługiwać zdarzenie, które usuwa zaznaczenie wszystkich komórek w DataGridBold.
        /// </summary>
        public event EventHandler<EventArgs> UnselectAllCellsHandler;
        /// <summary></summary>
        public event Func<double> GetVerticalOffsetHandler;
        /// <summary></summary>
        public event Func<double> GetHorizontalOffsetHandler;
        /// <summary></summary>
        public event Action<object> ScrollIntoViewHandler;
        /// <summary></summary>
        public event Action<object, double, double> ScrollIntoViewAndOffsetsHandler;
        /// <summary></summary>
        public event Func<object, double, double, Task> ScrollIntoViewAndOffsetsAsyncHandler;
        /// <summary></summary>
        public event Action<double, double> ScrollToOffsetsHandler;
        /// <summary></summary>
        public event EventHandler<EventArgs> CopyCellContentToClipboardHandler;
        /// <summary></summary>
        public event EventHandler<EventArgs> CopySelectedContentToClipboardHandler;
        /// <summary></summary>
        public event Func<bool> FocusHandler;
        /// <summary></summary>
        public event Func<ListSortDirection?> GetSortDirectionHandler;

        /// <summary>
        /// Usuwa zaznaczenie wszystkich komórek w DataGridBold.
        /// </summary>
        public void UnselectAllCells()
        {
            UnselectAllCellsHandler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets a value that contains the vertical offset of the scrolled content.
        /// </summary>
        public double GetVerticalOffset()
        {
            return GetVerticalOffsetHandler();
        }

        /// <summary>
        /// Gets a value that contains the horizontal offset of the scrolled content.
        /// </summary>
        public double GetHorizontalOffset()
        {
            return GetHorizontalOffsetHandler();
        }

        /// <summary>
        /// Przewija DataGridBold w pionie, aby wyświetlić wiersz dla określonego elementu danych.
        /// </summary>
        /// <param name="item">Element danych do pokazania.</param>
        public void ScrollIntoView(object item)
        {
            ScrollIntoViewHandler?.Invoke(item);
        }

        /// <summary>
        /// Przewija DataGridBold w pionie, aby wyświetlić wiersz dla określonego elementu danych i do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="item">Element danych do pokazania.</param>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        public void ScrollIntoViewAndOffsets(object item, double verticalOffset, double horizontalOffset)
        {
            ScrollIntoViewAndOffsetsHandler(item, verticalOffset, horizontalOffset);
        }

        /// <summary>
        /// Przewija DataGridBold w pionie, aby wyświetlić wiersz dla określonego elementu danych i do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="item">Element danych do pokazania.</param>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        public Task ScrollIntoViewAndOffsetsAsync(object item, double verticalOffset, double horizontalOffset)
        {
            return ScrollIntoViewAndOffsetsAsyncHandler(item, verticalOffset, horizontalOffset);
        }

        /// <summary>
        /// Przewija zawartość w DataGridBold do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        public void ScrollToOffsets(double verticalOffset, double horizontalOffset)
        {
            ScrollToOffsetsHandler?.Invoke(verticalOffset, horizontalOffset);
        }

        /// <summary>
        /// Kopiuje pojedynczą komórkę do schowka.
        /// </summary>
        public void CopyCellContentToClipboard()
        {
            CopyCellContentToClipboardHandler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Kopiuje zaznaczoną zawartość do schowka.
        /// </summary>
        public void CopySelectedContentToClipboard()
        {
            CopySelectedContentToClipboardHandler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Próbuje ustawić fokus na ten element.
        /// </summary>
        /// <returns>True, jeśli fokus klawiatury i fokus logiczny zostały ustawione na ten element; false, jeśli tylko logiczny fokus został ustawiony na ten element lub jeśli wywołanie tej metody nie wymusiło jego zmiany.</returns>
        public bool Focus()
        {
            return FocusHandler();
        }

        /// <summary>
        /// Pobiera kierunek sortowania (rosnąco lub malejąco) kolumny. Zarejestrowana wartość domyślna to null.
        /// </summary>
        public ListSortDirection? GetSortDirection()
        {
            return GetSortDirectionHandler();
        }
    }
}
