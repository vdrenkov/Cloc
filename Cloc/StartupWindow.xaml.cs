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
using static Cloc.Database.DatabaseConnection;
using static Cloc.Classes.Validator;

//Queries -> Entry check
// Current user token
/*
CheckIn + IsCheckedIn (true) -> DB
CheckOut + IsCheckedIn (false) -> DB
CheckOut - CheckIn -> DB (TotalHours)
UCN + CheckIn + CheckOut -> Checks.txt
DateTime + UCN + Activity -> Logs.txt

StartupWindow -> Exit confirmation uncomment, delete test button
Password -> 348_sha765_KaD3l

 //private static DatabaseConnection _instance = null;
        //public static DatabaseConnection Instance()
        //{
        //    if (_instance == null)
        //        _instance = new DatabaseConnection();
        //    return _instance;
        //}
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

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            ChangeAccessCodeQuery("9902130044", "77777");
            Person p = SelectPersonQuery("9902130044");
            MessageBox.Show(p.UCN + p.Name + p.Surname + p.Email + p.PhoneNumber + p.Country + p.City + p.Address + p.Position);
            User u = SelectUserQuery("9902130044");
            MessageBox.Show(u.UserUCN + u.AccessCode + u.CheckIn + u.CheckOut + u.Percent);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                //MessageBoxResult result = MessageBox.Show("Сигурни ли сте, че искате да излезнете от приложението?", "CLOC", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                //switch (result)
                //{
                //    case MessageBoxResult.Yes:
                Close();
                //        break;
                //}
            }
        }
        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateEntry(passwordBoxAccessCode.Password.ToString()))
            {
                Person person = new Person(); //TODO here +1 below
                person.Position = WorkPosition.Admin;

                if (isAdmin(person))
                {
                    AdminWindow aw = new AdminWindow();
                    aw.Show();
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
                passwordBoxAccessCode.Password = null;
            }
        }

        private void buttonChangeAccessCode_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow hw = new HelpWindow();
            hw.label.Content = "Моля, въведете ЕГН и желания код за достъп:";
            hw.actionButton.Visibility = Visibility.Hidden;
            hw.Show();
            this.Close();
        }

        private void buttonSetup_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow hw = new HelpWindow();
            hw.textBox.Visibility = Visibility.Hidden;
            hw.changeAccessCodeButton.Visibility = Visibility.Hidden;
            hw.Show();
            this.Close();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Сигурни ли сте, че искате да излезнете от приложението?", "CLOC", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Close();
                    break;
            }
        }
        private void buttonCredits_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Настоящият сайт бе разработен от Валентин Дренков, студент от ТУ - София, ФКСТ, специалност КСИ, IV курс, 51. група, факултетен номер: 121218025, като задание за дипломна работа." +
                "\nВсички права запазени.\n" +
                "Валентин Дренков, София, 03.03.2022");
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxAccessCode.Password.Length < 5)
            {
                passwordBoxAccessCode.Password += 1;
            }
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxAccessCode.Password.Length < 5)
            {
                passwordBoxAccessCode.Password += 2;
            }
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxAccessCode.Password.Length < 5)
            {
                passwordBoxAccessCode.Password += 3;
            }
        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxAccessCode.Password.Length < 5)
            {
                passwordBoxAccessCode.Password += 4;
            }
        }
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxAccessCode.Password.Length < 5)
            {
                passwordBoxAccessCode.Password += 5;
            }
        }
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxAccessCode.Password.Length < 5)
            {
                passwordBoxAccessCode.Password += 6;
            }
        }
        private void button7_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxAccessCode.Password.Length < 5)
            {
                passwordBoxAccessCode.Password += 7;
            }
        }
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxAccessCode.Password.Length < 5)
            {
                passwordBoxAccessCode.Password += 8;
            }
        }
        private void button9_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxAccessCode.Password.Length < 5)
            {
                passwordBoxAccessCode.Password += 9;
            }
        }
        private void buttonX_Click(object sender, RoutedEventArgs e)
        {
            passwordBoxAccessCode.Password = "";
        }
        private void button0_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxAccessCode.Password.Length < 5)
            {
                passwordBoxAccessCode.Password += 0;
            }
        }
        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxAccessCode.Password.Length > 0)
            {
                passwordBoxAccessCode.Password = passwordBoxAccessCode.Password.Remove(passwordBoxAccessCode.Password.Length - 1);
            }
        }
    }
}
