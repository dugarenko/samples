using MVVMCore.Reflection;
using System.Reflection;
using System.Windows.Input;

namespace MVVMCore.Windows.Input
{
    /// <summary>
    /// Rozszerzenie klasy ExecutedRoutedEventArgs.
    /// </summary>
    internal static class ExecutedRoutedEventArgsEx
    {
        private static PropertyInfo PROPERTYINFO_USER_INITIATED = typeof(ExecutedRoutedEventArgs).GetProperty("UserInitiated", PropertyInfoEx.DefaultFlags);

        /// <summary>
        /// Sprawdzanie bitu zainicjowanego przez użytkownika jako bezpiecznego.
        /// </summary>
        public static bool GetUserInitiated(this ExecutedRoutedEventArgs args)
        {
            return (bool)PROPERTYINFO_USER_INITIATED.GetValue(args);
        }
    }
}
