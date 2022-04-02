using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cloc.Database.DatabaseQuery;
using static Cloc.Classes.Checker;
using System.Windows;

namespace Cloc.Classes
{
    internal class Salary
    {
        const int MonthWorkHours = 176;
        const int MaxWorkShiftsForAMonth = 62;

        public static bool HasOvertime(User user)
        {
            double totalHours = 0;
            DateTime monthAgo = DateTime.Now.AddDays(-30);

            try
            {
                List<string> monthChecks = UserChecks(user.UserUCN, MaxWorkShiftsForAMonth);

                foreach (string item in monthChecks)
                {
                    string[] results = item.Split(';', ';');

                    DateTime checkIn = DateTime.Parse(results[1]);

                    DateTime checkOut = DateTime.Parse(results[2]);

                    totalHours += (checkOut - checkIn).TotalHours;

                    if (checkOut.Month == monthAgo.Month && checkOut.Day == monthAgo.Day)
                    {
                        break;
                    }
                }

                if (totalHours > MonthWorkHours)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Възникна неочаквана грешка при пресмятане на вашата заплата.");
                return false;
            }
        }

        public static double CheckSalary(string UCN)
        {
            double salary = 0, overtime;

            try
            {
                User user = SelectUserQuery(UCN);

                if (HasOvertime(user) && user.TotalHours > MonthWorkHours)
                {
                    overtime = (user.TotalHours - MonthWorkHours) * 1.5;
                    salary = user.TotalHours * user.HourPayment + overtime;
                }
                else
                {
                    salary = user.TotalHours * user.HourPayment;
                }
                salary += (salary * user.Percent) / 100;
            }
            catch (Exception)
            { Console.WriteLine("Възникна неочаквана грешка при пресмятане на вашата заплата."); }

            return salary;
        }
    }
}