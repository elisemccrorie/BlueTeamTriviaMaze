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
        private string _style;

        public static MazeWindow GetInstance() { return _instance; }
        public enum MoveDirection { North, South, East, West };
        public Maze GetMaze() { return _maze; }

        public MazeWindow(int maze_width, int maze_height, Theme theme, string player, string style)
        {
            _instance = this;

            InitializeComponent();
            Title = "Maze - " + maze_width + "x" + maze_height;
            _style = style;

            // size the canvas(es) to the maze size
            cvsMaze.Width = maze_width * Room.ROOM_SIZE;
            cvsMaze.Height = cvsInformation.Height = maze_height * Room.ROOM_SIZE; 
            

            // construct and add the maze to its canvas
            CreateMaze(maze_width, maze_height, _style, theme, player);

            GetMaze().GetPlayer().Keys = 3;

            // start the game timer
            StartTimer();

        } // end MazeWindow(width, height, theme, player)

        private void CreateMaze(int maze_width, int maze_height, string style, Theme theme, string player)
        {
            _maze = new Maze();

            if (style.ToLower() == "classic")
            {
                MazeBuilder mb = new MazeBuilder(ref _maze, maze_width, maze_height, theme, player);
                _maze = mb.GetPlainMaze();
                _maze = mb.GetTrueMaze(ref _maze);
            }
            else if (style.ToLower() == "open")
            {
                _maze.Initialize(maze_width, maze_height,                    // maze dimensions
                                 0, 0,                                       // maze entrance
                                 maze_width - 1, maze_height - 1,            // maze exit
                                 theme,                                     // maze background theme image
                                 player);                                   //player image
            }

            //make sure start room doors are visible
            _maze.GetRoom(0, 0).EastDoor.Opacity = 1.0;
            _maze.GetRoom(0, 0).SouthDoor.Opacity = 1.0;

            cvsMaze.Children.Add(_maze);
        }//end CreateMAze(int, int, string)

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
            //if a key has been used
            DisableKey(GetMaze().GetPlayer().Keys);

            lblAvgAnsTimeValue.Content = _maze.GetPlayer().Stats.AverageAnswerTime.ToString() + " s";
            lblQuestionsCorrectValue.Content = _maze.GetPlayer().Stats.QuestionsCorrect.ToString();
            lblQuestionsIncorrectValue.Content = _maze.GetPlayer().Stats.QuestionsIncorrect.ToString();
        }

        private void DisableKey(int numKeys)
        {
            if (numKeys <= 2)
                imgKey1.Opacity = 0.4;

            if (numKeys <= 1)
                imgKey2.Opacity = 0.4;

            if (numKeys <= 0)
                imgKey3.Opacity = 0.4;
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
            CheckDeadEnds();

            lblWinOrLose.Content = "You Lose...";
            lblWinOrLose.Foreground = Brushes.Red;
        }

        private void CheckDeadEnds()
        {
            if (_style.ToLower() == "classic")
            {
                bool closedDoors = false;

                // check if there exists at least 1 door that is closed and may be potentially opened
                // Cord: optimized to bail once one closed door is found
                for (int i = 0; i < _maze.Rows && !closedDoors; i++)
                {
                    for (int j = 0; j < _maze.Columns; j++)
                    {
                        Room r = _maze.GetRoom(j, i);
                        if (r.GetState() == Room.State.Visited)
                        {
                            if (r.NorthDoor != null)
                                if (r.NorthDoor.GetState() == Door.State.Closed)
                                {
                                    closedDoors = true;
                                    break;
                                }

                            if (r.SouthDoor != null)
                                if (r.SouthDoor.GetState() == Door.State.Closed)
                                {
                                    closedDoors = true;
                                    break;
                                }

                            if (r.EastDoor != null)
                                if (r.EastDoor.GetState() == Door.State.Closed)
                                {
                                    closedDoors = true;
                                    break;
                                }

                            if (r.WestDoor != null)
                                if (r.WestDoor.GetState() == Door.State.Closed)
                                {
                                    closedDoors = true;
                                    break;
                                }
                        }
                    }
                }

                //if the exit room doesnt have both doors locked, and the player 
                //is at a dead end, or all paths lead to dead ends
                if (!(_maze.GetExitRoom().NorthDoor.GetState() == Door.State.Locked
                    && _maze.GetExitRoom().WestDoor.GetState() == Door.State.Locked)
                    && closedDoors)
                {
                    DeadEndWindow d = new DeadEndWindow();
                    d.txblkDeadEnd.Text = "All other paths lead to a dead end. No path to the exit remains!";
                    d.ShowDialog();
                }
            }
        }//end CheckDeadEnds

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
