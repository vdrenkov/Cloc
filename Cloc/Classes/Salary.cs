using System;
using System.Collections.Generic;
using System.Windows;
using static Cloc.Classes.Checker;
using static Cloc.Database.DatabaseQuery;

namespace Cloc.Classes
{
    internal static class Salary
    {
        const int MonthWorkHours = 176;
        const int MaxWorkShifts = 255;

        internal static double TotalHours(User user)
        {
            bool isAll = false;
            double totalHours = 0;
            DateTime monthAgo = DateTime.Now.AddDays(-30);
            List<string> monthChecks = UserChecks(user.UserUCN, MaxWorkShifts, isAll);

            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна грешка по време на работа.");
                ErrorLog.AddErrorLog(ex.ToString());
                return 0;
            }

            return totalHours;
        }

        internal static bool IsOvertime(User user)
        {
            double totalHours = TotalHours(user);

            if (totalHours > MonthWorkHours)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static double CheckSalary(string ucn)
        {
            double salary = 0, overtime;

            try
            {
                User user = SelectUserQuery(ucn);
                bool isOvertime = IsOvertime(user);

                if (isOvertime && user.TotalHours > MonthWorkHours)
                {
                    overtime = (user.TotalHours - MonthWorkHours) * 1.5;

                    if (overtime > 0)
                    { salary = user.TotalHours * user.HourPayment + overtime; }
                }
                else
                {
                    salary = user.TotalHours * user.HourPayment;
                }
                salary += (salary * user.Percent) / 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при пресмятане на вашата заплата.");
                ErrorLog.AddErrorLog(ex.ToString());
            }

            return salary;
        }
    }
}