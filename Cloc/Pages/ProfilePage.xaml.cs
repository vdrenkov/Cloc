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
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при зареждане на данните.");
                ErrorLog.AddErrorLog(ex.ToString());
            }
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
                    TextBlockUCN.Text = "ЕГН: " + person.UCN;
                    TextBlockName.Text = "Име: " + person.Name;
                    TextBlockSurname.Text = "Фамилия: " + person.Surname;
                    TextBlockEmail.Text = "Имейл: " + person.Email;
                    TextBlockPhoneNumber.Text = "Телефонен номер: " + person.PhoneNumber;
                    TextBlockCountry.Text = "Държава: " + person.Country;
                    TextBlockCity.Text = "Град: " + person.City;
                    TextBlockAddress.Text = "Адрес: " + person.Address;
                    TextBlockPosition.Text = "Позиция: " + Person.TranslateFromWorkPosition(person.Position);
                    TextBlockHourPayment.Text = "Часова ставка: " + Math.Round(user.HourPayment, 2).ToString() + " лева";
                    TextBlockPercent.Text = "Бонус процент: " + Math.Round(user.Percent, 2).ToString()+"%";
                }));
            }
            catch (Exception ex)
            {
                ErrorLog.AddErrorLog(ex.ToString());
            }
        }

        private void ComboBoxPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ComboBoxPerson.SelectedItem != null)
                {
                    string personInfo = ComboBoxPerson.SelectedItem.ToString();
                    string[] split = personInfo.Split(", ");
                    string name = split[0];
                    string ucn = split[1];

                    FillData(ucn);

                    if (Session.UserToken.GetLoginData() != ucn)
                    {
                        if (!Logger.AddLog(Session.UserToken.GetLoginData(), "Преглед данните на профила на " + name + "."))
                        {
                            MessageBox.Show("Възникна грешка при записване на активността.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при изпълнението на вашата заявка.");
                ErrorLog.AddErrorLog(ex.ToString());
            }
        }
    }
}
