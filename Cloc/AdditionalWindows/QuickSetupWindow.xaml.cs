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
using static Cloc.Classes.Validator;
using static Cloc.Database.CreateQuery;

namespace Cloc.AdditionalWindows
{
    /// <summary>
    /// Interaction logic for QuickSetupWindow.xaml
    /// </summary>
    public partial class QuickSetupWindow : Window
    {
        internal QuickSetupWindow()
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
            Database.DatabaseParameters db = new();
            string accessCode;
            int flag = 0;

            if (ValidateAccessCode(PasswordBoxAccessCode.Password.ToString()))
            {
                accessCode = PasswordBoxAccessCode.Password.ToString();

                User user = Database.SelectQuery.SelectUserByAccessCodeQuery(accessCode);

                if (user.UserUCN == null)
                { flag++; }
            }
            else
            {
                accessCode = string.Empty;
                flag++;
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
                if (SetupQuery(db, accessCode))
                {
                    MessageBox.Show($"Настройката на системата беше успешна!");
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
                    "\nКодът за достъп се използва с цел проверка на връзката.");
                ReloadPage();
            }
        }
    }
}
