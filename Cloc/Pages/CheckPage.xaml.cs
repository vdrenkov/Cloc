using Cloc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using static Cloc.Database.DatabaseQuery;
using static Cloc.Session.UserToken;

namespace Cloc.Pages
{
    /// <summary>
    /// Interaction logic for CheckPage.xaml
    /// </summary>
    public partial class CheckPage : Page
    {
        static string ucn;
        readonly User user = new();

        public CheckPage()
        {
            try
            {
                ucn = GetLoginData();
                user = SelectUserQuery(ucn);
            }

            catch (Exception)
            { MessageBox.Show("Възникна неочаквана грешка при зареждане на данните."); }

            finally
            { InitializeComponent(); }
        }

        private void CheckIn_Click(object sender, RoutedEventArgs e)
        {
            if (user.IsCheckedIn == false)
            {
                user.CheckIn = DateTime.Now;
                user.IsCheckedIn = true;

                if (CheckInQuery(user) && Logger.AddLog(user.UserUCN, "Check - in."))
                {
                    MessageBox.Show("Успешно се чекирахте.");
                }
                else
                {
                    MessageBox.Show("Възникна грешка при чекиране, моля, опитайте отново!");
                }
            }
            else
            {
                MessageBox.Show("Вече сте се чекирали.");
            }
        }

        private void CheckOut_Click(object sender, RoutedEventArgs e)
        {
            if (user.IsCheckedIn == true)
            {
                user.CheckOut = DateTime.Now;
                user.IsCheckedIn = false;

                if (CheckOutQuery(user) && ChangeTotalHoursQuery(user) && Logger.AddLog(user.UserUCN, "Check - out.") && Checker.AddCheck(user))
                {
                    MessageBox.Show("Готово :)");
                }
                else
                {
                    MessageBox.Show("Възникна грешка при чекиране, моля, опитайте отново!");
                }
            }
            else
            {
                MessageBox.Show("Не сте се чекирали.");
            }
        }

        private void CheckSalary_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Текуща сума за получаване: " + Math.Round(Salary.CheckSalary(ucn), 2).ToString() + " лева.");
            Logger.AddLog(user.UserUCN, "Проверка на текуща сума за изплащане.");
        }
    }
}
