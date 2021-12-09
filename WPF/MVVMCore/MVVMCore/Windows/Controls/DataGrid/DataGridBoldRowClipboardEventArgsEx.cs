using MVVMCore.Reflection;
using System.Reflection;
using System.Windows.Controls;

namespace MVVMCore.Windows.Controls
{
    /// <summary>
    /// Rozszerzenie klasy DataGridBoldRowClipboardEventArgs.
    /// </summary>
    internal static class DataGridBoldRowClipboardEventArgsEx
    {
        private static PropertyInfo PROPERTYINFO_ROWINDEXHINT = typeof(DataGridRowClipboardEventArgs).GetProperty("RowIndexHint", PropertyInfoEx.DefaultFlags);

        public static int GetRowIndexHint(this DataGridRowClipboardEventArgs args)
        {
            return (int)PROPERTYINFO_ROWINDEXHINT.GetValue(args);
        }
    }
}
