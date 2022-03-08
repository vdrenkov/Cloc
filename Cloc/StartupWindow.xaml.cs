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

//TODO Button Refactoring, Settings Button, Change Access Code Button -> Startup Window
//TODO ReadMe.txt, overtime option, хеш, queries

/*Check -in +IsCheckedIn->DB
Check -out -> DB
 Check-out - check-in -> DB (total hours)
ucn + check -in +check -out -> Checks.txt
 Ucn + logs -> Logs.txt */

namespace Cloc
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (EntryRegex.ValidateEntry(tb1.Text.ToString()))
            {
                if ((tb1.Text.ToString()) == "77777")
                {
                    BossWindow bw = new BossWindow();
                    bw.Show();
                    this.Close();
                }
                else
                {
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                }
            }
            else
            {
                tb1.Text = "";
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnCredits_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Настоящият сайт бе разработен от Валентин Дренков, студент от ТУ - София, ФКСТ, специалност КСИ, IV курс, 51. група, факултетен номер: 121218025, като задание за дипломна работа." +
                "\nВсички права запазени.\n" +
                "Валентин Дренков, София, 03.03.2022");
        }
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Length < 5)
            {
                tb1.Text += 1;
            }
        }
        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Length < 5)
            {
                tb1.Text += 2;
            }
        }
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Length < 5)
            {
                tb1.Text += 3;
            }
        }
        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Length < 5)
            {
                tb1.Text += 4;
            }
        }
        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Length < 5)
            {
                tb1.Text += 5;
            }
        }
        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Length < 5)
            {
                tb1.Text += 6;
            }
        }
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Length < 5)
            {
                tb1.Text += 7;
            }
        }
        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Length < 5)
            {
                tb1.Text += 8;
            }
        }
        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Length < 5)
            {
                tb1.Text += 9;
            }
        }
        private void btnX_Click(object sender, RoutedEventArgs e)
        {
               tb1.Text = "";
        }
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Length < 5)
            {
                tb1.Text += 0;
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Length>0)
            {
                tb1.Text = tb1.Text.Remove(tb1.Text.Length - 1);
            }
        }
    }
}
