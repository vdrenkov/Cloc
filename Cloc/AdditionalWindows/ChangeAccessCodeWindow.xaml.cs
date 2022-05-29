using Cloc.Classes;
using System.Windows;
using System.Windows.Input;

namespace Cloc.AdditionalWindows
{
    /// <summary>
    /// Interaction logic for ChangeAccessCodeWindow.xaml
    /// </summary>
    public partial class ChangeAccessCodeWindow : Window
    {
        public ChangeAccessCodeWindow()
        {
            InitializeComponent();
            PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void CloseTab()
        {
            StartupWindow sw = new();
            Close();
            sw.Show();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CloseTab();
            }
        }

        private void ChangeAccessCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validator.ValidateAccessCodeChange(TextBoxUCN.Text.ToString(), MyPasswordBox.Password.ToString()))
            {

                MessageBox.Show($"Промяната беше успешна! Новият код за достъп е: {MyPasswordBox.Password}");
                if (!Database.InsertQuery.AddLogQuery(TextBoxUCN.Text.ToString(), "Промяна на кода за достъп."))
                {
                    MessageBox.Show("Възникна грешка при записване на активността.");
                }
                CloseTab();
            }
            else
            {
                TextBoxUCN.Text = string.Empty;
                MyPasswordBox.Password = string.Empty;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CloseTab();
        }
    }
}
