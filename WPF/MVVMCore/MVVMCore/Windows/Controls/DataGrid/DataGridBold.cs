using MVVMCore.Internal;
using MVVMCore.Reflection;
using MVVMCore.Windows.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVMCore.Windows.Controls
{
    /// <summary>
    /// Kontrolka DataGridBold.
    /// </summary>
    [ToolboxItem(true)]
    [DesignTimeVisible(true)]
    public sealed class DataGridBold : DataGrid, IDisposable
    {
        #region Declarations.

        private static Type _thisType = typeof(DataGridBold);
        private static ConstructorInfo CONSTRUCTORINFO_DATAGRIDROWCLIPBOARDEVENTARGS = typeof(DataGridRowClipboardEventArgs).GetConstructor(
            MethodInfoEx.DefaultFlags, System.Type.DefaultBinder, new Type[] { typeof(object), typeof(int), typeof(int), typeof(bool), typeof(int) }, null);
        private static ConstructorInfo CONSTRUCTORINFO_EXECUTEDROUTEDEVENTARGS = typeof(ExecutedRoutedEventArgs).GetConstructor(
            MethodInfoEx.DefaultFlags, System.Type.DefaultBinder, new Type[] { typeof(ICommand), typeof(object) }, null);
        //
        private Window _window = null;
        private ScrollViewer _scrollViewer = null;
        private long _selectedItemsChanging = 0;
        //
        private static PropertyDescriptor _propertyDescriptorActualWidth = DependencyPropertyDescriptor.FromProperty(DataGridColumn.ActualWidthProperty, typeof(DataGridColumn));

        #endregion

        #region Handlers.

        /// <summary>
        /// Występuje, gdy zmienia się wartość ColumnActualWidth.
        /// </summary>
        public event EventHandler<DataGridColumnEventArgs> ColumnActualWidthProperty;

        /// <summary>
        /// Występuje, gdy zmienia się pozycja paska przewijania.
        /// </summary>
        public event ScrollChangedEventHandler ScrollChanged
        {
            add { AddHandler(ScrollViewer.ScrollChangedEvent, value); }
            remove { RemoveHandler(ScrollViewer.ScrollChangedEvent, value); }
        }

        #endregion

        #region DependencyProperty.

        /// <summary>
        /// DependencyProperty for SelectedItems property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems",
            typeof(System.Collections.IList), _thisType, new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnSelectedItemsChanged)));

        #endregion

        #region Constructors.

        /// <summary>
        /// Inicjuje nową instancję kontrolki DataGridBold.
        /// </summary>
        public DataGridBold()
        {
            this.ID = Guid.NewGuid();

            string assemblyName = _thisType.Assembly.GetName().Name;
            Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri($"/{assemblyName};component/Styles/DataGridBoldStyle.xaml", UriKind.Relative)
            });

            RowStyle = Resources["DataGridRowDefault"] as Style;
            CellStyle = Resources["DataGridCellDefault"] as Style;

            // Wczytujemy oryginalną wartość 'base.SelectedItems' do 'SelectedItemsProperty'.
            SetCurrentValue(SelectedItemsProperty, base.SelectedItems);
            this.Columns.CollectionChanged += Columns_CollectionChanged;
            this.ApplyTemplate();

            this.Loaded += DataGrid_Loaded;
        }

        #endregion

        #region Private methods - PropertyChangedCallback

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (DataGridBold)d;
            if (c.IsDisposed)
            {
                return;
            }

            if (c.SelectionMode == DataGridSelectionMode.Extended)
            {
                c.UpdateSelectedItems((System.Collections.IList)e.NewValue);
            }
        }

        private static void OnVScrollBarWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (DataGridBold)d;
            if (c.IsDisposed)
            {
                return;
            }

            if (c._scrollViewer != null)
            {
                c._scrollViewer.Width = (double)e.NewValue;
            }
        }

        private static void OnHScrollBarHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (DataGridBold)d;
            if (c.IsDisposed)
            {
                return;
            }

            if (c._scrollViewer != null)
            {
                c._scrollViewer.Height = (double)e.NewValue;
            }
        }

        #endregion

        #region Private methods.

        /// <summary>
        /// Unieważnia renderowanie elementu i wymusza kompletne nowe przejście układu (MeasureOverride -> ArrangeOverride -> OnRender).
        /// </summary>
        private void Invalidate()
        {
            this.InvalidateMeasure();
            this.InvalidateVisual();
        }

        private void UpdateSelectedItems(System.Collections.IList selectedItems)
        {
            if (Interlocked.Read(ref _selectedItemsChanging) == 0)
            {
                // Blokujemy dostęp do tej metody.
                Interlocked.Exchange(ref _selectedItemsChanging, 1);

                try
                {
                    if (selectedItems == null)
                    {
                        base.SelectedItems.Clear();
                        return;
                    }

                    // Robimy kopię zaznaczonych elementów na wypadek jakby kolekcja w trakcie przetwarzania miała się zmienić,
                    // zwłaszcza po wykonaniu 'base.SelectedItems.Clear()'.
                    System.Collections.IList saveSelectedItems = selectedItems.Cast<object>().ToList();
                    base.SelectedItems.Clear();

                    foreach (var item in saveSelectedItems)
                    {
                        base.SelectedItems.Add(item);
                    }

                    // Aktualizujemy właściwość 'SelectedItemsProperty'.
                    SetValue(SelectedItemsProperty, base.SelectedItems);
                }
                finally
                {
                    // Odblokowanie tej metody.
                    Interlocked.Exchange(ref _selectedItemsChanging, 0);
                }
            }
        }

        /// <summary>
        /// Zwraca nazwę kolumny na podstawie nagłówka kolumny.
        /// </summary>
        /// <param name="header">Nagłówek kolumny.</param>
        private string GetColumnNameFromHeader(object header)
        {
            if (header is string)
            {
                return header.ToString();
            }
            else if (header is TextBlock)
            {
                return ((TextBlock)header).Text ?? "";
            }
            else if (header is Label)
            {
                if (((Label)header).Content is string)
                {
                    return ((Label)header).Content.ToString();
                }
                else
                {
                    return GetColumnNameFromHeader(((Label)header).Content);
                }
            }

            var hdr = header as FrameworkElement;
            if (hdr != null)
            {
                var textBlock = hdr.Descendants<TextBlock>().FirstOrDefault();
                if (textBlock != null && textBlock.Text != null)
                {
                    return textBlock.Text;
                }
                else
                {
                    var label = hdr.Descendants<Label>().FirstOrDefault();
                    if (label != null && label.Content != null)
                    {
                        if (label.Content is string)
                        {
                            return label.Content.ToString();
                        }
                        else
                        {
                            return GetColumnNameFromHeader(label.Content);
                        }
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// Przewija DataGridBold w pionie, aby wyświetlić wiersz dla określonego elementu danych i do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="sender">Dostarczony obiekt.</param>
        /// <param name="e">Dostarczone dane.</param>
        private async void ScrollIntoViewAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = (BackgroundWorker)((object[])e.Argument)[0];
            object item = ((object[])e.Argument)[1];
            double verticalOffset = (double)((object[])e.Argument)[2];
            double horizontalOffset = (double)((object[])e.Argument)[3];
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
                    Dispatcher.Invoke(() =>
                    {
                        if (!isNullItem)
                        {
                            this.ScrollIntoView(item);
                        }
                        else
                        {
                            if (this.Items.Count > 0)
                            {
                                this.ScrollIntoView(this.Items[0]);
                            }
                        }
                        this.ScrollToVerticalOffset(verticalOffset);
                        this.ScrollToHorizontalOffset(horizontalOffset);
                        currentVerticalOffset = this.VerticalOffset;
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

        private void FillClipboardRowContent(DataGridRowClipboardEventArgs args)
        {
            if (args.IsColumnHeadersRow)
            {
                for (int i = args.StartColumnDisplayIndex; i <= args.EndColumnDisplayIndex; i++)
                {
                    DataGridColumn column = ColumnFromDisplayIndex(i);
                    if (!(column.Visibility == Visibility.Visible))
                    {
                        continue;
                    }

                    args.ClipboardRowContent.Add(new DataGridClipboardCellContent(args.Item, column, column.Header));
                }
            }
            else
            {
                int rowIndex = args.GetRowIndexHint();
                if (rowIndex < 0)
                {
                    rowIndex = Items.IndexOf(args.Item);
                }

                // If row has selection
                if (this.SelectedCellsIntersects(rowIndex))
                {
                    for (int i = args.StartColumnDisplayIndex; i <= args.EndColumnDisplayIndex; i++)
                    {
                        DataGridColumn column = ColumnFromDisplayIndex(i);
                        if (!(column.Visibility == Visibility.Visible))
                        {
                            continue;
                        }

                        object cellValue = null;

                        // Get cell value only if the cell is selected - otherwise leave it null
                        if (this.SelectedCellsContains(rowIndex, i))
                        {
                            cellValue = column.OnCopyingCellClipboardContent(args.Item);
                        }

                        args.ClipboardRowContent.Add(new DataGridClipboardCellContent(args.Item, column, cellValue));
                    }
                }
            }
        }

        /// <summary>
        /// Provides handling for the System.Windows.Input.CommandBinding.Executed event associated with the System.Windows.Input.ApplicationCommands.Copy command.
        /// </summary>
        /// <param name="args">The data for the event.</param>
        /// <exception cref="System.NotSupportedException">System.Windows.Controls.DataGrid.ClipboardCopyMode is set to System.Windows.Controls.DataGridClipboardCopyMode.None.</exception>
        [SecurityCritical, SecurityTreatAsSafe]
        private void OnExecutedCopyInternal(ExecutedRoutedEventArgs args)
        {
            if (ClipboardCopyMode == DataGridClipboardCopyMode.None)
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
            if (this.GetSelectionRange(out minColumnDisplayIndex, out maxColumnDisplayIndex, out minRowIndex, out maxRowIndex))
            {
                // Add column headers if enabled
                if (ClipboardCopyMode == DataGridClipboardCopyMode.IncludeHeader)
                {
                    DataGridRowClipboardEventArgs preparingRowClipboardContentEventArgs = new DataGridRowClipboardEventArgs(null, minColumnDisplayIndex, maxColumnDisplayIndex, true /*IsColumnHeadersRow*/);
                    OnCopyingRowClipboardContent(preparingRowClipboardContentEventArgs);

                    foreach (string format in formats)
                    {
                        dataGridStringBuilders[format].Append(preparingRowClipboardContentEventArgs.FormatClipboardCellValues(format));
                    }
                }

                // Add each selected row
                for (int i = minRowIndex; i <= maxRowIndex; i++)
                {
                    object row = Items[i];

                    // Row has a selecion
                    if (this.SelectedCellsIntersects(i))
                    {
                        DataGridRowClipboardEventArgs preparingRowClipboardContentEventArgs = (DataGridRowClipboardEventArgs)CONSTRUCTORINFO_DATAGRIDROWCLIPBOARDEVENTARGS.Invoke(
                            new object[] { row, minColumnDisplayIndex, maxColumnDisplayIndex, false /*IsColumnHeadersRow*/, i });
                        OnCopyingRowClipboardContent(preparingRowClipboardContentEventArgs);

                        foreach (string format in formats)
                        {
                            dataGridStringBuilders[format].Append(preparingRowClipboardContentEventArgs.FormatClipboardCellValues(format));
                        }
                    }
                }
            }

            DataGridBoldClipboardHelper.GetClipboardContentForHtml(dataGridStringBuilders[DataFormats.Html]);

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

        private void OnActualWidthPropertyChanged(object sender, EventArgs e)
        {
            try
            {
                ColumnActualWidthProperty?.Invoke(this, new DataGridColumnEventArgs((DataGridColumn)sender));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        }

        #endregion

        #region Protected methods.

        /// <summary>
        /// Wywoływane, gdy zmienia się wybór elementu.
        /// </summary>
        /// <param name="e">Dane dotyczące zdarzenia.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (SelectionMode == DataGridSelectionMode.Extended)
            {
                UpdateSelectedItems(base.SelectedItems);
            }
        }

        /// <summary>
        /// Wywoływane, gdy wystąpi zdarzenie MouseLeftButtonDown.
        /// </summary>
        /// <param name="e">System.Windows.Input.MouseButtonEventArgs, który zawiera dane zdarzenia. Dane zdarzenia informują o naciśnięciu lewego przycisku myszy.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (SelectionUnit != DataGridSelectionUnit.Cell)
            {
                Border border = e.OriginalSource as Border;
                if (border != null)
                {
                    //var row = ItemContainerGenerator.ContainerFromItem(CurrentItem) as DataGridRow;
                    DataGridRow dataGridRow = border.TemplatedParent as DataGridRow;
                    if (dataGridRow != null)
                    {
                        if (Keyboard.IsKeyDown(Key.LeftCtrl))
                        {
                            base.SelectedItems.Add(dataGridRow.Item);
                        }
                        else
                        {
                            DataGridSelectionUnit saveSelectionUnit = SelectionUnit;
                            SelectionUnit = DataGridSelectionUnit.Cell;
                            UnselectAll();
                            SelectionUnit = saveSelectionUnit;

                            dataGridRow.IsSelected = true;
                            dataGridRow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Wywołuje zdarzenie System.Windows.Controls.DataGrid.Sorting.
        /// </summary>
        /// <param name="eventArgs">Dane zdarzenia.</param>
        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            base.OnSorting(eventArgs);
            SortDirection = eventArgs.Column.SortDirection;
        }

        #endregion

        #region Public methods.

        /// <summary>
        /// Gdy zostanie zastąpione w klasie pochodnej, jest wywoływane za każdym razem, gdy kod aplikacji lub procesy wewnętrzne wywołują System.Windows.FrameworkElement.ApplyTemplate.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Template == null)
            {
                return;
            }

            _scrollViewer = this.Descendants<ScrollViewer>().FirstOrDefault(x => x.Name == "DG_ScrollViewer");
        }

        /// <summary>
        /// Zaznacza komórkę wskazaną indeksem wiersze i kolumny.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        public bool SelectCell(int rowIndex, int columnIndex)
        {
            if (rowIndex < 0 || columnIndex < 0 || rowIndex >= this.Items.Count || columnIndex >= this.Columns.Count)
            {
                return false;
            }

            this.CurrentCell = new DataGridCellInfo(this.Items[rowIndex], this.Columns[columnIndex]);
            if (!this.SelectedCells.Contains(this.CurrentCell))
            {
                this.SelectedCells.Add(this.CurrentCell);
            }
            return true;
        }

        /// <summary>
        /// Przewija zawartość w DataGridBold do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        public void ScrollToOffsets(double verticalOffset, double horizontalOffset)
        {
            this.ScrollToVerticalOffset(verticalOffset);
            this.ScrollToHorizontalOffset(horizontalOffset);
        }

        /// <summary>
        /// Przewija DataGridBold w pionie, aby wyświetlić wiersz dla określonego elementu danych i do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="item">Element danych do pokazania.</param>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        public async Task ScrollIntoViewAsync(object item, double verticalOffset, double horizontalOffset)
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
            backgroundWorker.RunWorkerAsync(new object[] { backgroundWorker, item, verticalOffset, horizontalOffset });

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
        /// Przewija DataGridBold w pionie, aby wyświetlić wiersz dla określonego elementu danych i do określonej pozycji przesunięcia w pionie i poziomie.
        /// </summary>
        /// <param name="item">Element danych do pokazania.</param>
        /// <param name="verticalOffset">Pozycja, do której przewija się treść w pionie.</param>
        /// <param name="horizontalOffset">Pozycja, do której przewija się treść w poziomie.</param>
        public void ScrollIntoView(object item, double verticalOffset, double horizontalOffset)
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
                this.ScrollIntoView(item);
            }
            else
            {
                verticalOffset = 0;
                if (this.Items.Count > 0)
                {
                    this.ScrollIntoView(this.Items[0]);
                }
            }
            this.ScrollToVerticalOffset(verticalOffset);
            this.ScrollToHorizontalOffset(horizontalOffset);
        }

        /// <summary>
        /// Kopiuje pojedynczą komórkę do schowka.
        /// </summary>
        public void CopyCellContentToClipboard()
        {
            DataGridCellInfo cellInfo;
            DataGridColumn currentColumn = null;
            object currentItem = null;
            string format = DataFormats.UnicodeText;

            if (this.CurrentCell != null)
            {
                cellInfo = this.CurrentCell;
            }
            else if (this.SelectedCells.Count > 0)
            {
                cellInfo = this.SelectedCells[0];
            }

            currentColumn = cellInfo.Column;

            if (currentColumn == null)
            {
                if (this.CurrentCell != null && this.CurrentCell.Column != null)
                {
                    currentColumn = this.CurrentCell.Column;
                }
                else if (this.SelectedCells.Count > 0)
                {
                    currentColumn = this.SelectedCells[0].Column;
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
            else if (this.CurrentItem != null)
            {
                currentItem = this.CurrentItem;
            }
            else if (this.SelectedCells.Count > 0 && this.SelectedCells[0].Item != null)
            {
                currentItem = this.SelectedCells[0].Item;
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
            DataGridBoldClipboardHelper.FormatPlainText(value, sb, format);

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
        public void CopySelectedContentToClipboard()
        {
            try
            {
                ExecutedRoutedEventArgs args = (ExecutedRoutedEventArgs)CONSTRUCTORINFO_EXECUTEDROUTEDEVENTARGS.Invoke(new object[] { ApplicationCommands.Copy, null });
                args.RoutedEvent = DataObject.CopyingEvent;
                args.Source = this;

                OnExecutedCopyInternal(args);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        }

        #endregion

        #region Public properties.

        /// <summary>
        /// Pobiera wartość wskazującą czy kontrolka jest obecnie w trybie projektowy.
        /// </summary>
        /// <returns>true jeśli kontrolka jest w trybie projektowym, w przeciwnym razie false.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DesignMode
        {
            get
            {
                return ApplicationEx.DesignMode();
            }
        }

        /// <summary>
        /// Pobiera lub ustawia wybrane elementy w System.Windows.Controls.Primitives.MultiSelector.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        new public System.Collections.IList SelectedItems
        {
            get { return (System.Collections.IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        /// <summary>
        /// Pobiera nazwę kolumny.
        /// </summary>
        public string CurrentColumnName
        {
            get
            {
                if (this.CurrentCell != null && this.CurrentCell.Column != null && this.CurrentCell.Column.Header != null)
                {
                    if (this.CurrentCell.Column.Header is string)
                    {
                        return this.CurrentCell.Column.Header.ToString();
                    }
                    else
                    {
                        return GetColumnNameFromHeader(this.CurrentCell.Column.Header);
                    }
                }
                else if (this.SelectedCells.Count > 0)
                {
                    if (this.SelectedCells[0].Column.Header is string)
                    {
                        return this.SelectedCells[0].Column.Header.ToString();
                    }
                    else
                    {
                        return GetColumnNameFromHeader(this.SelectedCells[0].Column.Header);
                    }
                }
                return "";
            }
        }

        /// <summary>
        /// Pobiera indeks kolumny.
        /// </summary>
        public int CurrentColumnIndex
        {
            get
            {
                if (this.CurrentCell != null && this.CurrentCell.Column != null)
                {
                    return this.ColumnIndexFromDisplayIndex(this.CurrentCell.Column.DisplayIndex);
                }
                else if (this.SelectedCells.Count > 0)
                {
                    return this.ColumnIndexFromDisplayIndex(this.SelectedCells[0].Column.DisplayIndex);
                }
                return -1;
            }
        }

        /// <summary>
        /// Pobiera wyświetlaną pozycję kolumny względem innych kolumn w System.Windows.Controls.DataGrid.
        /// </summary>
        public int CurrentColumnDisplayIndex
        {
            get
            {
                if (this.CurrentCell != null && this.CurrentCell.Column != null)
                {
                    return this.CurrentCell.Column.DisplayIndex;
                }
                else if (this.SelectedCells.Count > 0)
                {
                    return this.SelectedCells[0].Column.DisplayIndex;
                }
                return -1;
            }
        }

        /// <summary>
        /// Pobiera indeks wiersza.
        /// </summary>
        public int CurrentRowIndex
        {
            get
            {
                if (this.CurrentItem != null)
                {
                    return this.Items.IndexOf(this.CurrentItem);
                }
                else if (this.SelectedCells.Count > 0)
                {
                    return this.Items.IndexOf(this.SelectedCells[0].Item);
                }
                return this.SelectedIndex;
            }
        }

        #endregion

        #region Interfaces Members.

        #region IControlID Members.

        /// <summary>
        /// Pobiera wartość, która określa identyfikator kontrolki.
        /// </summary>
        public Guid ID { get; private set; }

        #endregion

        #region IScrollViewerFresh Members.

        /// <summary>
        /// Scrolls the ScrollViewerFresh content downward by one line.
        /// </summary>
        public void LineDown()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.LineDown();
            }
        }

        /// <summary>
        /// Scrolls the ScrollViewerFresh content to the left by a predetermined amount.
        /// </summary>
        public void LineLeft()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.LineLeft();
            }
        }

        /// <summary>
        /// Scrolls the ScrollViewerFresh content to the right by a predetermined amount.
        /// </summary>
        public void LineRight()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.LineRight();
            }
        }

        /// <summary>
        /// Scrolls the ScrollViewerFresh content upward by one line.
        /// </summary>
        public void LineUp()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.LineUp();
            }
        }

        /// <summary>
        /// Scrolls the ScrollViewerFresh content downward by one page.
        /// </summary>
        public void PageDown()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.PageDown();
            }
        }

        /// <summary>
        /// Scrolls the ScrollViewerFresh content to the left by one page.
        /// </summary>
        public void PageLeft()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.PageLeft();
            }
        }

        /// <summary>
        /// Scrolls the ScrollViewerFresh content to the right by one page.
        /// </summary>
        public void PageRight()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.PageRight();
            }
        }

        /// <summary>
        /// Scrolls the ScrollViewerFresh content upward by one page.
        /// </summary>
        public void PageUp()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.PageUp();
            }
        }

        /// <summary>
        /// Scrolls vertically to the end of the ScrollViewerFresh content.
        /// </summary>
        public void ScrollToBottom()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollToBottom();
            }
        }

        /// <summary>
        /// Scrolls vertically to the end of the ScrollViewerFresh content.
        /// </summary>
        public void ScrollToEnd()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollToEnd();
            }
        }

        /// <summary>
        /// Scrolls vertically to the beginning of the ScrollViewerFresh content.
        /// </summary>
        public void ScrollToHome()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollToHome();
            }
        }

        /// <summary>
        /// Scrolls the content within the ScrollViewerFresh to the specified horizontal offset position.
        /// </summary>
        /// <param name="offset">The position that the content scrolls to.</param>
        public void ScrollToHorizontalOffset(double offset)
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollToHorizontalOffset(offset);
            }
        }

        /// <summary>
        /// Scrolls horizontally to the beginning of the ScrollViewerFresh content.
        /// </summary>
        public void ScrollToLeftEnd()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollToLeftEnd();
            }
        }

        /// <summary>
        /// Scrolls horizontally to the end of the ScrollViewerFresh content.
        /// </summary>
        public void ScrollToRightEnd()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollToRightEnd();
            }
        }

        /// <summary>
        /// Scrolls vertically to the beginning of the ScrollViewerFresh content.
        /// </summary>
        public void ScrollToTop()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollToTop();
            }
        }

        /// <summary>
        /// Scrolls the content within the ScrollViewerFresh to the specified vertical offset position.
        /// </summary>
        /// <param name="offset">The position that the content scrolls to.</param>
        public void ScrollToVerticalOffset(double offset)
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollToVerticalOffset(offset);
            }
        }

        /// <summary>
        /// Scrolls the content in ScrollViewerFresh according to the indicated action.
        /// </summary>
        /// <param name="scrollAction">The indicated action.</param>
        public void ScrollTo(ScrollAction scrollAction)
        {
            switch (scrollAction)
            {
                case ScrollAction.LineDown:
                    {
                        LineDown();
                    }
                    break;
                case ScrollAction.LineLeft:
                    {
                        LineLeft();
                    }
                    break;
                case ScrollAction.LineRight:
                    {
                        LineRight();
                    }
                    break;
                case ScrollAction.LineUp:
                    {
                        LineUp();
                    }
                    break;
                case ScrollAction.PageDown:
                    {
                        PageDown();
                    }
                    break;
                case ScrollAction.PageLeft:
                    {
                        PageLeft();
                    }
                    break;
                case ScrollAction.PageRight:
                    {
                        PageRight();
                    }
                    break;
                case ScrollAction.PageUp:
                    {
                        PageUp();
                    }
                    break;
                case ScrollAction.ToBottom:
                    {
                        ScrollToBottom();
                    }
                    break;
                case ScrollAction.ToEnd:
                    {
                        ScrollToEnd();
                    }
                    break;
                case ScrollAction.ToHome:
                    {
                        ScrollToHome();
                    }
                    break;
                case ScrollAction.ToLeftEnd:
                    {
                        ScrollToLeftEnd();
                    }
                    break;
                case ScrollAction.ToRightEnd:
                    {
                        ScrollToRightEnd();
                    }
                    break;
                case ScrollAction.ToTop:
                    {
                        ScrollToTop();
                    }
                    break;
                    //case ScrollAction.ToHorizontalOffset:
                    //    {
                    //        ScrollToHorizontalOffset(double offset);
                    //    }
                    //    break;
                    //case ScrollAction.ToVerticalOffset:
                    //    {
                    //        ScrollToVerticalOffset(double offset);
                    //    }
                    //    break;
            }
        }

        #endregion

        #region IDataGridFreshScrollViewer Members.

        /// <summary>
        /// Gets or sets a value that indicates whether elements that support the System.Windows.Controls.Primitives.IScrollInfo interface are allowed to scroll.
        /// </summary>
        public bool CanContentScroll
        {
            get { return _scrollViewer.CanContentScroll; }
            set { _scrollViewer.CanContentScroll = value; }
        }

        /// <summary>
        /// Gets a value that indicates whether the horizontal ScrollBarFresh is visible.
        /// </summary>
        public Visibility ComputedHorizontalScrollBarVisibility
        {
            get { return _scrollViewer != null ? _scrollViewer.ComputedHorizontalScrollBarVisibility : Visibility.Collapsed; }
        }

        /// <summary>
        /// Gets a value that indicates whether the vertical ScrollBarFresh is visible.
        /// </summary>
        public Visibility ComputedVerticalScrollBarVisibility
        {
            get { return _scrollViewer != null ? _scrollViewer.ComputedVerticalScrollBarVisibility : Visibility.Visible; }
        }

        /// <summary>
        /// Gets a value that contains the horizontal offset of the scrolled content.
        /// </summary>
        public double HorizontalOffset
        {
            get { return _scrollViewer != null ? _scrollViewer.HorizontalOffset : 0; }
        }

        /// <summary>
        /// Gets a value that contains the vertical offset of the scrolled content.
        /// </summary>
        public double VerticalOffset
        {
            get { return _scrollViewer != null ? _scrollViewer.VerticalOffset : 0; }
        }

        /// <summary>
        /// Gets the vertical offset of the visible content.
        /// </summary>
        public double ContentVerticalOffset
        {
            get { return _scrollViewer != null ? _scrollViewer.ContentVerticalOffset : 0; }
        }

        /// <summary>
        /// Gets the horizontal offset of the visible content.
        /// </summary>
        public double ContentHorizontalOffset
        {
            get { return _scrollViewer != null ? _scrollViewer.ContentHorizontalOffset : 0; }
        }

        #endregion

        #region IDataGridFresh Members.

        /// <summary>
        /// Pobiera kierunek sortowania (rosnąco lub malejąco) kolumny. Zarejestrowana wartość domyślna to null.
        /// </summary>
        [Description("Gets the sort direction (ascending or descending) of the column. The registered default is null.")]
        public ListSortDirection? SortDirection
        { get; private set; }

        #endregion

        #endregion

        #region IDisposable members.

        [NonSerialized()]
        private bool _isDisposed = false;

        /// <summary>
        /// Destruktor na podstawie którego zostanie wygenerowana metoda Finalize().
        /// </summary>
        ~DataGridBold()
        {
            Dispose(false);
        }

        /// <summary>
        /// Metoda sprzątająca. Zwalnia zasoby zarządzane.
        /// </summary>
        private void Cleaner()
        {
            try
            {
                //BindingOperations.ClearAllBindings(this);
                if (_window != null)
                {
                    _window.Closed -= _window_Closed;
                }
                _window = null;

                Loaded -= DataGrid_Loaded;
                Columns.CollectionChanged -= Columns_CollectionChanged;

                if (_scrollViewer != null)
                {
                    _scrollViewer = null;
                }

                foreach (DataGridColumn column in Columns)
                {
                    _propertyDescriptorActualWidth.RemoveValueChanged(column, OnActualWidthPropertyChanged);
                }

                //if (this.Items != null)
                //{
                //    BindingExpressionBase beb = BindingOperations.GetBindingExpressionBase(this, ItemsSourceProperty);
                //    if (beb == null)
                //    {
                //        // ItemsSource nie jest powiązany z danymi.
                //        UserControlBase.ItemsDispose(this.Items);

                //        bool isUsingItemsSource = PropertyInfoEx.GetValue<bool>(this.Items, "IsUsingItemsSource");
                //        if (!isUsingItemsSource)
                //        {
                //            // 'this.ItemsSource' trzeba wyczyścić przed 'this.Items.Clear()', ponieważ zgłoszony zostanie wyjątek.
                //            this.ItemsSource = null;
                //            this.Items.Clear();
                //        }
                //    }
                //    this.ItemsSource = null;
                //    this.Items.DetachFromSourceCollection();
                //}
                //this.ItemsSource = null;

                this.ColumnHeaderStyle = null;
                this.RowHeaderStyle = null;
                this.RowStyle = null;
                this.CellStyle = null;

                this.Template = null;
                this.Style = null;
                if (!string.IsNullOrEmpty(this.Name) && this.FindName(this.Name) != null)
                {
                    this.UnregisterName(this.Name);
                }
                //this.Remove(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        }

        /// <summary>
        /// Utylizacja wykonuje się w dwóch różnych scenariuszach. Jeśli parametr 'disposing'
        /// równa się (true) to metoda została wywołana bezpośrednio lub pośrednio przez kod
        /// użytkownika. W tym przypadku zarządzane i niezarządzane zasoby zostaną zniszczone.
        /// Jeśli parametr 'disposing' równa się (false) metoda została wywołana przez
        /// środowisko wykonawcze (runtime) z wnętrza finalizatora i nie należy odwoływać się
        /// do innych obiektów tylko zniszczyć zasoby niezarządzane.
        /// </summary>
        /// <param name="disposing">Wywołano bezpośrednio (true) lub pośrednio (false).</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (IsDisposed == false)
                {
                    Cleaner();
                    IsDisposed = true;
                }
            }
        }

        /// <summary>
        /// Zwolnienie zajmowanych zasobów przez obiekt i efektywne jego zniszczenie.
        /// </summary>
        public void Dispose()
        {
            // Wywołanie utylizacji z poziomu kodu użytkownika.
            Dispose(true);

            // Ten obiekt zostanie zniszczony przez metodę 'Dispose'.
            // W związku z tym, należy powiadomić (Garbage Collector)
            // GC.SupressFinalize aby wyłączył ten obiekt z kolejki
            // finalizacji. Skutek będzie taki, że kod finalizacji
            // nie zostanie wykonany po raz drugi.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Pobiera wartość wskazującą, czy kontrolka została usunięta.
        /// </summary>
        /// <returns>true, jeśli kontrola została usunięta, w przeciwnym razie false.</returns>
        public bool IsDisposed
        {
            get
            {
                return _isDisposed;
            }
            private set
            {
                _isDisposed = value;
            }
        }

        #endregion

        #region Events.

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Parent != null)
                {
                    if (_window == null)
                    {
                        _window = Window.GetWindow(this);
                        if (_window != null && !this.DesignMode)
                        {
                            _window.Closed += _window_Closed;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        }

        private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var column in e.NewItems)
                    {
                        _propertyDescriptorActualWidth.AddValueChanged(column, OnActualWidthPropertyChanged);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var column in e.OldItems)
                    {
                        _propertyDescriptorActualWidth.RemoveValueChanged(column, OnActualWidthPropertyChanged);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        }

        private void _window_Closed(object sender, EventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        }

        #endregion
    }
}
