using MySql.Data.MySqlClient;
using System;
using static Cloc.Settings.SystemSetup;

namespace Cloc.Database
{
    internal class DatabaseConnection
    {
        internal string Server = GetServer();
        internal string Username = GetUsername();
        internal string Password = GetPassword();
        internal string Port = GetPort();

        internal MySqlConnection Connection { get; set; }

        internal bool IsConnect()
        {
            bool flag = true;

            if (Connection == null)
            {
                string connstring = string.Format("Server={0}; Username={1}; Password={2}; Port={3}", Server, Username, Password, Port);
                try
                {
                    Connection = new MySqlConnection(connstring);
                    Connection.Open();
                }
                catch (Exception)
                {
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
