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
using static Cloc.Session.UserToken;

//CheckOut Button, брой логове/чекове, AddLog, AddCheck
/*
CheckIn + IsCheckedIn (true) -> DB
CheckOut + IsCheckedIn (false) -> DB
CheckOut - CheckIn -> DB (TotalHours)
UCN + CheckIn + CheckOut -> Checks.txt
DateTime + UCN + Activity -> Logs.txt
AddLog + AddCheck -> StartupQuery
Language menu

StartupWindow -> Exit confirmation uncomment, delete test button
Access Password -> 1cm13*8vCt19_xRc
DB Password -> 348_sha765_KaD3l
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
            Person person = new Person();
            Person p1=new Person();
            User user = new User();
            User u1=new User();
    
            person.UCN = "9902130044";
            person.Name = "Валентин";
            person.Surname = "Дренков";
            person.Email = "vdrenkov@tu-sofia.bg";
            person.PhoneNumber = "+359888992278";
            person.Country = "България";
            person.City = "Разлог";
            person.Address = "Цар Иван Асен II 5";
            person.Position = WorkPosition.Admin;

            user.UserUCN = person.UCN;
            user.AccessCode = "77777";
            user.CheckIn = DateTime.Now;
            user.CheckOut = DateTime.Now;
            user.HourPayment = 1;
            user.TotalHours = 1;
            user.Percent = 1;

            p1.UCN = "9988776655";
            p1.Name = "Любомира";
            p1.Surname = "Петрова";
            p1.Email = "lpetrova@tu-sofia.bg";
            p1.PhoneNumber = "+359889153573";
            p1.Country = "България";
            p1.City = "Банско";
            p1.Address = "Струма 6";
            p1.Position = WorkPosition.Manager;

            u1.UserUCN = p1.UCN;
            u1.AccessCode = "55555";
            u1.CheckIn = DateTime.Now;
            u1.CheckOut = DateTime.Now.AddHours(7);
            u1.IsCheckedIn = false;
            u1.HourPayment = 11;
            u1.TotalHours = 13;
            u1.Percent = 13;

            MessageBox.Show(Logger.AddLog(u1.UserUCN, "Logging...").ToString());
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
                Person person = SelectPersonQuery(GetLoginData());

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
