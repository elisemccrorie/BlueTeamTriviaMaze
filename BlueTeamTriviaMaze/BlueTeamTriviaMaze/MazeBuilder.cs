//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: Builds a maze where up to 75% of the doors are
//  locked by default. Makes a more "true" maze with dead ends
//  and as few as one path from entrance to exit. Enabled by
//  "classic" mode

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BlueTeamTriviaMaze
{
    public class MazeBuilder
    {
        Pathfinder _checkPath;
        Maze _maze;
        int _height, _width;
        double _totalDoors, _blockPercentage;

        public MazeBuilder(ref Maze m, int maze_width, int maze_height, Theme theme, string player)
        {
            _height = maze_height;
            _width = maze_width;

            _maze = m;
            _maze.Initialize(_width, _height,                    // maze dimensions
                             0, 0,                                       // maze entrance
                             maze_width - 1, maze_height - 1,                // array of maze exit
                             theme,                                     //maze background theme image
                             player);                                   //player image

            _totalDoors = 2*(maze_height * maze_width) - maze_height - maze_width;  //formula for the number of doors in a particular maze
            _blockPercentage = _totalDoors * .75;
        }

        private void ConstructMaze(ref Maze m)
        {
            _maze = m;
            Random randWidth = new Random(DateTime.Now.Millisecond);
            Random randHeight = new Random(DateTime.Now.Second);
            Random randDoor = new Random((int)DateTime.Now.Ticks);

            for (int count = 0; count < _blockPercentage;)  //block off 75% of the doors
            {
                Room room = _maze.GetRoom(randWidth.Next(0, _width), randHeight.Next(0, _height));  //random room
                int door = randDoor.Next(0, 4); //random door

                door = SetDoorState(room, door, Door.State.Locked);    //lock it

                _checkPath = new Pathfinder(_maze.GetRoom(0, 0), _maze.GetExitRoom());  //make sure the maze can still be completed

                if (_checkPath.PathExists)  //if it can be completed
                    count++;
                else    //if it cant (ie. locking this door made the maze unbeatable)
                    SetDoorState(room, door, Door.State.Closed);    //unlock it and keep trying
            }
        }

        private int SetDoorState(Room room, int door, Door.State state)
        {
            ArrayList chosen = new ArrayList();

            while (true)
            {
                chosen.Add(door);
                if (door == 0 && room.NorthDoor != null)   //north
                {
                    room.NorthDoor.SetState(state);
                    room.NorthDoor.Opacity = 0;
                    return door;
                }
                else if (door == 1 && room.SouthDoor != null) //south
                {    
                    room.SouthDoor.SetState(state);
                    room.SouthDoor.Opacity = 0;
                    return door;
                }
                else if (door == 2 && room.EastDoor != null)  //east
                {    
                    room.EastDoor.SetState(state);
                    room.EastDoor.Opacity = 0;
                    return door;
                }
                else if (door == 3 && room.WestDoor != null)//west
                {
                    room.WestDoor.SetState(state);
                    room.WestDoor.Opacity = 0;
                    return door;
                }

                if (chosen.Count == 4)
                    return door;

                while (chosen.Contains(door))
                    door = new Random((int)DateTime.Now.Ticks).Next(0, 4);
            }
        }//end SetDoorState

        public Maze GetPlainMaze()
        {
            return _maze;
        }

        public Maze GetTrueMaze(ref Maze m)
        {
            ConstructMaze(ref m);
            return _maze;
        }
    }
}
