//Author: Blue Team (Elise Peterson, Cord Rehn, Zak Steele)
//Class: Spring 2014 CSCD 350-01
//Description: Displays the "about" information for the Blue Team.
//  Includes team name, group members, version class, framework
//  and platform

using System;
using System.Windows;
using System.Reflection;

namespace BlueTeamTriviaMaze
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        private string _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private string _framework = Assembly.GetExecutingAssembly().ImageRuntimeVersion.ToString();
        private string _platform = Assembly.GetExecutingAssembly().GetName().ProcessorArchitecture.ToString();

        public AboutWindow()
        {
        /*    InitializeComponent();

            txblkAbout.Text = String.Format("Publisher: Blue Team\n" +
                                            "Authors: Elise Peterson, Cord Rehn, and" +
                                            "       \tZak Steele\n" +
                                            "Version: {0}\n" +
                                            "Class: CSCD 350-01 Spring 2014\n" +
                                            ".NET Framework: {1}\n" +
                                            "Platform: {2}", _version, _framework, _platform);*/
        }
    }
}
