using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using static Cloc.Classes.Security;

namespace Cloc.Classes
{
    internal static class Reporter
    {
        private readonly static string path = ".\\Reports.txt";

        internal static bool RefreshReports()
        {
            List<string> reports = FileHelper.ReadFileForRefresh(path);

            bool isSuccessful = FileHelper.RefreshFile(path, reports);

            if (isSuccessful)
            { return true; }
            else { return false; }
        }

        static internal bool AddReport(string ucn, string names, double payment)
        {
            ucn = EncryptString(ucn);
            string activityLine = DateTime.Now + ";" + ucn + ";" + names + ";" + Math.Round(payment, 2);

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

        static internal List<string> UserReports(DateTime dateFrom, DateTime dateTo, string ucn, bool isAll)
        {
            List<string> reports = new();

            try
            {
                using (StreamReader reader = new(path))
                {
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        string[] results = line.Split(';', ';', ';');
                        results[1] = DecryptString(results[1]);

                        if (DateTime.TryParse(results[0], out DateTime date))
                        {
                            if (isAll)
                            {
                                if (dateFrom <= date && dateTo >= date)
                                {
                                    string temp = results[0] + "     ЕГН: " + results[1] + "     Име: " + results[2] + "     Сума: " + results[3] + " лева.";
                                    reports.Add(temp);
                                }
                            }
                            else
                            {
                                if (dateFrom <= date && dateTo >= date && ucn == results[1])
                                {
                                    string temp = results[0] + "     ЕГН: " + results[1] + "     Име: " + results[2] + "     Сума: " + results[3] + " лева.";
                                    reports.Add(temp);
                                }
                            }
                        }
                        line = reader.ReadLine();
                    }
                }

                reports.Reverse();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при извличане на плащанията.");
                ErrorLog.AddErrorLog(ex.ToString());
                return new List<string>();
            }

            if (reports.Count > 0)
            {
                return reports;
            }
            else
            {
                return new List<string>();
            }
        }

        internal static double SumAllPayments(DateTime dateFrom, DateTime dateTo, string ucn, bool isAll)
        {
            double total = 0;
            try
            {
                using StreamReader reader = new(path);
                var line = reader.ReadLine();

                while (line != null)
                {
                    string[] results = line.Split(';', ';', ';');
                    results[1] = DecryptString(results[1]);

                    if (DateTime.TryParse(results[0], out DateTime date))
                    {
                        if (isAll)
                        {
                            if (dateFrom <= date && dateTo >= date)
                            {
                                if (double.TryParse(results[3], out double result))
                                {
                                    total += result;
                                }
                            }
                        }
                        else
                        {
                            if (dateFrom <= date && dateTo >= date && ucn == results[1])
                            {
                                if (double.TryParse(results[3], out double result))
                                {
                                    total += result;
                                }
                            }
                        }
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при извличане на плащанията.");
                ErrorLog.AddErrorLog(ex.ToString());
                return 0;
            }
            return total;
        }
    }
}