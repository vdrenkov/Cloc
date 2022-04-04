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
        readonly Person person = new();
        readonly User user = new();
        static string ucn;

        public ProfilePage()
        {
            try
            {
                ucn = Session.UserToken.GetLoginData();
                person = SelectPersonQuery(ucn);
                user = SelectUserQuery(ucn);

                FillData(person, user);
            }
            catch (Exception)
            { MessageBox.Show("Възникна неочаквана грешка при зареждане на данните!"); }
            finally
            {
                InitializeComponent();
            }
        }

        private void FillData(Person person, User user)
        {
            try
            {
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
                    TextBlockPosition.Text = "Position: " + person.Position.ToString();
                    TextBlockHourPayment.Text = "Hour payment: " + user.HourPayment.ToString();
                    TextBlockPercent.Text = "Percent: " + user.Percent.ToString();
                }));
            }
            catch (Exception) { }
        }

        private void ComboBoxPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = (e.AddedItems[0] as ComboBoxItem).Content.ToString();//null reference
            MessageBox.Show(str);
        }
    }
}
