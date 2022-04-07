using Cloc.Classes;
using System;
using System.Windows;
using System.Windows.Input;
using static Cloc.Classes.Validator;
using static Cloc.Database.DatabaseQuery;

namespace Cloc.AdditionalWindows
{
    /// <summary>
    /// Interaction logic for SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : Window
    {
        public SetupWindow()
        {
            InitializeComponent();
            PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                StartupWindow sw = new();
                Close();
                sw.Show();
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            StartupWindow sw = new();
            Close();
            sw.Show();
        }

        internal static bool AddUser(Person person, User user)
        {
            bool flag = false;

            try
            {
                if (AddWorkerQuery(person, user))
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Неуспешно добавяне на потребител. Моля, опитайте отново!");
            }

            return flag;
        }

        private void ButtonAddUser_Click(object sender, RoutedEventArgs e)
        {
            Person person = new();
            User user = new();
            if (ValidateUCN(TextBoxUCN.Text.ToString()))
            {
                person.UCN = TextBoxUCN.Text.ToString();
                user.UserUCN = person.UCN;
            }
            else
            {
                MessageBox.Show("Моля, въведете правилно ЕГН!");
                TextBoxUCN.Text = null;
            }

            person.Name = TextBoxName.Text.ToString();
            person.Surname = TextBoxSurname.Text.ToString();
            person.Email = TextBoxEmail.Text.ToString();
            person.PhoneNumber = TextBoxPhoneNumber.Text.ToString();
            person.Country = TextBoxCountry.Text.ToString();
            person.City = TextBoxCity.Text.ToString();
            person.Address = TextBoxAddress.Text.ToString();

            if (ComboBoxPosition != null && ComboBoxPosition.SelectedIndex != -1)
            {
                person.Position = Person.TranslateToWorkPosition(ComboBoxPosition.SelectedItem.ToString());
            }

            if (ValidateAccessCode(PasswordBoxAccessCode.Password.ToString()))
            {
                if (!SelectAccessCodeQuery(PasswordBoxAccessCode.Password.ToString()))
                { user.AccessCode = PasswordBoxAccessCode.Password.ToString(); }
                else
                {
                    PasswordBoxAccessCode.Password = null;
                    MessageBox.Show("Въведеният от вас код вече е зает.");
                }
            }

            user.CheckIn = DateTime.Now;
            user.CheckOut = DateTime.Now;
            user.IsCheckedIn = false;
            user.TotalHours = 0;

            if (double.TryParse(TextBoxHourPayment.Text.ToString(), out double hourPayment))
            {
                user.HourPayment = hourPayment;
            }
            else
            {
                MessageBox.Show("Моля, въведете правилна часова ставка!");
                TextBoxHourPayment.Text = null;
            }

            if (double.TryParse(TextBoxPercent.Text.ToString(), out double percent) && percent >= -10 && percent <= 25)
            {
                user.Percent = percent;
            }
            else
            {
                MessageBox.Show("Моля, въведете процент в диапазона от -10 до +25!");
                TextBoxHourPayment.Text = null;
            }

            if (AddUser(person, user))
            {
                MessageBox.Show("Потребителят беше добавен успешно.");
                Close();
            }
            else
            {
                MessageBox.Show("Моля, проверете коректността на въведените данни и опитайте отново!");
            }
        }

        private void ButtonSetup_Click(object sender, RoutedEventArgs e)
        {
            Person person = new();
            string accessCode, server, user, password, port;
            int flag = 0;

            if (ValidateUCN(TextBoxUCN.Text.ToString()))
            {
                person.UCN = TextBoxUCN.Text.ToString();
            }
            else
            {
                flag++;
            }
            if (string.IsNullOrEmpty(TextBoxName.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxSurname.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxEmail.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxPhoneNumber.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxCountry.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxCity.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxAddress.Text)) { flag++; }

            person.Name = TextBoxName.Text.ToString();
            person.Surname = TextBoxSurname.Text.ToString();
            person.Email = TextBoxEmail.Text.ToString();
            person.PhoneNumber = TextBoxPhoneNumber.Text.ToString();
            person.Country = TextBoxCountry.Text.ToString();
            person.City = TextBoxCity.Text.ToString();
            person.Address = TextBoxAddress.Text.ToString();
            person.Position = WorkPosition.Admin;

            if (ValidateAccessCode(PasswordBoxAccessCode.Password.ToString()))
            {
                accessCode = PasswordBoxAccessCode.Password.ToString();
            }
            else
            {
                Random random = new();
                accessCode = random.Next(10000, 99999).ToString();
            }

            if (string.IsNullOrEmpty(TextBoxServer.Text)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxUser.Text)) { flag++; }
            if (string.IsNullOrEmpty(PasswordBoxDBPassword.Password)) { flag++; }
            if (string.IsNullOrEmpty(TextBoxPort.Text)) { flag++; }

            server = TextBoxServer.Text.ToString();
            user = TextBoxUser.Text.ToString();
            password = PasswordBoxDBPassword.Password.ToString();
            port = TextBoxPort.Text.ToString();

            if (flag == 0)
            {
                if (StartupQuery(server, user, password, port, person, accessCode))
                {
                    MessageBox.Show($"Настройката на системата беше успешна!\nВашият код за достъп е: {accessCode}");
                    StartupWindow sw = new();
                    Close();
                    sw.Show();
                }
                else
                {
                    MessageBox.Show("Моля, проверете коректността на въведените данни и опитайте отново!");
                }
            }
            else
            {
                MessageBox.Show("Моля, проверете коректността на въведените данни и опитайте отново!\n" +
                    "Всички полета са задължителни!");
            }
        }
    }
}
