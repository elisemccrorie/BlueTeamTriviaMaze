using System;
using System.Collections;
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
    /// Interaction logic for Question.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {
        public static int ANSWER_CANCELLED = -1,
            ANSWER_INCORRECT = 0,
            ANSWER_CORRECT = 1;

        public static int TYPE_TRUE_FALSE = 0,
            TYPE_MULTIPLE_CHOICE = 1;



        private string _guess;
        TriviaItem _triviaItem;
        DatabaseTriviaItemFactory _dtif;

        public int Answer { get; private set; }



        public QuestionWindow()
        {
            InitializeComponent();

            Answer = ANSWER_CANCELLED;
            _dtif = new DatabaseTriviaItemFactory();
            _triviaItem = _dtif.GenerateTriviaItem(new ArrayList());
            
            questionLayout();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (_guess.Equals(_triviaItem.Answer))
                Answer = ANSWER_CORRECT;
            else
                Answer = ANSWER_INCORRECT;

            this.Close();
        }

        private void rb_Checked(object sender, RoutedEventArgs e)
        {
            btnSubmit.IsEnabled = true;

            RadioButton rb = (RadioButton)sender;
            if (rb.IsChecked.Value)
                _guess = (string)rb.Content;
        }

        private void questionLayout()
        {
            txtblkQuestion.Text = _triviaItem.Question;
            string[] options = shuffleChoices();
            rbOptionOne.Content = options[0];
            rbOptionTwo.Content = options[1];
            rbOptionThree.Content = options[2];
            rbOptionFour.Content = options[3];

            if (_triviaItem.Type == TYPE_TRUE_FALSE)
            {
                grdQuestion.Children.Remove(rbOptionThree);
                grdQuestion.Children.Remove(rbOptionFour);
            }
        }

        private string[] shuffleChoices()
        {
            string[] mixed = new string[4];
            string[] choices = new string[] {_triviaItem.Answer, _triviaItem.DummyAnswer[0],
                                                _triviaItem.DummyAnswer[1], _triviaItem.DummyAnswer[2]};

            if (_triviaItem.Type == TYPE_TRUE_FALSE)
            {
                int salt = DateTime.Now.Millisecond % 2;

                for (int i = 0; i < choices.Length - 2; i++)
                {
                    mixed[i] = choices[salt];
                    salt = salt == 1 ? 0 : ++salt;
                }

                mixed[2] = choices[2];
                mixed[3] = choices[3];
            }
            else if(_triviaItem.Type == TYPE_MULTIPLE_CHOICE)   //multiple choice
            {
                int salt = DateTime.Now.Millisecond % 4;

                for (int i = 0; i < choices.Length; i++)
                {
                    mixed[i] = choices[salt];
                    salt = salt == 3 ? 0 : ++salt;
                }
            }

            return mixed;
        }
    }
}
