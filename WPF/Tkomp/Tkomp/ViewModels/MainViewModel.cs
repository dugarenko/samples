using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Caliburn.Micro;
using Tkomp.Interfaces;
using Tkomp.Models;
using Tkomp.Properties;

namespace Tkomp.ViewModels
{
    internal class MainViewModel : ViewAware
    {
        private bool _canLoadData;

        public void OnPasswordChanged(dynamic obj)
        {
            Password = obj.Password;
            CanLoadData = false;
        }

        /// <summary>
        /// Wykonuje test połączenia z bazą danych SQL.
        /// </summary>
        public void ConnectionTest()
        {
            string msg = "";
            var messageBox = (IMessageBox)GetView();
            SqlConnectionStringBuilder connectionStringBuilder = null;

            try
            {
                connectionStringBuilder = new SqlConnectionStringBuilder(string.Format(Settings.Default.ConnectionString, Login ?? "", Password ?? ""));
                using (SqlConnection cnn = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    cnn.Open();
                }

                msg = string.Format(Resources.MainViewModel_Msg_ConnectionSucces, connectionStringBuilder.DataSource);
                messageBox.Show(msg);
                CanLoadData = true;
            }
            catch (Exception ex)
            {
                msg = string.Format(Resources.MainViewModel_Msg_ConnectionSucces, connectionStringBuilder.DataSource) + "\r\n" + ex.Message;
                messageBox.Show(msg);
            }
        }

        /// <summary>
        /// Ładuje dane.
        /// </summary>
        public void LoadData()
        {
            try
            {
                var list = new List<ColumnInfo>();
                var connectionStringBuilder = new SqlConnectionStringBuilder(string.Format(Settings.Default.ConnectionString, Login ?? "", Password ?? ""));

                using (var cnn = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    cnn.Open();
                    using (var cmd = new SqlCommand(Resources.MSSQL_GetTablesColumnsV10, cnn))
                    {
                        cmd.Parameters.Add("@ObjectSchema1", SqlDbType.NVarChar).Value = "dbo";
                        cmd.Parameters.Add("@ObjectSchema2", SqlDbType.NVarChar).Value = "dbo";
                        cmd.Parameters.Add("@ObjectSchema3", SqlDbType.NVarChar).Value = "dbo";

                        cmd.Parameters.Add("@ObjectName1", SqlDbType.NVarChar).Value = "Table_A";
                        cmd.Parameters.Add("@ObjectName2", SqlDbType.NVarChar).Value = "Table_B";
                        cmd.Parameters.Add("@ObjectName3", SqlDbType.NVarChar).Value = "Table_C";

                        cmd.Parameters.Add("@SqlDbType1", SqlDbType.NVarChar).Value = "int";
                        cmd.Parameters.Add("@SqlDbType2", SqlDbType.NVarChar).Value = "int";
                        cmd.Parameters.Add("@SqlDbType3", SqlDbType.NVarChar).Value = "int";

                        cmd.Parameters.Add("@AllSchemas", SqlDbType.Bit).Value = 0;
                        cmd.Parameters.Add("@AllObjects", SqlDbType.Bit).Value = 0;
                        cmd.Parameters.Add("@AllSqlDbType", SqlDbType.Bit).Value = 0;

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var columnInfo = new ColumnInfo();
                                columnInfo.Fill(dr);
                                list.Add(columnInfo);
                            }
                        }
                    }
                }

                Data = list;
                NotifyOfPropertyChange(() => Data);
            }
            catch (Exception ex)
            {
                var messageBox = (IMessageBox)GetView();
                messageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Pobiera lub ustawia dane.
        /// </summary>
        public IEnumerable<ColumnInfo> Data
        { get; private set; }

        /// <summary>
        /// Pobiera lub ustawia login użytkownika.
        /// </summary>
        public string Login
        {
            get => Settings.Default.Login;
            set
            {
                if (Settings.Default.Login != value)
                {
                    Settings.Default.Login = value;
                    Settings.Default.Save();
                    CanLoadData = false;
                    NotifyOfPropertyChange();
                }
            }
        }

        /// <summary>
        /// Pobiera lub ustawia hasło użytkownika.
        /// </summary>
        private string Password
        { get; set; }

        /// <summary>
        /// Pobiera lub ustawia wartość, która określa czy można załadować dane z bazy danych SQL.
        /// </summary>
        public bool CanLoadData
        {
            get => _canLoadData;
            private set
            {
                if (_canLoadData != value)
                {
                    _canLoadData = value;
                    NotifyOfPropertyChange();
                }
            }
        }
    }
}
