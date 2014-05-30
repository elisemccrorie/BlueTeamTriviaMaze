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
using System.Data;
using System.IO;
using System.Xml;
using System.Windows.Markup;


namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {

        public LoadWindow()
        {
            InitializeComponent();
            DatabaseSavedGames game = new DatabaseSavedGames();
            dgvSavedGames.ItemsSource = game.Load().AsDataView();
        }

        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            //credit to http://shrinandvyas.blogspot.com/2011/08/wpf-how-to-deep-copy-wpf-object-eg.html

            var xaml = ((DataRowView)dgvSavedGames.Items[0]).Row.ItemArray[2].ToString();
            var xamlString = new StringReader(xaml);
            var xmlTextReader = new XmlTextReader(xamlString);
            Maze maze = (Maze)XamlReader.Load(xmlTextReader);
            MazeWindow mazeWnd = new MazeWindow(maze);
            mazeWnd.Show();
        }
    }
}
