//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: Displays a trivia item as a question, and handles
//  the players response to the question

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
using System.Windows.Media.Animation;

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Interaction logic for Question.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {

        public enum QuestionAnswer { Cancelled, Incorrect, Correct };


        private string _guess;
        TriviaItem _triviaItem;

        public QuestionAnswer Answer { get; private set; }


        public QuestionWindow(TriviaItem triviaItem)
        {
            InitializeComponent();

            Answer = QuestionAnswer.Cancelled;
            _triviaItem = triviaItem;
            
            questionLayout();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (_triviaItem.CheckAnswer(_guess))
                Answer = QuestionAnswer.Correct;
            else
                Answer = QuestionAnswer.Incorrect;

            Close();
        }

        private void rb_Checked(object sender, RoutedEventArgs e)
        {
            lblSubmit.IsEnabled = true;

            RadioButton rb = (RadioButton)sender;
            if (rb.IsChecked.Value)
                _guess = (string)rb.Content;
        }


        //prepares the trivia item content to be displayed on the window
        private void questionLayout()
        {
            txtblkQuestion.Text = _triviaItem.Question;
            string[] options = shuffleChoices();
            rbOptionOne.Content = options[0];
            rbOptionTwo.Content = options[1];
            rbOptionThree.Content = options[2];
            rbOptionFour.Content = options[3];

            if (_triviaItem.QuestionType == TriviaItem.Type.TrueFalse)
            {
                grdQuestion.Children.Remove(rbOptionThree);
                grdQuestion.Children.Remove(rbOptionFour);
            }
        }

        //suffles the possible answers of a question so that they don't always
        //appear in the same order
        private string[] shuffleChoices()
        {
            string[] mixed = new string[4];
            string[] choices = new string[] {_triviaItem.Answer, _triviaItem.DummyAnswer[0],
                                                _triviaItem.DummyAnswer[1], _triviaItem.DummyAnswer[2]};

            if (_triviaItem.QuestionType == TriviaItem.Type.TrueFalse)
            {
                int salt = new Random().Next(0, 2);    //random number 0 or 1

                for (int i = 0; i < choices.Length - 2; i++)    //i from 0 to 2
                {
                    mixed[i] = choices[salt];
                    salt = salt == 1 ? 0 : ++salt;  //count 0, 1 or 1, 0
                }

                mixed[2] = choices[2];
                mixed[3] = choices[3];
            }
            else if(_triviaItem.QuestionType == TriviaItem.Type.MultipleChoice)   //multiple choice
            {
                int salt = new Random().Next(0, 4);    //random number 0-3

                for (int i = 0; i < choices.Length; i++)    //i from 0 to 4
                {
                    mixed[i] = choices[salt];
                    salt = salt == 3 ? 0 : ++salt;  //count four times, if 3 is reached start back at 0
                }
            }

            return mixed;
        }

        //don't allow player to submit an answer until they have selected a choice
        private void lblSubmit_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (lblSubmit.IsEnabled)
            {
                DoubleAnimation dblanim = new DoubleAnimation(1.0, 0.2, new Duration(TimeSpan.FromSeconds(1)));
                dblanim.RepeatBehavior = RepeatBehavior.Forever;
                dblanim.AutoReverse = true;
                lblSubmit.BeginAnimation(OpacityProperty, dblanim);
            }
        }
    }
}
