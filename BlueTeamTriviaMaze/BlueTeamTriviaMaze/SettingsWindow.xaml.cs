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
        private string _themeName = null;
        private Theme _theme;
        private string _playerName = null;
        private double _startOpacity = 0.4, _pickedOpacity = 1.0;
        private readonly Image[] _playerImages, _themeImages;


        public SettingsWindow()
        {
            InitializeComponent();

            _playerImages = new Image[] {imgPlayerYoshi, imgPlayerSailboat, imgPlayerJohnny, 
                                                imgPlayerLink, imgPlayerRed, imgPlayerPurple};

            _themeImages = new Image[] {imgCave, imgDungeon, imgForest, imgGreyRoom, imgBlueRoom, imgSea};

            lblEnterMaze.IsEnabled = false;
        }

        private void btnEnterMaze_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            _mazeWindow = new MazeWindow((int)this.sldWidth.Value, (int)this.sldHeight.Value, _theme, _playerName);
            _mazeWindow.Closed += new EventHandler(mazeWindow_Closed);
            _mazeWindow.Show();
        }

        private void mazeWindow_Closed(object sender, EventArgs e)
        {
            Show();
        }

        private void setTheme_Click(object sender, MouseButtonEventArgs e)
        {
            opacityOff(_themeImages);
            ((Image)sender).Opacity = ((Image)sender).Opacity == _startOpacity ? _pickedOpacity : _startOpacity; //change opacity of most recently picked image
            int start = ((Image)sender).Source.ToString().LastIndexOf(@"/") + 1;
            int stop = ((Image) sender).Source.ToString().LastIndexOf("4");
            _themeName = ((Image)sender).Source.ToString().Substring(start, stop - start);
            _theme = new Theme(_themeName);
            if (_themeName != null && _playerName != null)
            {
                lblEnterMaze.Opacity = 1.0;
                lblEnterMaze.IsEnabled = true;
            }
        }

        private void setPlayer_Click(object sender, MouseButtonEventArgs e)
        {
            opacityOff(_playerImages);
            ((Image)sender).Opacity = ((Image)sender).Opacity == _startOpacity ? _pickedOpacity : _startOpacity; //change opacity of most recently picked image
            int start = ((Image)sender).Source.ToString().LastIndexOf(@"/") + 1;
            int stop = ((Image)sender).Source.ToString().IndexOf(@".");
            _playerName = ((Image)sender).Source.ToString().Substring(start, stop - start);

            if (_themeName != null && _playerName != null)
            {
                lblEnterMaze.Opacity = 1.0;
                lblEnterMaze.IsEnabled = true;
            }
        }

        private void opacityOff(Image[] images)
        {
            foreach (Image i in images)
                i.Opacity = _startOpacity;
        }

        private void lblEnterMaze_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (lblEnterMaze.IsEnabled)
            {
                DoubleAnimation dblanim = new DoubleAnimation(1.0, 0.2, new Duration(TimeSpan.FromSeconds(1)));
                dblanim.RepeatBehavior = RepeatBehavior.Forever;
                dblanim.AutoReverse = true;
                lblEnterMaze.BeginAnimation(OpacityProperty, dblanim);
            }
        }
    }
}
