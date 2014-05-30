using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Windows.Markup;
using System.Runtime.Serialization;

namespace BlueTeamTriviaMaze
{
    class DatabaseSavedGames : Database
    {
        private readonly string _dbTable = "saved_games";
        private SQLiteConnection _dbConn;
        public bool IsAlive{ get; private set; }
        public string DBFile { get; private set; }

        public DatabaseSavedGames()
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
                             + "player text,"
                             + "game text);", _dbTable);

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

        public bool Save(string player, Maze game)
        {
            //credit to http://stackoverflow.com/questions/217187/storing-c-sharp-data-structure-into-a-sql-database

            //NetDataContractSerializer serializer = new NetDataContractSerializer();
            //MemoryStream stream = new MemoryStream();
            //serializer.Serialize(stream, game);
            //stream.Position = 0;

            //var xaml = XamlWriter.Save(game);

            string sql = "insert into saved_games (player, game) values (@player, @game)";

            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
                command.Parameters.Add(new SQLiteParameter("@player", player));
                command.Parameters.Add(new SQLiteParameter("@game", XamlWriter.Save(game)));
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return false;
        }

        public DataTable Load()
        {
            if (!IsAlive)
            {
                MessageBox.Show("Cannot perform select, database connection is closed!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }

            try
            {
                string sql = String.Format("select * from {0}", _dbTable);
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn);

                SQLiteDataReader sqlDR = command.ExecuteReader();
                DataTable _select = new DataTable();
                _select.Load(sqlDR);
                sqlDR.Close();

                return _select;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return null;
        }
    }
}
