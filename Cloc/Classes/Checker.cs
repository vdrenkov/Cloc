using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using static Cloc.Classes.Security;

namespace Cloc.Classes
{
    internal static class Checker
    {
        private readonly static string path = ".\\Checks.txt";

        static internal bool RefreshChecks()
        {
            List<string> checks = FileHelper.ReadFileForRefresh(path);

            bool isSuccessful = FileHelper.RefreshFile(path, checks);

            if (isSuccessful)
            { return true; }
            else { return false; }
        }

        static internal bool AddCheck(User user)
        {
            user.UserUCN = EncryptString(user.UserUCN);
            string checkLine = user.CheckIn + ";" + user.CheckOut + ";" + user.UserUCN;

            bool isSuccessful = FileHelper.WriteToFile(path, checkLine);
            user.UserUCN = DecryptString(user.UserUCN);

            if (isSuccessful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static internal List<string> UserChecks(string ucn, int count, bool isAll)
        {
            List<string> checks;
            List<string> allChecks = new();

            try
            {
                using (StreamReader reader = new(path))
                {
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        string[] results = line.Split(';', ';');
                        results[2] = DecryptString(results[2]);

                        if (results[2] == ucn)
                        {
                            string temp = results[2] + ";" + results[1] + ";" + results[0];
                            allChecks.Add(temp);
                        }
                        line = reader.ReadLine();
                    }
                }

                allChecks.Reverse();

                checks = FileHelper.FilterItems(path, allChecks, count, isAll);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при извличане на вашите чекирания.");
                ErrorLog.AddErrorLog(ex.ToString());
                return null;
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

                    string temp = "ЕГН: " + results[2] + "     Check-in: " + results[1] + "     Check-out: " + results[0];
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