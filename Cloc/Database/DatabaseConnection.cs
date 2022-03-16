using MySql.Data.MySqlClient;
using System;

namespace Cloc.Database
{
    public class DatabaseConnection
    {
        public string Server = "localhost";
        public string DatabaseName = "ClocDB";
        public string UserName = "root";
        public string Password = "348_sha765_KaD3l";

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
                if (String.IsNullOrEmpty(DatabaseName))
                    flag= false;
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
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
