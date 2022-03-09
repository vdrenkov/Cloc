using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloc.Classes
{
    internal class Checker
    {
        static public void AddCheck(User user)
        {
            string checkLine = user.UserUCN + ";" + user.CheckIn + ";" + user.CheckOut;

            if (File.Exists(".\\Checks.txt"))
            {
                File.AppendAllText(".\\Checks.txt", checkLine + Environment.NewLine);
            }
            else
            {
                File.Create(".\\Checks.txt");
            }
        }
        static public List<string> UserChecks(User user, int count)
        {
            List<string> checks = new List<string>();
            List<string> allChecks = new List<string>();
            string[] lines = File.ReadAllLines(".\\Checks.txt");

            foreach (string line in lines)
            {
                string[] results = line.Split(';', ';');

                if (results[0] == user.UserUCN)
                {
                    string temp = results[0] + ";" + results[1] + ";" + results[2];
                    allChecks.Add(temp);
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
            return checks;
        }

        public static bool HasOvertime(User user)
        {
            double totalHours = 0;
            List<string> checks = UserChecks(user, 30);

            foreach (string check in checks)
            {
                string[] results = check.Split(';', ';');
                DateTime checkIn = DateTime.Parse(results[1]);
                DateTime checkOut = DateTime.Parse(results[2]);
                totalHours += (checkOut - checkIn).TotalHours;
            }

            if (totalHours > 176)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
