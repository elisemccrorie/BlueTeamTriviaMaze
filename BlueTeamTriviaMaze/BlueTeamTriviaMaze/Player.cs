//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: The user who plays the game. Shows the user selected
//  graphic, handles movement between maze rooms via door clicks,
//  and checks if maze is still complete-able after answering
//  a question

using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows;
using System.Collections.Generic;

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Player - the entity that moves between Rooms through Doors
    ///          in the Maze in the 4 cardinal directions (N,S,E,W).
    /// </summary>


    public class Player
    {
        private const int PLAYER_SIZE = 25;
        private const double ANIMATION_DURATION = 0.6; // seconds

        private Room _currentRoom;
        private BitmapImage _playerImage;
        private Queue<KeyValuePair<DependencyProperty, DoubleAnimation>> _currentAnimations;

        public enum MoveDirection { North, South, East, West };
        public int Keys { get; set; }
    
        public Room GetCurrentRoom() { return _currentRoom; }
        public Shape Drawable { get; private set; }
        public Statistics Stats;


        public Player(int num_keys, string player)
        {
            Keys = num_keys;
            Stats = new Statistics();
            _currentAnimations = new Queue<KeyValuePair<DependencyProperty, DoubleAnimation>>();

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
                if (!target_door.TryToOpen(ref Stats)) // failed to open for whatever reason, don't move thru
                    return;
            

            // Door must be open, so move to it
            MoveToRoom(target_room);

        }//end TryToMove(direction)


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


                // offset to center of the room
                end += (Room.ROOM_SIZE / 2) - (Drawable.Width / 2);
                start += (Room.ROOM_SIZE / 2) - (Drawable.Width / 2);


                // queue up the animation for playing
                DoubleAnimation new_anim = new DoubleAnimation(start, end, new Duration(TimeSpan.FromSeconds(ANIMATION_DURATION)));
                new_anim.Completed += new EventHandler(OnAnimationComplete);

                _currentAnimations.Enqueue(new KeyValuePair<DependencyProperty, DoubleAnimation>(property, new_anim));

                if (_currentAnimations.Count == 1) // if this is the first animation, then start playing it
                    PlayNextAnimation();
            }
        } //end MoveToRoom(Room)

        private void PlayNextAnimation()
        {
            // start animating
            KeyValuePair<DependencyProperty, DoubleAnimation> next_anim = _currentAnimations.Peek();
            Drawable.BeginAnimation(next_anim.Key, next_anim.Value);


            // flip player image based on direction heading ONLY if moving left/right
            if (next_anim.Key == Canvas.LeftProperty)
                if (next_anim.Value.To < next_anim.Value.From)
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
        }

        private void OnAnimationComplete(object sender, EventArgs e)
        {
            _currentAnimations.Dequeue(); // get rid of the current animation

            if (_currentAnimations.Count > 0) // play the next one
                PlayNextAnimation();
        }


        public void Door_Click(Object sender, MouseButtonEventArgs e)
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
        }//end Door_Click


        private void CheckMazeStatus()
        {
            // check if we won
            if (_currentRoom.GetType() == Room.Type.Exit)
                MazeWindow.GetInstance().Win();

            else if (DidLose()) // check if we lost
                MazeWindow.GetInstance().Lose();
        }


        private bool DidLose()
        {
            // find a path from the current room to the exit room
            Pathfinder path = new Pathfinder(_currentRoom, MazeWindow.GetInstance().GetMaze().GetExitRoom());

            return !path.PathExists; // return we LOST if a path DOES NOT exist
        }
    }
}
