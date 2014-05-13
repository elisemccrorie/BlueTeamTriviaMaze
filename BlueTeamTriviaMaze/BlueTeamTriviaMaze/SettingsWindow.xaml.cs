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

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private MazeWindow maze;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void btnEnterMaze_Click(object sender, RoutedEventArgs e)
        {
            maze = new MazeWindow();
            maze.Show();
            this.Hide();
        }
    }
}
