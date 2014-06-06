using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
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
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        private string _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private string _framework = Assembly.GetExecutingAssembly().ImageRuntimeVersion.ToString();
        private string _platform = Assembly.GetExecutingAssembly().GetName().ProcessorArchitecture.ToString();

        public About()
        {
            InitializeComponent();

            txblkAbout.Text = String.Format("Publisher: Blue Team\n" +
                                            "Authors: Elise Peterson, Cord Rehn, and" +
                                            "       \tZak Steele\n" +
                                            "Version: {0}\n" +
                                            "Class: CSCD 350-01 Spring 2014\n" +
                                            ".NET Framework: {1}\n" +
                                            "Platform: {2}", _version, _framework, _platform);
        }
    }
}
