using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;
using Image = System.Drawing.Image;

namespace BlueTeamTriviaMaze
{
    public class Room
    {
        public static int ROOM_SIZE = 100;

        public enum Type { Normal, Entrance, Exit }
        public enum State { NotVisited, Visited }

        private Type _type;
        private State _state;

        public Shape Drawable;

        public int X { get; private set; }
        public int Y { get; private set; }

        public Door WestDoor { get; private set; }
        public Door EastDoor { get; private set; }
        public Door NorthDoor { get; private set; }
        public Door SouthDoor { get; private set; }

        new public Type GetType() { return _type; }
        public State GetState() { return _state; }
        public void SetState(State state)
        {
            _state = state;

            if (_state == State.Visited)
            {
                Drawable.StrokeThickness = 0;
                if (WestDoor != null) WestDoor.Opacity = 1;
                if (EastDoor != null) EastDoor.Opacity = 1;
                if (NorthDoor != null) NorthDoor.Opacity = 1;
                if (SouthDoor != null) SouthDoor.Opacity = 1;
            }

            else if (_state == State.NotVisited)
            {
                Drawable.StrokeThickness = 50;
                Drawable.Stroke = Brushes.Black;
                if (WestDoor != null) WestDoor.Opacity = 0;
                if (EastDoor != null) EastDoor.Opacity = 0;
                if (NorthDoor != null) NorthDoor.Opacity = 0;
                if (SouthDoor != null) SouthDoor.Opacity = 0;
            }
        }

        public void SetDoorsEnabled(bool enabled)
        {
            if (NorthDoor != null)
                NorthDoor.IsEnabled = enabled;
            if (SouthDoor != null)
                SouthDoor.IsEnabled = enabled;
            if (EastDoor != null)
                EastDoor.IsEnabled = enabled;
            if (WestDoor != null)
                WestDoor.IsEnabled = enabled;
        }

        public Room(int x, int y, Type type, Door north, Door south, Door east, Door west, string theme)
        {
            _type = type;
            X = x;
            Y = y;

            // store all the doors- some can be null for edge cases
            NorthDoor = north;
            SouthDoor = south;
            EastDoor = east;
            WestDoor = west;




            // create what the door will look like- an outlined rectangle
            Drawable = new Rectangle();

            Canvas.SetLeft(Drawable, x * ROOM_SIZE);
            Canvas.SetTop(Drawable, y * ROOM_SIZE);

            //if (_type == Type.Exit)
            //    Drawable.Stroke = Brushes.LimeGreen;
            //else
            //    Drawable.Stroke = Brushes.Black;

            Drawable.Width = Drawable.MinWidth = Drawable.MaxWidth = Drawable.Height = Drawable.MinHeight = Drawable.MaxHeight = ROOM_SIZE;

            SetBackgroundFill(theme);

            // all rooms start as not visited
            SetState(State.NotVisited);


        } // end public Room(...)

        private TransformedBitmap rotate(BitmapImage bi, int angle)
        {   
            //credit to http://stackoverflow.com/questions/7309086/rotate-a-bitmapimage

            TransformedBitmap tmp = new TransformedBitmap();
            tmp.BeginInit();
            tmp.Source = bi; // MyImageSource of type BitmapImage
            RotateTransform rotate = new RotateTransform(angle);
            tmp.Transform = rotate;
            tmp.EndInit();

            return tmp;
        }

        private void SetBackgroundFill(string theme)
        {
            if (NorthDoor != null && SouthDoor != null && EastDoor != null && WestDoor != null)
                Drawable.Fill = new ImageBrush(new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}4Door.png", theme))));

            if (NorthDoor == null)
                Drawable.Fill = new ImageBrush(new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}3Door.png", theme))));

            if (EastDoor == null)
            {
                BitmapImage room = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}3Door.png", theme)));
                Drawable.Fill = new ImageBrush(rotate(room, 90));
            }

            if (SouthDoor == null)
            {
                BitmapImage room = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}3Door.png", theme)));
                Drawable.Fill = new ImageBrush(rotate(room, 180));
            }

            if (WestDoor == null)
            {
                BitmapImage room = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}3Door.png", theme)));
                Drawable.Fill = new ImageBrush(rotate(room, 270));
            }

            if (NorthDoor == null && WestDoor == null)
                Drawable.Fill = new ImageBrush(new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}2Door.png", theme))));

            if (NorthDoor == null && EastDoor == null)
            {
                BitmapImage room = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}2Door.png", theme)));
                Drawable.Fill = new ImageBrush(rotate(room, 90));
            }

            if (SouthDoor == null && EastDoor == null)
            {
                BitmapImage room = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}2Door.png", theme)));
                Drawable.Fill = new ImageBrush(rotate(room, 180));
            }

            if (SouthDoor == null && WestDoor == null)
            {
                BitmapImage room = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}2Door.png", theme)));
                Drawable.Fill = new ImageBrush(rotate(room, 270));
            }

        }
    }
}
