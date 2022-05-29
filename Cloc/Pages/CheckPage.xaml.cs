using Cloc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using static Cloc.Database.SelectQuery;
using static Cloc.Database.UpdateQuery;
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

            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при зареждане на данните.");
                ErrorLog.AddErrorLog(ex.ToString());
            }

            finally
            { InitializeComponent(); }
        }

        private void CheckIn_Click(object sender, RoutedEventArgs e)
        {
            if (user.IsCheckedIn == false)
            {
                user.CheckIn = DateTime.Now;
                user.IsCheckedIn = true;

                if (CheckInQuery(user))
                {
                    MessageBox.Show("Успешно се чекирахте.");
                    if (!Database.InsertQuery.AddLogQuery(user.UserUCN, "Check - in."))
                    {
                        MessageBox.Show("Възникна грешка при записване на активността.");
                    }
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

                if (CheckOutQuery(user) && ChangeTotalHoursQuery(user))
                {
                    MessageBox.Show("Готово :)");
                    if ((!Database.InsertQuery.AddLogQuery(user.UserUCN, "Check - out.")) || (!Database.InsertQuery.AddCheckQuery(user)))
                    {
                        MessageBox.Show("Възникна грешка при записване на активността.");
                    }
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
            if (!Database.InsertQuery.AddLogQuery(user.UserUCN, "Проверка на текуща сума за изплащане."))
            {
                MessageBox.Show("Възникна грешка при записване на активността.");
            }
        }
    }
}
