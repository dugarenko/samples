using MVVMCore.Properties;
using System;
using System.Reflection;

namespace MVVMCore.Reflection
{
    /// <summary>
    /// Dostarcza metody, które wykonują operacje na metadanych metod.
    /// </summary>
	public static class MethodInfoEx
    {
        /// <summary>
        /// Domyślne flagi: BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase.
        /// </summary>
        public const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
            BindingFlags.NonPublic | BindingFlags.IgnoreCase;

        /// <summary>
        /// Weryfikacja danych.
        /// </summary>
        /// <param name="obj">Obiekt do zweryfikowania.</param>
        /// <param name="methodName">Nazwa metody.</param>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        private static void VerifyData(object obj, string methodName)
        {
            if (obj.Equals(null))
                throw new ArgumentNullException("obj");
            else if (methodName.Equals(null))
                throw new ArgumentNullException("methodName");
            else if (methodName.Trim() == "")
                throw new ArgumentEmptyException("methodName");
        }

        /// <summary>
        /// Sprawdza czy obiekt ma wskazaną metodę.
        /// </summary>
        /// <param name="obj">Obiekt, który poddany zostatnie weryfikacji czy posiada wskazaną metodę.</param>
        /// <param name="methodName">Nazwa metody.</param>
        /// <returns>True lub false.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static Boolean HasMethod(object obj, string methodName)
        {
            return (GetMethod(obj, methodName, DefaultFlags) != null);
        }

        /// <summary>
        /// Sprawdza czy obiekt ma wskazaną metodę.
        /// </summary>
        /// <param name="obj">Obiekt, który poddany zostatnie weryfikacji czy posiada wskazaną metodę.</param>
        /// <param name="methodName">Nazwa metody.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>True lub false.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static Boolean HasMethod(object obj, string methodName, BindingFlags flags)
        {
            return (GetMethod(obj, methodName, flags) != null);
        }

        /// <summary>
        /// Zwraca wskazaną metodę obiektu.
        /// </summary>
        /// <param name="obj">Obiekt, którego metoda zostanie zwrócona.</param>
        /// <param name="methodName">Nazwa szukanej metody.</param>
        /// <returns>System.Reflection.MethodInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static MethodInfo GetMethod(object obj, string methodName)
        {
            return GetMethod(obj, methodName, DefaultFlags);
        }

        /// <summary>
        /// Zwraca wskazaną metodę obiektu.
        /// </summary>
        /// <param name="obj">Obiekt, którego metoda zostanie zwrócona.</param>
        /// <param name="methodName">Nazwa szukanej metody.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>System.Reflection.MethodInfo lub null.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static MethodInfo GetMethod(object obj, string methodName, BindingFlags flags)
        {
            VerifyData(obj, methodName);
            return obj.GetType().GetMethod(methodName, flags);
        }

        /// <summary>
        /// Wywołanie metody.
        /// </summary>
        /// <param name="obj">Obiekt, na którym wywołana zostanie wskazana metoda.</param>
        /// <param name="methodName">Nazwa metody.</param>
        /// <returns>Obiekt zwrócony z metody.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static object Invoke(object obj, string methodName)
        {
            return Invoke<object>(obj, methodName, DefaultFlags, null);
        }

        /// <summary>
        /// Wywołanie metody.
        /// </summary>
        /// <param name="obj">Obiekt, na którym wywołana zostanie wskazana metoda.</param>
        /// <param name="methodName">Nazwa metody.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>Obiekt zwrócony z metody.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static object Invoke(object obj, string methodName, BindingFlags flags)
        {
            return Invoke<object>(obj, methodName, flags, null);
        }

        /// <summary>
        /// Wywołanie metody.
        /// </summary>
        /// <param name="obj">Obiekt, na którym wywołana zostanie wskazana metoda.</param>
        /// <param name="methodName">Nazwa metody.</param>
        /// <param name="parameters">Parametry przekazane do metody.</param>
        /// <returns>Obiekt zwrócony z metody.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static object Invoke(object obj, string methodName, object[] parameters)
        {
            return Invoke<object>(obj, methodName, DefaultFlags, parameters);
        }

        /// <summary>
        /// Wywołanie metody.
        /// </summary>
        /// <param name="obj">Obiekt, na którym wywołana zostanie wskazana metoda.</param>
        /// <param name="methodName">Nazwa metody.</param>
        /// <param name="flags">Flagi.</param>
        /// <param name="parameters">Parametry przekazane do metody.</param>
        /// <returns>Obiekt zwrócony z metody.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static object Invoke(object obj, string methodName, BindingFlags flags, object[] parameters)
        {
            return Invoke<object>(obj, methodName, flags, parameters);
        }

        /// <summary>
        /// Wywołanie metody.
        /// </summary>
        /// <typeparam name="T">Typ zwróconej wartości.</typeparam>
        /// <param name="obj">Obiekt, na którym wywołana zostanie wskazana metoda.</param>
        /// <param name="methodName">Nazwa metody.</param>
        /// <returns>Wartość zwrócona z metody.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static T Invoke<T>(object obj, string methodName)
        {
            return (T)Invoke<T>(obj, methodName, DefaultFlags, null);
        }

        /// <summary>
        /// Wywołanie metody.
        /// </summary>
        /// <typeparam name="T">Typ zwróconej wartości.</typeparam>
        /// <param name="obj">Obiekt, na którym wywołana zostanie wskazana metoda.</param>
        /// <param name="methodName">Nazwa metody.</param>
        /// <param name="flags">Flagi.</param>
        /// <returns>Wartość zwrócona z metody.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static T Invoke<T>(object obj, string methodName, BindingFlags flags)
        {
            return (T)Invoke<T>(obj, methodName, flags, null);
        }

        /// <summary>
        /// Wywołanie metody.
        /// </summary>
        /// <typeparam name="T">Typ zwróconej wartości.</typeparam>
        /// <param name="obj">Obiekt, na którym wywołana zostanie wskazana metoda.</param>
        /// <param name="methodName">Nazwa metody.</param>
        /// <param name="parameters">Parametry przekazane do metody.</param>
        /// <returns>Wartość zwrócona z metody.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static T Invoke<T>(object obj, string methodName, object[] parameters)
        {
            return (T)Invoke<T>(obj, methodName, DefaultFlags, parameters);
        }

        /// <summary>
        /// Wywołanie metody.
        /// </summary>
        /// <typeparam name="T">Typ zwróconej wartości.</typeparam>
        /// <param name="obj">Obiekt, na którym wywołana zostanie wskazana metoda.</param>
        /// <param name="methodName">Nazwa metody.</param>
        /// <param name="flags">Flagi.</param>
        /// <param name="parameters">Parametry przekazane do metody.</param>
        /// <returns>Wartość zwrócona z metody.</returns>
        /// <exception cref="ArgumentNullException">Argument nie może przyjmować wartości 'null'.</exception>
        /// <exception cref="ArgumentException">Argument nie może przyjmować wartości pustej (Empty).</exception>
        public static T Invoke<T>(object obj, string methodName, BindingFlags flags, object[] parameters)
        {
            MethodInfo mi = GetMethod(obj, methodName, flags);

            if (mi == null)
                throw new ArgumentException(string.Format(Resources.ObjectNotHaveMethod, methodName), "methodName");

            if (mi.ReturnType != typeof(void))
                return (T)mi.Invoke(obj, parameters);

            mi.Invoke(obj, parameters);
            return default(T);
        }
    }
}
