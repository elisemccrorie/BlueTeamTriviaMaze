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
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace BlueTeamTriviaMaze
{
    public class TriviaItem
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string[] DummyAnswer { get; set; }
        public string Category { get; set; }

        public TriviaItem()
        {
            //default constructor
            //TriviaItem is a container filled by
            //the TriviaItemFactory
        }

        public bool CheckAnswer(String choice) 
        {
            return choice == Answer;
        }
    }
}