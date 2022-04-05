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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Cloc.Database.DatabaseQuery;

namespace Cloc.Pages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        Person person = new();
        User user = new();
        static string UCN;

        public ProfilePage()
        {
            try
            {
                UCN = Session.UserToken.GetLoginData();

                FillData(UCN);
            }
            catch (Exception)
            { MessageBox.Show("Възникна неочаквана грешка при зареждане на данните!"); }
            finally
            {
                InitializeComponent();
            }
        }

        private void FillData(string ucn)
        {
            try
            {
                person = SelectPersonQuery(ucn);
                user = SelectUserQuery(ucn);

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TextBlockUCN.Text = "UCN: " + person.UCN;
                    TextBlockName.Text = "Name: " + person.Name;
                    TextBlockSurname.Text = "Surname: " + person.Surname;
                    TextBlockEmail.Text = "Email: " + person.Email;
                    TextBlockPhoneNumber.Text = "Phone number: " + person.PhoneNumber;
                    TextBlockCountry.Text = "Country: " + person.Country;
                    TextBlockCity.Text = "City: " + person.City;
                    TextBlockAddress.Text = "Address: " + person.Address;
                    TextBlockPosition.Text = "Position: " + TranslateWorkPosition(person.Position);
                    TextBlockHourPayment.Text = "Hour payment: " + user.HourPayment.ToString();
                    TextBlockPercent.Text = "Percent: " + user.Percent.ToString();
                }));
            }
            catch (Exception) { }
        }

        private void ComboBoxPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string personInfo = ComboBoxPerson.SelectedItem.ToString();
                string[] split = personInfo.Split(", ");

                FillData(split[1]);

                if (Session.UserToken.GetLoginData() != split[1])
                { Logger.AddLog(Session.UserToken.GetLoginData(), "Преглед данните на профила на " + split[0] + "."); }
            }
            catch (Exception)
            {
                MessageBox.Show("Възникна неочаквана грешка при изпълнението на вашата заявка.");
            }
        }

        private static string TranslateWorkPosition(WorkPosition wp)
        {
            return wp switch
            {
                WorkPosition.Hygienist => "Хигиенист/ка",
                WorkPosition.PrepCook => "Заготовщик",
                WorkPosition.LineCook => "Готвач поточна линия",
                WorkPosition.Cook => "Готвач",
                WorkPosition.SousChef => "Помощник главен готвач",
                WorkPosition.Chef => "Главен готвач",
                WorkPosition.Bartender => "Барман",
                WorkPosition.Barmaid => "Барманка",
                WorkPosition.Barista => "Бариста",
                WorkPosition.Waiter => "Сервитьор",
                WorkPosition.Waitress => "Сервитьорка",
                WorkPosition.Busser => "Помощник-сервитьор/ка",
                WorkPosition.Host => "Хост",
                WorkPosition.Hostess => "Хостеса",
                WorkPosition.FnB => "Специалист храни и напитки",
                WorkPosition.Manager => "Мениджър",
                WorkPosition.Admin => "Администратор / Собственик",
                _ => "На изпитателен срок",
            };
        }
    }
}
