﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVVMCore.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MVVMCore.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument nie może przyjmować wartości pustej..
        /// </summary>
        internal static string ArgumentEmptyException {
            get {
                return ResourceManager.GetString("ArgumentEmptyException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to W międzyczasie ustawienia zostały zmienione przez innego użytkownika. Czy chcesz kontynuować zapis i nadpisać dane wprowadzone przez innego użytkownika?
        ///
        ///Kliknij TAK i nadpisz dane lub kliknij NIE, wówczas dane na formularzu zostaną przeładowane..
        /// </summary>
        internal static string Msg_Warning_DataConcurrency {
            get {
                return ResourceManager.GetString("Msg_Warning_DataConcurrency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Obiekt nie posiada metody: {0}..
        /// </summary>
        internal static string ObjectNotHaveMethod {
            get {
                return ResourceManager.GetString("ObjectNotHaveMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Obiekt nie posiada właściwości: {0}..
        /// </summary>
        internal static string ObjectNotHaveProperty {
            get {
                return ResourceManager.GetString("ObjectNotHaveProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nie można wczytać wartości ponieważ właściwość: {0} jest tylko do odczytu..
        /// </summary>
        internal static string PropertyIsReadOnly {
            get {
                return ResourceManager.GetString("PropertyIsReadOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nie można odczytać właściwości {0} ponieważ jest ona tylko do zapisu..
        /// </summary>
        internal static string PropertyIsWriteOnly {
            get {
                return ResourceManager.GetString("PropertyIsWriteOnly", resourceCulture);
            }
        }
    }
}