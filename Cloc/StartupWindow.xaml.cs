using Cloc.AdditionalWindows;
using Cloc.Classes;
using System;
using System.Windows;
using System.Windows.Input;
using static Cloc.Classes.Validator;
using static Cloc.Database.DatabaseQuery;
using static Cloc.Session.UserToken;

namespace Cloc
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        static int count = 0;

        public StartupWindow()
        {
            InitializeComponent();

            PreviewKeyDown += new KeyEventHandler(HandleEsc);

            RefreshFiles();
        }

        private static void RefreshFiles()
        {
            if (count == 0)
            {
                if (!Logger.RefreshLogs()) { MessageBox.Show("Неуспешно актуализиране на файла с логове."); }
                if (!Checker.RefreshChecks()) { MessageBox.Show("Неуспешно актуализиране на файла с чекове."); }
                if (!Reporter.RefreshReports()) { MessageBox.Show("Неуспешно актуализиране на файла със справки."); }
                if (!ErrorLog.RefreshErrorLogs()) { MessageBox.Show("Неуспешно актуализиране на файла с грешки."); }

                count++;
            }
        }

        //TODO Final
        private void ButtonTest_Click(object sender, RoutedEventArgs e)
        {
            Person person = new();
            Person p1 = new();
            User user = new();
            User u1 = new();

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
            user.HourPayment = 0;
            user.TotalHours = 0;
            user.Percent = 0;

            p1.UCN = "6666666666";
            p1.Name = "Любомира";
            p1.Surname = "Петрова";
            p1.Email = "lpetrova@tu-sofia.bg";
            p1.PhoneNumber = "+359889153573";
            p1.Country = "България";
            p1.City = "Банско";
            p1.Address = "Струма 6";
            p1.Position = WorkPosition.Manager;

            u1.UserUCN = p1.UCN;
            u1.AccessCode = "66666";
            u1.CheckIn = DateTime.Now;
            u1.CheckOut = DateTime.Now.AddHours(1.7685);
            u1.IsCheckedIn = false;
            u1.HourPayment = 1234.897;
            u1.TotalHours = 10;
            u1.Percent = 20;

            //MessageBox.Show(StartupQuery("localhost", "root", "348_sha765_KaD3l", "3306", person, "77777").ToString());
            //MessageBox.Show(AddWorkerQuery(p1, u1).ToString());
            //Reporter.AddReport("0000000000", "Vivaldi", 1500);
            //MessageBox.Show(ChangeHourPaymentQuery("0000000000", 1000000).ToString());
            //MessageBox.Show(ErrorLog.AddErrorLog("Error test...").ToString());

        }

        private void CloseApp()
        {
            MessageBoxResult result = MessageBox.Show("Сигурни ли сте, че искате да излезнете от приложението?", "CLOC", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Close();
                    break;
            }
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                //TODO Final
                //CloseApp(); -> Uncomment
                Close(); // -> Delete
            }
        }
        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password != null)
            {
                if (ValidateEntry(PasswordBoxAccessCode.Password.ToString()))
                {
                    Person person = SelectPersonQuery(GetLoginData());
                    if (!Logger.AddLog(person.UCN, "Вход в системата."))
                    {
                        MessageBox.Show("Възникна грешка при записване на активността.");
                    }

                    if (IsAdmin(person))
                    {
                        AdminWindow aw = new();
                        Close();
                        aw.Show();
                    }
                    else
                    {
                        MainWindow mw = new();
                        Close();
                        mw.Show();
                    }
                }
                else
                {
                    PasswordBoxAccessCode.Password = string.Empty;
                }
            }
        }

        private void ButtonChangeAccessCode_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow hw = new();
            hw.InfoLabel.Content = "Моля, въведете ЕГН и желания код за достъп:";
            hw.ActionButton.Visibility = Visibility.Hidden;
            hw.ChangeAccessCodeButton.IsDefault = true;
            Close();
            hw.Show();
        }

        private void ButtonSetup_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow hw = new();
            hw.TextBoxUCN.Visibility = Visibility.Hidden;
            hw.ChangeAccessCodeButton.Visibility = Visibility.Hidden;
            hw.PasswordBox.MaxLength = 255;
            hw.ActionButton.IsDefault = true;
            Close();
            hw.Show();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            CloseApp();
        }

        private void ButtonCredits_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Настоящият сайт бе разработен от Валентин Димитров Дренков, студент от ТУ - София, ФКСТ, специалност КСИ, IV курс, 51-ва група, факултетен номер: 121218025, като задание за дипломна работа." +
                "\nВсички права запазени.\n" +
                "Валентин Дренков, София, 03.03.2022");
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password.Length < 5)
            {
                PasswordBoxAccessCode.Password += 1;
            }
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password.Length < 5)
            {
                PasswordBoxAccessCode.Password += 2;
            }
        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password.Length < 5)
            {
                PasswordBoxAccessCode.Password += 3;
            }
        }
        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password.Length < 5)
            {
                PasswordBoxAccessCode.Password += 4;
            }
        }
        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password.Length < 5)
            {
                PasswordBoxAccessCode.Password += 5;
            }
        }
        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password.Length < 5)
            {
                PasswordBoxAccessCode.Password += 6;
            }
        }
        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password.Length < 5)
            {
                PasswordBoxAccessCode.Password += 7;
            }
        }
        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password.Length < 5)
            {
                PasswordBoxAccessCode.Password += 8;
            }
        }
        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password.Length < 5)
            {
                PasswordBoxAccessCode.Password += 9;
            }
        }
        private void ButtonX_Click(object sender, RoutedEventArgs e)
        {
            PasswordBoxAccessCode.Password = string.Empty;
        }
        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password.Length < 5)
            {
                PasswordBoxAccessCode.Password += 0;
            }
        }
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxAccessCode.Password.Length > 0)
            {
                PasswordBoxAccessCode.Password = PasswordBoxAccessCode.Password.Remove(PasswordBoxAccessCode.Password.Length - 1);
            }
        }
    }
}
