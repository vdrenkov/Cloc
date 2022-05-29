using Cloc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using static Cloc.Database.InsertQuery;
using static Cloc.Database.SelectQuery;

namespace Cloc.Pages
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        public AddUserPage()
        {
            InitializeComponent();
        }

        private void ComboBoxPositionLoad()
        {
            if (ComboBoxPosition != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
               {
                   ComboBoxPosition.Items.Clear();
                   foreach (WorkPosition value in Enum.GetValues(typeof(WorkPosition)))
                   {
                       string position = Person.TranslateFromWorkPosition(value);
                       ComboBoxPosition.Items.Add(position);
                   }
                   ComboBoxPosition.SelectedIndex = -1;
               }));
            }
        }

        private void ReloadPage()
        {
            TextBoxUCN.Text = string.Empty;
            TextBoxName.Text = string.Empty;
            TextBoxSurname.Text = string.Empty;
            TextBoxEmail.Text = string.Empty;
            TextBoxPhoneNumber.Text = string.Empty;
            TextBoxCountry.Text = string.Empty;
            TextBoxCity.Text = string.Empty;
            TextBoxAddress.Text = string.Empty;

            PasswordBoxAccessCode.Password = string.Empty;

            TextBoxHourPayment.Text = string.Empty;
            TextBoxPercent.Text = string.Empty;

            ComboBoxPositionLoad();
        }

        private static bool AddUser(Person person, User user)
        {
            bool flag = false;

            try
            {
                if (AddWorkerQuery(person, user))
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неуспешно добавяне на потребител. Моля, опитайте отново!");
                ErrorLog.AddErrorLog(ex.ToString());
            }

            return flag;
        }

        private void ButtonAddUser_Click(object sender, RoutedEventArgs e)
        {
            int flag = 0;
            Person person = new();
            User user = new();

            bool isUCNValid = Validator.ValidateUCN(TextBoxUCN.Text.ToString());
            if (!isUCNValid)
            {
                MessageBox.Show("Моля, въведете 10-цифрено ЕГН!");
                ReloadPage();
                return;
            }

            if (Validator.ValidateAccessCode(PasswordBoxAccessCode.Password.ToString()))
            {
                if (SelectAccessCodeQuery(PasswordBoxAccessCode.Password.ToString()))
                {
                    MessageBox.Show("Въведеният код за достъп вече е зает.");
                    ReloadPage();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Моля, въведете 5-цифрен код за достъп!");
                ReloadPage();
                return;
            }

            if (string.IsNullOrEmpty(TextBoxName.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxSurname.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxEmail.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxPhoneNumber.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxCountry.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxCity.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxAddress.Text)) { flag++; }

            if (ComboBoxPosition != null && ComboBoxPosition.SelectedIndex != -1)
            { person.Position = Person.TranslateToWorkPosition(ComboBoxPosition.SelectedItem.ToString()); }
            else { flag++; }

            if (double.TryParse(TextBoxHourPayment.Text.ToString(), out double hourPayment) && hourPayment >= 0 && hourPayment <= 500)
            {
                user.HourPayment = hourPayment;
            }
            else
            {
                flag++;
            }

            if (double.TryParse(TextBoxPercent.Text.ToString(), out double percent) && percent >= -10 && percent <= 25)
            {
                user.Percent = percent;
            }
            else
            {
                flag++;
            }

            if (flag != 0)
            {
                MessageBox.Show("Моля, проверете коректността на въведените данни!\nВсички полета са задължителни!" +
                    "\nЧасова ставка: >= 0 и <= 500 лева.\nПроцент: >= -10 и <= 25%.");
                ReloadPage();
            }
            else
            {
                person.UCN = TextBoxUCN.Text.ToString();
                person.Name = TextBoxName.Text.ToString();
                person.Surname = TextBoxSurname.Text.ToString();
                person.Email = TextBoxEmail.Text.ToString();
                person.PhoneNumber = TextBoxPhoneNumber.Text.ToString();
                person.Country = TextBoxCountry.Text.ToString();
                person.City = TextBoxCity.Text.ToString();
                person.Address = TextBoxAddress.Text.ToString();
                user.UserUCN = person.UCN;
                user.AccessCode = PasswordBoxAccessCode.Password.ToString();
                user.CheckIn = DateTime.Now;
                user.CheckOut = DateTime.Now;
                user.IsCheckedIn = false;
                user.TotalHours = 0;

                if (AddUser(person, user))
                {
                    MessageBox.Show("Потребителят беше добавен успешно.");
                    if (!AddLogQuery(Session.UserToken.GetLoginData(), "Добавяне на потребител " + person.Name + " " + person.Surname + "."))
                    {
                        MessageBox.Show("Възникна грешка при записване на активността.");
                    }
                    if (!AddLogQuery(person.UCN, "Създаване на профил."))
                    {
                        MessageBox.Show("Възникна грешка при записване на активността.");
                    }
                }
                else
                {
                    MessageBox.Show("Потребителят не беше добавен. Възможно е да сте въвели вече съществуващи данни.");
                }
                ReloadPage();
            }
        }
    }
}
