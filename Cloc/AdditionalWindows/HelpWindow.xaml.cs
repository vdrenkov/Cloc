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
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
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

        private void SetupButton_Click(object sender, RoutedEventArgs e)
        {
            string password = "1cm13*8vCt19_xRc";
            if (passwordBox.Password == password)
            {
                MessageBox.Show("Внимание!\nМоля уверете се, че сте прочели README.txt файла, преди да продължите!");
                SetupWindow sw = new SetupWindow();
                sw.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Въвели сте грешна парола!");
                StartupWindow stw = new StartupWindow();
                stw.Show();
                this.Close();
            }
        }

        private void ChangeAccessCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string ucn = textBox.Text.ToString();
            string accessCode = passwordBox.Password.ToString();

            if (ValidateUCN(ucn))
            {
                if (ValidateAccessCode(accessCode))
                {
                    if (ChangeAccessCodeQuery(ucn, accessCode))
                    {
                        MessageBox.Show($"Промяната беше успешна! Новият код за достъп е: {passwordBox.Password.ToString()}");
                    }
                    StartupWindow stw = new StartupWindow();
                    stw.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Моля, въведете 5-цифрен код за достъп!");
                    passwordBox.Password = "";
                }
            }
            else
            {
                MessageBox.Show("Моля, въведете съществуващо ЕГН!");
                textBox.Text = "";
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            StartupWindow stw = new StartupWindow();
            stw.Show();
            this.Close();
        }
    }
}
