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
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private MazeWindow _mazeWindow;
        private string _themeImg = null;
        private string _playerImg = null;
        private double _startOpacity = 0.4, _pickedOpacity = 1.0;


        public SettingsWindow()
        {
            InitializeComponent();
            lblEnterMaze.IsEnabled = false;
        }

        private void btnEnterMaze_Click(object sender, RoutedEventArgs e)
        {
            _mazeWindow = new MazeWindow((int)this.sldWidth.Value, (int)this.sldHeight.Value, _themeImg, _playerImg);
            _mazeWindow.Show();
        }

        private void setTheme_Click(object sender, MouseButtonEventArgs e)
        {
            ((Image)sender).Opacity = ((Image)sender).Opacity == _startOpacity ? _pickedOpacity : _startOpacity; //change opacity of most recently picked image
            int start = ((Image)sender).Source.ToString().LastIndexOf(@"/") + 1;
            int stop = ((Image) sender).Source.ToString().LastIndexOf("4");
            _themeImg = ((Image)sender).Source.ToString().Substring(start, stop - start);

            if (_themeImg != null && _playerImg != null)
            {
                lblEnterMaze.Opacity = 1.0;
                lblEnterMaze.IsEnabled = true;
            }
        }

        private void setPlayer_Click(object sender, MouseButtonEventArgs e)
        {
            ((Image)sender).Opacity = ((Image)sender).Opacity == _startOpacity ? _pickedOpacity : _startOpacity; //change opacity of most recently picked image
            int start = ((Image)sender).Source.ToString().LastIndexOf(@"/") + 1;
            int stop = ((Image)sender).Source.ToString().IndexOf(@".");
            _playerImg = ((Image)sender).Source.ToString().Substring(start, stop - start);

            if (_themeImg != null && _playerImg != null)
            {
                lblEnterMaze.Opacity = 1.0;
                lblEnterMaze.IsEnabled = true;
            }
        }

        private void lblEnterMaze_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (lblEnterMaze.IsEnabled == true)
            {
                DoubleAnimation dblanim = new DoubleAnimation(1.0, 0.2, new Duration(TimeSpan.FromSeconds(1)));
                dblanim.RepeatBehavior = RepeatBehavior.Forever;
                dblanim.AutoReverse = true;
                lblEnterMaze.BeginAnimation(OpacityProperty, dblanim);
            }
        }
    }
}
