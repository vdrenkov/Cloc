using System;
using System.Collections.Generic;
using System.Windows;
using static Cloc.Classes.Security;

namespace Cloc.Classes
{
    internal static class Logger
    {
        static internal List<string> UserLogs(string UCN, int count, bool isAll)
        {
            List<string> logs = new();
            List<string> allLogs = new();
            List<string> filteredLogs;

            try
            {
                foreach (string line in allLogs)
                {
                    string[] results = line.Split(';', ';');
                    results[0] = DecryptString(results[0]);

                    if (results[0] == UCN)
                    {
                        string temp = results[0] + ";" + results[1] + ";" + results[2];
                        logs.Add(temp);
                    }
                }

                logs.Reverse();

                if (!isAll)
                {
                    filteredLogs = Filter.FilterItems(logs, count);
                    return filteredLogs;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при извличане на вашата активност.");
                ErrorLog.AddErrorLog(ex.ToString());
                return new List<string>();
            }

            return logs;
        }
    }
}