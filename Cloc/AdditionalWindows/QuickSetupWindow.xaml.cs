using System.Windows;
using System.Windows.Input;
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
                if (SetupQuery(db))
                {
                    bool isConnected = Database.SelectQuery.SelectAccessCodeQuery(accessCode);

                    if (isConnected)
                    {
                        MessageBox.Show($"Настройката на системата беше успешна!");
                        ExitSession();
                    }
                    else
                    {
                        ReloadPage();
                        MessageBox.Show("Моля, проверете коректността на въведените данни и опитайте отново!\n" +
                                 "Всички полета са задължителни!" +
                                 "\nКодът за достъп се използва с цел проверка на връзката.");
                    }
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
