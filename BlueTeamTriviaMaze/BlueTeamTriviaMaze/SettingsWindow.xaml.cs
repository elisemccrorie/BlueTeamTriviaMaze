//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: Allows the player to customize their gameplay
//  experience by selecting a character, theme, and maze
//  size (width and height)

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

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
        private string _styleName = null, _playerName = null;
        private double _startOpacity = 0.4, _pickedOpacity = 1.0;
        private readonly Label[] _styleLabels;
        private readonly Image[] _playerImages, _themeImages;

        //captures images in appropriate arrays in order to un/highlight them as they are un/selected
        public SettingsWindow()
        {
            InitializeComponent();

            _styleLabels = new Label[] { lblStyleOpen, lblStyleClassic };
            OpacityOff(_styleLabels);

            _playerImages = new Image[] {imgPlayerYoshi, imgPlayerSailboat, imgPlayerJohnny, 
                                                imgPlayerLink, imgPlayerRed, imgPlayerPurple};
            OpacityOff(_playerImages);

            _themeImages = new Image[] {imgCave, imgDungeon, imgForest, imgGreyRoom, imgBlueRoom, imgSea};
            OpacityOff(_themeImages);

            lblEnterMaze.IsEnabled = false;
        }

        //close this window when maze is entered, reopen when maze is closed
        private void btnEnterMaze_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            _mazeWindow = new MazeWindow((int)this.sldWidth.Value, (int)this.sldHeight.Value, _theme, _playerName, _styleName);
            _mazeWindow.Closed += new EventHandler(mazeWindow_Closed);
            _mazeWindow.Show();
        }

        private void mazeWindow_Closed(object sender, EventArgs e)
        {
            Show();
        }

        //highlight selected style
        private void setStyle_Click(object sender, MouseButtonEventArgs e)
        {
            OpacityOff(_styleLabels);
            ((Label)sender).Opacity = ((Label)sender).Opacity == _startOpacity ? _pickedOpacity : _startOpacity; //change opacity of most recently picked image
            _styleName = ((Label)sender).Content.ToString();

            //only enable entermaze button once all settings are selected
            if (_styleName != null && _themeName != null && _playerName != null)
            {
                lblEnterMaze.Opacity = 1.0;
                lblEnterMaze.IsEnabled = true;
            }
        }

        //highlight selected character
        private void setCharacter_Click(object sender, MouseButtonEventArgs e)
        {
            OpacityOff(_playerImages);
            ((Image)sender).Opacity = ((Image)sender).Opacity == _startOpacity ? _pickedOpacity : _startOpacity; //change opacity of most recently picked image
            int start = ((Image)sender).Source.ToString().LastIndexOf(@"/") + 1;
            int stop = ((Image)sender).Source.ToString().IndexOf(@".");
            _playerName = ((Image)sender).Source.ToString().Substring(start, stop - start);

            //only enable entermaze button once all settings are selected
            if (_styleName != null && _themeName != null && _playerName != null)
            {
                lblEnterMaze.Opacity = 1.0;
                lblEnterMaze.IsEnabled = true;
            }
        }
        
        //highlight selected theme
        private void setTheme_Click(object sender, MouseButtonEventArgs e)
        {
            OpacityOff(_themeImages);
            ((Image)sender).Opacity = ((Image)sender).Opacity == _startOpacity ? _pickedOpacity : _startOpacity; //change opacity of most recently picked image
            int start = ((Image)sender).Source.ToString().LastIndexOf(@"/") + 1;
            int stop = ((Image) sender).Source.ToString().LastIndexOf("4");
            _themeName = ((Image)sender).Source.ToString().Substring(start, stop - start);
            _theme = new Theme(_themeName);

            //only enable entermaze button once all settings are selected
            if (_styleName != null && _themeName != null && _playerName != null)
            {
                lblEnterMaze.Opacity = 1.0;
                lblEnterMaze.IsEnabled = true;
            }
        }

        private void OpacityOff(UIElement[] ctrls)
        {
            foreach (UIElement c in ctrls)
                c.Opacity = _startOpacity;
        }

        //only allow player to enter maze if both character and theme have been selected
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
