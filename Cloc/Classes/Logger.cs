using System;
using System.Collections.Generic;
using System.IO;
using static Cloc.Classes.Security;

namespace Cloc.Classes
{
    internal static class Logger
    {
        static internal bool AddLog(string UCN, string activity)
        {
            UCN = EncryptString(UCN);
            string activityLine = DateTime.Now + ";" + UCN + ";" + activity;

            try
            {
                if (File.Exists(".\\Logs.txt"))
                {
                    File.AppendAllText(".\\Logs.txt", activityLine + Environment.NewLine);
                }
                else
                {
                    File.Create(".\\Logs.txt").Close();
                }
                UCN = DecryptString(UCN);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка при записване на вашата активност.");
                return false;
            }
        }

        static internal void RefreshLogs()
        {
            List<string> logs = new();

            try
            {
                using (StreamReader reader = new(".\\Logs.txt"))
                {
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        string[] results = line.Split(';', ';', ';');

                        if (DateTime.TryParse(results[0], out DateTime date) && date >= DateTime.Now.AddYears(-5))
                        {
                            logs.Add(line);
                        }
                        line = reader.ReadLine();
                    }
                }

                if (File.Exists(".\\Logs.txt"))
                {
                    File.Delete(".\\Logs.txt");
                    File.Create(".\\Logs.txt").Close();

                    foreach (string log in logs)
                    {
                        File.AppendAllText(".\\Logs.txt", log + Environment.NewLine);
                    }
                }
                else
                {
                    File.Create(".\\Logs.txt").Close();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка по време на работа.");
            }
        }

        static internal List<string> UserLogs(string UCN, int count)
        {
            List<string> logs = new();
            List<string> allLogs = new();

            try
            {
                using (StreamReader reader = new(".\\Logs.txt"))
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

                for (int i = 0; i < allLogs.Count; i++)
                {
                    logs.Add(allLogs[i]);
                    if (i == (count - 1))
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка при извличане на вашата активност.");
                return null;
            }

            return logs;
        }
    }
}