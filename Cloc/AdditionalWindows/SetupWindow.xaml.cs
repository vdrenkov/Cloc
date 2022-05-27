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
        internal SetupWindow()
        {
            InitializeComponent();
            PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void ExitSession()
        {
            StartupWindow sw = new();
            Close();
            sw.Show();
        }

        internal void ReloadPage()
        {
            TextBoxServer.Text = string.Empty;
            TextBoxUser.Text = string.Empty;
            PasswordBoxDBPassword.Password = string.Empty;
            TextBoxPort.Text = string.Empty;
            TextBoxUCN.Text = string.Empty;
            TextBoxName.Text = string.Empty;
            TextBoxSurname.Text = string.Empty;
            TextBoxEmail.Text = string.Empty;
            TextBoxPhoneNumber.Text = string.Empty;
            TextBoxCountry.Text = string.Empty;
            TextBoxCity.Text = string.Empty;
            TextBoxAddress.Text = string.Empty;
            PasswordBoxAccessCode.Password = string.Empty;
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ExitSession();
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            ExitSession();
        }

        private void ButtonSetup_Click(object sender, RoutedEventArgs e)
        {
            Person person = new();
            Database.DBInfo db = new();
            string accessCode;
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

            db.Server = TextBoxServer.Text.ToString();
            db.UserID = TextBoxUser.Text.ToString();
            db.Password = PasswordBoxDBPassword.Password.ToString();
            db.Port = TextBoxPort.Text.ToString();

            if (flag == 0)
            {
                if (StartupQuery(db, person, accessCode))
                {
                    MessageBox.Show($"Настройката на системата беше успешна!\nВашият код за достъп е: {accessCode}");
                    ExitSession();
                }
                else
                {
                    MessageBox.Show("Неуспешна връзка към базата данни.");
                    ReloadPage();
                }
            }
            else
            {
                MessageBox.Show("Моля, проверете коректността на въведените данни и опитайте отново!\n" +
                    "Всички полета са задължителни!" +
                    "\nЕГН-то трябва да бъде 10-цифрено." +
                    "\nКодът за достъп ще бъде автоматично генериран при невалиден такъв.");
                ReloadPage();
            }
        }
    }
}
