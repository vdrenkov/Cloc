using Cloc.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using static Cloc.Database.DeleteQuery;
using static Cloc.Database.SelectQuery;
using static Cloc.Database.UpdateQuery;

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
                        ComboBoxPosition.Items.Clear();
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
            { TextBoxChange.Text = string.Empty; }
        }

        private bool DeleteUser()
        {
            bool flag = false;

            if (ComboBoxUsers != null && ComboBoxUsers.SelectedItem != null)
            {
                string userInfo = ComboBoxUsers.SelectedItem.ToString();
                string[] split = userInfo.Split(", ");
                string name = split[0];
                string ucn = split[1];

                if (Session.UserToken.GetLoginData() != ucn)
                {
                    User user = SelectUserQuery(ucn);

                    if (user.TotalHours == 0)
                    {
                        if (DeleteWorkerQuery(ucn))
                        {
                            flag = true;
                            if (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Изтриване профила на " + name + "."))
                            {
                                MessageBox.Show("Възникна грешка при записване на активността.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не може да бъде изтрит избраният потребител, понеже има неизплатена сума.");
                    }
                }
                else if (Person.AdminCount() != 1)
                {
                    if (DeleteWorkerQuery(ucn))
                    {
                        flag = true;
                        if ((!Database.InsertQuery.AddLogQuery(ucn, "Изтриване на профила.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Изтриване профила на " + name + ".")))
                        {
                            MessageBox.Show("Възникна грешка при записване на активността.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Невалидна селекция. Системата не може да остане без администратор!");
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
            }
            ReloadPage();
        }

        private void ChangeName(string ucn, string changeParam, string name)
        {
            if (ChangePersonQuery(ucn, "Name", changeParam))
            {
                MessageBox.Show("Промяната бе успешна.");
                if ((!Database.InsertQuery.AddLogQuery(ucn, "Промяна на име от администратор.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна името на потребител " + name + " на " + changeParam + ".")))
                {
                    MessageBox.Show("Възникна грешка при записване на активността.");
                }
            }
            else
            {
                MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
            }

            ReloadPage();
        }

        private void ChangeSurname(string ucn, string changeParam, string name)
        {
            if (ChangePersonQuery(ucn, "Surname", changeParam))
            {
                MessageBox.Show("Промяната бе успешна.");
                if (!Database.InsertQuery.AddLogQuery(ucn, "Промяна на фамилно име от администратор.") || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна фамилията на потребител " + name + " на " + changeParam + ".")))
                {
                    MessageBox.Show("Възникна грешка при записване на активността.");
                }
            }
            else
            {
                MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
            }

            ReloadPage();
        }

        private void ChangeEmail(string ucn, string changeParam, string name)
        {
            if (ChangePersonQuery(ucn, "Email", changeParam))
            {
                MessageBox.Show("Промяната бе успешна.");
                if ((!Database.InsertQuery.AddLogQuery(ucn, "Промяна на E-Mail от администратор.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна имейла на потребител " + name + " на " + changeParam + ".")))
                {
                    MessageBox.Show("Възникна грешка при записване на активността.");
                }
            }
            else
            {
                MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
            }

            ReloadPage();
        }

        private void ChangePhoneNumber(string ucn, string changeParam, string name)
        {
            if (ChangePersonQuery(ucn, "PhoneNumber", changeParam))
            {
                MessageBox.Show("Промяната бе успешна.");
                if ((!Database.InsertQuery.AddLogQuery(ucn, "Промяна на телефонен номер от администратор.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна телефонния номер на потребител " + name + " на " + changeParam + ".")))
                {
                    MessageBox.Show("Възникна грешка при записване на активността.");
                }
            }
            else
            {
                MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
            }

            ReloadPage();
        }

        private void ChangeCountry(string ucn, string changeParam, string name)
        {
            if (ChangePersonQuery(ucn, "Country", changeParam))
            {
                MessageBox.Show("Промяната бе успешна.");
                if ((!Database.InsertQuery.AddLogQuery(ucn, "Промяна на държава от администратор.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна държавата на потребител " + name + " на " + changeParam + ".")))
                {
                    MessageBox.Show("Възникна грешка при записване на активността.");
                }
            }
            else
            {
                MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
            }

            ReloadPage();
        }

        private void ChangeCity(string ucn, string changeParam, string name)
        {
            if (ChangePersonQuery(ucn, "City", changeParam))
            {
                MessageBox.Show("Промяната бе успешна.");
                if ((!Database.InsertQuery.AddLogQuery(ucn, "Промяна на град от администратор.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна града на потребител " + name + " на " + changeParam + ".")))
                {
                    MessageBox.Show("Възникна грешка при записване на активността.");
                }
            }
            else
            {
                MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
            }

            ReloadPage();
        }

        private void ChangeAddress(string ucn, string changeParam, string name)
        {
            if (ChangePersonQuery(ucn, "Address", changeParam))
            {
                MessageBox.Show("Промяната бе успешна.");
                if ((!Database.InsertQuery.AddLogQuery(ucn, "Промяна на адрес от администратор.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна адреса на потребител " + name + " на " + changeParam + ".")))
                {
                    MessageBox.Show("Възникна грешка при записване на активността.");
                }
            }
            else
            {
                MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
            }

            ReloadPage();
        }

        private void ChangeWorkPosition(string ucn, string name)
        {
            if (ComboBoxPosition != null && ComboBoxPosition.SelectedIndex != -1)
            {
                WorkPosition wp = Person.TranslateToWorkPosition(ComboBoxPosition.SelectedItem.ToString());

                if (ucn != Session.UserToken.GetLoginData())
                {
                    if (ChangePersonQuery(ucn, "Position", wp.ToString()))
                    {
                        MessageBox.Show("Промяната бе успешна.");
                        if ((!Database.InsertQuery.AddLogQuery(ucn, "Промяна на работна позиция от администратор.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна позицията на потребител " + name + " на " + Person.TranslateFromWorkPosition(wp) + ".")))
                        {
                            MessageBox.Show("Възникна грешка при записване на активността.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
                    }
                }
                else if (Person.AdminCount() != 1)
                {
                    if (ChangePersonQuery(ucn, "Position", wp.ToString()))
                    {
                        MessageBox.Show("Промяната бе успешна.");
                        if (ucn != Session.UserToken.GetLoginData())
                        {
                            if (!Database.InsertQuery.AddLogQuery(ucn, "Промяна на работна позиция от администратор."))
                            {
                                MessageBox.Show("Възникна грешка при записване на активността.");

                            }
                        }
                        if (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна позицията на потребител " + name + " на " + Person.TranslateFromWorkPosition(wp) + "."))
                        {
                            MessageBox.Show("Възникна грешка при записване на активността.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Промяната не бе успешна. Моля, опитайте по-късно!");
                    }
                }
                else
                {
                    MessageBox.Show("Невалидна селекция. Системата не може да остане без администратор!");
                }
            }
            else
            {
                MessageBox.Show("Не сте избрали работна позиция.");
            }

            ReloadPage();
            ComboBoxPosition.SelectedIndex = -1;
        }

        private void ChangeAccessCode(string ucn, string changeParam, string name)
        {
            if (Validator.ValidateAccessCode(changeParam) && !SelectAccessCodeQuery(changeParam))
            {
                if (ChangeAccessCodeQuery(ucn, changeParam))
                {
                    MessageBox.Show("Промяната бе успешна.");
                    if ((!Database.InsertQuery.AddLogQuery(ucn, "Промяна на код за достъп от администратор.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна кода за достъп на потребител " + name + ".")))
                    {
                        MessageBox.Show("Възникна грешка при записване на активността.");
                    }
                }
                else
                {
                    MessageBox.Show("Възникна неочаквана грешка.");
                }
            }
            else
            {
                MessageBox.Show("Въвели сте невалиден или вече съществуващ код. Той трябва да бъде 5-цифрен. Моля, опитайте отново!");
            }

            ReloadPage();
        }

        private void ChangeHourPayment(string ucn, string changeParam, string name)
        {
            if (double.TryParse(changeParam, out double hourPayment) && hourPayment > 0 && hourPayment <= 500)
            {
                if (ChangeHourPaymentQuery(ucn, hourPayment))
                {
                    MessageBox.Show("Промяната бе успешна.");
                    if ((!Database.InsertQuery.AddLogQuery(ucn, "Промяна на часова ставка от администратор.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна часовата ставка на потребител " + name + ".")))
                    {
                        MessageBox.Show("Възникна грешка при записване на активността.");
                    }
                }
                else
                {
                    MessageBox.Show("Възникна неочаквана грешка.");
                }
            }
            else
            {
                MessageBox.Show("Въвели сте невалидна часова ставка. Тя трябва да бъде между 0 и 500. Моля, опитайте отново!");
            }

            ReloadPage();
        }

        private void ChangePercent(string ucn, string changeParam, string name)
        {
            if (double.TryParse(changeParam, out double percent) && percent >= -10 && percent <= 25)
            {
                if (ChangePercentQuery(ucn, percent))
                {
                    MessageBox.Show("Промяната бе успешна.");
                    if ((!Database.InsertQuery.AddLogQuery(ucn, "Промяна на процент от администратор.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Промяна бонус-процента на потребител " + name + ".")))
                    {
                        MessageBox.Show("Възникна грешка при записване на активността.");
                    }
                }
                else
                {
                    MessageBox.Show("Възникна неочаквана грешка.");
                }
            }
            else
            {
                MessageBox.Show("Въвели сте невалиден процент. Той трябва да бъде между -10 и +25. Моля, опитайте отново!");
            }

            ReloadPage();
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
                        string name = split[0];
                        string ucn = split[1];

                        switch (choice)
                        {
                            case 0:
                                ChangeName(ucn, changeParam, name);
                                break;
                            case 1:
                                ChangeSurname(ucn, changeParam, name);
                                break;
                            case 2:
                                ChangeEmail(ucn, changeParam, name);
                                break;
                            case 3:
                                ChangePhoneNumber(ucn, changeParam, name);
                                break;
                            case 4:
                                ChangeCountry(ucn, changeParam, name);
                                break;
                            case 5:
                                ChangeCity(ucn, changeParam, name);
                                break;
                            case 6:
                                ChangeAddress(ucn, changeParam, name);
                                break;
                            case 7:
                                ChangeWorkPosition(ucn, name);
                                break;
                            case 8:
                                ChangeAccessCode(ucn, changeParam, name);
                                break;
                            case 9:
                                ChangeHourPayment(ucn, changeParam, name);
                                break;
                            case 10:
                                ChangePercent(ucn, changeParam, name);
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
                string name = split[0];
                string ucn = split[1];

                Person person = SelectPersonQuery(ucn);
                User user = SelectUserQuery(ucn);

                  if (person.Position!=WorkPosition.Admin)
                {
                    double salary = Salary.CheckSalary(ucn);
                    MessageBox.Show("Текущата сума за изплащане на " + name + " е " + Math.Round(salary, 2).ToString() + " лева.");
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
                                    if ((!Database.InsertQuery.AddLogQuery(user.UserUCN, "Изплащане на стойност " + Math.Round(salary, 2) + " лв.")) || (!Database.InsertQuery.AddLogQuery(Session.UserToken.GetLoginData(), "Изплащане на " + Math.Round(salary, 2) + " лева на " + name + " .")))
                                    {
                                        MessageBox.Show("Възникна грешка при записване на активността.");
                                    }
                                    if (!Database.InsertQuery.AddReportQuery(ucn, name, salary))
                                    {
                                        MessageBox.Show("Възникна грешка при записване на изплащане.");
                                    }
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
            ReloadPage();
        }
    }
}
