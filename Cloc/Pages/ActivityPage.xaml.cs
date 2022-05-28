using Cloc.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using static Cloc.Database.SelectQuery;
using static Cloc.Session.UserToken;

namespace Cloc.Pages
{
    /// <summary>
    /// Interaction logic for ActivityPage.xaml
    /// </summary>
    public partial class ActivityPage : Page
    {
        internal const int COUNT = 100;
        internal static string ucn;
        internal static int count = COUNT;

        public ActivityPage()
        {
            try
            {
                ucn = GetLoginData();

                FillChecks(ucn, count, false);
                FillLogs(ucn, count, false);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Възникнa неочаквана грешка при зареждане на данните.");
                ErrorLog.AddErrorLog(ex.ToString());
            }

            finally
            { InitializeComponent(); }
        }

        private void FillChecks(string ucn, int count, bool isAll)
        {
            List<string> checks = new();

            checks = Checker.PrintChosenChecks(ucn, count, isAll);

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
                if (checks != null && checks.Count > 0)
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

        private void FillLogs(string ucn, int count, bool isAll)
        {
            List<string> logs = new();

            logs = Logger.UserLogs(ucn, count, isAll);

            if (ListBoxLogs != null && ListBoxLogs.Items != null)
            {
                if (int.TryParse(ListBoxLogs.Items.Count.ToString(), out count))
                {
                    if (count > 0)
                    {
                        ListBoxLogs.Items.Clear();
                    }
                }
            }

            if (logs != null && logs.Count > 0)
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
            if (ComboBoxCount.SelectedIndex == 5)
            {
                bool isAll = true;
                FillChecks(ucn, count, isAll);
                FillLogs(ucn, count, isAll);
            }
            else
            {
                if (int.TryParse((e.AddedItems[0] as ComboBoxItem).Content.ToString(), out count))
                {
                    bool isAll = false;
                    FillChecks(ucn, count, isAll);
                    FillLogs(ucn, count, isAll);
                }
                else
                {
                    ListBoxChecks.Items.Clear();
                    ListBoxLogs.Items.Clear();
                }
            }
        }

        private void ComboBoxUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ComboBoxUser != null && ComboBoxUser.SelectedItem != null)
                {
                    string userInfo = ComboBoxUser.SelectedItem.ToString();
                    string[] split = userInfo.Split(", ");
                    string name = split[0];
                    ucn = split[1];

                    bool isAll = false;
                    FillChecks(ucn, count, isAll);
                    FillLogs(ucn, count, isAll);

                    if (GetLoginData() != ucn)
                    {
                        if (!Cloc.Database.InsertQuery.AddLogQuery(GetLoginData(), "Преглед активността на профила на " + name + "."))
                        {
                            MessageBox.Show("Възникна грешка при записване на активността.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при изпълнение на вашата заявка.");
                ErrorLog.AddErrorLog(ex.ToString());
            }
        }
    }
}