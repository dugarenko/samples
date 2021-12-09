using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Xml;
using System.Linq;

namespace MVVMCore.Reflection
{
    /// <summary>
    /// Dostarcza metody, które zwracają różne informacje z danego zestawu.
    /// </summary>
    public static class AssemblyEx
    {
        private static Dictionary<string, Assembly> _domainAssemblies = null;
        private static Assembly _executingAssembly = Assembly.GetExecutingAssembly();
        private static Dictionary<string, List<Attribute>> _assemblyAttributes = new Dictionary<string, List<Attribute>>();

        /// <summary>
        /// Zwraca kolekcję informacji o atrybutach zestawu.
        /// </summary>
        /// <param name="assembly">Zestaw.</param>
        /// <returns>Kolekcja informacji o atrybutach zestawu.</returns>
        public static List<Attribute> GetAssemblyAttributes(Assembly assembly)
        {
            string guid = "";
            Attribute attribute = GetCustomAttribute<GuidAttribute>(assembly);

            if (attribute != null)
            {
                guid = ((GuidAttribute)attribute).Value;
                if (_assemblyAttributes.ContainsKey(guid))
                {
                    return _assemblyAttributes[guid];
                }
            }

            List<Attribute> attributes = new List<Attribute>();

            if (string.IsNullOrEmpty(guid) == false)
            {
                _assemblyAttributes.Add(guid, attributes);
            }

            attribute = GetCustomAttribute<AssemblyAlgorithmIdAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyCompanyAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyConfigurationAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyCopyrightAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyCultureAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyDefaultAliasAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyDelaySignAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyDescriptionAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyFileVersionAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyFlagsAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyInformationalVersionAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyKeyFileAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyKeyNameAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyProductAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyTitleAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyTrademarkAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<AssemblyVersionAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }
            else
            {
                attributes.Add(new AssemblyVersionAttribute(assembly.GetName().Version.ToString()));
            }

            // Atrybut NeutralResourcesLanguageAttribute posiada wartości: CultureName, Location.
            attribute = GetCustomAttribute<NeutralResourcesLanguageAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<GuidAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            attribute = GetCustomAttribute<ComVisibleAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            // Atrybut AssemblyMetadataAttribute posiada wartości Key, Value.
            attribute = GetCustomAttribute<AssemblyMetadataAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            // Atrybut AssemblySignatureKeyAttribute posiada wartości: Countersignature, PublicKey.
            attribute = GetCustomAttribute<AssemblySignatureKeyAttribute>(assembly);
            if (attribute != null)
            {
                attributes.Add(attribute);
            }

            return attributes;
        }

