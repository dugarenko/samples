using System;
using System.Windows;
using System.Windows.Media;

namespace MVVMCore.Windows
{
    /// <summary>
    /// Rozszerzenie struktury DependencyObject.
    /// </summary>
    public static class DependencyObjectEx
    {
        /// <summary>
        /// Zwraca rodzica elementu, który dziedziczy wskazany interfejs.
        /// </summary>
        /// <typeparam name="T">Szukany typ interfejsu rodzica.</typeparam>
        /// <param name="element">Element dla którego szukany będzie rodzic, który dziedziczy wskazany interfejs.</param>
        /// <param name="parent">Znaleziony rodzic.</param>
        /// <returns>true jeśli rodzic został znaleziony, w przeciwnym razie false.</returns>
        public static bool TryGetParentWithInterface<T>(this DependencyObject element, out DependencyObject parent) where T : class
        {
            parent = null;
            string name = typeof(T).FullName;
            Type typeInterface = null;
            DependencyObject test = null;

            try
            {
                parent = VisualTreeHelper.GetParent(element);
                if (parent != null)
                {
                    while (true)
                    {
                        typeInterface = parent.GetType().GetInterface(name);
                        if (typeInterface != null)
                        {
                            return true;
                        }
                        else
                        {
                            test = VisualTreeHelper.GetParent(parent);
                            if (test == null)
                                break;
                            parent = test;
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Zwraca rodzica elementu o wskazanym typie.
        /// </summary>
        /// <typeparam name="T">Typ rodzica.</typeparam>
        /// <param name="element">Element dla którego szukany będzie rodzic.</param>
        /// <param name="parent">Znaleziony rodzic.</param>
        /// <returns>true jeśli rodzic został znaleziony, w przeciwnym razie false.</returns>
        public static bool TryGetParentWithType<T>(this DependencyObject element, out DependencyObject parent) where T : class
        {
            parent = null;
            Type type = typeof(T);
            DependencyObject test = null;

            try
            {
                parent = VisualTreeHelper.GetParent(element);
                if (parent != null)
                {
                    while (true)
                    {
                        if (parent.GetType() == type)
                        {
                            return true;
                        }
                        else
                        {
                            test = VisualTreeHelper.GetParent(parent);
                            if (test == null)
                                break;
                            parent = test;
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Zwraca rodzica elementu o wskazanym typie bazowym.
        /// </summary>
        /// <typeparam name="T">Typ bazowy rodzica.</typeparam>
        /// <param name="element">Element dla którego szukany będzie rodzic.</param>
        /// <param name="parent">Znaleziony rodzic.</param>
        /// <returns>true jeśli rodzic został znaleziony, w przeciwnym razie false.</returns>
        public static bool TryGetParentWithBaseType<T>(this DependencyObject element, out DependencyObject parent) where T : class
        {
            parent = null;
            Type type = typeof(T);
            DependencyObject test = null;

            try
            {
                parent = VisualTreeHelper.GetParent(element);
                if (parent != null)
                {
                    while (true)
                    {
                        if (parent.GetType().BaseType == type)
                        {
                            return true;
                        }
                        else
                        {
                            test = VisualTreeHelper.GetParent(parent);
                            if (test == null)
                                break;
                            parent = test;
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        public static DependencyObject GetRoot(this DependencyObject element)
        {
            DependencyObject scan = element;
            for (; ; )
            {
                DependencyObject test = VisualTreeHelper.GetParent(scan);
                if (test == null)
                    break;
                scan = test;
            }
            return scan;
        }

        public static Window GetWindow(this DependencyObject element)
        {
            // Patrz jeszcze w WindowEx.
            //System.Windows.Interop.HwndSource hwndSource = System.Windows.Interop.HwndSource.FromDependencyObject(element) as System.Windows.Interop.HwndSource;
            //IntPtr handle = ((System.Windows.Interop.HwndSource)PresentationSource.FromDependencyObject(element)).Handle;

            return Window.GetWindow(element);
        }
    }
}
