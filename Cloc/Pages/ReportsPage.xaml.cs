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

                if (ComboBoxFilter != null && ComboBoxFilter.SelectedItem != null && TextBoxSum != null)
                {
                    if (ComboBoxFilter.SelectedIndex != 0)
                    {
                        string userInfo = ComboBoxFilter.SelectedItem.ToString();
                        string[] split = userInfo.Split(", ");
                        string ucn = split[1];
                        sum = SumAllPaymentsPerPerson(dateFrom, dateTo, ucn);

                        if (ListBoxPayments != null)
                        {
                            ListBoxPayments.Items.Clear();

                            List<string> UserReports = Reporter.UserReports(dateFrom, dateTo, ucn);

                            if (UserReports != null && UserReports.Count != 0)
                            {
                                TextBoxSum.Text = sum.ToString();

                                foreach (string UserReport in UserReports)
                                {
                                    ListBoxPayments.Items.Add(UserReport);
                                }
                            }
                            else
                            {
                                TextBoxSum.Text = "0";
                                ListBoxPayments.Items.Add("Няма изплащания за показване...");
                            }
                            if (Session.UserToken.GetLoginData() != split[1])
                            { Logger.AddLog(Session.UserToken.GetLoginData(), "Преглед изплащанията на потребител " + split[0] + "."); }
                        }
                    }
                    else
                    {
                        sum = SumAllPaymentsForAChosenPeriod(dateFrom, dateTo);

                        if (ListBoxPayments != null)
                        {
                            ListBoxPayments.Items.Clear();

                            List<string> UserReports = AllUserReports(dateFrom, dateTo);

                            if (UserReports != null && UserReports.Count != 0)
                            {
                                TextBoxSum.Text = sum.ToString();

                                foreach (string UserReport in UserReports)
                                {
                                    ListBoxPayments.Items.Add(UserReport);
                                }
                            }
                            else
                            {
                                TextBoxSum.Text = "0";
                                ListBoxPayments.Items.Add("Няма изплащания за показване...");
                            }
                            Logger.AddLog(Session.UserToken.GetLoginData(), "Преглед изплащанията на всички потребители.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Възникна неочаквана грешка при изпълнение на вашата заявка.");
            }
        }
    }
}
