using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Cloc.Session.UserToken;
using static Cloc.Database.DatabaseQuery;
using Cloc.Classes;

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
            { MessageBox.Show("Възникна неочаквана грешка при зареждане на данните!"); }

            finally
            { InitializeComponent(); }
        }

        private void CheckIn_Click(object sender, RoutedEventArgs e)
        {
            if (user.IsCheckedIn == false)
            {
                user.CheckIn = DateTime.Now;
                user.IsCheckedIn = true;

                MessageBox.Show(CheckInQuery(user).ToString());
                MessageBox.Show(Logger.AddLog(user.UserUCN, "Check - in.").ToString());
            }
            else
            {
                MessageBox.Show("Вече сте се чекирали!");
            }
        }

        private void CheckOut_Click(object sender, RoutedEventArgs e)
        {
            if (user.IsCheckedIn == true)
            {
                user.CheckOut = DateTime.Now;
                user.IsCheckedIn = false;

                MessageBox.Show(CheckOutQuery(user).ToString());
                MessageBox.Show(ChangeTotalHoursQuery(user).ToString());

                MessageBox.Show(Logger.AddLog(user.UserUCN, "Check - out.").ToString());
                MessageBox.Show(Checker.AddCheck(user).ToString());
            }
            else
            {
                MessageBox.Show("Не сте се чекирали!");
            }
        }

        private void CheckSalary_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Salary.CheckSalary(ucn).ToString());
            Logger.AddLog(user.UserUCN, "Проверка на текуща сума за изплащане.");
        }
    }
}
