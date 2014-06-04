using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Interaction logic for Maze.xaml
    /// </summary>
    public partial class MazeWindow : Window
    {
        private static MazeWindow _instance;
        public static MazeWindow GetInstance() { return _instance; }
        public enum MoveDirection { North, South, East, West };



        private Maze _maze;
        private int _currentTime;
        private DispatcherTimer _timer;
        private string _theme;
        private string _player;

        public Maze GetMaze() { return _maze; }

        public MazeWindow(Maze maze) 
        {
            InitializeComponent();
            _instance = this;
            _maze = maze;

            Title = "Maze - " + _maze.Rows + "x" + _maze.Columns;
            _theme = _maze.Theme;
            _player = _maze.Player;

            // size the canvas(es) to the maze size
            cvsMaze.Width = _maze.Columns * Room.ROOM_SIZE;
            cvsMaze.Height = cvsInformation.Height = _maze.Rows * Room.ROOM_SIZE;


            //add the maze to its canvas
            cvsMaze.Children.Add(_maze);
        }

        public MazeWindow(int maze_width, int maze_height, string theme, string player)
        {
            _instance = this;

            InitializeComponent();
            Title = "Maze - " + maze_width + "x" + maze_height;
            _theme = theme;
            _player = player;

            // size the canvas(es) to the maze size
            cvsMaze.Width = maze_width * Room.ROOM_SIZE;
            cvsMaze.Height = cvsInformation.Height = maze_height * Room.ROOM_SIZE; 
            

            // construct and add the maze to its canvas
            CreateMaze(maze_width, maze_height);


            // start the game timer
            StartTimer();

        } // end MazeWindow(width, height)



        private void CreateMaze(int maze_width, int maze_height)
        {
            _maze = new Maze();
            _maze.Initialize(maze_width, maze_height,                    // maze dimensions
                             0, 0,                                       // maze entrance
                             maze_width-1, maze_height-1,                // array of maze exit
                             _theme,                                     //maze background theme image
                             _player);                                   //player image

            cvsMaze.Children.Add(_maze);
        }



        private void StartTimer()
        {
            _currentTime = 0;

            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = "Time:  " + TimeSpan.FromSeconds(++_currentTime).ToString(@"m\:ss");
            lblAvgAnsTimeValue.Content = _maze.GetPlayer().Stats.AverageAnswerTime.ToString();
            lblQuestionsCorrectValue.Content = _maze.GetPlayer().Stats.QuestionsCorrect.ToString();
            lblQuestionsIncorrectValue.Content = _maze.GetPlayer().Stats.QuestionsIncorrect.ToString();
        }

        public void Win()
        {
            IsEnabled = false;
            _timer.Stop();

            //MessageBox.Show("You won in " + _currentTime + " seconds!", "Winner!");

            lblWinOrLose.Content = "You Win!";

            //Close();
        }

        public void Lose()
        {
            IsEnabled = false;
            _timer.Stop();

            //MessageBox.Show("You Lose!", "Loser!");

            lblWinOrLose.Content = "You Lose...";

            //Close();
        }
    }
}
