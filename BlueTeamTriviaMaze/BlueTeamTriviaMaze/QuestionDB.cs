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
    class QuestionDB : Database
    {
        private readonly string _dbTable = "questions";
        private SQLiteConnection _dbConn;
        public bool IsAlive{ get; private set; }
        public string DBFile { get; private set; }

        public QuestionDB()
        {
            DBFile = "TriviaMaze.db";
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

        public bool Close()
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

        public Hashtable Query()
        {
            if (!IsAlive)
            {
                MessageBox.Show("Cannot perform select, database connection is closed!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }

            try
            {
                //string sql = String.Format("select * from {0} where type={1} order by random() limit 1", _dbTable, type);
                string sql = String.Format("select * from {0} order by random() limit 1", _dbTable);

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
    }
}
