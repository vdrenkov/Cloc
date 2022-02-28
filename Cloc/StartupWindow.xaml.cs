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

namespace Cloc
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += 1;
        }
        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += 2;
        }
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += 3;
        }
        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += 4;
        }
        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += 5;
        }
        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += 6;
        }
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += 7;
        }
        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += 8;
        }
        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += 9;
        }
        private void btnX_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text ="";
        }
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += 0;
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text = tb1.Text.Remove(tb1.Text.Length - 1);
        }
    }
}
