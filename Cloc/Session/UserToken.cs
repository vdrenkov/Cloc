using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Cloc.Session
{
    internal class UserToken
    {
        public static string GetData()
        {
            return ConfigurationManager.AppSettings.Get("UCN").ToString();
        }

        public static void SetData(string ucn)
        {
            ConfigurationManager.AppSettings.Set("UCN", ucn);
        }

        public static void RemoveData()
        {
            ConfigurationManager.AppSettings.Set("UCN","");
        }
    }
}
