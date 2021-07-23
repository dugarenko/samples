using Microsoft.CSharp;
using MSMove.Common.Interfaces;
using MSMove.Properties;
using MSMove.Security.Cryptography;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace MSMove.Common
{
    internal static class WrapperHelper
    {
        private static byte[] KEY = new byte[] { 0x45, 0xCD, 0x76, 0x4E, 0x64, 0x15, 0x72, 0xB0, 0xB2, 0xB6, 0x49, 0x92, 0x35, 0xBA, 0xB9, 0x5F, 0xF8, 0xA4, 0xDD, 0xE8, 0x30, 0x0E, 0xD9, 0x03, 0x9B, 0x01, 0xCC, 0x2A, 0x6E, 0xB2, 0x42, 0x75 };
        private static byte[] InitializationVector = new byte[] { 0x59, 0x12, 0x98, 0xDF, 0x33, 0xE5, 0xB1, 0x1F, 0x62, 0x14, 0x6E, 0x91, 0x99, 0xFE, 0xA2, 0x8E };
        //
        private static Assembly _assembly = typeof(FrmMain).Assembly;
        private static string _assemblyName = _assembly.GetName().Name;
        private static string _typeWrapperName = string.Format("{0}.{1}", _assemblyName, "Wrapper");
        //
        private static Type _wrapperType = null;
        private static MethodInfo _getWindowInfoFromPointMethodInfo = null;
        private static IWrapper _wrapper = null;

        static WrapperHelper()
        {
            // Jeśli klasa Wrapper ma ustawioną właściwość 'Build Action = Compile' to typ zostaie zwrócony.
            // Oznacza to, że klasa jest w kompilowana w aplikację, w przeciwnym razie klasę trzeba skompilować
            // w locie, ponieważ znajduje się w zasobach.
            _wrapperType = _assembly.DefinedTypes.FirstOrDefault(x => x.FullName == _typeWrapperName);
            if (_wrapperType == null)
            {
                _wrapperType = CompileWrapper();
            }

            // Pobranie informacji o statycznej metodzie 'GetWindowInfoFromPoint'.
            _getWindowInfoFromPointMethodInfo = _wrapperType.GetMethod("GetWindowInfoFromPoint", BindingFlags.Public | BindingFlags.Static);
        }

        #region Private methods.

        /// <summary>
        /// Tworzy i zwraca obiekt Wrapper.
        /// </summary>
        private static IWrapper CreateWrapper(string className, string windowName, bool rollbackState)
        {
            ConstructorInfo constructor = _wrapperType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                Type.DefaultBinder,
                new Type[] { typeof(string), typeof(string), typeof(bool) },
                null);

            return (IWrapper)constructor.Invoke(new object[] { className, windowName, rollbackState });
        }

        /// <summary>
        /// Kompiluje kod Wrapper-a i zwraca jego typ.
        /// </summary>
        private static Type CompileWrapper()
        {

            Type type = null;
            var options = new CompilerParameters();
            options.GenerateInMemory = true;
            string msMoveCommonDllPath = Path.Combine(Path.GetDirectoryName(_assembly.Location), "MSMove.Common.dll");

            options.ReferencedAssemblies.AddRange(new string[]
            {
                "System.dll",
                "System.Drawing.dll",
                "System.Windows.Forms.dll",
                msMoveCommonDllPath
            });

            string sourceCode = Decrypt(Resources.WrapperBase64String);

            using (var provider = new CSharpCodeProvider())
            {
                var compile = provider.CompileAssemblyFromSource(options, sourceCode);
                type = compile.CompiledAssembly.GetType(_typeWrapperName);
            }
            return type;
        }

        /// <summary>
        /// Deszyfruje przekazany ciąg znaków Base64. Deszyfrowanie globalne może wykonać każdy użytkownik systemu.
        /// </summary>
        /// <param name="base64String">Ciąg znaków Base64 do odszyfrowania.</param>
        /// <returns>Odszyfrowany ciąg znaków.</returns>
        private static string Decrypt(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return base64String;
            }

            try
            {
                byte[] data = Convert.FromBase64String(base64String);
                byte[] decryptedData;
                RijndaelManagedEx.Decrypt(data, KEY, InitializationVector, out decryptedData);
                string result = Encoding.UTF8.GetString(decryptedData);
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return base64String;
        }

        #endregion

        #region Internal methods.

        /// <summary>
        /// Umożliwia aplikacji informowanie systemu, że jest w użyciu, zapobiegając w ten sposób
        /// przejściu systemu w stan uśpienia lub wyłączeniu wyświetlacza podczas działania aplikacji.
        /// </summary>
        /// <param name="noSleepOrTurnOff">Wratość 'true' zapobiega przejściu systemu w stan uśpienia lub wyłączeniu wyświetlacza.</param>
        internal static void Display(bool noSleepOrTurnOff, string className, string windowName, bool rollbackState)
        {
            if (_wrapper == null)
            {
                _wrapper = CreateWrapper(className, windowName, rollbackState);
            }
            else if (_wrapper.ClassName != className || _wrapper.WindowName != windowName)
            {
                _wrapper = CreateWrapper(className, windowName, rollbackState);
            }

            _wrapper.Display(noSleepOrTurnOff);
        }

        internal static bool Move(string className, string windowName, bool rollbackState)
        {
            if (_wrapper == null)
            {
                _wrapper = CreateWrapper(className, windowName, rollbackState);
            }
            else if (_wrapper.ClassName != className || _wrapper.WindowName != windowName)
            {
                _wrapper = CreateWrapper(className, windowName, rollbackState);
            }

            bool result = _wrapper.Move();
            if (!result)
            {
                _wrapper = null;
            }
            return result;
        }

        /// <summary>
        /// Zwraca informacje o oknie z pod bieżącej pozycji kursora myszy.
        /// </summary>
        internal static IWindowInfo GetWindowInfoFromPoint()
        {
            return (IWindowInfo)_getWindowInfoFromPointMethodInfo.Invoke(null, null);
        }

        /// <summary>
        /// Szyfruje przekazany ciąg znaków. Szyfrowanie globalne oznacza, że odszyfrować może każdy użytkownik systemu.
        /// </summary>
        /// <param name="value">Ciąg znaków do zaszyfrowania.</param>
        /// <returns>Zaszyfrowany ciąg znaków w formacie Base64.</returns>
        internal static string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(value);
                byte[] encryptedData;
                RijndaelManagedEx.Encrypt(data, KEY, InitializationVector, out encryptedData);
                string result = Convert.ToBase64String(encryptedData);
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return value;
        }

        #endregion
    }
}
