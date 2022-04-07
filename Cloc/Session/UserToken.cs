using System.Configuration;

namespace Cloc.Session
{
    internal class UserToken
    {
        public static string GetLoginData()
        {
            return ConfigurationManager.AppSettings.Get("UCN").ToString();
        }

        public static void SetLoginData(string ucn)
        {
            ConfigurationManager.AppSettings.Set("UCN", ucn);
        }

        public static void RemoveLoginData()
        {
            ConfigurationManager.AppSettings.Set("UCN", "");
        }
    }
}
