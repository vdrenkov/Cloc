using System;
using System.Collections.Generic;
using System.Windows;
using static Cloc.Classes.Security;
using static Cloc.Database.SelectQuery;


namespace Cloc.Classes
{
    internal static class Reporter
    {
        static internal List<string> UserReports(DateTime dateFrom, DateTime dateTo, string ucn, bool isAll)
        {
            List<string> reports = new();
            List<string> allReports = SelectAllReportsQuery(ucn);

            try
            {
                foreach (string line in allReports)
                {
                    string[] results = line.Split(';', ';', ';');
                    results[0] = DecryptString(results[0]);

                    if (DateTime.TryParse(results[3], out DateTime date))
                    {
                        if (isAll)
                        {
                            if (dateFrom <= date && dateTo >= date)
                            {
                                string temp = results[3] + "     ЕГН: " + results[0] + "     Име: " + results[1] + "     Сума: " + results[2] + " лева.";
                                reports.Add(temp);
                            }
                        }
                        else
                        {
                            if (dateFrom <= date && dateTo >= date && ucn == results[1])
                            {
                                string temp = results[3] + "     ЕГН: " + results[0] + "     Име: " + results[1] + "     Сума: " + results[2] + " лева.";
                                reports.Add(temp);
                            }
                        }
                    }
                }

                reports.Reverse();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при извличане на изплащанията.");
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
            List<string> allReports = SelectAllReportsQuery(ucn);

            try
            {
                foreach (string line in allReports)
                {
                    string[] results = line.Split(';', ';', ';');
                    results[1] = DecryptString(results[1]);

                    if (DateTime.TryParse(results[3], out DateTime date))
                    {
                        if (isAll)
                        {
                            if (dateFrom <= date && dateTo >= date)
                            {
                                if (double.TryParse(results[2], out double result))
                                {
                                    total += result;
                                }
                            }
                        }
                        else
                        {
                            if (dateFrom <= date && dateTo >= date && ucn == results[0])
                            {
                                if (double.TryParse(results[2], out double result))
                                {
                                    total += result;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при извличане на изплащанията.");
                ErrorLog.AddErrorLog(ex.ToString());
                return 0;
            }
            return total;
        }
    }
}