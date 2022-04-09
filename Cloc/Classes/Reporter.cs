using System;
using System.Collections.Generic;
using System.IO;
using static Cloc.Classes.Security;

namespace Cloc.Classes
{
    internal class Reporter
    {
        static public bool AddReport(string UCN, string names, double payment)
        {
            UCN = EncryptString(UCN);
            string activityLine = DateOnly.FromDateTime(DateTime.Now) + ";" + UCN + ";" + names + ";" + payment;

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

        static public List<string> AllUserReports(DateOnly dateFrom, DateOnly dateTo)
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

                        if (DateOnly.TryParse(results[0], out DateOnly date))
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

        static public List<string> UserReports(DateOnly dateFrom, DateOnly dateTo, string ucn)
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

                        if (DateOnly.TryParse(results[0], out DateOnly date))
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

        internal static double SumAllPaymentsForAChosenPeriod(DateOnly dateFrom, DateOnly dateTo)
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

                    if (DateOnly.TryParse(results[0], out DateOnly date))
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

        internal static double SumAllPaymentsPerPerson(DateOnly dateFrom, DateOnly dateTo, string ucn)
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

                    if (DateOnly.TryParse(results[0], out DateOnly date))
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
