using Cloc.AdditionalWindows;
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
using static Cloc.Classes.Security;
using static Cloc.Database.DatabaseQuery;

//TODO Settings Button + README.txt, Change Access Code Button -> StartupWindow

/*
CheckIn + IsCheckedIn (true) -> DB
CheckOut + IsCheckedIn (false) -> DB
CheckOut - CheckIn -> DB (TotalHours)
UCN + CheckIn + CheckOut -> Checks.txt
DateTime + UCN + Activity -> Logs.txt
*/

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
        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (EntryRegex.ValidateEntry(textboxKey.Text.ToString()))
            {
                if (textboxKey.Text.ToString() == "77777")
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
                textboxKey.Text = "";
            }
        }

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            Person p = new Person();
            p = SelectPersonQuery("9902130044");
            MessageBox.Show(p.UCN + p.Name + p.Surname + p.Email + p.PhoneNumber + p.Country + p.City + p.Address);
        }

        private void buttonChangeAccessCode_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void buttonSetup_Click(object sender, RoutedEventArgs e)
        {
            SetupWindow sw=new SetupWindow();
            sw.Show();
            this.Close();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void buttonCredits_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Настоящият сайт бе разработен от Валентин Дренков, студент от ТУ - София, ФКСТ, специалност КСИ, IV курс, 51. група, факултетен номер: 121218025, като задание за дипломна работа." +
                "\nВсички права запазени.\n" +
                "Валентин Дренков, София, 03.03.2022");
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textboxKey.Text.Length < 5)
            {
                textboxKey.Text += 1;
            }
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (textboxKey.Text.Length < 5)
            {
                textboxKey.Text += 2;
            }
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (textboxKey.Text.Length < 5)
            {
                textboxKey.Text += 3;
            }
        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (textboxKey.Text.Length < 5)
            {
                textboxKey.Text += 4;
            }
        }
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (textboxKey.Text.Length < 5)
            {
                textboxKey.Text += 5;
            }
        }
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (textboxKey.Text.Length < 5)
            {
                textboxKey.Text += 6;
            }
        }
        private void button7_Click(object sender, RoutedEventArgs e)
        {
            if (textboxKey.Text.Length < 5)
            {
                textboxKey.Text += 7;
            }
        }
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            if (textboxKey.Text.Length < 5)
            {
                textboxKey.Text += 8;
            }
        }
        private void button9_Click(object sender, RoutedEventArgs e)
        {
            if (textboxKey.Text.Length < 5)
            {
                textboxKey.Text += 9;
            }
        }
        private void buttonX_Click(object sender, RoutedEventArgs e)
        {
            textboxKey.Text = "";
        }
        private void button0_Click(object sender, RoutedEventArgs e)
        {
            if (textboxKey.Text.Length < 5)
            {
                textboxKey.Text += 0;
            }
        }
        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (textboxKey.Text.Length > 0)
            {
                textboxKey.Text = textboxKey.Text.Remove(textboxKey.Text.Length - 1);
            }
        }
    }
}
