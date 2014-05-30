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

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Interaction logic for Maze.xaml
    /// </summary>
    public partial class MazeWindow : Window
    {
        private static MazeWindow _instance;
        public static MazeWindow GetInstance() { return _instance; }
        


        private Maze _maze;
        private int _currentTime;
        private QuestionWindow _questionWindow;
        private string _theme;
        private string _player;

        public Maze GetMaze() { return _maze; }



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
                             new int[,] {{maze_width-1, maze_height-1},  // array of maze exits- (x,y) pairs
                                         {0, maze_height-2}},
                             _theme,                                     //maze background theme image
                             _player);                                   //player image

            cvsMaze.Children.Add(_maze);
        }



        private void StartTimer()
        {
            _currentTime = 0;

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = "Time:  " + TimeSpan.FromSeconds(++_currentTime).ToString(@"m\:ss");
        }

        
        private void btnQuestion_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                _questionWindow = new QuestionWindow();
                //_questionWindow.Closed += new EventHandler(displayAnswer);

                _questionWindow.ShowDialog();

                if (_questionWindow.Answer == QuestionWindow.ANSWER_CORRECT)
                    _maze.GetPlayer().DoorClick(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void displayAnswer(object sender, EventArgs e)
        {
            //if (_questionWindow.Answer == QuestionWindow.ANSWER_CANCELLED)
            //    lblAnswerVal.Content = "Cancelled!";
            //else if (_questionWindow.Answer == QuestionWindow.ANSWER_INCORRECT)
            //    lblAnswerVal.Content = "Incorrect!";

            //else
            //{
            //    lblAnswerVal.Content = "Correct!";

            //    // animation
            //    double left = Canvas.GetLeft(rect);
            //    Anim = new DoubleAnimation(left, 120 + left, new Duration(TimeSpan.FromSeconds(3)));
            //    rect.BeginAnimation(Canvas.LeftProperty, Anim);
            //}

            


            //_questionWindow = null;
        }
    }
}
