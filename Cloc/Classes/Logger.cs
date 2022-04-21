using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using static Cloc.Classes.Security;

namespace Cloc.Classes
{
    internal static class Logger
    {
        private readonly static string path = ".\\Logs.txt";

        static internal bool RefreshLogs()
        {
            List<string> logs = FileHelper.ReadFileForRefresh(path);

            bool isSuccessful = FileHelper.RefreshFile(path, logs);

            if (isSuccessful)
            { return true; }
            else { return false; }
        }

        static internal bool AddLog(string ucn, string activity)
        {
            ucn = EncryptString(ucn);
            string activityLine = DateTime.Now + ";" + ucn + ";" + activity;

            bool isSuccessful = FileHelper.WriteToFile(path, activityLine);

            if (isSuccessful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static internal List<string> UserLogs(string UCN, int count, bool isAll)
        {
            List<string> logs;
            List<string> allLogs = new();

            try
            {
                using (StreamReader reader = new(path))
                {
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        string[] results = line.Split(';', ';');
                        results[1] = DecryptString(results[1]);

                        if (DateTime.TryParse(results[0], out DateTime date))
                        {
                            if (results[1] == UCN)
                            {
                                string temp = results[0] + "     ЕГН: " + results[1] + "     Действие: " + results[2];
                                allLogs.Add(temp);
                            }
                        }
                        line = reader.ReadLine();
                    }
                }

                allLogs.Reverse();

                logs = FileHelper.FilterItems(path, allLogs, count, isAll);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при извличане на вашата активност.");
                ErrorLog.AddErrorLog(ex.ToString());
                return null;
            }

            return logs;
        }
    }
}