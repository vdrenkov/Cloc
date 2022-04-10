using System;

namespace Cloc.Settings
{
    internal class SystemSetup
    {
        internal static string GetServer()
        {
            return Settings.Default.Server.ToString();
        }

        internal static string GetUsername()
        {
            return Settings.Default.Username.ToString();
        }

        internal static string GetPassword()
        {
            return Settings.Default.Password.ToString();
        }
        internal static string GetPort()
        {
            return Settings.Default.Port.ToString();
        }

        internal static bool SetSettings(string server, string username, string password, string port)
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
