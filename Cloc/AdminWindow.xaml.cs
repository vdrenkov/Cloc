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
using static Cloc.Database.DatabaseQuery;

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
                StartupWindow sw = new();
                sw.Show();
                this.Close();
                Session.UserToken.RemoveLoginData();
            }
        }
        private void ButtonMain_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new Pages.MainPage());
        }
        private void ButtonBossOptions_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new Pages.AdminOptionsPage());
        }

        private void ButtonProfile_Click(object sender, RoutedEventArgs e)
        {
            Pages.ProfilePage pp = new();

            if (pp.ComboBoxPerson != null)
            {
                pp.ComboBoxPerson.Visibility = Visibility.Visible;
                pp.ComboBoxPerson.Items.Clear();
            }

            List<Person> people = SelectAllPeopleQuery();

            foreach (Person person in people)
            {
                    pp.ComboBoxPerson.Items.Add(person.Name + " " + person.Surname + ", " + person.UCN);
            }

            Main.Navigate(pp);
        }

        private void ButtonLogs_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new Pages.ActivityPage());
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            StartupWindow sw = new();
            sw.Show();
            this.Close();
            Session.UserToken.RemoveLoginData();
        }
    }
}
