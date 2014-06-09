//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: Contains all the required images to create a "themed"
//  background for a maze

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BlueTeamTriviaMaze
{
    public class Theme
    {
        public BitmapImage Start { get; private set; }
        public BitmapImage Exit { get; private set; }
        public BitmapImage ExitHidden { get; private set; }
        public BitmapImage FourDoor { get; private set; }
        public BitmapImage ThreeDoor { get; private set; }
        public BitmapImage TwoDoor { get; private set; }
        public BitmapImage Door { get; private set; }
        public BitmapImage DoorLocked { get; private set; }
        public BitmapImage DoorReturn { get; private set; }

        public Theme(string theme)
        {
            Start = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}Start.png", theme)));
            Exit = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}Exit.png", theme)));
            ExitHidden = new BitmapImage(new Uri(@"pack://application:,,,/Resources/ExitHidden.png"));
            FourDoor = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}4Door.png", theme)));
            ThreeDoor = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}3Door.png", theme)));
            TwoDoor = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}2Door.png", theme)));
            Door = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}Door.png", theme)));
            DoorLocked = new BitmapImage(new Uri(String.Format(@"pack://application:,,,/Resources/{0}DoorLocked.png", theme)));
            DoorReturn = new BitmapImage(new Uri(@"pack://application:,,,/Resources/DoorReturn.png"));
        }

    }
}
