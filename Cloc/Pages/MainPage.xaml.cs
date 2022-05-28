using Cloc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using static Cloc.Database.SelectQuery;

namespace Cloc.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        readonly System.Windows.Threading.DispatcherTimer Timer = new();
        readonly Person person = new();
        static string ucn;

        public MainPage()
        {
            try
            {
                ucn = Session.UserToken.GetLoginData();
                person = SelectPersonQuery(ucn);

                Greeter(person);

                Timer.Tick += new EventHandler(Timer_Click);
                Timer.Interval = new TimeSpan(0, 0, 1);
                Timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка при зареждане на данните.");
                ErrorLog.AddErrorLog(ex.ToString());
            }
            finally
            { InitializeComponent(); }
        }

        private void Greeter(Person person)
        {
            if (person.UCN != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                { GreetingTextBlock.Text = "Здравейте, " + person.Name + " " + person.Surname + "!"; }));
            }
            else
            {
                MessageBox.Show("Възникна неочаквана грешка при зареждане на данните.");
            }
        }

        private void Timer_Click(object sender, EventArgs e)

        {
            DateTime now = DateTime.Now;

            ClockLabel.Content = now.Hour + " : " + now.Minute + " : " + now.Second;
        }
    }
}
