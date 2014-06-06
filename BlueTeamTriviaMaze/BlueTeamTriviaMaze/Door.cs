﻿using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Runtime.Serialization;
using System.Diagnostics;


namespace BlueTeamTriviaMaze
{
    public class Door : Image
    {
        public const int DOOR_SIZE = 40;


        public enum State { Closed, Locked, Opened };
        public TriviaItem Question { get; private set; }

        private Theme _theme;

        private State _state;

        public State GetState() { return _state; }
        internal void SetState(State new_state)
        {
            _state = new_state;

            if (_state == State.Opened)
            {
                IsEnabled = true;  // door is clickable

                //animate the opening of the door via fading out
                DoubleAnimation dblAnim = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(1)));
                BeginAnimation(OpacityProperty, dblAnim);

                Source = _theme.DoorReturn;

                dblAnim = new DoubleAnimation(0.0, 1.0, new Duration(TimeSpan.FromSeconds(1)));
                BeginAnimation(OpacityProperty, dblAnim);
            }
            else if (_state == State.Locked)
            {
                Source = _theme.DoorLocked; // locked door image
                IsEnabled = false;  // door is NOT clickable (might change for key usage later)
                Opacity = 1.0;      // show the door
            }
            else if (_state == State.Closed)
            {
                Source = _theme.Door;  // closed door image
                IsEnabled = true;  // door is clickable
                Opacity = 1.0;     // show the door
            }
        } // end SetState(new_state)



        public Door(float x_index, float y_index, Theme theme, TriviaItem triviaItem)
        {
            MinWidth = MaxWidth = MinHeight = MaxHeight = Width = Height = DOOR_SIZE;
            _theme = theme;
            Question = triviaItem;

            SetState(State.Closed); // all doors start closed


            // this ABSOLUTELY positions the door
            Canvas.SetLeft(this, x_index * Room.ROOM_SIZE + Room.ROOM_SIZE/2 - DOOR_SIZE/2); // +ROOM_SIZE/2 to move the door onto the N,S,E,W edges of the room
            Canvas.SetTop(this, y_index * Room.ROOM_SIZE + Room.ROOM_SIZE/2 - DOOR_SIZE/2);  // -DOOR_SIZE/2 to center the door itself on the edge


            // connect the Click event
            MouseDown += new MouseButtonEventHandler(MazeWindow.GetInstance().GetMaze().GetPlayer().DoorClick);
        }

        public void Open()
        {
            if (GetState() != State.Opened)
                SetState(State.Opened);
        }

        public void Lock()
        {
            if (GetState() != State.Locked)
                SetState(State.Locked);
        }
        
        public bool TryToOpen(ref Statistics stats)
        {
            if (GetState() == State.Locked) // Door is locked, cannot be open
                return false;


            // run statistics on player responses
            DateTime start = DateTime.Now;

            // open the question window with this door's trivia item as the content
            QuestionWindow question_window = new QuestionWindow(Question);
            question_window.ShowDialog();


            if (question_window.Answer == QuestionWindow.QuestionAnswer.Correct) // Open the door on a correct answer
            {
                stats.DoorsOpen++;
                stats.QuestionsCorrect++;
                stats.AverageAnswerTime = (DateTime.Now.Subtract(start)).TotalSeconds;
                MazeWindow.GetInstance().UpdateStatistics();

                Open();
                return true;
            }
            else if (question_window.Answer == QuestionWindow.QuestionAnswer.Incorrect) // Lock the door on an incorrect answer
            {
                stats.DoorsLocked++;
                stats.QuestionsIncorrect++;
                stats.AverageAnswerTime = (DateTime.Now.Subtract(start)).TotalSeconds;
                MazeWindow.GetInstance().UpdateStatistics();

                Lock();
            }

            return false; // cancelled, don't touch the door's state
        }
    }
}
