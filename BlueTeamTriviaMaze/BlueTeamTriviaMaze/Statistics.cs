//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: Collects statistics on the players gameplay

using System;

namespace BlueTeamTriviaMaze
{
    public class Statistics
    {
        private double _totalAnswerTime, _averageAnswerTime;
        public int DoorsOpen { get; set; }
        public int DoorsLocked { get; set; }
        public int QuestionsCorrect { get; set; }
        public int QuestionsIncorrect { get; set; }

        public double AverageAnswerTime 
        {
            get
            {
                if (QuestionsCorrect + QuestionsIncorrect == 0)
                    return 0;

                return _averageAnswerTime;
            }
            set //input is seconds
            {
                if ((QuestionsCorrect + QuestionsIncorrect) == 0)
                    _averageAnswerTime = 0;
                else
                {
                    _totalAnswerTime += value;
                    //output is seconds
                    _averageAnswerTime = Math.Round((_totalAnswerTime / (QuestionsCorrect + QuestionsIncorrect)), 2);
                }
            }
        }//end AverageAnswerTime

        public Statistics()
        {
            _totalAnswerTime = 0;
            DoorsOpen = 0;
            DoorsLocked = 0;
            QuestionsCorrect = 0;
            QuestionsIncorrect = 0;
            AverageAnswerTime = 0;
        }
    }
}
