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
        private QuestionWindow question;
        private DoubleAnimation Anim;

        public MazeWindow()
        {
            InitializeComponent();
            this.Closing += new CancelEventHandler(exit);
        }

        private void btnQuestion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                question = new QuestionWindow();
                question.Closed += new EventHandler(displayAnswer);
                //RemoveVisualChild(question);
                //AddVisualChild(question);   //problems
                question.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void displayAnswer(object sender, EventArgs e)
        {
            lblAnswerVal.Content = question.Answer.ToString();

            if (question.Answer)
            {
                double left = Canvas.GetLeft(rect);
                Anim = new DoubleAnimation(left, 120 + left, new Duration(TimeSpan.FromSeconds(3)));
                rect.BeginAnimation(Canvas.LeftProperty, Anim);
            }

            question = null;
        }

        private void exit(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
