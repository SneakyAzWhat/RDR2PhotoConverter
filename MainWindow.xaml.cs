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

            SetDefaultDirectory();
            SetAppDirectories();

            activeDir = defaultDirPRDR;

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
            if (myDefaultPathRadioButton.IsChecked == true)
            {
                statusBarTextBlock.Text = $"Valid Path found at {defaultDirPRDR}";
            }
            else if (myCustomPathRadioButton.IsChecked == true)
            {
                GetCustomDir();
                activeDir = customDirPRDR;
            }

            GetValidFiles(activeDir);
            ////////////MessageBox.Show("set directory clicked test yeah this this is a long long boi message to see what this looks lijke i dont know why my app doesnt work pls microsoft fix me ty");

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

        private void GetCustomDir()
        {
            customDirPRDR = dirInputTextBox.Text;
            if (Directory.Exists(customDirPRDR))
            {
                statusBarTextBlock.Text = $"Valid Custom Path entered";
            }
            else
            {
                statusBarTextBlock.Text = $"Invalid Custom Path entered, please double check your entered path and try again";
            }
        }

        private void GetValidFiles(string path)
        {
            string[] files = Directory.GetFiles(path);
            statusBarTextBlock.Text = $"Retrieving the applicable PRDR files";

            foreach (var file in files)
            {
                if (file.Contains("PRDR"))
                {
                    prdrFiles.Add(file);
                }
            }

            statusBarTextBlock.Text = $"PRDRs retrieved, ready to convert files";
        }

        #endregion

        #region Setters

        private void SetDefaultDirectory()
        {
            string[] fulldir = Directory.GetDirectories($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Rockstar Games\\Red Dead Redemption 2\\Profiles");
            defaultDirPRDR = fulldir[0];

            Debug.WriteLine(defaultDirPRDR);
        }

        private void SetAppDirectories()
        {
            string myPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            convertedFilesDir = $"{myPictures}\\RDR2 Photos";
            backupDirPRDR = $"{myPictures}\\RDR2 Photos\\prdr backups";

            Debug.WriteLine($"converted: {convertedFilesDir} backup: {backupDirPRDR}");

            if (!Directory.Exists(convertedFilesDir))
            {
                Directory.CreateDirectory(convertedFilesDir);
            }
            if (!Directory.Exists(backupDirPRDR))
            {
                Directory.CreateDirectory(backupDirPRDR);
            }
        }
        #endregion
    }
}
