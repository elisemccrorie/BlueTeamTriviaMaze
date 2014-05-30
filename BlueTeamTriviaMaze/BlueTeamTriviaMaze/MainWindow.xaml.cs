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
        private LoadWindow _loadWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void lblStart_Click(object sender, MouseButtonEventArgs e)
        {
            _settingsWindow = new SettingsWindow();
            _settingsWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation dblanim = new DoubleAnimation(1.0, 0.2, new Duration(TimeSpan.FromSeconds(1)));
            dblanim.RepeatBehavior = RepeatBehavior.Forever;
            dblanim.AutoReverse = true;
            lblStart.BeginAnimation(OpacityProperty, dblanim);
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            _loadWindow = new LoadWindow();
            _loadWindow.Show();
        }
    }
}