        /// <summary>
        /// Zwraca atrybut zestawu.
        /// </summary>
        /// <typeparam name="T">Typ atrybutu.</typeparam>
        /// <param name="assembly">Zestaw.</param>
        /// <returns>Atrybut zestawu.</returns>
        public static T GetCustomAttribute<T>(Assembly assembly) where T : Attribute
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(T), true);
            if (attributes == null || attributes.Length == 0)
            {
                return null;
            }
            return (T)attributes[0];
        }

        /// <summary>
        /// Zwraca wartość atrybutu wskazanego zestawu.
        /// </summary>
        /// <param name="assembly">Zestaw.</param>
        /// <returns>Wartość atrybutu.</returns>
        public static TValue GetAttributeValue<TValue, TAttribute>(Assembly assembly) where TAttribute : Attribute
        {
            return GetAttributeValue<TValue, TAttribute>(assembly, null);
        }

        /// <summary>
        /// Zwraca wartość atrybutu wskazanego zestawu.
        /// </summary>
        /// <param name="assembly">Zestaw.</param>
        /// <param name="propertyName">Nazwa właściwości atrybutu.</param>
        /// <returns>Wartość atrybutu.</returns>
        public static TValue GetAttributeValue<TValue, TAttribute>(Assembly assembly, string propertyName) where TAttribute : Attribute
        {
            Type attributeType = typeof(TAttribute);
            List<Attribute> attributes = GetAssemblyAttributes(assembly);
            Attribute attribute = attributes.FirstOrDefault(item => item.TypeId.Equals(attributeType));

            if (attribute != null)
            {
                if (attributeType == typeof(AssemblyAlgorithmIdAttribute))
                    return (TValue)(object)((AssemblyAlgorithmIdAttribute)attribute).AlgorithmId;

                if (attributeType == typeof(AssemblyCompanyAttribute))
                    return (TValue)(object)((AssemblyCompanyAttribute)attribute).Company;

                if (attributeType == typeof(AssemblyConfigurationAttribute))
                    return (TValue)(object)((AssemblyConfigurationAttribute)attribute).Configuration;

                if (attributeType == typeof(AssemblyCopyrightAttribute))
                    return (TValue)(object)((AssemblyCopyrightAttribute)attribute).Copyright;

                if (attributeType == typeof(AssemblyCultureAttribute))
                    return (TValue)(object)((AssemblyCultureAttribute)attribute).Culture;

                if (attributeType == typeof(AssemblyDefaultAliasAttribute))
                    return (TValue)(object)((AssemblyDefaultAliasAttribute)attribute).DefaultAlias;

                if (attributeType == typeof(AssemblyDelaySignAttribute))
                    return (TValue)(object)((AssemblyDelaySignAttribute)attribute).DelaySign;

                if (attributeType == typeof(AssemblyDescriptionAttribute))
                    return (TValue)(object)((AssemblyDescriptionAttribute)attribute).Description;

                if (attributeType == typeof(AssemblyFileVersionAttribute))
                    return (TValue)(object)((AssemblyFileVersionAttribute)attribute).Version;

                if (attributeType == typeof(AssemblyFlagsAttribute))
                    return (TValue)(object)((AssemblyFlagsAttribute)attribute).AssemblyFlags;

                if (attributeType == typeof(AssemblyInformationalVersionAttribute))
                    return (TValue)(object)((AssemblyInformationalVersionAttribute)attribute).InformationalVersion;

                if (attributeType == typeof(AssemblyKeyFileAttribute))
                    return (TValue)(object)((AssemblyKeyFileAttribute)attribute).KeyFile;

                if (attributeType == typeof(AssemblyKeyNameAttribute))
                    return (TValue)(object)((AssemblyKeyNameAttribute)attribute).KeyName;

                if (attributeType == typeof(AssemblyProductAttribute))
                    return (TValue)(object)((AssemblyProductAttribute)attribute).Product;

                if (attributeType == typeof(AssemblyTitleAttribute))
                    return (TValue)(object)((AssemblyTitleAttribute)attribute).Title;

                if (attributeType == typeof(AssemblyTrademarkAttribute))
                    return (TValue)(object)((AssemblyTrademarkAttribute)attribute).Trademark;

                if (attributeType == typeof(AssemblyVersionAttribute))
                {
                    string version = ((AssemblyVersionAttribute)attribute).Version;
                    if (string.IsNullOrEmpty(version) == false && typeof(TValue) == typeof(Version))
                    {
                        return (TValue)(object)new Version(version);
                    }
                    return (TValue)(object)version;
                }

                // Atrybut NeutralResourcesLanguageAttribute posiada wartości: CultureName, Location.
                if (attributeType == typeof(NeutralResourcesLanguageAttribute))
                {
                    if (string.IsNullOrEmpty(propertyName) == false)
                    {
                        if (propertyName.ToLower() == "culturename")
                            return (TValue)(object)((NeutralResourcesLanguageAttribute)attribute).CultureName;

                        if (propertyName.ToLower() == "location")
                            return (TValue)(object)((NeutralResourcesLanguageAttribute)attribute).Location;
                    }
                }

                if (attributeType == typeof(GuidAttribute))
                {
                    string value = ((GuidAttribute)attribute).Value;
                    if (typeof(TValue) == typeof(Guid))
                    {
                        Guid guid;
                        if (string.IsNullOrEmpty(value) == false && Guid.TryParse(value, out guid))
                        {
                            return (TValue)(object)guid;
                        }
                        return (TValue)(object)Guid.Empty;
                    }
                    return (TValue)(object)value;
                }

                if (attributeType == typeof(ComVisibleAttribute))
                    return (TValue)(object)((ComVisibleAttribute)attribute).Value;

                // Atrybut AssemblyMetadataAttribute posiada wartości Key, Value.
                if (attributeType == typeof(AssemblyMetadataAttribute))
                {
                    if (string.IsNullOrEmpty(propertyName) == false)
                    {
                        if (propertyName.ToLower() == "key")
                            return (TValue)(object)((AssemblyMetadataAttribute)attribute).Key;

                        if (propertyName.ToLower() == "value")
                            return (TValue)(object)((AssemblyMetadataAttribute)attribute).Value;
                    }
                }

                // Atrybut AssemblySignatureKeyAttribute posiada wartości: Countersignature, PublicKey.
                if (attributeType == typeof(AssemblySignatureKeyAttribute))
                {
                    if (string.IsNullOrEmpty(propertyName) == false)
                    {
                        if (propertyName.ToLower() == "countersignature")
                            return (TValue)(object)((AssemblySignatureKeyAttribute)attribute).Countersignature;

                        if (propertyName.ToLower() == "publickey")
                            return (TValue)(object)((AssemblySignatureKeyAttribute)attribute).PublicKey;
                    }
                }
            }

            return default(TValue);
        }

        #region Informacje o tej bibliotece.

        /// <summary>
        /// Nazwa firmy (Biblioteka AssemblyEx).
        /// </summary>
        public static string AssemblyCompany
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyCompanyAttribute>(_executingAssembly);
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Prawa autorskie (Biblioteka AssemblyEx).
        /// </summary>
        public static string AssemblyCopyright
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyCopyrightAttribute>(_executingAssembly);
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Opis produktu (Biblioteka AssemblyEx).
        /// </summary>
        public static string AssemblyDescription
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyDescriptionAttribute>(_executingAssembly);
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Wersja pliku Win32, może być różna od wersji assembly. (Biblioteka AssemblyEx).
        /// </summary>
        public static string AssemblyFileVersion
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyFileVersionAttribute>(_executingAssembly);
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Nazwa produktu (Biblioteka AssemblyEx).
        /// </summary>
        public static string AssemblyProduct
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyProductAttribute>(_executingAssembly);
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Tytuł produktu (Biblioteka AssemblyEx).
        /// </summary>
        public static string AssemblyTitle
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyTitleAttribute>(_executingAssembly);
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Wersja assembly - produktu (Biblioteka AssemblyEx).
        /// </summary>
        public static Version AssemblyVersion
        {
            get
            {
                return GetAttributeValue<Version, AssemblyVersionAttribute>(_executingAssembly);
            }
        }

        /// <summary>
        /// Dodatkowa informacja o wersji assembly. (Biblioteka AssemblyEx).
        /// </summary>
        public static string AssemblyInformationalVersion
        {
            get
            {
                return GetAttributeValue<string, AssemblyInformationalVersionAttribute>(_executingAssembly);
            }
        }

        /// <summary>
        /// Zwraca informację o kompatybilności z innymi powiązanymi zestawami. (Biblioteka AssemblyEx).
        /// </summary>
        public static AssemblyVersionCompatibility AssemblyVersionCompatibility
        {
            get
            {
                return _executingAssembly.GetName().VersionCompatibility;
            }
        }

        #endregion

        #region Informacje o programie wywołującym tą bibliotekę.

        /// <summary>
        /// Nazwa firmy (Aplikacja wywołująca).
        /// </summary>
        public static string CallingAssemblyCompany
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyCompanyAttribute>(Assembly.GetCallingAssembly());
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Prawa autorskie (Aplikacja wywołująca).
        /// </summary>
        public static string CallingAssemblyCopyright
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyCopyrightAttribute>(Assembly.GetCallingAssembly());
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Opis produktu (Aplikacja wywołująca).
        /// </summary>
        public static string CallingAssemblyDescription
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyDescriptionAttribute>(Assembly.GetCallingAssembly());
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Wersja pliku Win32, może być różna od wersji assembly. (Aplikacja wywołująca).
        /// </summary>
        public static string CallingAssemblyFileVersion
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyFileVersionAttribute>(Assembly.GetCallingAssembly());
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Nazwa produktu (Aplikacja wywołująca).
        /// </summary>
        public static string CallingAssemblyProduct
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyProductAttribute>(Assembly.GetCallingAssembly());
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Tytuł produktu (Aplikacja wywołująca).
        /// </summary>
        public static string CallingAssemblyTitle
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyTitleAttribute>(Assembly.GetCallingAssembly());
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Wersja assembly - produktu (Aplikacja wywołująca).
        /// </summary>
        public static Version CallingAssemblyVersion
        {
            get
            {
                return GetAttributeValue<Version, AssemblyVersionAttribute>(Assembly.GetCallingAssembly());
            }
        }

        /// <summary>
        /// Dodatkowa informacja o wersji assembly. (Aplikacja wywołująca).
        /// </summary>
        public static string CallingAssemblyInformationalVersion
        {
            get
            {
                string value = GetAttributeValue<string, AssemblyInformationalVersionAttribute>(Assembly.GetCallingAssembly());
                return string.IsNullOrEmpty(value) ? "" : value;
            }
        }

        /// <summary>
        /// Zwraca informację o kompatybilności z innymi powiązanymi zestawami. (Aplikacja wywołująca).
        /// </summary>
        public static AssemblyVersionCompatibility CallingAssemblyVersionCompatibility
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().VersionCompatibility;
            }
        }

        #endregion

        #region Inne.

        /// <summary>
        /// Ścieżka do katalogu aplikacji, który wywołał tą bibliotekę.
        /// </summary>
        public static string CurrentDirectory
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.WinCE)
                    return Path.GetDirectoryName(Assembly.GetCallingAssembly().GetName().CodeBase);
                else
                    return Directory.GetCurrentDirectory();
            }
        }

        /// <summary>
        /// Zwraca bitmapę (*.bmp).
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        /// <param name="fileName">Ścieżka (przestrzeń nazw) do pliku *.bmp.</param>
        /// <returns>Bitmapa.</returns>
        public static Bitmap GetEmbeddedBitmap(Assembly assembly, string fileName)
        {
            using (Stream str = assembly.GetManifestResourceStream(fileName))
            {
                return new Bitmap(str);
            }
        }

        /// <summary>
        /// Zwraca dokument (*.xml).
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        /// <param name="fileName">Ścieżka (przestrzeń nazw) do pliku *.xml.</param>
        /// <returns>Dokument xml.</returns>
        public static XmlDocument GetEmbeddedXmlFile(Assembly assembly, string fileName)
        {
            using (Stream str = assembly.GetManifestResourceStream(fileName))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(str);
                return xml;
            }
        }

        /// <summary>
        /// Zwraca strumień bajtów z pliku tekstowego (*.txt) wkompilowanego w projekt (Embedded Resource).
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        /// <param name="fileName">Ścieżka (przestrzeń nazw) do pliku *.txt.</param>
        /// <returns></returns>
        public static StreamReader GetEmbeddedTextFile(Assembly assembly, string fileName)
        {
            Stream str = assembly.GetManifestResourceStream(fileName);
            return new StreamReader(str);
        }

        /// <summary>
        /// Pobiera tekst z zasobu.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        /// <param name="embeddedResourceName">Ścieżka do zasobu.</param>
        /// <param name="name">Nazwa klucza.</param>
        public static string GetManifestResourceStream(Assembly assembly, string embeddedResourceName, string name)
        {
            try
            {
                using (Stream str = assembly.GetManifestResourceStream(embeddedResourceName))
                {
                    ResourceReader reader = new ResourceReader(str);
                    IDictionaryEnumerator enumerator = reader.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Key.ToString() == name)
                            return enumerator.Value.ToString();
                    }
                }
            }
            #region Blok catch i finally.
            catch
            {
                throw;
            }
            #endregion

            return "";
        }

        /// <summary>
        /// Zwraca pełną nazwę assembly.
        /// </summary>
        /// <param name="assemblyName">Nazwa assembly: System, System.Windows.Forms itd.</param>
        /// <returns>Pełna nazwa assembly.</returns>
        public static string GetAssemblyFullName(string assemblyName)
        {
            Assembly asm = GetAssembly(assemblyName);

            if (asm != null)
                return asm.FullName;

            return "";
        }

        /// <summary>
        /// Zwraca pełną nazwę assembly.
        /// </summary>
        /// <param name="assemblyName">Nazwa assembly: System, System.Windows.Forms itd.</param>
        /// <param name="recursiveSearch">Rekurencyjne przeszukiwanie. Oznacza, że jeśli podano nazwę
        /// zestawu np. System.Windows.Forms.Form to algorytm przeszukiwania będzie skracał podaną
        /// nazwę do momentu aż znaleziona zostanie prawidłowa System.Windows.Forms.</param>
        /// <returns>Pełna nazwa assembly.</returns>
        public static string GetAssemblyFullName(string assemblyName, bool recursiveSearch)
        {
            if (recursiveSearch == false)
            {
                return GetAssemblyFullName(assemblyName);
            }

            string temp = assemblyName;
            string assemblyFullName = "";

            while (assemblyFullName == "")
            {
                assemblyFullName = GetAssemblyFullName(temp);
                if (assemblyFullName == "")
                {
                    int index = temp.LastIndexOf('.', temp.Length - 1);
                    if (index == -1)
                        return "";
                    temp = temp.Substring(0, index);
                }
            }
            return assemblyFullName;
        }

        /// <summary>
        /// Zwraca Assembly.
        /// </summary>
        /// <param name="assemblyName">Nazwa assembly: System, System.Windows.Forms itd.</param>
        /// <returns>Assembly.</returns>
        public static Assembly GetAssembly(string assemblyName)
        {
            string name = "";
            string key = null;

            if (_domainAssemblies == null)
            {
                _domainAssemblies = new Dictionary<string, Assembly>();
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    name = asm.GetName().Name;
                    if (_domainAssemblies.ContainsKey(name) == false)
                    {
                        _domainAssemblies.Add(name, asm);
                    }
                }
                key = _domainAssemblies.Keys.FirstOrDefault(item => item.ToLower() == assemblyName.ToLower());
                if (key != null)
                {
                    assemblyName = key;
                }
            }
            else
            {
                key = _domainAssemblies.Keys.FirstOrDefault(item => item.ToLower() == assemblyName.ToLower());
                if (key != null)
                {
                    assemblyName = key;
                }

                if (_domainAssemblies.ContainsKey(assemblyName))
                {
                    return _domainAssemblies[assemblyName];
                }
                else
                {
                    // Sprawdzenie czy do domeny aplikacji załadowane zostały
                    // kolejne zestawy i dodanie ich do kolekcji.
                    foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        name = asm.GetName().Name;
                        if (_domainAssemblies.ContainsKey(name) == false)
                        {
                            _domainAssemblies.Add(name, asm);
                        }
                    }

                    key = _domainAssemblies.Keys.FirstOrDefault(item => item.ToLower() == assemblyName.ToLower());
                    if (key != null)
                    {
                        assemblyName = key;
                    }
                }
            }

            if (_domainAssemblies.ContainsKey(assemblyName))
            {
                return _domainAssemblies[assemblyName];
            }

            return null;
        }

        /// <summary>
        /// Zwraca informację czy wskazane assembly załadowane jest do domeny aplikacji.
        /// </summary>
        /// <param name="assemblyName">Nazwa assembly: System, System.Windows.Forms itd.</param>
        /// <returns>True, jeśli wskazane assembly załadowane jest do domeny aplikacji.</returns>
        public static bool AssemblyExists(string assemblyName)
        {
            return (GetAssembly(assemblyName) != null);
        }

        /// <summary>
        /// Zwraca zestaw tej biblioteki.
        /// </summary>
        /// <returns>Assembly tej biblioteki.</returns>
        public static Assembly GetExecutingAssembly()
        {
            return _executingAssembly;
        }

        #endregion
    }
}
