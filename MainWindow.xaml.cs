using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

using Path = System.IO.Path;

namespace RDR2PhotoConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string userName;

        private static string defaultDirPRDR;
        private static string customDirPRDR;
        private static string activeDir;

        private static string backupDirPRDR;
        private static string convertedFilesDir;

        List<string> prdrFiles = new List<string>();
        string fileName;

        public MainWindow()
        {
            InitializeComponent();

            userName = Environment.UserName;

            GetDefaultDirectory();
            SetDirectories();

            dirInputTextBox.Text = defaultDirPRDR;

        }

        #region ClickEvents
        private void OnDefaultPathClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnCustomPathClicked(object sender, RoutedEventArgs e)
        {

        }
        private void OnDblClickTextBox(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnSetDirectoryClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("set directory clicked test yeah this this is a long long boi message to see what this looks lijke i dont know why my app doesnt work pls microsoft fix me ty");

        }

        private void OnConvertFilesClick(object sender, RoutedEventArgs e)
        {
            //TODO - MessageBox.Show() at the end of converting process to notify user it is completed! :)
        }

        private void OnMyTwitterClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnMyGithubClick(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Getters
        private void GetDefaultDirectory() //Trying to get default directory, small % of users may have an issue with their USER not matching their USERNAME
        {

            string[] fulldir = Directory.GetDirectories($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Rockstar Games\\Red Dead Redemption 2\\Profiles");
            defaultDirPRDR = fulldir[0];

            Debug.WriteLine(defaultDirPRDR);

        }

        #endregion

        #region Setters
        private void SetDirectories()
        {
            ////This stuff runs only after the default directory issue is resolved and set (in case it needs to be changed from environment.username)
            //MessageBox.Show("Congrats, this program doesn't break on your PC. The rest of the application is disabled right now as this is just a testing build. PLEASE let me know it worked for you!");

            ////Use this to find the pictures folder and then create the new subdirectories based on that
            //Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            //convertedFilesDir = $"C:\\Users\\{userName}\\Pictures\\RDR2 Photos";
            //backupDirPRDR = $"C:\\Users\\{userName}\\Pictures\\RDR2 Photos\\prdr backups";

            //if (!Directory.Exists(convertedFilesDir))
            //{
            //    Directory.CreateDirectory(convertedFilesDir);
            //}
            //else if (!Directory.Exists(backupDirPRDR))
            //{
            //    Directory.CreateDirectory(backupDirPRDR);
            //}
        }
        #endregion
    }
}
