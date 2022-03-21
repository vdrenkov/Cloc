using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Cloc.Classes.Security;

namespace Cloc.Classes
{
    public static class Checker
    {
        static public bool AddCheck(User user)
        {
            user.UserUCN = EncryptString(user.UserUCN);
            string checkLine = user.UserUCN + ";" + user.CheckIn + ";" + user.CheckOut;

            try
            {
                if (File.Exists(".\\Checks.txt"))
                {
                    File.AppendAllText(".\\Checks.txt", checkLine + Environment.NewLine);
                }
                else
                {
                    File.Create(".\\Checks.txt").Close();
                }
                user.UserUCN = DecryptString(user.UserUCN);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка при чекиране.");
                return false;
            }
        }

        static public List<string> UserChecks(string ucn, int count)
        {
            List<string> checks = new List<string>();
            List<string> allChecks = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(".\\Checks.txt"))
                {
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        string[] results = line.Split(';', ';');
                        results[0] = DecryptString(results[0]);

                        if (results[0] == ucn)
                        {
                            string temp = results[0] + ";" + results[1] + ";" + results[2];
                            allChecks.Add(temp);
                        }
                        line = reader.ReadLine();
                    }
                }

                allChecks.Reverse();

                for (int i = 0; i < allChecks.Count; i++)
                {
                    checks.Add(allChecks[i]);
                    if (i == (count - 1))
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка при извличане на вашите чекирания.");
                return null;
            }

            return checks;
        }

        public static List<string> PrintChosenChecks(string ucn, int count)
        {
            List<string> list = new List<string>();
            List<string> printList = new List<string>();

            try
            {
                list = UserChecks(ucn, count);
                foreach (string check in list)
                {
                    string[] results = check.Split(';', ';');

                    string temp = "ЕГН: " + results[0] + "     Check-in: " + results[1] + "     Check-out: " + results[2];
                    printList.Add(temp);
                }
                return printList;
            }
            catch (Exception)
            { return null; }
        }
    }
}