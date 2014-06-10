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
