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
        private QuestionDB DB;
        public int Id { get; private set; }
        public int Type { get; private set; }
        public string Question { get; private set; }
        public string Answer { get; private set; }
        public string[] DummyAnswer { get; private set; }
        public string Category { get; private set; }

        private void getDBQuestion()
        {
            //get question from database
            //Question = question column from db
            //Answer = answer column from db
            //...and so on
            try
            {
                DB = new QuestionDB();
                Hashtable query = DB.Query();
                Id = Convert.ToInt32(query["id"]);   
                Type = Convert.ToInt32(query["type"]); 
                Question = (string)query["question"];
                Answer = (string)query["answer"];

                if (Type == 0)    //true/false question
                    DummyAnswer = new string[] { (string)query["dummy_1"] , null, null};
                else
                {
                    DummyAnswer = new string[] { (string)query["dummy_1"], (string)query["dummy_2"], (string)query["dummy_3"] };
                }

                Category = (string)query["category_disp"];

                DB.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public TriviaItem()
        {
            getDBQuestion();
        }
    }
}