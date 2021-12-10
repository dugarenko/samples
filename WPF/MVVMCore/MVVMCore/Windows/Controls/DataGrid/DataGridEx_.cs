using MVVMCore.Diagnostics;
using MVVMCore.Internal;
using MVVMCore.Reflection;
using MVVMCore.Windows.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVMCore.Windows.Controls
{
    public static class DataGridEx
    {
        private static Assembly ASSEMBLY = Assembly.GetAssembly(typeof(TextBlock));
        private static Type TYPE_SELECTED_CELLS_COLLECTION = ASSEMBLY.GetType("System.Windows.Controls.SelectedCellsCollection");
        private static MethodInfo METHODINFO_GET_SELECTION_RANGE = TYPE_SELECTED_CELLS_COLLECTION.GetMethod("GetSelectionRange", MethodInfoEx.DefaultFlags,
            Type.DefaultBinder, new Type[] { typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType() }, null);
        private static MethodInfo METHODINFO_INTERSECTS = TYPE_SELECTED_CELLS_COLLECTION.GetMethod("Intersects", MethodInfoEx.DefaultFlags, Type.DefaultBinder, new Type[] { typeof(int) }, null);
        private static MethodInfo METHODINFO_CONTAINS = TYPE_SELECTED_CELLS_COLLECTION.GetMethod("Contains", MethodInfoEx.DefaultFlags, Type.DefaultBinder, new Type[] { typeof(int), typeof(int) }, null);
        private static PropertyInfo PROPERTYINFO_SELECTED_CELLS_INTERNAL = typeof(DataGrid).GetProperty("SelectedCellsInternal", PropertyInfoEx.DefaultFlags);
        private static ConstructorInfo CONSTRUCTORINFO_DATAGRIDROWCLIPBOARDEVENTARGS = typeof(DataGridRowClipboardEventArgs).GetConstructor(
            MethodInfoEx.DefaultFlags, Type.DefaultBinder, new Type[] { typeof(object), typeof(int), typeof(int), typeof(bool), typeof(int) }, null);
        private static ConstructorInfo CONSTRUCTORINFO_EXECUTEDROUTEDEVENTARGS = typeof(ExecutedRoutedEventArgs).GetConstructor(
            MethodInfoEx.DefaultFlags, Type.DefaultBinder, new Type[] { typeof(ICommand), typeof(object) }, null);

        #region Private methods.

        private static ScrollViewer GetScrollViewer(this DataGrid dataGrid)
        {
            var scrollViewer = dataGrid.Descendants<ScrollViewer>().FirstOrDefault(x => x.Name == "DG_ScrollViewer");
            return scrollViewer;
        }

        /// <summary>
        /// Przewija DataGridFresh w pionie, aby wyświetlić wiersz dla określonego elementu danych i do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="sender">Dostarczony obiekt.</param>
        /// <param name="e">Dostarczone dane.</param>
        private static async void ScrollIntoViewAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)((object[])e.Argument)[0];
            BackgroundWorker backgroundWorker = (BackgroundWorker)((object[])e.Argument)[1];
            object item = ((object[])e.Argument)[2];
            double verticalOffset = (double)((object[])e.Argument)[3];
            double horizontalOffset = (double)((object[])e.Argument)[4];
            int count = 0;
            double currentVerticalOffset = 0;
            bool isNullItem = (item == null);

            if (isNullItem)
            {
                verticalOffset = 0;
            }

            try
            {
                do
                {
                    dataGrid.Dispatcher.Invoke(() =>
                    {
                        if (!isNullItem)
                        {
                            dataGrid.ScrollIntoView(item);
                        }
                        else
                        {
                            if (dataGrid.Items.Count > 0)
                            {
                                dataGrid.ScrollIntoView(dataGrid.Items[0]);
                            }
                        }
                        dataGrid.ScrollToVerticalOffset(verticalOffset);
                        dataGrid.ScrollToHorizontalOffset(horizontalOffset);
                        currentVerticalOffset = dataGrid.VerticalOffset();
                    });

                    await Task.Delay(10);
                    count++;

                    if (count > 50)
                    {
                        break;
                    }

                } while (currentVerticalOffset > verticalOffset);
            }
            finally
            {
                if (backgroundWorker != null)
                {
                    backgroundWorker.Dispose();
                }
            }
        }

        /// <summary>
        /// Provides handling for the System.Windows.Input.CommandBinding.Executed event associated with the System.Windows.Input.ApplicationCommands.Copy command.
        /// </summary>
        /// <param name="args">The data for the event.</param>
        /// <exception cref="System.NotSupportedException">System.Windows.Controls.DataGrid.ClipboardCopyMode is set to System.Windows.Controls.DataGridClipboardCopyMode.None.</exception>
        [SecurityCritical, SecurityTreatAsSafe]
        private static void OnExecutedCopyInternal(this DataGrid dataGrid, ExecutedRoutedEventArgs args)
        {
            if (dataGrid.ClipboardCopyMode == DataGridClipboardCopyMode.None)
            {
                throw new NotSupportedException("Cannot perform copy if ClipboardCopyMode is None.");
            }

            args.Handled = true;

            // Supported default formats: Html, Text, UnicodeText and CSV
            Collection<string> formats = new Collection<string>(new string[] { DataFormats.Html, DataFormats.Text, DataFormats.UnicodeText, DataFormats.CommaSeparatedValue });
            Dictionary<string, StringBuilder> dataGridStringBuilders = new Dictionary<string, StringBuilder>(formats.Count);
            foreach (string format in formats)
            {
                dataGridStringBuilders[format] = new StringBuilder();
            }

            int minRowIndex;
            int maxRowIndex;
            int minColumnDisplayIndex;
            int maxColumnDisplayIndex;

            // Get the bounding box of the selected cells
            if (dataGrid.GetSelectionRange(out minColumnDisplayIndex, out maxColumnDisplayIndex, out minRowIndex, out maxRowIndex))
            {
                // Add column headers if enabled
                if (dataGrid.ClipboardCopyMode == DataGridClipboardCopyMode.IncludeHeader)
                {
                    DataGridRowClipboardEventArgs preparingRowClipboardContentEventArgs = new DataGridRowClipboardEventArgs(null, minColumnDisplayIndex, maxColumnDisplayIndex, true /*IsColumnHeadersRow*/);
                    MethodInfoEx.Invoke(dataGrid, "OnCopyingRowClipboardContent", MethodInfoEx.DefaultFlags, new object[] { preparingRowClipboardContentEventArgs });

                    foreach (string format in formats)
                    {
                        dataGridStringBuilders[format].Append(preparingRowClipboardContentEventArgs.FormatClipboardCellValues(format));
                    }
                }

                // Add each selected row
                for (int i = minRowIndex; i <= maxRowIndex; i++)
                {
                    object row = dataGrid.Items[i];

                    // Row has a selecion
                    if (dataGrid.SelectedCellsIntersects(i))
                    {
                        DataGridRowClipboardEventArgs preparingRowClipboardContentEventArgs = (DataGridRowClipboardEventArgs)CONSTRUCTORINFO_DATAGRIDROWCLIPBOARDEVENTARGS.Invoke(
                            new object[] { row, minColumnDisplayIndex, maxColumnDisplayIndex, false /*IsColumnHeadersRow*/, i });
                        MethodInfoEx.Invoke(dataGrid, "OnCopyingRowClipboardContent", MethodInfoEx.DefaultFlags, new object[] { preparingRowClipboardContentEventArgs });

                        foreach (string format in formats)
                        {
                            dataGridStringBuilders[format].Append(preparingRowClipboardContentEventArgs.FormatClipboardCellValues(format));
                        }
                    }
                }
            }

            DataGridClipboardHelper.GetClipboardContentForHtml(dataGridStringBuilders[DataFormats.Html]);

            DataObject dataObject;
            bool hasPerms = SecurityHelper.CallerHasAllClipboardPermission() && SecurityHelper.CallerHasSerializationPermission();

            // Copy unconditionally in full trust.
            // Only copy in partial trust if user initiated.
            if (hasPerms || args.GetUserInitiated())
            {
                (new UIPermission(UIPermissionClipboard.AllClipboard)).Assert();
                try
                {
                    dataObject = new DataObject();
                }
                finally
                {
                    UIPermission.RevertAssert();
                }

                foreach (string format in formats)
                {
                    if (dataGridStringBuilders[format] != null)
                    {
                        dataObject.SetData(format, dataGridStringBuilders[format].ToString(), false /*autoConvert*/);
                    }
                }

                // This assert is there for an OLE Callback to register CSV type for the clipboard
                (new SecurityPermission(SecurityPermissionFlag.SerializationFormatter | SecurityPermissionFlag.UnmanagedCode)).Assert();
                try
                {
                    if (dataObject != null)
                    {
                        Clipboard.SetDataObject(dataObject, true /* Copy */);
                    }
                }
                finally
                {
                    SecurityPermission.RevertAll();
                }
            }
        }

        #endregion

        #region Internal methods.

        /// <summary>
        /// Określa, czy wiersz zawiera komórki w kolekcji SelectedCellsInternal.
        /// </summary>
        internal static bool SelectedCellsIntersects(this DataGrid dataGrid, int rowIndex)
        {
            object selectedCellsCollection = PROPERTYINFO_SELECTED_CELLS_INTERNAL.GetValue(dataGrid);
            return (bool)METHODINFO_INTERSECTS.Invoke(selectedCellsCollection, new object[] { rowIndex });
        }

        internal static bool SelectedCellsContains(this DataGrid dataGrid, int rowIndex, int columnIndex)
        {
            object selectedCellsCollection = PROPERTYINFO_SELECTED_CELLS_INTERNAL.GetValue(dataGrid);
            return (bool)METHODINFO_CONTAINS.Invoke(selectedCellsCollection, new object[] { rowIndex, columnIndex });
        }

        #endregion

        #region Public methods.

        /// <summary>
        /// Scrolls the content within the ScrollViewerFresh to the specified vertical offset position.
        /// </summary>
        /// <param name="offset">The position that the content scrolls to.</param>
        public static void ScrollToVerticalOffset(this DataGrid dataGrid, double offset)
        {
            var scrollViewer = dataGrid.GetScrollViewer();
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset(offset);
            }
        }

        /// <summary>
        /// Scrolls the content within the ScrollViewerFresh to the specified horizontal offset position.
        /// </summary>
        /// <param name="offset">The position that the content scrolls to.</param>
        public static void ScrollToHorizontalOffset(this DataGrid dataGrid, double offset)
        {
            var scrollViewer = dataGrid.GetScrollViewer();
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToHorizontalOffset(offset);
            }
        }

        /// <summary>
        /// Przewija zawartość w DataGridFresh do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        public static void ScrollToOffsets(this DataGrid dataGrid, double verticalOffset, double horizontalOffset)
        {
            ScrollToVerticalOffset(dataGrid, verticalOffset);
            ScrollToHorizontalOffset(dataGrid, horizontalOffset);
        }

        /// <summary>
        /// Przewija DataGridFresh w pionie, aby wyświetlić wiersz dla określonego elementu danych i do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="item">Element danych do pokazania.</param>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        public async static  Task ScrollIntoViewAsync(this DataGrid dataGrid, object item, double verticalOffset, double horizontalOffset)
        {
            if (verticalOffset < 0)
            {
                verticalOffset = 0;
            }
            if (horizontalOffset < 0)
            {
                horizontalOffset = 0;
            }

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += ScrollIntoViewAsync_DoWork;
            backgroundWorker.RunWorkerAsync(new object[] { dataGrid, backgroundWorker, item, verticalOffset, horizontalOffset });

            int count = 0;
            while (backgroundWorker.IsBusy)
            {
                await Task.Delay(10);
                count++;

                if (count > 100)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Przewija DataGridFresh w pionie, aby wyświetlić wiersz dla określonego elementu danych i do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="item">Element danych do pokazania.</param>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        public static void ScrollIntoView(this DataGrid dataGrid, object item, double verticalOffset, double horizontalOffset)
        {
            if (verticalOffset < 0)
            {
                verticalOffset = 0;
            }
            if (horizontalOffset < 0)
            {
                horizontalOffset = 0;
            }

            if (item != null)
            {
                dataGrid.ScrollIntoView(item);
            }
            else
            {
                verticalOffset = 0;
                if (dataGrid.Items.Count > 0)
                {
                    dataGrid.ScrollIntoView(dataGrid.Items[0]);
                }
            }
            ScrollToVerticalOffset(dataGrid, verticalOffset);
            ScrollToHorizontalOffset(dataGrid, horizontalOffset);
        }

        /// <summary>
        /// Kopiuje pojedynczą komórkę do schowka.
        /// </summary>
        public static void CopyCellContentToClipboard(this DataGrid dataGrid)
        {
            DataGridCellInfo cellInfo;
            DataGridColumn currentColumn = null;
            object currentItem = null;
            string format = DataFormats.UnicodeText;

            if (dataGrid.CurrentCell != null)
            {
                cellInfo = dataGrid.CurrentCell;
            }
            else if (dataGrid.SelectedCells.Count > 0)
            {
                cellInfo = dataGrid.SelectedCells[0];
            }

            currentColumn = cellInfo.Column;

            if (currentColumn == null)
            {
                if (dataGrid.CurrentCell != null && dataGrid.CurrentCell.Column != null)
                {
                    currentColumn = dataGrid.CurrentCell.Column;
                }
                else if (dataGrid.SelectedCells.Count > 0)
                {
                    currentColumn = dataGrid.SelectedCells[0].Column;
                }
            }

            if (currentColumn == null)
            {
                return;
            }

            if (cellInfo.Item != null)
            {
                currentItem = cellInfo.Item;
            }
            else if (dataGrid.CurrentItem != null)
            {
                currentItem = dataGrid.CurrentItem;
            }
            else if (dataGrid.SelectedCells.Count > 0 && dataGrid.SelectedCells[0].Item != null)
            {
                currentItem = dataGrid.SelectedCells[0].Item;
            }

            if (currentItem == null)
            {
                return;
            }

            var value = currentColumn.OnCopyingCellClipboardContent(currentItem);
            if (value == null || value == DBNull.Value)
            {
                value = "";
            }

            StringBuilder sb = new StringBuilder();
            DataGridClipboardHelper.FormatPlainText(value, sb, format);

            DataObject dataObject;
            bool hasPerms = SecurityHelper.CallerHasAllClipboardPermission() && SecurityHelper.CallerHasSerializationPermission();

            // Copy unconditionally in full trust.
            // Only copy in partial trust if user initiated.
            if (hasPerms)
            {
                (new UIPermission(UIPermissionClipboard.AllClipboard)).Assert();
                try
                {
                    dataObject = new DataObject();
                }
                finally
                {
                    UIPermission.RevertAssert();
                }

                dataObject.SetData(format, sb.ToString(), false /*autoConvert*/);

                // This assert is there for an OLE Callback to register CSV type for the clipboard
                (new SecurityPermission(SecurityPermissionFlag.SerializationFormatter | SecurityPermissionFlag.UnmanagedCode)).Assert();
                try
                {
                    if (dataObject != null)
                    {
                        Clipboard.SetDataObject(dataObject, true /* Copy */);
                    }
                }
                finally
                {
                    SecurityPermission.RevertAll();
                }
            }
        }

        /// <summary>
        /// Kopiuje zaznaczoną zawartość do schowka.
        /// </summary>
        public static void CopySelectedContentToClipboard(this DataGrid dataGrid)
        {
            try
            {
                ExecutedRoutedEventArgs args = (ExecutedRoutedEventArgs)CONSTRUCTORINFO_EXECUTEDROUTEDEVENTARGS.Invoke(new object[] { ApplicationCommands.Copy, null });
                args.RoutedEvent = DataObject.CopyingEvent;
                args.Source = dataGrid;

                OnExecutedCopyInternal(dataGrid, args);
            }
            catch (Exception ex)
            {
                if (DebugEx.IsDebug)
                {
                    MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
                }
            }
        }

        /// <summary>
        /// Gets a value that contains the vertical offset of the scrolled content.
        /// </summary>
        public static double VerticalOffset(this DataGrid dataGrid)
        {
            var scrollViewer = dataGrid.GetScrollViewer();
            return scrollViewer != null ? scrollViewer.VerticalOffset : 0;
        }

        /// <summary>
        /// Gets a value that contains the horizontal offset of the scrolled content.
        /// </summary>
        public static double HorizontalOffset(this DataGrid dataGrid)
        {
            var scrollViewer = dataGrid.GetScrollViewer();
            return scrollViewer != null ? scrollViewer.HorizontalOffset : 0;
        }

        /// <summary>
        /// Oblicza obwiednię komórek.
        /// </summary>
        /// <returns>true, jeśli nie jest pusta, false, jeśli jest pusta.</returns>
        public static bool GetSelectionRange(this DataGrid dataGrid, out int minColumnDisplayIndex, out int maxColumnDisplayIndex, out int minRowIndex, out int maxRowIndex)
        {
            minColumnDisplayIndex = 0;
            maxColumnDisplayIndex = 0;
            minRowIndex = 0;
            maxRowIndex = 0;
            object[] args = new object[] { minColumnDisplayIndex, maxColumnDisplayIndex, minRowIndex, maxRowIndex };

            object selectedCellsCollection = PROPERTYINFO_SELECTED_CELLS_INTERNAL.GetValue(dataGrid);
            if ((bool)METHODINFO_GET_SELECTION_RANGE.Invoke(selectedCellsCollection, args))
            {
                minColumnDisplayIndex = Convert.ToInt32(args[0]);
                maxColumnDisplayIndex = Convert.ToInt32(args[1]);
                minRowIndex = Convert.ToInt32(args[2]);
                maxRowIndex = Convert.ToInt32(args[3]);
                return true;
            }
            return false;
        }

        #endregion
    }
}
