using MVVMCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace MVVMCore.Windows
{
    public static class FrameworkElementEx
    {
        /// <summary>
        /// Przechodzi przez drzewo wskazanego elementu i zwraca listę znalezionych potomków wskazanego typu.
        /// </summary>
        /// <param name="element">Element do przeszukania.</param>
        /// <param name="elements">Lista znalezionych potomków wskazanego typu.</param>
        /// <param name="level">Określa poziom przeszukiwania drzewa. Minus jeden (-1) oznacza pełne przeszukanie drzewa elementu.</param>
        private static void Descendants<T>(this FrameworkElement element, List<T> elements, int level) where T : FrameworkElement
        {
            if (element == null)
            {
                return;
            }

            element.ApplyTemplate();

            T c = default(T);
            FrameworkElement child = null;
            int count = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < count; i++)
            {
                child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;

                if (child == null)
                {
                    continue;
                }

                c = child as T;
                if (c != null)
                {
                    elements.Add(c);
                }

                if (level < 0)
                {
                    child.Descendants(elements, level);
                }
                else if (level > 0)
                {
                    child.Descendants(elements, level - 1);
                }
            }
        }

        /// <summary>
        /// Ratio of screen to layout DPI in x dimension.
        /// </summary>
        internal static double DpiScaleX
        {
            get
            {
                if (SystemParametersEx.DpiX != 96)
                {
                    return (double)SystemParametersEx.DpiX / 96.0;
                }
                return 1.0;
            }
        }

        /// <summary>
        /// Ratio of screen to layout DPI in y dimension.
        /// </summary>
        internal static double DpiScaleY
        {
            get
            {
                if (SystemParametersEx.Dpi != 96)
                {
                    return (double)SystemParametersEx.Dpi / 96.0;
                }
                return 1.0;
            }
        }

        public static void DoWhenLoaded<T>(this T element, Action<T> action) where T : FrameworkElement
        {
            if (element.IsLoaded)
            {
                action(element);
            }
            else
            {
                RoutedEventHandler handler = null;
                handler = (sender, e) =>
                {
                    element.Loaded -= handler;
                    action(element);
                };
                element.Loaded += handler;
            }
        }

        /// <summary>
        /// Przechodzi przez drzewo wskazanego elementu i zwraca listę znalezionych potomków wskazanego typu.
        /// </summary>
        /// <param name="element">Element do przeszukania.</param>
        /// <returns>Lista znalezionych potomków wskazanego typu.</returns>
        public static List<T> Descendants<T>(this FrameworkElement element) where T : FrameworkElement
        {
            return element.Descendants<T>(false);
        }

        /// <summary>
        /// Przechodzi przez drzewo wskazanego elementu i zwraca listę znalezionych potomków wskazanego typu.
        /// </summary>
        /// <param name="element">Element do przeszukania.</param>
        /// <param name="onlyFirstLevel">true oznacza, że szukanie odbędzie się tylko na pierwszym poziomie, false oznacza pełne przeszukanie drzewa elementu.</param>
        /// <returns>Lista znalezionych potomków wskazanego typu.</returns>
        public static List<T> Descendants<T>(this FrameworkElement element, bool onlyFirstLevel) where T : FrameworkElement
        {
            List<T> list = new List<T>();
            element.Descendants(list, onlyFirstLevel ? 0 : -1);
            return list;
        }

        /// <summary>
        /// Przechodzi przez drzewo wskazanego elementu i zwraca listę znalezionych potomków wskazanego typu.
        /// </summary>
        /// <param name="element">Element do przeszukania.</param>
        /// <param name="level">Określa poziom przeszukiwania drzewa. Minus jeden (-1) oznacza pełne przeszukanie drzewa elementu.</param>
        /// <returns>Lista znalezionych potomków wskazanego typu.</returns>
        public static List<T> Descendants<T>(this FrameworkElement element, int level) where T : FrameworkElement
        {
            List<T> list = new List<T>();
            element.Descendants(list, level);
            return list;
        }

        /// <summary>
        /// Usuwa element.
        /// </summary>
        /// <param name="control">Element do usunięcia.</param>
        /// <param name="refreshParent">Określa czy odświeżyć rodzica kontrolki.</param>
        /// <returns>True, jeśli element został usunięty, w przeciwnym razie false.</returns>
        public static bool Remove(this FrameworkElement control, bool refreshParent)
        {
            bool removed = false;

            if (control.Parent == null)
            {
                return removed;
            }

            Type parentType = control.Parent.GetType();
            dynamic parent = control.Parent;

            if (parentType.GetProperty("Children") != null)
            {
                parent.Children.Remove(control);
                removed = true;
            }
            else if (parentType.GetProperty("Child") != null)
            {
                parent.Child = null;
                removed = true;
            }
            else if (parentType.GetProperty("Content") != null)
            {
                parent.Content = null;
                removed = true;
            }
            else if (parentType.GetProperty("Items") != null)
            {
                if (parent.Items.Contains(control))
                {
                    parent.Items.Remove(control);
                    removed = true;
                }
            }
            else if (parentType.FullName == "System.Windows.Controls.ToolBarTray")
            {
                var c = parent as FrameworkElement;
                if (c != null)
                {
                    return c.Remove(refreshParent);
                }
            }

            if (removed)
            {
                if (refreshParent)
                {
                    if (removed && parentType.GetMethod("InvalidateMeasure") != null && parent != null)
                    {
                        // Odświeżamy rodzica.
                        parent.InvalidateMeasure();
                    }
                }
            }
            else
            {
                if (DebugEx.IsDebug)
                {
                    MessageBox.Show(ExceptionFormater.AppendMethod(new Exception("Nie znaleziono elementu."), MethodBase.GetCurrentMethod()),
                        System.Windows.Forms.Application.CompanyName);
                }
            }

            return removed;
        }
    }
}
