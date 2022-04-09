using Cloc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Cloc.Pages
{
    /// <summary>
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            InitializeComponent();
        }

        private static void ChangeData()
        {

        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string userInfo = ComboBoxFilter.SelectedItem.ToString();
                string[] split = userInfo.Split(", ");
               string ucn = split[1];

                MessageBox.Show(ucn);

                if (Session.UserToken.GetLoginData() != split[1])
                { Logger.AddLog(Session.UserToken.GetLoginData(), "Преглед активността на профила на " + split[0] + "."); }
            }
            catch (Exception)
            {
                MessageBox.Show("Възникна неочаквана грешка при изпълнение на вашата заявка.");
            }
        }
    }
}
