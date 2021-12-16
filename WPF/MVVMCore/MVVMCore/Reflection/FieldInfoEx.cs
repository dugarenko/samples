using MVVMCore.Properties;
using System;
using System.Reflection;

namespace MVVMCore.Reflection
{
    /// <summary>
    /// Dostarcza metody, które wykonują operacje na metadanych pól.
    /// </summary>
    public static class FieldInfoEx
    {
        /// <summary>
        /// Domyślne flagi: BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase.
        /// </summary>
        public const BindingFlags DefaultFlags = MethodInfoEx.DefaultFlags;

        /// <summary>
        /// Weryfikacja danych.
        /// </summary>
        /// <param name="obj">Obiekt do zweryfikowania.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        private static void VerifyData(object obj, string fieldName)
        {
            if (obj.Equals(null))
                throw new ArgumentNullException("obj");
            else if (fieldName.Equals(null))
                throw new ArgumentNullException("fieldName");
            else if (fieldName.Trim() == "")
                throw new ArgumentEmptyException("fieldName");
        }

        /// <summary>
        /// Sprawdza czy obiekt ma wskazane pole.
        /// </summary>
        /// <param name="obj">Obiekt, który poddany zostatnie weryfikacji czy posiada wskazane pole.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <returns>True lub false.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static Boolean HasField(object obj, string fieldName)
        {
            return (GetField(obj, fieldName, DefaultFlags) != null);
        }

        /// <summary>
        /// Sprawdza czy obiekt ma wskazane pole.
        /// </summary>
        /// <param name="obj">Obiekt, który poddany zostatnie weryfikacji czy posiada wskazane pole.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>True lub false.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static Boolean HasField(object obj, string fieldName, BindingFlags flags)
        {
            return (GetField(obj, fieldName, flags) != null);
        }

        /// <summary>
        /// Zwraca wskazane pole obiektu.
        /// </summary>
        /// <param name="obj">Obiekt, którego pole zostanie zwrócone.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <returns>System.Reflection.FieldInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static FieldInfo GetField(object obj, string fieldName)
        {
            return GetField(obj, fieldName, DefaultFlags);
        }

        /// <summary>
        /// Zwraca wskazane pole obiektu.
        /// </summary>
        /// <typeparam name="TObject">Typ obiektu.</typeparam>
        /// <param name="obj">Obiekt, którego pole zostanie zwrócone.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <returns>System.Reflection.FieldInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static FieldInfo GetField<TObject>(object obj, string fieldName)
        {
            VerifyData(obj, fieldName);
            return typeof(TObject).GetField(fieldName, DefaultFlags);
        }

        /// <summary>
        /// Zwraca wskazane pole obiektu.
        /// </summary>
        /// <param name="obj">Obiekt, którego pole zostanie zwrócone.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>System.Reflection.FieldInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static FieldInfo GetField(object obj, string fieldName, BindingFlags flags)
        {
            VerifyData(obj, fieldName);
            return obj.GetType().GetField(fieldName, flags);
        }

        /// <summary>
        /// Zwraca wskazane pole obiektu.
        /// </summary>
        /// <typeparam name="T">Typ obiektu.</typeparam>
        /// <param name="obj">Obiekt, którego pole zostanie zwrócone.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>System.Reflection.FieldInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static FieldInfo GetField<T>(object obj, string fieldName, BindingFlags flags)
        {
            VerifyData(obj, fieldName);
            return typeof(T).GetField(fieldName, flags);
        }

        /// <summary>
        /// Zwraca wartość wskazanego pola.
        /// </summary>
        /// <param name="obj">Obiekt, którego wartość pola zostanie zwrócona.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <returns>Wartość wskazanego pola.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static T GetValue<T>(object obj, string fieldName)
        {
            return (T)GetValue<T>(obj, fieldName, DefaultFlags);
        }

        /// <summary>
        /// Zwraca wartość wskazanego pola.
        /// </summary>
        /// <typeparam name="TObject">Typ obiektu.</typeparam>
        /// <typeparam name="TValue">Typ wartości.</typeparam>
        /// <param name="obj">Obiekt, którego wartość pola zostanie zwrócona.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <returns>Wartość wskazanego pola.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static TValue GetValue<TObject, TValue>(object obj, string fieldName)
        {
            return GetValue<TObject, TValue>(obj, fieldName, DefaultFlags);
        }

        /// <summary>
        /// Zwraca wartość wskazanego pola.
        /// </summary>
        /// <typeparam name="TObject">Typ obiektu.</typeparam>
        /// <typeparam name="TValue">Typ wartości.</typeparam>
        /// <param name="obj">Obiekt, którego wartość pola zostanie zwrócona.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>Wartość wskazanego pola.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static TValue GetValue<TObject, TValue>(object obj, string fieldName, BindingFlags flags)
        {
            FieldInfo fi = GetField<TObject>(obj, fieldName, flags);
            if (fi == null)
            {
                throw new ArgumentException(string.Format(Resources.ObjectNotHaveField, fieldName), "fieldName");
            }

#if UNLOCK_REFLECTION
            fi.TryUnlock();
#endif
            return (TValue)fi.GetValue(obj);
        }

        /// <summary>
        /// Zwraca wartość wskazanego pola.
        /// </summary>
        /// <param name="obj">Obiekt, którego wartość pola zostanie zwrócona.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>Wartość wskazanego pola.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static T GetValue<T>(object obj, string fieldName, BindingFlags flags)
        {
            FieldInfo fi = GetField(obj, fieldName, flags);
            if (fi == null)
            {
                throw new ArgumentException(string.Format(Resources.ObjectNotHaveField, fieldName), "fieldName");
            }
            return (T)fi.GetValue(obj);
        }

        /// <summary>
        /// Wczytuje wartość do wskazanego pola.
        /// </summary>
        /// <param name="obj">Obiekt, którego wartość pola zostanie zmieniona.</param>
        /// <param name="value">Wartość.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static void SetValue(object obj, object value, string fieldName)
        {
            SetValue(obj, value, fieldName, DefaultFlags);
        }

        /// <summary>
        /// Wczytuje wartość do wskazanego pola.
        /// </summary>
        /// <param name="obj">Obiekt, którego wartość pola zostanie zmieniona.</param>
        /// <param name="value">Wartość.</param>
        /// <param name="fieldName">Nazwa pola.</param>
        /// <param name="flags">Flagi.</param>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static void SetValue(object obj, object value, string fieldName, BindingFlags flags)
        {
            FieldInfo fi = GetField(obj, fieldName, flags);

            if (fi == null)
                throw new ArgumentException(string.Format(Resources.ObjectNotHaveField, fieldName), "fieldName");
            fi.SetValue(obj, value);
        }
    }
}
