using Cloc.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Cloc.Database.DatabaseQuery;
using static Cloc.Classes.Security;
using static Cloc.Classes.Validator;

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
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                StartupWindow sw = new();
                sw.Show();
                this.Close();
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            StartupWindow sw = new();
            sw.Show();
            this.Close();
        }

        private void ButtonSetup_Click(object sender, RoutedEventArgs e)
        {
            Person person = new();
            string accessCode, server, user, password, port;

            if (ValidateUCN(TextBoxUCN.Text.ToString()))
            {
                person.UCN = TextBoxUCN.Text.ToString();
            }
            else
            {
                MessageBox.Show("Моля въведете правилно ЕГН!");
                SetupWindow sw=new();
                sw.Show();
                this.Close();
                return;
            }

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

            server = TextBoxServer.Text.ToString();
            user = TextBoxUser.Text.ToString();
            password = PasswordBoxDBPassword.Password.ToString();
            port = TextBoxPort.Text.ToString();

            if (StartupQuery(server, user, password, port, person, accessCode))
            {
                MessageBox.Show($"Настройката на системата беше успешна!\nВашият код за достъп е: {accessCode}");
                StartupWindow sw = new();
                sw.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Възникна неочаквана грешка!\nМоля проверете коректността на въведените от вас данни!");
            }
        }
    }
}
