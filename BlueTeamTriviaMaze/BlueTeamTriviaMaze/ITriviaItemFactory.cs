//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: An interface for a trivia item factory to adhere to

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueTeamTriviaMaze
{
    public interface ITriviaItemFactory
    {
        TriviaItem GenerateTriviaItem();
        void Destroy();
    }
}
