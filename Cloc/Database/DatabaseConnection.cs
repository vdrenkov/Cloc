using MySql.Data.MySqlClient;
using System;
using static Cloc.Settings.SystemSetup;

namespace Cloc.Database
{
    public class DatabaseConnection
    {
        public string Server = GetServer();
        public string Username = GetUsername();
        public string Password = GetPassword();
        public string Port = GetPort();

        public MySqlConnection Connection { get; set; }

        public bool IsConnect()
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

        public void Close()
        {
            Connection.Close();
        }
    }
}
