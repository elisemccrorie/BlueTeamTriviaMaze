using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    public class DatabaseTriviaItemFactory : Database, TriviaItemFactory
    {
        private readonly string _dbTable = "questions";
        private SQLiteConnection _dbConn;
        public bool IsAlive{ get; private set; }
        public string DBFile { get; private set; }

        public DatabaseTriviaItemFactory()
        {
            DBFile = @"..\..\TriviaMaze.db";
            Connect();
            CreateTable();
        }

        public bool Connect()
        {
            try
            {
                if (!(new FileInfo(DBFile).Exists))
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
                MessageBox.Show("Cannot create table, database connection is closed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private Hashtable Query(ArrayList usedID)
        {
            if (!IsAlive)
            {
                MessageBox.Show("Cannot perform select, database connection is closed!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }

            try
            {
                string notlist = ",";
                foreach (string u in usedID)
                    notlist += String.Format("{0},", u);
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

        public TriviaItem GenerateTriviaItem(ArrayList usedID)
        {
            TriviaItem _triviaItem = new TriviaItem();
            try
            {
                Hashtable query = Query(usedID);    //whose responsibility to remember used question ids?
                _triviaItem.Id = Convert.ToInt32(query["id"]);
                _triviaItem.Type = Convert.ToInt32(query["type"]);
                _triviaItem.Question = (string)query["question"];
                _triviaItem.Answer = (string)query["answer"];

                if (_triviaItem.Type == 0)    //true/false question
                    _triviaItem.DummyAnswer = new string[] { (string)query["dummy_1"], null, null };
                else
                {
                    _triviaItem.DummyAnswer = new string[] { (string)query["dummy_1"], (string)query["dummy_2"], (string)query["dummy_3"] };
                }

                _triviaItem.Category = (string)query["category_disp"];

                Disconnect();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return _triviaItem;
        }
    }
}
