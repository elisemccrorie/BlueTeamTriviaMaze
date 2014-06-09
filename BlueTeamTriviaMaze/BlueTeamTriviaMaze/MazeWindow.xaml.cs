//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: The main window for gameplay. A container for the maze
//  and the statistics side panel. 

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Interaction logic for Maze.xaml
    /// </summary>
    public partial class MazeWindow : Window
    {
        private static MazeWindow _instance;
        private Maze _maze;
        private int _currentTime;
        private DispatcherTimer _timer;
        private Theme _theme;
        private string _player;

        public static MazeWindow GetInstance() { return _instance; }
        public enum MoveDirection { North, South, East, West };
        public Maze GetMaze() { return _maze; }

        public MazeWindow(int maze_width, int maze_height, Theme theme, string player)
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

        } // end MazeWindow(width, height, theme, player)



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
        }

        public void UpdateStatistics()
        {
            lblAvgAnsTimeValue.Content = _maze.GetPlayer().Stats.AverageAnswerTime.ToString() + " s";
            lblQuestionsCorrectValue.Content = _maze.GetPlayer().Stats.QuestionsCorrect.ToString();
            lblQuestionsIncorrectValue.Content = _maze.GetPlayer().Stats.QuestionsIncorrect.ToString();
        }

        private void GameOver()
        {
            IsEnabled = false;
            _timer.Stop();

            // make the label flash
            DoubleAnimation dblanim = new DoubleAnimation(1.0, 0.2, new Duration(TimeSpan.FromSeconds(1.0)));
            dblanim.RepeatBehavior = RepeatBehavior.Forever;
            dblanim.AutoReverse = true;
            lblWinOrLose.BeginAnimation(OpacityProperty, dblanim);
        }

        public void Win()
        {
            GameOver();

            lblWinOrLose.Content = "You Win!";
            lblWinOrLose.Foreground = Brushes.Green;
        }

        public void Lose()
        {
            GameOver();

            lblWinOrLose.Content = "You Lose...";
            lblWinOrLose.Foreground = Brushes.Red;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
