using Cloc.Classes;
using System;
using System.Collections.Generic;
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

        private void ComboBoxPositionLoad()
        {
            if (ComboBoxChange != null && ComboBoxChange.SelectedIndex == 7)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (ComboBoxPosition != null)
                    {
                        ComboBoxPosition.Visibility = Visibility.Visible;
                        foreach (WorkPosition value in Enum.GetValues(typeof(WorkPosition)))
                        {
                            string position = Person.TranslateFromWorkPosition(value);
                            ComboBoxPosition.Items.Add(position);
                        }
                    }
                }));
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (ComboBoxPosition != null)
                    {
                        ComboBoxPosition.Visibility = Visibility.Hidden;
                    }
                }));
            }
        }

        private void ComboBoxChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxPositionLoad();
        }

        private void ReloadPage()
        {
            if (ComboBoxUsers != null)
            {
                ComboBoxUsers.Items.Clear();

                List<Person> people = SelectAllPeopleQuery();

                foreach (Person person in people)
                {
                    ComboBoxUsers.Items.Add(person.Name + " " + person.Surname + ", " + person.UCN);
                }

                ComboBoxChange.SelectedIndex = -1;
            }
            ComboBoxPositionLoad();
            if (TextBoxChange != null)
            { TextBoxChange.Text = null; }
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
                    if (DeleteWorkerQuery(split[1]))
                    {
                        flag = true;

                        Logger.AddLog(Session.UserToken.GetLoginData(), "Изтриване профила на " + split[0] + ".");
                    }
                }
                else
                {
                    MessageBox.Show("Не може да бъде изтрит администраторски акаунт.");
                }
            }
            else
            {
                MessageBox.Show("Не сте избрали потребител.");
            }

            return flag;
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteUser())
            {
                MessageBox.Show("Избраният потребител беше изтрит успешно.");
                ReloadPage();
            }
        }

        private void ChangeDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxUsers.SelectedIndex != -1)
            {
                if (ComboBoxChange.SelectedIndex != -1)
                {
                    if (string.IsNullOrEmpty(TextBoxChange.Text) && ComboBoxChange.SelectedIndex != 7)
                    {
                        MessageBox.Show("Не сте въвели новите данни.");
                        ReloadPage();
                    }
                    else
                    {
                        int choice = ComboBoxChange.SelectedIndex;
                        string changeParam = TextBoxChange.Text;
                        string userInfo = ComboBoxUsers.SelectedItem.ToString();
                        string[] split = userInfo.Split(", ");
                        string ucn = split[1];

                        switch (choice)
                        {
                            case 0:
                                if (ChangePersonQuery(ucn, "Name", changeParam))
                                {
                                    MessageBox.Show("Промяната бе успешна.");
                                    Logger.AddLog(Session.UserToken.GetLoginData(), "Промяна името на потребител " + split[0] + " на " + changeParam + ".");
                                }
                                else
                                {
                                    MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
                                }

                                ReloadPage();
                                break;
                            case 1:
                                if (ChangePersonQuery(ucn, "Surname", changeParam))
                                {
                                    MessageBox.Show("Промяната бе успешна.");
                                    Logger.AddLog(Session.UserToken.GetLoginData(), "Промяна фамилията на потребител " + split[0] + " на " + changeParam + ".");
                                }
                                else
                                {
                                    MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
                                }

                                ReloadPage();
                                break;
                            case 2:
                                if (ChangePersonQuery(ucn, "Email", changeParam))
                                {
                                    MessageBox.Show("Промяната бе успешна.");
                                    Logger.AddLog(Session.UserToken.GetLoginData(), "Промяна имейла на потребител " + split[0] + " на " + changeParam + ".");
                                }
                                else
                                {
                                    MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
                                }

                                ReloadPage();
                                break;
                            case 3:
                                if (ChangePersonQuery(ucn, "PhoneNumber", changeParam))
                                {
                                    MessageBox.Show("Промяната бе успешна.");
                                    Logger.AddLog(Session.UserToken.GetLoginData(), "Промяна телефонния номер на потребител " + split[0] + " на " + changeParam + ".");
                                }
                                else
                                {
                                    MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
                                }

                                ReloadPage();
                                break;
                            case 4:
                                if (ChangePersonQuery(ucn, "Country", changeParam))
                                {
                                    MessageBox.Show("Промяната бе успешна.");
                                    Logger.AddLog(Session.UserToken.GetLoginData(), "Промяна държавата на потребител " + split[0] + " на " + changeParam + ".");
                                }
                                else
                                {
                                    MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
                                }

                                ReloadPage();
                                break;
                            case 5:
                                if (ChangePersonQuery(ucn, "City", changeParam))
                                {
                                    MessageBox.Show("Промяната бе успешна.");
                                    Logger.AddLog(Session.UserToken.GetLoginData(), "Промяна града на потребител " + split[0] + " на " + changeParam + ".");
                                }
                                else
                                {
                                    MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
                                }

                                ReloadPage();
                                break;
                            case 6:
                                if (ChangePersonQuery(ucn, "Address", changeParam))
                                {
                                    MessageBox.Show("Промяната бе успешна.");
                                    Logger.AddLog(Session.UserToken.GetLoginData(), "Промяна адреса на потребител " + split[0] + " на " + changeParam + ".");
                                }
                                else
                                {
                                    MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
                                }

                                ReloadPage();
                                break;
                            case 7:
                                if (ComboBoxPosition != null && ComboBoxPosition.SelectedIndex != -1)
                                {
                                    WorkPosition wp = Person.TranslateToWorkPosition(ComboBoxPosition.SelectedItem.ToString());

                                    if (ucn == Session.UserToken.GetLoginData())
                                    {
                                        MessageBox.Show("Не може да се променя позицията на администратор.");
                                    }
                                    else
                                    {
                                        if (ChangePersonQuery(ucn, "Position", wp.ToString()))
                                        {
                                            MessageBox.Show("Промяната бе успешна.");
                                            Logger.AddLog(Session.UserToken.GetLoginData(), "Промяна позицията на потребител " + split[0] + " на " + Person.TranslateFromWorkPosition(wp) + ".");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Не сте избрали работна позиция.");
                                }

                                ReloadPage();
                                ComboBoxPosition.SelectedIndex = -1;
                                break;
                            case 8:
                                if (Validator.ValidateAccessCode(changeParam))
                                {
                                    if (ChangeAccessCodeQuery(ucn, changeParam))
                                    {
                                        MessageBox.Show("Промяната бе успешна.");
                                        Logger.AddLog(Session.UserToken.GetLoginData(), "Промяна кода за достъп на потребител " + split[0] + ".");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Възникна неочаквана грешка.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Въвели сте невалидни данни.");
                                }

                                ReloadPage();
                                break;
                            case 9:
                                if (double.TryParse(changeParam, out double hourPayment) && hourPayment > 0 && hourPayment <= 500)
                                {
                                    if (ChangeHourPaymentQuery(ucn, hourPayment))
                                    {
                                        MessageBox.Show("Промяната бе успешна.");
                                        Logger.AddLog(Session.UserToken.GetLoginData(), "Промяна часовата ставка на потребител " + split[0] + ".");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Възникна неочаквана грешка.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Въвели сте невалидни данни.");
                                }

                                ReloadPage();
                                break;
                            case 10:
                                if (double.TryParse(changeParam, out double percent) && percent >= -10 && percent <= 25)
                                {
                                    if (ChangePercentQuery(ucn, percent))
                                    {
                                        MessageBox.Show("Промяната бе успешна.");
                                        Logger.AddLog(Session.UserToken.GetLoginData(), "Промяна бонус-процента на потребител " + split[0] + ".");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Възникна неочаквана грешка.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Въвели сте невалидни данни.");
                                }

                                ReloadPage();
                                break;
                            default:
                                MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
                                ReloadPage();
                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Не сте избрали опция за смяна.");
                    ReloadPage();
                }
            }
            else
            {
                MessageBox.Show("Не сте избрали потребител.");
                ReloadPage();
            }
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
                    double salary = Salary.CheckSalary(split[1]);
                    MessageBox.Show("Текущата сума за изплащане на " + split[0] + " е " + Math.Round(salary, 2).ToString() + " лева.");
                    if (salary != 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Желаете ли да нулирате изработената сума до момента?", "CLOC", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                        if (result == MessageBoxResult.Yes)
                        {
                            MessageBoxResult confirmation = MessageBox.Show("Сигурни ли сте в избора си?\nТова означава, че ще трябва да изплатите на вашия служител натрупаната сума.\nФинализиране на заявката?", "CLOC", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                            if (confirmation == MessageBoxResult.Yes)
                            {
                                user.TotalHours = 0;
                                if (NullTotalHoursQuery(user))
                                {
                                    MessageBox.Show("Сумата бе успешно нулирана.");
                                    Logger.AddLog(Session.UserToken.GetLoginData(), "Изплащане на " + salary + " лева на " + split[0] + " .");
                                }
                                else
                                {
                                    MessageBox.Show("Възникна грешка при извършване на избраното действие. Моля, опитайте по-късно!");
                                }
                            }
                        }
                    }
                    ReloadPage();
                }
                else
                {
                    MessageBox.Show("Невалидна селекция (Администратор).");
                }
            }
            else
            {
                MessageBox.Show("Не сте избрали потребител.");
            }
        }
    }
}
