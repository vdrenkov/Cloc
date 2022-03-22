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
using System.Windows.Threading;
using static Cloc.Database.DatabaseQuery;

namespace Cloc.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        public MainPage()
        {
            Greeter();

            Timer.Tick += new EventHandler(Timer_Click);

            Timer.Interval = new TimeSpan(0, 0, 1);

            Timer.Start();

            InitializeComponent();
        }

        private void Greeter()
        {
            try
            {
                string ucn = Session.UserToken.GetLoginData();
                Person person = SelectPersonQuery(ucn);

                if (person.UCN != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => 
                    {   greetingTextBlock.Text = "Здравейте, " + person.Name + " " + person.Surname + "!";}));
                }
            }
            catch(Exception) { }
        }

        private void Timer_Click(object sender, EventArgs e)

        {
            DateTime now= DateTime.Now;

            clockLabel.Content = now.Hour + " : " + now.Minute + " : " + now.Second;
        }
    }
}
