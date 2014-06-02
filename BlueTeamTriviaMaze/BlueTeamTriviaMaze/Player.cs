using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows;
using System.Runtime.Serialization;


namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Player - the entity that moves between Rooms through Doors
    ///          in the Maze in the 4 cardinal directions (N,S,E,W).
    /// </summary>


    [Serializable]
    public class Player
    {
        private const int PLAYER_SIZE = 25;
        private const int ANIMATION_DURATION = 2; // seconds

        public enum MoveDirection { North, South, East, West };

        private Room _currentRoom;
        private int _currentKeys;
        private BitmapImage _playerImage;

        public Room GetCurrentRoom() { return _currentRoom; }
        public Shape Drawable { get; private set; }


        public Player(int num_keys, string player)
        {
            _currentKeys = num_keys;

            // Load the player drawable
            Drawable = new Rectangle();
            Drawable.Width = Drawable.Height = PLAYER_SIZE;
            Drawable.StrokeThickness = 0;
            _playerImage = new BitmapImage(new Uri(String.Format("pack://application:,,,/Resources/{0}.png", player)));
            Drawable.Fill = new ImageBrush(_playerImage);
        }

        // Will 'try' to move the Player in the 'direction' given.
        // 
        // If the Door in that 'direction' is closed, open the question window with that Door's trivia item, only allow passage on a correct answer.
        // If the Door in that 'direction' is open, freely move to the adjacent room in that direction.
        // If the Door in that 'direction' is locked, don't do anything.
        public void TryToMove(MoveDirection direction)
        {
            // get the target door and Room for the 'direction' to try to move
            Door target_door = _currentRoom.WestDoor;    // WEST
            Room target_room = MazeWindow.GetInstance().GetMaze().GetRoom(_currentRoom.X - 1, _currentRoom.Y);

            if (direction == MoveDirection.North)        // NORTH
            {
                target_door = _currentRoom.NorthDoor;
                target_room = MazeWindow.GetInstance().GetMaze().GetRoom(_currentRoom.X, _currentRoom.Y - 1);
            }
            else if (direction == MoveDirection.South)   // SOUTH
            {
                target_door = _currentRoom.SouthDoor;
                target_room = MazeWindow.GetInstance().GetMaze().GetRoom(_currentRoom.X, _currentRoom.Y + 1);
            }
            else if (direction == MoveDirection.East)    // EAST
            {
                target_door = _currentRoom.EastDoor;
                target_room = MazeWindow.GetInstance().GetMaze().GetRoom(_currentRoom.X + 1, _currentRoom.Y);
            }


            // attempt to open it, if it's not already opened
            if (target_door.GetState() != Door.State.Opened)
                if (!target_door.TryToOpen()) // failed to open for whatever reason, don't move thru
                    return;
            

            // Door must be open, so move to it
            MoveToRoom(target_room);
        }


        // Will absolutely move the Player to the specified 'target_room' with animation (if applicable)
        public void MoveToRoom(Room target_room)
        {
            DependencyProperty property = null;
            double start = 0;


            // if the Player is coming from a room, we can animate
            if (_currentRoom != null)
            {
                _currentRoom.SetDoorsEnabled(false); // disable the old room's doors

                // figure out what property (direction) to animate- Left or Top
                if (Canvas.GetTop(target_room.Drawable) == Canvas.GetTop(_currentRoom.Drawable)) // if we should animate WEST/EAST
                { 
                    property = Canvas.LeftProperty;                                              // then the animation should operate on the 'Left' property
                    start = Canvas.GetLeft(_currentRoom.Drawable);
                }
                else
                {                                                                               // else animating NORTH/SOUTH
                    property = Canvas.TopProperty;                                              // then the animation should operate on the 'Top' property
                    start = Canvas.GetTop(_currentRoom.Drawable);
                }
            }

            _currentRoom = target_room;
            _currentRoom.SetDoorsEnabled(true); // enable the new room's doors
            _currentRoom.SetState(Room.State.Visited);


            // if we aren't coming from a room, then just position the player at the target room
            if (property == null)
            {
                Canvas.SetLeft(Drawable, Canvas.GetLeft(_currentRoom.Drawable) + Room.ROOM_SIZE / 2 - Drawable.Width / 2);
                Canvas.SetTop(Drawable, Canvas.GetTop(_currentRoom.Drawable) + Room.ROOM_SIZE / 2 - Drawable.Height / 2);
            }
            else
            {   // animate the player's position moving it toward the new current room's location
                double end = property == Canvas.LeftProperty ? Canvas.GetLeft(_currentRoom.Drawable) : Canvas.GetTop(_currentRoom.Drawable);

                  // flip player image based on direction heading
                if (property == Canvas.LeftProperty && end < start)
                {
                    TransformedBitmap tmp = new TransformedBitmap();
                    tmp.BeginInit();
                    tmp.Source = _playerImage;
                    ScaleTransform flip = new ScaleTransform(-1, 1, 0.5, 0.5);
                    tmp.Transform = flip;
                    tmp.EndInit();
                    Drawable.Fill = new ImageBrush(tmp);
                }
                else
                    Drawable.Fill = new ImageBrush(_playerImage);

                
                // offset end to center of the room
                end += (Room.ROOM_SIZE / 2) - (Drawable.Width / 2);


                // set the start to the current player's position, so it doesn't JUMP to the old room if switching rooms fast
                start = property == Canvas.LeftProperty ? Canvas.GetLeft(Drawable) : Canvas.GetTop(Drawable);


                // begin the animation!           -- the fancy duration calculation is a proportion!: so if you click an open door twice fast, it doesnt take 2 seconds to move a couple pixels back to where it was
                Drawable.BeginAnimation(property, new DoubleAnimation(start, end, new Duration(TimeSpan.FromSeconds( Math.Abs(start - end) * ANIMATION_DURATION / Room.ROOM_SIZE ))));
            }
        }



        public void DoorClick(Object sender, MouseButtonEventArgs e)
        {
            // figure out the direction to move based on the door clicked's direction (NSEW) relative to the current room 
            Door clicked_door = (Door)sender;
            MoveDirection move_direction = MoveDirection.West;        // Default as western door

            if (clicked_door == _currentRoom.NorthDoor) // This Door was clicked as a North Door
                move_direction = Player.MoveDirection.North;
            
            else if (clicked_door == _currentRoom.SouthDoor) // This Door was clicked as a South Door
                move_direction = Player.MoveDirection.South;
            
            else if (clicked_door == _currentRoom.EastDoor) // This Door was clicked as an East Door
                move_direction = Player.MoveDirection.East;
            

            //"cheat": right click will forcibly open doors
            if (e.ChangedButton == MouseButton.Right)
                clicked_door.Open();
            

            // try to move the direction of the door relative to the current room
            TryToMove(move_direction);

            CheckMazeStatus();
        }


        private void CheckMazeStatus()
        {
            // check if we won
            if (_currentRoom.GetType() == Room.Type.Exit)
                MazeWindow.GetInstance().Win();

            else // check if we lost
            {
                // TODO: logic to see if all doors accesible to the player are locked
            }
        }
    }
}
