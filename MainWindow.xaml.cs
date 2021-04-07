using System;
using System.Collections.Generic;
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
        private static string userName = Environment.UserName;
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

            string[] fullDir = Directory.GetDirectories($"C:\\Users\\{userName}\\Documents\\Rockstar Games\\Red Dead Redemption 2\\Profiles");
            defaultDirPRDR = fullDir[0];

            convertedFilesDir = $"C:\\Users\\{userName}\\Pictures\\RDR2 Photos";
            backupDirPRDR = $"C:\\Users\\{userName}\\Pictures\\RDR2 Photos\\prdr backups";

            if (!Directory.Exists(convertedFilesDir))
            {
                Directory.CreateDirectory(convertedFilesDir);
            }
            else if (!Directory.Exists(backupDirPRDR))
            {
                Directory.CreateDirectory(backupDirPRDR);
            }

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

        }

        private void OnConvertFilesClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnMyTwitterClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnMyGithubClick(object sender, RoutedEventArgs e)
        {

        }

        #endregion


    }
}
