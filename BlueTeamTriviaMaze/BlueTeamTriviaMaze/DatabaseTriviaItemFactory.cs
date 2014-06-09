//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: Responsible for querying the question db and returning
//  a unique trivia item that has not yet been used. However, if by 
//  some chance all the questions have been used, we allow for the
//  repeat of questions.

using System;
using System.Collections;
using System.Windows;
using System.Data.SQLite;
using System.IO;

namespace BlueTeamTriviaMaze
{
    public class DatabaseTriviaItemFactory : IDatabase, ITriviaItemFactory
    {
        private readonly string _dbTable = "questions";
        private SQLiteConnection _dbConn;
        private int _questionCount;

        public ArrayList _usedQuestions;                  // keeps track of used questions so as to not generate the same one twice
        public bool IsAlive{ get; private set; }
        public string DBFile { get; private set; }

        public DatabaseTriviaItemFactory()
        {
            DBFile = @"..\..\TriviaMaze.db";

            _usedQuestions = new ArrayList();

            Connect();  // stay connected!!
            CreateTable();
            _questionCount = getQuestionCount();
        }

        public void Destroy()
        {
            Disconnect();
        }

        public bool Connect()
        {
            try
            {
                if (!(new FileInfo(DBFile).Exists)) //if db file doesnt exists
                {
                    StreamWriter s = new StreamWriter(DBFile);
                    s.Close();
                }

                _dbConn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", DBFile));
                _dbConn.Open();
                IsAlive = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                IsAlive = false;
            }

            return IsAlive;
        }

        public bool Disconnect()
        {
            if (!IsAlive)
            {
                MessageBox.Show("Cannot disconnect, database connection is closed!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return IsAlive;
            }

            try
            {
                _dbConn.Close();
                IsAlive = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return IsAlive;
        }

        public bool CreateTable()
        {
            if (!IsAlive)
            {
                MessageBox.Show("Cannot CreateTable, database connection is closed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                string sql = String.Format("create table if not exists {0}("
                             + "id integer primary key autoincrement,"
                             + "type integer,"
                             + "type_disp text,"
                             + "question text,"
                             + "answer text,"
                             + "dummy_1 text,"
                             + "dummy_2 text,"
                             + "dummy_3 text,"
                             + "category integer,"
                             + "category_disp text,"
                             + "difficulty integer);", _dbTable);

                SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }

        private Hashtable Query()
        {
            if (!IsAlive)
            {
                MessageBox.Show("Cannot perform Query, database connection is closed!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }

            try
            {                
                //check if we have used all the questions, if so, allow repeats
                if(_usedQuestions.Count >= _questionCount)
                    _usedQuestions = new ArrayList();

                string notlist = ",";
                foreach (int u in _usedQuestions)   //get the list of used questions' id's
                    notlist += String.Format("{0},", u.ToString());
                notlist = notlist.Substring(0, notlist.Length - 1); //strip last comma

                string sql = String.Format("select * from {0} where id not in (0{1}) order by random() limit 1", _dbTable, notlist);

                SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
                SQLiteDataReader dr = command.ExecuteReader();
                Hashtable htbl = new Hashtable();

                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                        htbl.Add(dr.GetName(i), dr[i]);
                }

                dr.Close();

                return htbl;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return null;
        }

        //returns the number of questions in the db
        private int getQuestionCount()
        {
            if (!IsAlive)
            {
                MessageBox.Show("Cannot perform Query, database connection is closed!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return 0;
            }

            string sql = String.Format("select count(*) from {0}", _dbTable);
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            SQLiteDataReader dr = command.ExecuteReader();
            int count = 0;

            while (dr.Read())
                count = Convert.ToInt32(dr[0]);

            return count;
        }

        // Spits out a unique TriviaItem that hasn't been generated before (by keeping track of what was previously generated by this factory already)
        public TriviaItem GenerateTriviaItem()
        {
            if (!IsAlive)
            {
                MessageBox.Show("Cannot generate TriviaItem, database connection is closed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            TriviaItem _triviaItem = new TriviaItem();
            try
            {
                Hashtable query = Query();

                _triviaItem.Id = Convert.ToInt32(query["id"]);
                _usedQuestions.Add(_triviaItem.Id);  // STORE THIS ID SO WE CAN'T GENERATE THIS TRIVIAITEM AGAIN LATER BY SUCESSIVE CALLS!

                _triviaItem.QuestionType = (TriviaItem.Type) Convert.ToInt32(query["type"]);
                _triviaItem.Question = (string)query["question"];
                _triviaItem.Answer = (string)query["answer"];

                if (_triviaItem.QuestionType == TriviaItem.Type.TrueFalse)    //true/false question
                    _triviaItem.DummyAnswer = new string[] { (string)query["dummy_1"], null, null };
                else    //multiple choice question
                    _triviaItem.DummyAnswer = new string[] { (string)query["dummy_1"], (string)query["dummy_2"], (string)query["dummy_3"] };
                

                _triviaItem.Category = (string)query["category_disp"];

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return _triviaItem;
        }
    }
}
