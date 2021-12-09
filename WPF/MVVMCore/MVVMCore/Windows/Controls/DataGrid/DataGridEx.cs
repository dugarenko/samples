using MVVMCore.Reflection;
using System;
using System.Reflection;
using System.Windows.Controls;

namespace MVVMCore.Windows.Controls
{
    /// <summary>
    /// Rozszerzenie kontrolki DataGrid.
    /// </summary>
    public static class DataGridEx
    {
        private static Assembly ASSEMBLY = Assembly.GetAssembly(typeof(TextBlock));
        private static Type TYPE_SELECTED_CELLS_COLLECTION = ASSEMBLY.GetType("System.Windows.Controls.SelectedCellsCollection");
        private static MethodInfo METHODINFO_GET_SELECTION_RANGE = TYPE_SELECTED_CELLS_COLLECTION.GetMethod("GetSelectionRange", MethodInfoEx.DefaultFlags,
            Type.DefaultBinder, new Type[] { typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType() }, null);
        private static MethodInfo METHODINFO_INTERSECTS = TYPE_SELECTED_CELLS_COLLECTION.GetMethod("Intersects", MethodInfoEx.DefaultFlags, Type.DefaultBinder, new Type[] { typeof(int) }, null);
        private static MethodInfo METHODINFO_CONTAINS = TYPE_SELECTED_CELLS_COLLECTION.GetMethod("Contains", MethodInfoEx.DefaultFlags, Type.DefaultBinder, new Type[] { typeof(int), typeof(int) }, null);
        private static PropertyInfo PROPERTYINFO_SELECTED_CELLS_INTERNAL = typeof(DataGrid).GetProperty("SelectedCellsInternal", PropertyInfoEx.DefaultFlags);

        /// <summary>
        /// Zwraca indeks kolumny.
        /// </summary>
        /// <param name="dataGrid">DataGrid.</param>
        /// <param name="displayIndex">Pozycja wyświetlania kolumny względem innych kolumn w System.Windows.Controls.DataGrid.</param>
        public static int ColumnIndexFromDisplayIndex(this DataGrid dataGrid, int displayIndex)
        {
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                if (dataGrid.Columns[i].DisplayIndex == displayIndex)
                {
                    return i;
                }
            }
            return -1;
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
    }
}
