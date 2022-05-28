using System;
using System.Collections.Generic;
using System.Windows;
using static Cloc.Classes.Security;
using static Cloc.Database.SelectQuery;


namespace Cloc.Classes
{
    internal static class Checker
    {
        static internal List<string> UserChecks(string ucn, int count, bool isAll)
        {
            List<string> checks = new();
            List<string> allChecks = SelectAllChecksQuery(ucn);
            List<string> filteredChecks;

            try
            {
                foreach (string line in allChecks)
                {
                    string[] results = line.Split(';', ';');
                    results[0] = DecryptString(results[0]);

                    if (results[0] == ucn)
                    {
                        string temp = results[0] + ";" + results[1] + ";" + results[2];
                        checks.Add(temp);
                    }
                }

                checks.Reverse();

                if (!isAll)
                {
                    filteredChecks = Filter.FilterItems(checks, count);
                    return filteredChecks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при извличане на вашите чекирания.");
                ErrorLog.AddErrorLog(ex.ToString());
                return new List<string>();
            }

            return checks;
        }

        internal static List<string> PrintChosenChecks(string ucn, int count, bool isAll)
        {
            List<string> printList = new();

            try
            {
                List<string> list = UserChecks(ucn, count, isAll);
                foreach (string check in list)
                {
                    string[] results = check.Split(';', ';');

                    string temp = "ЕГН: " + results[0] + "     Чекиране: " + results[1] + "     Приключване: " + results[2];
                    printList.Add(temp);
                }

                if (printList.Count > 0)
                {
                    return printList;
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
    }
}