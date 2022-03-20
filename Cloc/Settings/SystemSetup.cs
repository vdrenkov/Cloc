using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloc.Settings
{
    internal class SystemSetup
    {
        public static string GetServer()
        {
            return Settings.Default.Server.ToString();
        }

        public static string GetUsername()
        {
            return Settings.Default.Username.ToString();
        }

        public static string GetPassword()
        {
            return Settings.Default.Password.ToString();
        }
        public static string GetPort()
        {
            return Settings.Default.Port.ToString();
        }

        public static bool SetSettings(string server, string username, string password, string port)
        {
            try
            {
                Settings.Default.Server = server;
                Settings.Default.Username = username;
                Settings.Default.Password = password;
                Settings.Default.Port = port;
                Settings.Default.Save();
                Settings.Default.Reload();
                return true;
            }
            catch (Exception)
            { return false; }
        }
    }
}
