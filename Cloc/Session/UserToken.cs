using System.Configuration;

namespace Cloc.Session
{
    internal class UserToken
    {
        internal static string GetLoginData()
        {
            return ConfigurationManager.AppSettings.Get("UCN").ToString();
        }

        internal static void SetLoginData(string ucn)
        {
            ConfigurationManager.AppSettings.Set("UCN", ucn);
        }

        internal static void RemoveLoginData()
        {
            ConfigurationManager.AppSettings.Set("UCN", "");
        }
    }
}
