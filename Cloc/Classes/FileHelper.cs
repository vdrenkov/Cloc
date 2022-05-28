using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Cloc.Classes
{
    internal class FileHelper
    {
        internal static void FileCreator()
        {
            if (!File.Exists(".\\DBInfo.txt"))
            {
                File.Create(".\\DBInfo.txt");
            }

            if (!File.Exists(".\\ErrorLogs.txt"))
            {
                File.Create(".\\ErrorLogs.txt");
            }
        }

        internal static bool RefreshFile(string filePath, List<string> items)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    File.Create(filePath).Close();

                    foreach (string item in items)
                    {
                        File.AppendAllText(filePath, item + Environment.NewLine);
                    }
                }
                else
                {
                    File.Create(filePath).Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна грешка по време на работа.");
                ErrorLog.AddErrorLog(ex.ToString());
                return false;
            }
        }

        internal static List<string> ReadFileForRefresh(string filePath)
        {
            List<string> list = new();
            try
            {
                using (StreamReader reader = new(filePath))
                {
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        string[] results = line.Split(';', ';', ';');

                        if (DateTime.TryParse(results[0], out DateTime date))
                        {
                            if (date >= DateTime.Now.AddYears(-5))
                            {
                                list.Add(line);
                            }
                        }
                        line = reader.ReadLine();
                    }
                }
                if (list.Count > 0)
                {
                    return list;
                }
                else
                {
                    return new List<string>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка по време на работа.");
                ErrorLog.AddErrorLog(ex.ToString());
                return new List<string>();
            }
        }

        internal static bool WriteToFile(string filePath, string line)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.AppendAllText(filePath, line + Environment.NewLine);
                }
                else
                {
                    File.Create(filePath).Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка по време на работа.");
                ErrorLog.AddErrorLog(ex.ToString());
                return false;
            }
        }
    }
}
