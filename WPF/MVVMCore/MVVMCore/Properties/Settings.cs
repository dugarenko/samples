using MVVMCore.Data.SqlClient;
using MVVMCore.Windows;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace MVVMCore.Properties
{
    internal sealed partial class Settings
    {
        private static ClientSettingsSection _clientSettingsSection = null;

        /// <summary>
        /// Statyczny konstruktor.
        /// </summary>
        static Settings()
        {
            if (!ApplicationEx.DesignMode())
            {
                UpdateSettings();
            }
        }

        #region Private methods.

        /// <summary>
        /// Aktualizuje ustawienia.
        /// </summary>
        private static void UpdateSettings()
        {
            if (Settings.Default.IsFirstRunUpdatedApp)
            {
                Settings.Default.Upgrade();
                Settings.Default.IsFirstRunUpdatedApp = false;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// Zwraca sekcję konfiguracyjną użytkownika.
        /// </summary>
        /// <returns></returns>
        private static ClientSettingsSection GetClientSettingsSection()
        {
            if (_clientSettingsSection != null)
            {
                return _clientSettingsSection;
            }

            Assembly asm = typeof(Settings).Assembly;
            string asmName = asm.GetName().Name;
            string configPath = Path.GetFileName(asm.Location);

            _clientSettingsSection = (ClientSettingsSection)ConfigurationManager.OpenExeConfiguration(configPath)
                                                                                                                    .GetSectionGroup("userSettings")
                                                                                                                    .Sections[asmName + ".Properties.Settings"];
            return _clientSettingsSection;
        }

        #endregion

        #region Internal properties.

        /// <summary>
        /// Pobiera lub ustawia informacje o połączeniu do serwera SQL.
        /// </summary>
        internal SqlConnectionInfo ConnectionInfo
        { get; set; }

        #endregion
    }
}
