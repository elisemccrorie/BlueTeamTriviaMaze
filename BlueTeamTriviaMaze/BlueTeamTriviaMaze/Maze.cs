using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Maze - the container for a Player to navigate through a collection 
    ///        of Rooms, each composed of doors that will only open if a
    ///        question is answered correctly.
    /// </summary>
    



    public class Maze : Canvas
    {
        private int _width, _height;

        private Door[] _doors;
        private Room[,] _rooms;

        public Maze(int width, int height)
        {
            _width = width;
            _height = height;
        }
    }
}
