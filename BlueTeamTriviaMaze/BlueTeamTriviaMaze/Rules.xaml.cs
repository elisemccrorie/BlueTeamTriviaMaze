using System;
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
    /// Interaction logic for Rules.xaml
    /// </summary>
    public partial class Rules : Window
    {
        public Rules()
        {
            InitializeComponent();
            txtblkRules.Text = String.Format("Navigate through the maze by clicking a door in the same room as your character." +
                "A question will pop up, if answer correctly your character will move through the selected door. If answered"+
                " incorrectly the selected door will be locked for the duration of the game. The game is lost if you become"+
                " locked in a room or are locked out of the exit room. You win if you reach the exit.");
        }
    }
}
