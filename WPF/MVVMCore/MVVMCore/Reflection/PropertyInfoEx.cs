using MVVMCore.Properties;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace MVVMCore.Reflection
{
    /// <summary>
    /// Dostarcza metody, które wykonują operacje na metadanych właściwości.
    /// </summary>
    public static class PropertyInfoEx
    {
        /// <summary>
        /// Domyślne flagi: BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase.
        /// </summary>
        public const BindingFlags DefaultFlags = MethodInfoEx.DefaultFlags;

        /// <summary>
        /// Weryfikacja nazwy właściwości.
        /// </summary>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        private static void VerifyData(string propertyName)
        {
            if (propertyName.Equals(null))
                throw new ArgumentNullException("propertyName");
            else if (propertyName.Trim() == "")
                throw new ArgumentEmptyException("propertyName");
        }

        /// <summary>
        /// Weryfikacja danych.
        /// </summary>
        /// <param name="obj">Obiekt do zweryfikowania.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        private static void VerifyData(object obj, string propertyName)
        {
            if (obj.Equals(null))
                throw new ArgumentNullException("obj");

            VerifyData(propertyName);
        }

        /// <summary>
        /// Sprawdza czy obiekt ma wskazaną właściwość.
        /// </summary>
        /// <param name="obj">Obiekt, który poddany zostatnie weryfikacji czy posiada wskazanną właściwość.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <returns>True lub false.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static Boolean HasProperty(object obj, string propertyName)
        {
            return (GetProperty(obj, propertyName, DefaultFlags) != null);
        }

        /// <summary>
        /// Sprawdza czy obiekt ma wskazaną właściwość.
        /// </summary>
        /// <param name="obj">Obiekt, który poddany zostatnie weryfikacji czy posiada wskazanną właściwość.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>True lub false.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static Boolean HasProperty(object obj, string propertyName, BindingFlags flags)
        {
            return (GetProperty(obj, propertyName, flags) != null);
        }

        /// <summary>
        /// Zwraca właściwość.
        /// </summary>
        /// <param name="obj">Obiekt, którego właściwość zostanie zwrócona.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <returns>System.Reflection.PropertyInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static PropertyInfo GetProperty(object obj, string propertyName)
        {
            return GetProperty(obj, propertyName, DefaultFlags);
        }

        /// <summary>
        /// Zwraca właściwość.
        /// </summary>
        /// <typeparam name="TObject">Typ obiektu.</typeparam>
        /// <param name="obj">Obiekt, którego właściwość zostanie zwrócona.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <returns>System.Reflection.PropertyInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static PropertyInfo GetProperty<TObject>(object obj, string propertyName)
        {
            return GetProperty<TObject>(obj, propertyName, DefaultFlags);
        }

        /// <summary>
        /// Zwraca właściwość.
        /// </summary>
        /// <typeparam name="T">Typ obiektu, którego właściwość zostanie zwrócona.</typeparam>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <returns>System.Reflection.PropertyInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static PropertyInfo GetProperty<T>(string propertyName)
        {
            VerifyData(propertyName);
            return typeof(T).GetProperty(propertyName, DefaultFlags);
        }

        /// <summary>
        /// Zwraca właściwość.
        /// </summary>
        /// <param name="obj">Obiekt, którego właściwość zostanie zwrócona.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>System.Reflection.PropertyInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static PropertyInfo GetProperty(object obj, string propertyName, BindingFlags flags)
        {
            VerifyData(obj, propertyName);
            return obj.GetType().GetProperty(propertyName, flags);
        }

        /// <summary>
        /// Zwraca właściwość.
        /// </summary>
        /// <typeparam name="TObject">Typ obiektu.</typeparam>
        /// <param name="obj">Obiekt, którego właściwość zostanie zwrócona.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>System.Reflection.PropertyInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static PropertyInfo GetProperty<TObject>(object obj, string propertyName, BindingFlags flags)
        {
            VerifyData(obj, propertyName);
            return typeof(TObject).GetProperty(propertyName, flags);
        }

        /// <summary>
        /// Zwraca właściwość.
        /// </summary>
        /// <typeparam name="T">Typ obiektu, którego właściwość zostanie zwrócona.</typeparam>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>System.Reflection.PropertyInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static PropertyInfo GetProperty<T>(string propertyName, BindingFlags flags)
        {
            VerifyData(propertyName);
            return typeof(T).GetProperty(propertyName, flags);
        }

        /// <summary>
        /// Zwraca wartość wskazanej właściwości.
        /// </summary>
        /// <param name="obj">Obiekt, którego wartość właściwości zostanie zwrócona.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <returns>Wartość wskazanej właściwości.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static T GetValue<T>(object obj, string propertyName)
        {
            return (T)GetValue<T>(obj, propertyName, DefaultFlags);
        }

        /// <summary>
        /// Zwraca wartość wskazanego pola.
        /// </summary>
        /// <typeparam name="TObject">Typ obiektu.</typeparam>
        /// <typeparam name="TValue">Typ wartości.</typeparam>
        /// <param name="obj">Obiekt, którego wartość właściwości zostanie zwrócona.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <returns>Wartość wskazanej właściwości.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static TValue GetValue<TObject, TValue>(object obj, string propertyName)
        {
            return GetValue<TObject, TValue>(obj, propertyName, DefaultFlags);
        }

        /// <summary>
        /// Zwraca wartość wskazanego pola.
        /// </summary>
        /// <typeparam name="TObject">Typ obiektu.</typeparam>
        /// <typeparam name="TValue">Typ wartości.</typeparam>
        /// <param name="obj">Obiekt, którego wartość właściwości zostanie zwrócona.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>Wartość wskazanej właściwości.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static TValue GetValue<TObject, TValue>(object obj, string propertyName, BindingFlags flags)
        {
            PropertyInfo pi = GetProperty<TObject>(obj, propertyName, flags);

            if (pi == null)
            {
                throw new ArgumentException(string.Format(Resources.ObjectNotHaveProperty, propertyName), "propertyName");
            }

            if (pi.CanRead == false)
            {
                throw new ArgumentException(string.Format(Resources.PropertyIsWriteOnly, propertyName), "propertyName");
            }

#if UNLOCK_REFLECTION
            pi.TryUnlock();
#endif
            return (TValue)pi.GetValue(obj, null);
        }

        /// <summary>
        /// Zwraca wartość wskazanej właściwości.
        /// </summary>
        /// <param name="obj">Obiekt, którego wartość właściwości zostanie zwrócona.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>Wartość wskazanej właściwości.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static T GetValue<T>(object obj, string propertyName, BindingFlags flags)
        {
            PropertyInfo pi = GetProperty(obj, propertyName, flags);

            if (pi == null)
            {
                throw new ArgumentException(string.Format(Resources.ObjectNotHaveProperty, propertyName), "propertyName");
            }

            if (pi.CanRead == false)
            {
                throw new ArgumentException(string.Format(Resources.PropertyIsWriteOnly, propertyName), "propertyName");
            }

#if UNLOCK_REFLECTION
            pi.TryUnlock();
#endif
            return (T)pi.GetValue(obj, null);
        }

        /// <summary>
        /// Wczytuje wartość do właściwość.
        /// </summary>
        /// <param name="obj">Obiekt, którego wartość właściwości zostanie zmieniona.</param>
        /// <param name="value">Wartość.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static void SetValue(object obj, object value, string propertyName)
        {
            SetValue(obj, value, propertyName, DefaultFlags);
        }

        /// <summary>
        /// Wczytuje wartość do właściwość.
        /// </summary>
        /// <param name="obj">Obiekt, którego wartość właściwości zostanie zmieniona.</param>
        /// <param name="value">Wartość.</param>
        /// <param name="propertyName">Nazwa właściwości.</param>
        /// <param name="flags">Flagi.</param>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static void SetValue(object obj, object value, string propertyName, BindingFlags flags)
        {
            PropertyInfo pi = GetProperty(obj, propertyName, flags);

            if (pi == null)
                throw new ArgumentException(string.Format(Resources.ObjectNotHaveProperty, propertyName), "propertyName");

            if (pi.CanWrite == false)
                throw new ArgumentException(string.Format(Resources.PropertyIsReadOnly, propertyName), "propertyName");

            pi.SetValue(obj, value, null);
        }

        /// <summary>
        /// Zwraca nazwę właściwości.
        /// </summary>
        /// <typeparam name="T">Typ danych.</typeparam>
        /// <param name="property">Właściwość.</param>
        public static string GetPropertyName<T>(Expression<Func<T>> property)
        {
            LambdaExpression lambdaExpression = (LambdaExpression)property;
            MemberExpression memberExpression = (!(lambdaExpression.Body is UnaryExpression) ?
                (MemberExpression)lambdaExpression.Body : (MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Zwraca właściwości interfejsu.
        /// </summary>
        /// <param name="interfaceType">Typ interfejsu.</param>
        /// <param name="includeBase">Określa czy dodać właściwości z interfejsów odziedziczonych.</param>
        /// <returns>Tablica właściwości.</returns>
        public static PropertyInfo[] GetInterfaceProperties(Type interfaceType, bool includeBase)
        {
            return GetInterfaceProperties(interfaceType, includeBase,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
        }

        /// <summary>
        /// Zwraca właściwości interfejsu.
        /// </summary>
        /// <param name="interfaceType">Typ interfejsu.</param>
        /// <param name="includeBase">Określa czy dodać właściwości z interfejsów odziedziczonych.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>Tablica właściwości.</returns>
        public static PropertyInfo[] GetInterfaceProperties(Type interfaceType, bool includeBase, BindingFlags flags)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();

            if (interfaceType != null && interfaceType.IsInterface)
            {
                PropertyInfo[] pis = interfaceType.GetProperties(flags);
                if (pis.Length > 0)
                {
                    properties.AddRange(pis);
                }

                if (includeBase)
                {
                    foreach (var interfaceBase in interfaceType.GetInterfaces())
                    {
                        pis = interfaceBase.GetProperties(flags);
                        if (pis.Length > 0)
                        {
                            properties.AddRange(pis);
                        }
                    }
                }
            }

            return properties.ToArray();
        }
    }
}
