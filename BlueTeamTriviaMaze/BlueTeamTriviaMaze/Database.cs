//Author: Zak Steele
//Description: This class is responsible for connecting to
//  selecting from, inserting into, deleting from, and closing
//  a sqlite database. It also creates a database if one is not 
//  provided, and creates the necessary table if it does not
//  exist. (The current version does not provide a user the 
//  option to provide their own database.)

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
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace BlueTeamTriviaMaze
{
    interface Database
    {
        bool Close();
        bool Connect();
        bool CreateTable();
    }
}
