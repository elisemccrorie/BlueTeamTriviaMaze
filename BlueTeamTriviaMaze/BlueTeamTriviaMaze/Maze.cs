using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections;
using System.Runtime.Serialization;

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Maze - the container for a Player to navigate through a collection 
    ///        of Rooms, each composed of doors that will only open if a
    ///        question is answered correctly.
    /// </summary>
    


   
    public class Maze : Canvas
    {
        private Room[,] _rooms;
        private Player _player;
        private ArrayList _doorsList;
        private bool _winnable = false;

        //necessary for loading a saved maze
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public string Theme { get; private set; }
        public string Player { get; private set; }

        public Player GetPlayer() { return _player; }
        


        public void Initialize(int width, int height, int entrance_x, int entrance_y, int[,] exits_xy, string theme, string player)
        {
            // Create and add the Player to the maze
            CreatePlayer(3, player); // num keys, player image source
            
            //necessary for loading in a saved maze
            Rows = height;
            Columns = width;
            Theme = theme;
            Player = player;


            // create each of the rooms
            _rooms = new Room[height, width];
            _doorsList = new ArrayList();


            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    // build each Room composed of 4 Doors (NSEW) and add it to the Maze.
                    //
                    // Note: this will only create new Doors if the neighboring Room that will share the
                    // same door hasn't already created one for them to share.
                    Room neighbor = null;



                    // NORTH DOOR
                    Door northDoor = null;
                    if (y > 0) // omit north doors on top-most rooms
                    {
                        neighbor = GetRoom(x, y - 1);  // (0,0) is the top-left most room, so y-1 means get the neighboring room 'north' of here
                        northDoor = neighbor != null ? neighbor.SouthDoor : new Door(x, y - 0.5f, theme);

                        if (neighbor == null)          // if a new door was just created, add the door to the temporary collection.
                            _doorsList.Add(northDoor);  // it will be added to the Maze later (for painting)
                    }



                    // SOUTH DOOR
                    Door southDoor = null;
                    if (y < height - 1) // omit south doors on bottom-most rooms
                    {
                        neighbor = GetRoom(x, y + 1);
                        southDoor = neighbor != null ? neighbor.NorthDoor : new Door(x, y + 0.5f, theme); // position door south = y+0.5
                        
                        //south doors need to be rotated 90 degrees
                        southDoor.LayoutTransform = new RotateTransform(90);

                        if (neighbor == null)
                            _doorsList.Add(southDoor);
                    }



                    // EAST DOOR
                    Door eastDoor = null;
                    if (x < width - 1) // omit east doors on right-most rooms
                    {
                        neighbor = GetRoom(x + 1, y);
                        eastDoor = neighbor != null ? neighbor.WestDoor : new Door(x + 0.5f, y, theme); // position door east = x+0.5
                        
                        if (neighbor == null)
                            _doorsList.Add(eastDoor);
                    }



                    // WEST DOOR
                    Door westDoor = null;
                    if (x > 0) // omit west doors on left-most rooms
                    {
                        neighbor = GetRoom(x - 1, y);
                        westDoor = neighbor != null ? neighbor.EastDoor : new Door(x - 0.5f, y, theme); // position door west = x-0.5

                        if (neighbor == null)
                            _doorsList.Add(westDoor);
                    }



                    // Determine the Room type based this Room's location- this figures out if this Room is the entrance, an exit or a normal room
                    Room.Type room_type = Room.Type.Normal;

                    if (x == entrance_x && y == entrance_y) // check if its the entrance
                        room_type = Room.Type.Entrance;
                    
                    else // check if this Room is an exit room
                        for (int i = 0; i < exits_xy.GetLength(0) && room_type == Room.Type.Normal; ++i)
                            if (x == exits_xy[i, 0] && y == exits_xy[i, 1])
                                room_type = Room.Type.Exit;



                    // Create and add the room given its (x,y) location, type and NSEW doors
                    CreateRoom(x, y, room_type, northDoor, southDoor, eastDoor, westDoor, theme);

                } // end for (width)
            } // end for (height)



            // Add all the doors as children of the Maze (canvas) for drawing
            //
            // Note: this is done LAST like this so the Doors are drawn on top the Rooms
            foreach (Door door in _doorsList)
                Children.Add(door);



            // Move the player to the entrance
            _player.MoveToRoom(_rooms[entrance_y, entrance_x]);

        } // end public Maze(...)



        // Returns the room given its array indicies [y,x] into the maze, or null if out of bounds.
        public Room GetRoom(int x, int y)
        {
            if (y < 0 || x < 0 || y >= _rooms.GetLength(0) || x >= _rooms.GetLength(1))
                return null;

            return _rooms[y, x];
        }



        private void CreateRoom(int x, int y, Room.Type room_type, Door northDoor, Door southDoor, Door eastDoor, Door westDoor, string theme)
        {
            // create and store a new Room, composed of all its
            // appropriate doors (either freshly-generated or as taken from the neighboring rooms)
            _rooms[y, x] = new Room(x, y, room_type, northDoor, southDoor, eastDoor, westDoor, theme);

            // add that new Room as a child of the Maze (canvas) so it may be drawn
            Children.Add(_rooms[y, x].Drawable);
        }



        private void CreatePlayer(int key_count, string player)
        {
            _player = new Player(key_count, player);

            Children.Add(_player.Drawable);
            Canvas.SetZIndex(_player.Drawable, 99);
        }


        //This is the recursive check to see if a maze is still winnable
        //the logic isn't quite right
        public bool Search(int x, int y, int[,] maze)
        {
            //MessageBox.Show(String.Format("Search in room ({0}, {1})", new_x, new_y));

            Room curr = GetRoom(x, y);
            int[,] solveable = maze;

            if (curr == GetRoom(Rows - 1, Columns - 1))
                return true;

            if (curr.EastDoor != null && curr.EastDoor.GetState() != Door.State.Locked 
                && GetRoom(x + 1, y) != null && solveable[x + 1, y] != 1)
            {
                solveable[x + 1, y] = 1;
                return Search(x + 1, y, solveable);
            }

            if (curr.SouthDoor != null && curr.SouthDoor.GetState() != Door.State.Locked
                && GetRoom(x, y + 1) != null && solveable[x, y + 1] != 1)
            {
                solveable[x, y + 1] = 1;
                return Search(x, y + 1, solveable);
            }

            if (curr.WestDoor != null && curr.WestDoor.GetState() != Door.State.Locked 
                && GetRoom(x - 1, y) != null && solveable[x - 1, y] != 1)
            {
                solveable[x - 1, y] = 1;
                return Search(x - 1, y, solveable);
            }

            if (curr.NorthDoor != null && curr.NorthDoor.GetState() != Door.State.Locked
                && GetRoom(x, y - 1) != null && solveable[x, y - 1] != 1)
            {
                solveable[x, y - 1] = 1;
                return Search(x, y - 1, solveable);
            }

            return false;
        }

        public void Win(Player player)
        {
            if (player.GetCurrentRoom() == GetRoom(Rows - 1, Columns - 1))
            {
                IsEnabled = false;

                MessageBox.Show("You Win!", "Winner!");

                MazeWindow.GetInstance().Close();
            }
        }

        public void Lose()
        {
            IsEnabled = false;

            MessageBox.Show("You Lose!", "Loser!");

            MazeWindow.GetInstance().Close();
        }
    }
}
