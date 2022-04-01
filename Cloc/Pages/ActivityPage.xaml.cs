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
        static int count = 15;
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

            checks = Checker.PrintChosenChecks(ucn, count);

            //if (ListBoxChecks.SelectedItem > 0)
            // { ListBoxChecks.Items.Clear(); }

            if (checks.Count > 0)
            {
                foreach (string check in checks)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ListBoxChecks.Items.Add(check);
                    }));
                }
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ListBoxChecks.Items.Add("Няма чекирания за показване...");
                }));
            }
        }

        private void FillLogs(string ucn, int count)
        {
            List<string> logs = new List<string>();

            logs = Logger.UserLogs(ucn, count);

            //   if (ListBoxLogs.Items.Count > 0)
            // { ListBoxLogs.Items.Clear(); }

            if (logs.Count > 0)
            {
                foreach (string log in logs)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ListBoxLogs.Items.Add(log);
                    }));
                }
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ListBoxLogs.Items.Add("Няма логове за показване...");
                }));
            }
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(this.ComboBoxCount.Text, out count))
            {
                FillChecks(ucn, count);
                FillLogs(ucn, count);
            }
        }
    }
}