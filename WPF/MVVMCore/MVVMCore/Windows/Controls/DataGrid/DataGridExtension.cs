using System;
using System.Windows;

namespace MVVMCore.Windows.Controls
{
    /// <summary></summary>
    public static class DataGridExtension
    {
        private static Type _thisType = typeof(DataGridExtension);

        /// <summary>
        /// DependencyProperty for MethodInvoker property.
        /// </summary>
        public static readonly DependencyProperty MethodInvokerProperty = DependencyProperty.RegisterAttached("MethodInvoker",
            typeof(IDataGridExtensionInvoker), _thisType, new FrameworkPropertyMetadata(null, OnMethodInvokerChanged));

        private static void OnMethodInvokerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGridFresh = (System.Windows.Controls.DataGrid)d;
            var newValue = (IDataGridExtensionInvoker)e.NewValue;
            var oldValue = (IDataGridExtensionInvoker)e.OldValue;

            if (newValue == null && oldValue != null)
            {
                oldValue.UnselectAllCellsHandler -= (sender, args) =>
                {
                    dataGridFresh.UnselectAllCells();
                };
                oldValue.GetVerticalOffsetHandler -= () =>
                {
                    return dataGridFresh.VerticalOffset();
                };
                oldValue.GetHorizontalOffsetHandler -= () =>
                {
                    return dataGridFresh.HorizontalOffset();
                };
                oldValue.ScrollIntoViewHandler -= (item) =>
                {
                    dataGridFresh.ScrollIntoView(item);
                };
                oldValue.ScrollIntoViewAndOffsetsHandler -= (item, verticalOffset, horizontalOffset) =>
                {
                    dataGridFresh.ScrollIntoView(item, verticalOffset, horizontalOffset);
                };
                oldValue.ScrollIntoViewAndOffsetsAsyncHandler -= (item, verticalOffset, horizontalOffset) =>
                {
                    return dataGridFresh.ScrollIntoViewAsync(item, verticalOffset, horizontalOffset);
                };
                oldValue.ScrollToOffsetsHandler -= (verticalOffset, horizontalOffset) =>
                {
                    dataGridFresh.ScrollToOffsets(verticalOffset, horizontalOffset);
                };
                oldValue.CopyCellContentToClipboardHandler -= (sender, args) =>
                {
                    dataGridFresh.CopyCellContentToClipboard();
                };
                oldValue.CopySelectedContentToClipboardHandler -= (sender, args) =>
                {
                    dataGridFresh.CopySelectedContentToClipboard();
                };
                oldValue.FocusHandler -= () =>
                {
                    return dataGridFresh.Focus();
                };
                oldValue.GetSortDirectionHandler -= () =>
                {
                    return dataGridFresh.SortDirection;
                };
                return;
            }

            newValue.UnselectAllCellsHandler += (sender, args) =>
            {
                dataGridFresh.UnselectAllCells();
            };
            newValue.GetVerticalOffsetHandler += () =>
            {
                return dataGridFresh.VerticalOffset();
            };
            newValue.GetHorizontalOffsetHandler += () =>
            {
                return dataGridFresh.HorizontalOffset();
            };
            newValue.ScrollIntoViewHandler += (item) =>
            {
                dataGridFresh.ScrollIntoView(item);
            };
            newValue.ScrollIntoViewAndOffsetsHandler += (item, verticalOffset, horizontalOffset) =>
            {
                dataGridFresh.ScrollIntoView(item, verticalOffset, horizontalOffset);
            };
            newValue.ScrollIntoViewAndOffsetsAsyncHandler += (item, verticalOffset, horizontalOffset) =>
            {
                return dataGridFresh.ScrollIntoViewAsync(item, verticalOffset, horizontalOffset);
            };
            newValue.ScrollToOffsetsHandler += (verticalOffset, horizontalOffset) =>
            {
                dataGridFresh.ScrollToOffsets(verticalOffset, horizontalOffset);
            };
            newValue.CopyCellContentToClipboardHandler += (sender, args) =>
            {
                dataGridFresh.CopyCellContentToClipboard();
            };
            newValue.CopySelectedContentToClipboardHandler += (sender, args) =>
            {
                dataGridFresh.CopySelectedContentToClipboard();
            };
            newValue.FocusHandler += () =>
            {
                return dataGridFresh.Focus();
            };
            newValue.GetSortDirectionHandler += () =>
            {
                return dataGridFresh.SortDirection;
            };
        }

        public static IDataGridExtensionInvoker GetMethodInvoker(DependencyObject d)
        {
            return (IDataGridExtensionInvoker)d.GetValue(MethodInvokerProperty);
        }

        public static void SetMethodInvoker(DependencyObject d, IDataGridExtensionInvoker value)
        {
            d.SetValue(MethodInvokerProperty, value);
        }
    }
}
