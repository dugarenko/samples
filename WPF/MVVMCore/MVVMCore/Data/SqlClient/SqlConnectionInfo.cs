using System.Data.SqlClient;

namespace MVVMCore.Data.SqlClient
{
    /// <summary>
    /// Reprezentuje informacje o połączeniu do serwera SQL.
    /// </summary>
    public class SqlConnectionInfo
    {
        /// <summary>
        /// Inicjuje nową instancję klasy.
        /// </summary>
        /// <param name="connectionStringBuilder">Budowniczy ciągu połączenia do serwera SQL.</param>
        /// <param name="commandTimeout">Czas oczekiwania na wykonanie polecenia, którego przekroczenie spowoduje wygenerowanie błędu.</param>
        /// <param name="serverVersion">Wersja serwera SQL.</param>
        public SqlConnectionInfo(SqlConnectionStringBuilder connectionStringBuilder, int commandTimeout, string serverVersion)
        {
            ConnectionStringBuilder = connectionStringBuilder;
            CommandTimeout = commandTimeout;
            ServerVersion = serverVersion;
        }

        /// <summary>
        /// Pobiera budowniczego ciągu połączenia do serwera SQL.
        /// </summary>
        public SqlConnectionStringBuilder ConnectionStringBuilder
        { get; private set; }

        /// <summary>
        /// Pobiera ciąg połączenia do serwera SQL.
        /// </summary>
        public string ConnectionString
        {
            get => ConnectionStringBuilder.ConnectionString;
        }

        /// <summary>
        /// Pobiera czas oczekiwania na wykonanie polecenia, którego przekroczenie spowoduje wygenerowanie błędu.
        /// </summary>
        public int CommandTimeout
        { get; private set; }

        /// <summary>
        /// Pobiera wersję serwera SQL.
        /// </summary>
        public string ServerVersion
        { get; private set; }
    }
}
