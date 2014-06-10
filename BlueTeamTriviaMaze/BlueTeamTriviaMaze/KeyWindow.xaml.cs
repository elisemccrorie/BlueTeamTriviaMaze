//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: A window to display the option to use a key to
//  open a locked door if the player has a key

using System.Windows;
using System.Windows.Input;


namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Interaction logic for KeyWindow.xaml
    /// </summary>
    public partial class KeyWindow : Window
    {
        public KeyWindow(Player player)
        {
            InitializeComponent();

            if (player.Keys == 0)
            {
                txblkKey.Text = "You do not have any keys left to unlock this door!";
                lblYes.Visibility = Visibility.Hidden;
                lblNo.Content = "Okay";
            }
            else
                txblkKey.Text = "Would you like to use a key to open this locked door?";
            
        }

        private void lblNo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void lblYes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
