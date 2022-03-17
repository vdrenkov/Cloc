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
    internal class Checker
    {
        static public void AddCheck(User user)
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
                    File.Create(".\\Checks.txt");
                }
            }
            catch (Exception)
            { MessageBox.Show("Възникна неочаквана грешка при чекиране."); }
        }
        static public List<string> UserChecks(User user, int count)
        {
            List<string> checks = new List<string>();
            List<string> allChecks = new List<string>();

            try
            {
                string[] lines = File.ReadAllLines(".\\Checks.txt");

                foreach (string line in lines)
                {
                    string[] results = line.Split(';', ';');
                    results[0] = DecryptString(results[0]);

                    if (results[0] == user.UserUCN)
                    {
                        string temp = results[0] + ";" + results[1] + ";" + results[2];
                        allChecks.Add(temp);
                    }
                }
                allChecks.Reverse();
            }
            catch (Exception)
            { MessageBox.Show("Възникна неочаквана грешка при чекиране."); }

            for (int i = 0; i < allChecks.Count; i++)
            {
                checks.Add(allChecks[i]);
                if (i == (count - 1))
                {
                    break;
                }
            }
            return checks;
        }
    }
}
