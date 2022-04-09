using Cloc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using static Cloc.Database.DatabaseQuery;

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

        internal static bool AddUser(Person person, User user)
        {
            // this.PreviewKeyDown += new KeyEventHandler(Escape);//TODO

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
            if (Validator.ValidateUCN(TextBoxUCN.Text.ToString()))
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

            if (Validator.ValidateAccessCode(PasswordBoxAccessCode.Password.ToString()))
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
                //Close();
            }
            else
            {
                MessageBox.Show("Моля, проверете коректността на въведените данни и опитайте отново!");
            }
        }
    }
}
