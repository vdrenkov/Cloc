using Cloc.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using static Cloc.Database.DatabaseQuery;
using static Cloc.Session.UserToken;

namespace Cloc.Pages
{
    /// <summary>
    /// Interaction logic for ActivityPage.xaml
    /// </summary>
    public partial class ActivityPage : Page
    {
        public const int COUNT = 30;
        static string ucn;
        internal static int count = COUNT;

        public ActivityPage()
        {
            try
            {
                ucn = GetLoginData();

                FillChecks(ucn, count);
                FillLogs(ucn, count);
            }

            catch (Exception)
            { MessageBox.Show("Възникнa неочаквана грешка при зареждане на данните."); }

            finally
            { InitializeComponent(); }
        }

        private void FillChecks(string ucn, int count)
        {
            List<string> checks = new();

            checks = Checker.PrintChosenChecks(ucn, count);

            if (ListBoxChecks != null)
            {
                if (int.TryParse(ListBoxChecks.Items.Count.ToString(), out count))
                {
                    if (count > 0)
                    {
                        ListBoxChecks.Items.Clear();
                    }
                }
            }

            Person person = SelectPersonQuery(ucn);
            if (!Validator.IsAdmin(person))
            {
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
            List<string> logs = new();

            logs = Logger.UserLogs(ucn, count);

            if (ListBoxLogs != null)
            {
                if (int.TryParse(ListBoxLogs.Items.Count.ToString(), out count))
                {
                    if (count > 0)
                    {
                        ListBoxLogs.Items.Clear();
                    }
                }
            }

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

        private void ComboBoxCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (int.TryParse((e.AddedItems[0] as ComboBoxItem).Content.ToString(), out count))
            {
                FillChecks(ucn, count);
                FillLogs(ucn, count);
            }
            else
            {
                ListBoxChecks.Items.Clear();
                ListBoxLogs.Items.Clear();
            }
        }

        private void ComboBoxUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string userInfo = ComboBoxUser.SelectedItem.ToString();
                string[] split = userInfo.Split(", ");
                ucn = split[1];

                FillChecks(ucn, count);
                FillLogs(ucn, count);

                if (GetLoginData() != split[1])
                { Logger.AddLog(GetLoginData(), "Преглед активността на профила на " + split[0] + "."); }
            }
            catch (Exception)
            {
                MessageBox.Show("Възникна неочаквана грешка при изпълнение на вашата заявка.");
            }
        }
    }
}