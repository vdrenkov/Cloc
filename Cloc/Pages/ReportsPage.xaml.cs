using Cloc.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using static Cloc.Classes.Reporter;

namespace Cloc.Pages
{
    /// <summary>
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (DateFrom != null)
                { DateFrom.SelectedDate = DateTime.Today.AddMonths(-1); }
                if (DateTo != null)
                { DateTo.SelectedDate = DateTime.Today.AddDays(1); }
            }));

            InitializeComponent();
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dateFrom = DateTime.MinValue;
            DateTime dateTo = DateTime.MaxValue;
            double sum;

            try
            {
                if (DateFrom != null && DateFrom.SelectedDate != null)
                {
                    dateFrom = (DateTime)DateFrom.SelectedDate;
                }

                if (DateTo != null && DateTo.SelectedDate != null)
                {
                    dateTo = (DateTime)DateTo.SelectedDate;
                }

                if (ComboBoxFilter != null && ComboBoxFilter.SelectedItem != null && TextBlockSum != null)
                {
                    if (ComboBoxFilter.SelectedIndex != 0)
                    {
                        string userInfo = ComboBoxFilter.SelectedItem.ToString();
                        string[] split = userInfo.Split(", ");
                        string name = split[0];
                        string ucn = split[1];
                        bool isAll = false;
                        sum = Math.Round(SumAllPayments(dateFrom, dateTo, ucn, isAll), 2);

                        if (ListBoxPayments != null)
                        {
                            ListBoxPayments.Items.Clear();

                            List<string> UserReports = Reporter.UserReports(dateFrom, dateTo, ucn, false);

                            if (UserReports != null && UserReports.Count != 0)
                            {
                                TextBlockSum.Text = sum.ToString() + " лв.";

                                foreach (string UserReport in UserReports)
                                {
                                    ListBoxPayments.Items.Add(UserReport);
                                }
                            }
                            else
                            {
                                TextBlockSum.Text = "0 лв.";
                                ListBoxPayments.Items.Add("Няма изплащания за показване...");
                            }
                            if (Session.UserToken.GetLoginData() != ucn)
                            {
                                if (!Cloc.Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Преглед изплащанията на потребител " + name + "."))
                                {
                                    MessageBox.Show("Възникна грешка при записване на активността.");
                                }
                            }
                        }
                    }
                    else
                    {
                        bool isAll = true;
                        sum = Math.Round(SumAllPayments(dateFrom, dateTo, string.Empty, isAll), 2);

                        if (ListBoxPayments != null)
                        {
                            ListBoxPayments.Items.Clear();

                            List<string> userReports = UserReports(dateFrom, dateTo, string.Empty, true);

                            if (userReports != null && userReports.Count != 0)
                            {
                                TextBlockSum.Text = sum.ToString() + " лв.";

                                foreach (string userReport in userReports)
                                {
                                    ListBoxPayments.Items.Add(userReport);
                                }
                            }
                            else
                            {
                                TextBlockSum.Text = "0 лв.";
                                ListBoxPayments.Items.Add("Няма изплащания за показване...");
                            }
                            if (!Cloc.Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Преглед изплащанията на всички потребители."))
                            {
                                MessageBox.Show("Възникна грешка при записване на активността.");
                            }
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