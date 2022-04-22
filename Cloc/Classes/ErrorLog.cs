using System;
using System.Collections.Generic;

namespace Cloc.Classes
{
    internal class ErrorLog
    {
        private readonly static string path = ".\\ErrorLogs.txt";

        static internal bool RefreshErrorLogs()
        {
            List<string> errorLogs = FileHelper.ReadFileForRefresh(path);

            bool isSuccessful = FileHelper.RefreshFile(path, errorLogs);

            if (isSuccessful)
            { return true; }
            else { return false; }
        }

        static internal bool AddErrorLog(string error)
        {
            string errorLine = DateTime.Now + ";     -----     " + error;

            bool isSuccessful = FileHelper.WriteToFile(path, errorLine);

            if (isSuccessful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
