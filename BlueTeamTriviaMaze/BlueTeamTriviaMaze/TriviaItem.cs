//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: A container for the trivia info returned by a 
//  database trivia item factor. Used for display in a question
//  window.

using System;

namespace BlueTeamTriviaMaze
{
    public class TriviaItem
    {

        public enum Type { TrueFalse, MultipleChoice };

        public int Id { get; set; }
        public Type QuestionType { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string[] DummyAnswer { get; set; }
        public string Category { get; set; }


        public bool CheckAnswer(String choice) 
        {
            return Answer.Equals(choice);
        }
    }
}