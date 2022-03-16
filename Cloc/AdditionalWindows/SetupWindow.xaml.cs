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
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                StartupWindow sw = new StartupWindow();
                sw.Show();
                this.Close();
            }
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            StartupWindow sw = new StartupWindow();
            sw.Show();
            this.Close();
        }

        private void buttonSetup_Click(object sender, RoutedEventArgs e)
        {
            Person person = new Person();
            string accessCode = null, server = null, user = null, password = null, port = null;

            if (ValidateUCN(textboxUCN.Text.ToString()))
            {
                person.UCN = textboxUCN.Text.ToString();
            }
            else
            {
                MessageBox.Show("Моля въведете правилно ЕГН!");
                SetupWindow sw=new SetupWindow();
                sw.Show();
                this.Close();
                return;
            }

            person.Name = textboxName.Text.ToString();
            person.Surname = textboxSurname.Text.ToString();
            person.Email = textboxEmail.Text.ToString();
            person.PhoneNumber = textboxPhoneNumber.Text.ToString();
            person.Country = textboxCountry.Text.ToString();
            person.City = textboxCity.Text.ToString();
            person.Address = textboxAddress.Text.ToString();
                person.Position = WorkPosition.Admin;

            if (ValidateAccessCode(passwordBoxAccessCode.Password.ToString()))
            {
                accessCode = passwordBoxAccessCode.Password.ToString();

            }
            else
            {
                    Random random = new Random();
                    accessCode = random.Next(10000, 99999).ToString();
            } 

            server = textboxServer.Text.ToString();
            user = textboxUser.Text.ToString();
            password = passwordBoxDBPassword.Password.ToString();
            port = textboxPort.Text.ToString();

            if (StartupQuery(server, user, password, port, person, accessCode))
            {
                MessageBox.Show($"Настройката на системата беше успешна!\nВашият код за достъп е: {accessCode}");
                StartupWindow sw = new StartupWindow();
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
