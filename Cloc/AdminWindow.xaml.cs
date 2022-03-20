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

namespace Cloc
{
    /// <summary>
    /// Interaction logic for BossWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow() 
        { 
                InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            Main.Navigate(new Pages.MainPage());
        }
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                StartupWindow sw = new StartupWindow();
                sw.Show();
                this.Close();
                Session.UserToken.RemoveLoginData();
            }
        }
        private void buttonMain_Click(object sender, RoutedEventArgs e)
            {
                Main.Navigate(new Pages.MainPage());
            }
            private void buttonBossOptions_Click(object sender, RoutedEventArgs e)
            {
                Main.Navigate(new Pages.BossOptionsPage());
            }
            private void buttonProfile_Click(object sender, RoutedEventArgs e)
            {
                Main.Navigate(new Pages.ProfilePage());
            }
            private void buttonLogs_Click(object sender, RoutedEventArgs e)
            {
                Main.Navigate(new Pages.ActivityPage());
            }
            private void buttonExit_Click(object sender, RoutedEventArgs e)
            {
                StartupWindow sw = new StartupWindow();
                sw.Show();
                this.Close();
            Session.UserToken.RemoveLoginData();
        }
    }
    }
