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

namespace Cloc.AdditionalWindows
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                StartupWindow sw = new StartupWindow();
                sw.Show();
                this.Close();
            }
        }

        private void actionButton_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password=="1cm13*8vCt19_xRc")
            {
                MessageBox.Show("Внимание!\nМоля уверете се, че сте прочели README.txt файла, преди да продължите!");
                SetupWindow sw=new SetupWindow();
                sw.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Въвели сте грешна парола!");
                StartupWindow stw=new StartupWindow();
                stw.Show();
                this.Close();
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            StartupWindow stw = new StartupWindow();
            stw.Show();
            this.Close();
        }
    }
}
