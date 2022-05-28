using Cloc.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Windows;
using static Cloc.Database.DatabaseInfo;

namespace Cloc.Database
{
    internal class DatabaseConnection
    {
        internal MySqlConnection Connection { get; set; }

        internal bool IsConnect()
        {
            bool flag = true;

            if (Connection == null)
            {
                if (!uint.TryParse(GetPort(), out uint port))
                {
                    port = 3306;
                }

                MySqlConnectionStringBuilder connstring = new()
                {
                    Server = GetServer(),
                    Port = port,
                    UserID = GetUserID(),
                    Password = GetPassword(),
                    Database = "ClocDB"
                };

                try
                {
                    Connection = new MySqlConnection(connstring.ToString());
                    Connection.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Възникна неочаквана грешка.");
                    ErrorLog.AddErrorLog(ex.ToString());
                    flag = false;
                }
            }

            return flag;
        }

        internal void Close()
        {
            Connection.Close();
        }
    }
}
