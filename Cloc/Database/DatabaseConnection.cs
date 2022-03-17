using MySql.Data.MySqlClient;
using System;

namespace Cloc.Database
{
    public class DatabaseConnection
    {
        public string Server = GetServer();
        public string Username = GetUsername();
        public string Password = GetPassword();
        public string Port = GetPort();


        public MySqlConnection Connection { get; set; }

        private static DatabaseConnection _instance = null;
        public static DatabaseConnection Instance()
        {
            if (_instance == null)
                _instance = new DatabaseConnection();
            return _instance;
        }

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

        public static string GetServer()
        {
            return Settings.Settings.Default.Server.ToString();
        }

        public static string GetUsername()
        {
            return Settings.Settings.Default.Username.ToString();
        }

        public static string GetPassword()
        {
            return Settings.Settings.Default.Password.ToString();
        }
        public static string GetPort()
        {
            return Settings.Settings.Default.Port.ToString();
        }

        public static void SetSettings(string server, string username, string password,string port)
        {
            Settings.Settings.Default.Server = server;
            Settings.Settings.Default.Username = username;
            Settings.Settings.Default.Password = password;
            Settings.Settings.Default.Port = port;
            Settings.Settings.Default.Save();
        }
    }
}
