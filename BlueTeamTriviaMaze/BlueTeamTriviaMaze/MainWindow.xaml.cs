//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: The first display window when the game is started. Used
//  to start the game, read the rules, or view the about.

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

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
            RulesWindow rules = new RulesWindow();
            rules.ShowDialog();
        }

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }
    }
}
