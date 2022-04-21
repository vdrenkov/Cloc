using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Cloc.Classes
{
    internal class FileHelper
    {
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

        internal static int GetFileLines(string filePath)
        {
            int lines = 0;

            using (StreamReader reader = new(filePath))
            {
                var line = reader.ReadLine();

                while (line != null)
                {
                    lines++;
                    line = reader.ReadLine();
                }
            }
            return lines;
        }

        internal static List<string> FilterItems(string path, List<string> allItems, int count, bool isAll)
        {
            int itemsCount;
            List<string> items = new();

            if (isAll)
            {
                itemsCount = GetFileLines(path);
            }
            else
            { itemsCount = count; }

            for (int i = 0; i < allItems.Count; i++)
            {
                items.Add(allItems[i]);
                if (i == (itemsCount - 1))
                {
                    break;
                }
            }

            if (items.Count > 0)
            { return items; }
            else
            { return new List<string>(); }
        }
    }
}
