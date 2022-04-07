using Cloc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
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
            { MessageBox.Show("Възникна неочаквана грешка при зареждане на данните."); }
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
                    TextBlockPosition.Text = "Position: " + Person.TranslateFromWorkPosition(person.Position);
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
    }
}
