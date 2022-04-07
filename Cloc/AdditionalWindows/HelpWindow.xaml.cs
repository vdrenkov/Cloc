using Cloc.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Cloc.Classes.Validator;

namespace Cloc.AdditionalWindows
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
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

        private void SetupButton_Click(object sender, RoutedEventArgs e)
        {
            string password = "1cm13*8vCt19_xRc";
            if (PasswordBox.Password == password)
            {
                MessageBox.Show("Внимание!\nМоля уверете се, че сте прочели README.txt файла, преди да продължите!");
                SetupWindow sw = new();
                sw.ButtonSetup.IsDefault = true;
                Close();
                sw.Show();
            }
            else
            {
                MessageBox.Show("Въвели сте грешна парола!");
                StartupWindow stw = new();
                Close();
                stw.Show();
            }
        }

        private void ChangeAccessCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAccessCodeChange(TextBoxUCN.Text.ToString(), PasswordBox.Password.ToString()))
            {
                Logger.AddLog(TextBoxUCN.Text.ToString(), "Промяна на кода за достъп.");
                MessageBox.Show($"Промяната беше успешна! Новият код за достъп е: {PasswordBox.Password}");
                StartupWindow stw = new();
                Close();
                stw.Show();
            }
            else
            {
                MessageBox.Show("Въвели сте неправилни или вече съществуващи данни!");
                TextBoxUCN.Text = null;
                PasswordBox.Password = null;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            StartupWindow stw = new();
            Close();
            stw.Show();
        }
    }
}
