using Cloc.Classes;
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

namespace Cloc.Pages
{
    /// <summary>
    /// Interaction logic for ActivityPage.xaml
    /// </summary>
    public partial class ActivityPage : Page
    {
        static string ucn;
        int count = 15;
        public ActivityPage()
        {
            try
            {
                ucn = GetLoginData();

                FillChecks(ucn, count);
                FillLogs(ucn, count);
            }
            catch (Exception)
            { MessageBox.Show("Възникнa неочаквана грешка при зареждане на данните!"); }
            finally
            { InitializeComponent(); }
        }

        private void FillChecks(string ucn, int count)
        {
            List<string> checks = new List<string>();

            checks = Checker.PrintChosenChecks(ucn, 15);

            foreach (string check in checks)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ListBoxChecks.Items.Add(check);
                }));
            }
        }

        private void FillLogs(string ucn, int count)
        {
            List<string> logs = new List<string>();

            logs = Logger.UserLogs(ucn, 15);

            foreach (string log in logs)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ListBoxLogs.Items.Add(log);
                }));
            }
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxCount.SelectedItem != null)
            { count = Convert.ToInt32(ComboBoxCount.SelectedValue); }
            else
            { count = 15; }

            FillChecks(ucn, count);
            FillLogs(ucn, count);
        }
    }
}