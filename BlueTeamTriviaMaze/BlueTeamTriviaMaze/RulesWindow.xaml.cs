//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: Displays the rules for the Blue Team Trivia Maze

using System;
using System.Windows;

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Interaction logic for Rules.xaml
    /// </summary>
    public partial class RulesWindow : Window
    {
        public RulesWindow()
        {
            InitializeComponent();
            txtblkRules.Text = String.Format("Navigate through the maze by clicking a door in the same room as your character." +
                "A question will pop up, if answered correctly your character will move through the selected door. If answered"+
                " incorrectly the selected door will be locked for the duration of the game. The game is lost if you become"+
                " locked in a room or are locked out of the exit room. You win if you reach the exit.");
        }
    }
}
