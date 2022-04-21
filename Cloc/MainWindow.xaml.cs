using Cloc.Classes;
using Cloc.Pages;
using System.Windows;
using System.Windows.Input;

namespace Cloc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PreviewKeyDown += new KeyEventHandler(HandleEsc);
            Main.Navigate(new MainPage());
        }

        private void ExitCurrentSession()
        {
            if (!Logger.AddLog(Session.UserToken.GetLoginData(), "Изход от системата."))
            {
                MessageBox.Show("Неуспешен запис на активността.");
            }

            Session.UserToken.RemoveLoginData();
            ActivityPage.count = ActivityPage.COUNT;

            StartupWindow sw = new();
            Close();
            sw.Show();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ExitCurrentSession();
            }
        }
        private void ButtonMain_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new MainPage());
        }
        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new CheckPage());
        }
        private void ButtonProfile_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new ProfilePage());
        }
        private void ButtonLogs_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new ActivityPage());
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            ExitCurrentSession();
        }
    }
}
