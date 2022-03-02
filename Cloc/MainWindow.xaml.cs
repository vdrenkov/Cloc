﻿using System;
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

namespace Cloc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //TODO Program exit with Esc
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Navigate(new Pages.MainPage());
        }
        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new Pages.MainPage());
        }
        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new Pages.CheckPage());
        }
        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new Pages.ProfilePage());
        }
        private void btnLogs_Click(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new Pages.LogsPage());
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            StartupWindow sw = new StartupWindow();
            sw.Show();
            this.Close();
        }
    }
}
