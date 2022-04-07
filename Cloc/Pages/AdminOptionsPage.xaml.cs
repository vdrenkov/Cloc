using Cloc.AdditionalWindows;
using Cloc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using static Cloc.Database.DatabaseQuery;

namespace Cloc.Pages
{
    /// <summary>
    /// Interaction logic for BossOptionsPage.xaml
    /// </summary>
    public partial class AdminOptionsPage : Page
    {
        public AdminOptionsPage()
        {
            InitializeComponent();
        }

        private bool DeleteUser()
        {
            bool flag = false;

            if (ComboBoxUsers != null && ComboBoxUsers.SelectedItem != null)
            {
                string userInfo = ComboBoxUsers.SelectedItem.ToString();
                string[] split = userInfo.Split(", ");

                if (Session.UserToken.GetLoginData() != split[1])
                {
                    if (Database.DatabaseQuery.DeleteWorkerQuery(split[1]))
                    {
                        flag = true;

                        Logger.AddLog(Session.UserToken.GetLoginData(), "Изтриване профила на " + split[0] + ".");
                    }
                }
                else
                {
                    MessageBox.Show("Не може да изтриете администраторския акаунт!");
                }
            }
            else
            {
                MessageBox.Show("Не сте избрали потребител.");
            }

            return flag;
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            SetupWindow sw = new();

            sw.LabelServer.Visibility = Visibility.Hidden;
            sw.LabelUser.Visibility = Visibility.Hidden;
            sw.LabelPassword.Visibility = Visibility.Hidden;
            sw.LabelPort.Visibility = Visibility.Hidden;
            sw.LabelHourPayment.Visibility = Visibility.Visible;
            sw.LabelPercent.Visibility = Visibility.Visible;

            sw.TextBoxServer.Visibility = Visibility.Hidden;
            sw.TextBoxUser.Visibility = Visibility.Hidden;
            sw.PasswordBoxDBPassword.Visibility = Visibility.Hidden;
            sw.TextBoxPort.Visibility = Visibility.Hidden;
            sw.TextBoxHourPayment.Visibility = Visibility.Visible;
            sw.TextBoxPercent.Visibility = Visibility.Visible;

            sw.ButtonAddUser.Visibility = Visibility.Visible;
            sw.ButtonSetup.Visibility = Visibility.Hidden;
            sw.ButtonAddUser.IsDefault = true;

            if (sw.ComboBoxPosition != null)
            {
                sw.ComboBoxPosition.Visibility = Visibility.Visible;
                foreach (WorkPosition value in Enum.GetValues(typeof(WorkPosition)))
                {
                    string position = Person.TranslateFromWorkPosition(value);
                    sw.ComboBoxPosition.Items.Add(position);
                }
            }

            sw.Show();
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteUser())
            {
                MessageBox.Show("Избраният потребител беше изтрит успешно.");
                NavigationService.Refresh();
                //  ComboBoxUsers.Items.Remove(sender);
            }
        }

        private void ChangeDataButton_Click(object sender, RoutedEventArgs e)
        {
            int choice = ComboBoxChange.SelectedIndex;
            string change = TextBoxChange.Text;
            string ucn = Session.UserToken.GetLoginData();//change

            switch (choice)
            {
                case 0:
                    MessageBox.Show(ChangePersonQuery(ucn, "Name", change).ToString());
                    break;
                default:
                    MessageBox.Show("To be done...");
                    break;
            }
            //ChangePersonQuery();//8 опции
            //ChangeAccessCodeQuery();
            //ChangeHourPaymentQuery();
            //ChangePercentQuery();
        }

        private void CheckSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxUsers != null && ComboBoxUsers.SelectedItem != null)
            {
                string userInfo = ComboBoxUsers.SelectedItem.ToString();
                string[] split = userInfo.Split(", ");
                User user = SelectUserQuery(split[1]);

                if (Session.UserToken.GetLoginData() != split[1])
                {
                    MessageBox.Show(Salary.CheckSalary(split[1]).ToString());
                    MessageBoxResult result = MessageBox.Show("Желаете ли да нулирате текущо изработената сума?", "CLOC", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        MessageBoxResult confirmation = MessageBox.Show("Сигурни ли сте в избора си?\nТова означава, че ще трябва да изплатите на вашия служител изведената сума!\nПродължаваме ли?", "CLOC", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                        if (confirmation == MessageBoxResult.Yes)
                        {
                            user.TotalHours = 0;
                            ChangeTotalHoursQuery(user);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Невалидна селекция!");
                }
            }
            else
            {
                MessageBox.Show("Не сте избрали потребител.");
            }
        }
    }
}
