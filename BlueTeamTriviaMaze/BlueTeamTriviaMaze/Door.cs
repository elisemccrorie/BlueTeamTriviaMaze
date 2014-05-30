using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Runtime.Serialization;



namespace BlueTeamTriviaMaze
{
    [Serializable]
    public class Door
    {
        public const int DOOR_SIZE = 40;

        public enum State {Closed, Locked, Opened};

        private State _state;

        public Shape Drawable;

        public State GetState() { return _state; }
        private void SetState(State new_state)
        {
            _state = new_state;

            if (_state == State.Closed)
            {
                //Content = "+";
                //Background = Brushes.LightGray;
                //Foreground = Brushes.Black;
            }
            else if (_state == State.Locked)
            {
                //Content = "X";
                //Background = Brushes.Transparent;
                //Foreground = Brushes.DarkRed;
            }
            else if (_state == State.Opened)
            {
                //Content = "";
                //Background = Brushes.Transparent;
                //Foreground = Brushes.Black;

            }
        } // end SetState(new_state)



        public Door(float x_index, float y_index, string theme)
        {
            //FontWeight = System.Windows.FontWeights.ExtraBold;
            Drawable = new Rectangle();
            Drawable.MinWidth = Drawable.MaxWidth = Drawable.MinHeight 
                = Drawable.MaxHeight = Drawable.Width = Drawable.Height = DOOR_SIZE;

            Drawable.IsEnabled = false;  // all Doors start disabled, the parent Rooms for this door will enable/disable this as the player enter/exits

            SetState(State.Closed);


            // this ABSOLUTELY positions the door

            //positioning of the doors needs to be based on whether or not a door 
            //sits horizonatally or vertically, so doors will need a "type"
            Canvas.SetLeft(Drawable, x_index * Room.ROOM_SIZE + Room.ROOM_SIZE/2 - DOOR_SIZE/2); // +ROOM_SIZE/2 to move the door onto the N,S,E,W edges of the room
            Canvas.SetTop(Drawable, y_index * Room.ROOM_SIZE + Room.ROOM_SIZE/2 - DOOR_SIZE/2);  // -DOOR_SIZE/2 to center the door itself on the edge


            // connect the Click event
            Drawable.MouseDown += new MouseButtonEventHandler(MazeWindow.GetInstance().GetMaze().GetPlayer().DoorClick);

            Drawable.Fill = new ImageBrush(new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}Door.png", theme))));
        }

        public void Open()
        {
            SetState(State.Opened);

            //animate the opening of the door
            //DoubleAnimation dblAnim = new DoubleAnimation(0, -90, new Duration(TimeSpan.FromSeconds(1)));
            //this.RenderTransform = new RotateTransform();
            //RotateTransform rt = (RotateTransform)this.RenderTransform;
            //rt.BeginAnimation(RotateTransform.AngleProperty, dblAnim);

            DoubleAnimation dblAnim = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(1)));
            Drawable.BeginAnimation(Shape.OpacityProperty, dblAnim);
        }
        
        public bool TryToOpen()
        {
            Open();
            return true;
        }


    }
}
