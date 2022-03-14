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
            comboBoxPosition.ItemsSource = Enum.GetValues(typeof(WorkPosition)).Cast<WorkPosition>();

        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonSetup_Click(object sender, RoutedEventArgs e)
        {
            Person person = new Person();

            person.UCN = textboxUCN.Text.ToString();
            person.Name = textboxName.Text.ToString();
            person.Surname = textboxSurname.Text.ToString();
            person.Email = textboxEmail.Text.ToString();
            person.PhoneNumber = textboxPhoneNumber.Text.ToString();
            person.Country = textboxCountry.Text.ToString();
            person.City = textboxCity.Text.ToString();
            person.Address = textboxAddress.Text.ToString();
            person.Position =(WorkPosition) Enum.Parse(typeof(WorkPosition), comboBoxPosition.SelectedValue.ToString());
            string accessCode = textboxAccessCode.Text.ToString();

            string server = textboxServer.Text.ToString();
            string user = textboxUser.Text.ToString();
            string password = textboxPassword.Text.ToString();
            string port = textboxPort.Text.ToString();

            if(StartupQuery(server,user,password,port,person,accessCode))
            {
                MessageBox.Show("Настройката на системата беше успешна!");
                StartupWindow sw=new StartupWindow();
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
