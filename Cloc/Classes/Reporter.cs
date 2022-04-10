using System;
using System.Collections.Generic;
using System.IO;
using static Cloc.Classes.Security;

namespace Cloc.Classes
{
    internal static class Reporter
    {
        static internal bool AddReport(string UCN, string names, double payment)
        {
            UCN = EncryptString(UCN);
            string activityLine = DateTime.Now + ";" + UCN + ";" + names + ";" + Math.Round(payment, 2);

            try
            {
                if (File.Exists(".\\Reports.txt"))
                {
                    File.AppendAllText(".\\Reports.txt", activityLine + Environment.NewLine);
                }
                else
                {
                    File.Create(".\\Reports.txt").Close();
                }
                UCN = DecryptString(UCN);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка при записване на изплащането.");
                return false;
            }
        }

        internal static void RefreshReports()
        {
            List<string> reports = new();

            try
            {
                using (StreamReader reader = new(".\\Reports.txt"))
                {
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        string[] results = line.Split(';', ';', ';');

                        if (DateTime.TryParse(results[0], out DateTime date) && date >= DateTime.Now.AddYears(-5))
                        {
                            reports.Add(line);
                        }
                        line = reader.ReadLine();
                    }
                }

                if (File.Exists(".\\Reports.txt"))
                {
                    File.Delete(".\\Reports.txt");
                    File.Create(".\\Reports.txt").Close();

                    foreach (string report in reports)
                    {
                        File.AppendAllText(".\\Reports.txt", report + Environment.NewLine);
                    }
                }
                else
                {
                    File.Create(".\\Reports.txt").Close();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка по време на работа.");
            }
        }

        static internal List<string> AllUserReports(DateTime dateFrom, DateTime dateTo)
        {
            List<string> reports = new();

            try
            {
                using (StreamReader reader = new(".\\Reports.txt"))
                {
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        string[] results = line.Split(';', ';', ';');
                        results[1] = DecryptString(results[1]);

                        if (DateTime.TryParse(results[0], out DateTime date))
                        {
                            if (dateFrom <= date && dateTo >= date)
                            {
                                string temp = results[0] + "     ЕГН: " + results[1] + "     Име: " + results[2] + "     Сума: " + results[3] + " лева.";
                                reports.Add(temp);
                            }
                        }
                        line = reader.ReadLine();
                    }
                }

                reports.Reverse();
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка при извличане на плащанията.");
                return null;
            }

            return reports;
        }

        static internal List<string> UserReports(DateTime dateFrom, DateTime dateTo, string ucn)
        {
            List<string> reports = new();

            try
            {
                using (StreamReader reader = new(".\\Reports.txt"))
                {
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        string[] results = line.Split(';', ';', ';');
                        results[1] = DecryptString(results[1]);

                        if (DateTime.TryParse(results[0], out DateTime date))
                        {
                            if (dateFrom <= date && dateTo >= date && ucn == results[1])
                            {
                                string temp = results[0] + "     ЕГН: " + results[1] + "     Име: " + results[2] + "     Сума: " + results[3] + " лева.";
                                reports.Add(temp);
                            }
                        }
                        line = reader.ReadLine();
                    }
                }

                reports.Reverse();
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка при извличане на плащанията.");
                return null;
            }

            return reports;
        }

        internal static double SumAllPaymentsForAChosenPeriod(DateTime dateFrom, DateTime dateTo)
        {
            double total = 0;
            try
            {
                using StreamReader reader = new(".\\Reports.txt");
                var line = reader.ReadLine();

                while (line != null)
                {
                    string[] results = line.Split(';', ';', ';');
                    results[1] = DecryptString(results[1]);

                    if (DateTime.TryParse(results[0], out DateTime date))
                    {
                        if (dateFrom <= date && dateTo >= date)
                        {
                            if (double.TryParse(results[3], out double result))
                            {
                                total += result;
                            }
                        }
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка при извличане на плащанията.");
                return 0;
            }
            return total;
        }

        internal static double SumAllPaymentsPerPerson(DateTime dateFrom, DateTime dateTo, string ucn)
        {
            double total = 0;
            try
            {
                using StreamReader reader = new(".\\Reports.txt");
                var line = reader.ReadLine();

                while (line != null)
                {
                    string[] results = line.Split(';', ';', ';');
                    results[1] = DecryptString(results[1]);

                    if (DateTime.TryParse(results[0], out DateTime date))
                    {
                        if (dateFrom <= date && dateTo >= date && ucn == results[1])
                        {
                            if (double.TryParse(results[3], out double result))
                            {
                                total += result;
                            }
                        }
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка при извличане на плащанията.");
                return 0;
            }
            return total;
        }
    }
}
