//Author: BlueTeam
//Class: Spring 2014 CSCD 350-01
//Description: this is the 
//  description and here is
//  another line

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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsWindow _settingsWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void lblStart_Click(object sender, MouseButtonEventArgs e)
        {
            Hide();
            _settingsWindow = new SettingsWindow();
            _settingsWindow.Closed += new EventHandler(settingsWindow_Closed);
            _settingsWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation dblanim = new DoubleAnimation(1.0, 0.2, new Duration(TimeSpan.FromSeconds(1)));
            dblanim.RepeatBehavior = RepeatBehavior.Forever;
            dblanim.AutoReverse = true;
            lblStart.BeginAnimation(OpacityProperty, dblanim);
        }

        private void settingsWindow_Closed(object sender, EventArgs e)
        {
            Show();
        }

        private void miRules_Click(object sender, RoutedEventArgs e)
        {
            Rules rules = new Rules();
            rules.ShowDialog();
        }

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }
    }
}
