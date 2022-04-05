using Cloc.Classes;
using Cloc.Pages;
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

        public void ExitCurrentSession()
        {
            Logger.AddLog(Session.UserToken.GetLoginData(), "Изход от системата.");
            Session.UserToken.RemoveLoginData();
            ActivityPage.count = ActivityPage.COUNT;

            StartupWindow sw = new();
            sw.Show();
            this.Close();
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
            Main.Navigate(new Pages.MainPage());
        }

        private void ButtonAdminOptions_Click(object sender, RoutedEventArgs e)
        {
            AdminOptionsPage aop = new();

            if (aop.ComboBoxUsers != null)
            {
                aop.ComboBoxUsers.Items.Clear();
            }

            List<Person> people = SelectAllPeopleQuery();

            foreach (Person person in people)
            {
                aop.ComboBoxUsers.Items.Add(person.Name + " " + person.Surname + ", " + person.UCN);
            }

            Main.Navigate(aop);
        }

        private void ButtonProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfilePage pp = new();

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

        private void ButtonActivity_Click(object sender, RoutedEventArgs e)
        {
            ActivityPage ap = new();

            if (ap.ComboBoxUser != null)
            {
                ap.ComboBoxUser.Visibility = Visibility.Visible;
                ap.ComboBoxUser.Items.Clear();
            }

            List<Person> people = SelectAllPeopleQuery();

            foreach (Person person in people)
            {
                ap.ComboBoxUser.Items.Add(person.Name + " " + person.Surname + ", " + person.UCN);
            }

            Main.Navigate(ap);
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            ExitCurrentSession();
        }
    }
}
