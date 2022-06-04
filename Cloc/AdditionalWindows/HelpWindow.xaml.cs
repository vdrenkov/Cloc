using System.Windows;
using System.Windows.Input;

namespace Cloc.AdditionalWindows
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        internal HelpWindow()
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

        private void SetupButton_Click(object sender, RoutedEventArgs e)
        {
            string password = "1cm13*8vCt19_xRc";
            if (MyPasswordBox.Password == password)
            {
                MessageBox.Show("Внимание!\nМоля, уверете се, че сте прочели README.txt файла, преди да продължите!");

                MessageBoxResult result = MessageBox.Show("Желаете ли да изтриете всички данни от базата данни (първоначална инициализация)?", "CLOC", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                switch (result)
                {
                    case MessageBoxResult.Yes:

                        SetupWindow sw = new();

                        sw.ButtonSetup.IsDefault = true;
                        Close();
                        sw.Show();

                        break;

                    case MessageBoxResult.No:
                        QuickSetupWindow qsw = new();
                        qsw.ButtonSetup.IsDefault = true;
                        Close();
                        qsw.Show();
                        break;
                }


               
            }
            else
            {
                MessageBox.Show("Въвели сте грешна парола!");
                CloseTab();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CloseTab();
        }
    }
}
