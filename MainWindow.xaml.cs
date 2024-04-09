using RDR2PhotoConverter.Frames;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using TextBox = System.Windows.Controls.TextBox;

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
        private string fileName;

        // pages
        private readonly DirectorySelect directorySelectPage;

        public MainWindow()
        {
            InitializeComponent();
            directorySelectPage = new DirectorySelect();
            directorySelectPage.defaultButton.Click += OnDefaultPathClicked;
            directorySelectPage.browseButton.Click += OnBrowseClick;
            parentContainer.Content = directorySelectPage;
            titleBar.Text = "Directory Select";
            try
            {
                userName = Environment.UserName;

            }
            catch (Exception e)
            {
                MessageBox.Show($"Exception when assigning userName = Environment.UserName, USER NOTE: Please reach out to developer on github for assistance, RAW:{e.Message}");
            }

            SetDefaultDirectory();
            SetAppDirectories();

            activeDir = defaultDirPRDR;

            directorySelectPage.dirInputTextBox.Text = defaultDirPRDR;
        }

        #region ClickEvents
        /// <summary>
        /// Updates the text displayed in the TextBox for clarity purposes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDefaultPathClicked(object sender, RoutedEventArgs e)
        {
            directorySelectPage.dirInputTextBox.Text = defaultDirPRDR;
        }

        /// <summary>
        /// Allows you to doubleclick on the text within the TextBox to select all of the text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDblClickTextBox(object sender, MouseButtonEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        /// <summary>
        /// Sets either the Default or Custom directory as the activeDir, dependent on which radio button is selected when this button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSetDirectoryClick(object sender, RoutedEventArgs e)
        {
            //if (myDefaultPathRadioButton.IsChecked == true)
            //{
            //    statusBarTextBlock.Text = $"Valid Default Path found";
            //}
            //else if (myCustomPathRadioButton.IsChecked == true)
            //{
            //    GetCustomDir();
            //    activeDir = customDirPRDR;
            //}
            // TODO
        }

        /// <summary>
        /// Converts list of prdrFiles into images, if you checked the 'backup' or 'delete' boxes those things will happen here too
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConvertFilesClick(object sender, RoutedEventArgs e)
        {
            GetValidFiles(activeDir);

            string backupInfo;
            // TODO
            //if (myBackupCheckbox.IsChecked == true)
            //{
            //    backupInfo = BackupPRDRs();
            //}
            //else
            //{
            //    backupInfo = "";
            //}

            foreach (var file in prdrFiles)
            {
                fileName = $"{GetMetaData(file)} {file.Substring(activeDir.Length + 1)}";

                byte[] fileInBytes = File.ReadAllBytes(file);
                byte[] fileInBytesTemp = new byte[fileInBytes.Length - 300];
                long counter = 0;

                for (long i = 300; i < fileInBytes.LongLength; i++)
                {
                    fileInBytesTemp[counter] = fileInBytes[i];
                    counter++;
                }

                try
                {
                    //ISSUE: This is triggering an exception for windows 7 user
                    File.WriteAllBytes($"{convertedFilesDir}\\{fileName}.jpg", fileInBytesTemp);
                }
                catch (Exception exception)
                {
                    //Exception: Access to the path 'C:\Users\USERNAME\Pictures\RDR2 Photos\FILENAME.jpg' is denied.
                    MessageBox.Show($"EXCEPTION: WriteAllBytes, USER NOTE:  chances are you just tried to convert the same files back to back OR some type of AntiVirus program is blocking the program from running properly. You can try restarting the application to see if that fixes the problem.\n\n{exception.Message} ");
                }
                // TODO
                //if (myDeleteCheckbox.IsChecked == true)
                //{
                //    File.Delete(file);
                //}
            }

            //statusBarTextBlock.Text = $"{backupInfo} {prdrFiles.Count} files converted into images.";

            prdrFiles.Clear();

            MessageBox.Show($"{statusBarTextBlock.Text} All done!");
        }

        /// <summary>
        /// Clicking Twitter button in bottom right corner opens your browser and directs you to my twitter account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMyTwitterClick(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://twitter.com/SneakyAzWhat",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        /// <summary>
        /// Clicking Github button in bottom right corner opens your browser and directs you to my github account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMyGithubClick(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://github.com/SneakyAzWhat",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        /// <summary>
        /// Clicking the close icon on the top right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Dragging the titlebar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTitleBarDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// On clicking the brose directory button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBrowseClick(object sender, RoutedEventArgs e)
        {
            var folderSelector = new FolderBrowserDialog();
            var result = folderSelector.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                directorySelectPage.dirInputTextBox.Text = folderSelector.SelectedPath;
                statusBarTextBlock.Text = "Status: Path set to selected folder";
                return;
            }
            statusBarTextBlock.Text = "Status: No path was selected";
        }


        #endregion


        /// <summary>
        /// Checking whether the user entered custom path exists or not
        /// </summary>
        #region Getters
        private void GetCustomDir()
        {
            customDirPRDR = directorySelectPage.dirInputTextBox.Text;
            if (Directory.Exists(customDirPRDR))
            {
                statusBarTextBlock.Text = "Valid Custom Path entered";
            }
            else
            {
                statusBarTextBlock.Text = "Invalid Custom Path entered, please double check your entered path and try again";
                MessageBox.Show("Invalid Custom Path entered, please double check your entered path and try again \n\n Example of a valid path: \n I:\\SomeFolder\\AnotherFolder");
            }
        }

        /// <summary>
        /// Searching the activeDir for PRDR files and adding them to prdrFiles List
        /// </summary>
        /// <param name="path"></param>
        private void GetValidFiles(string path)
        {
            string[] files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                if (file.Contains("PRDR"))
                {
                    prdrFiles.Add(file);
                }
            }
            // TODO
            //statusBarTextBlock.Text = $"PRDRs retrieved, ready to convert files";
        }

        /// <summary>
        /// Reading the MetaData of each PRDR file to get the date and time when the picture was taken
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static string GetMetaData(string file)
        {
            byte[] fileInBytes = File.ReadAllBytes(file);
            string dataString = "";

            //Iterating through the indexes 20-54 to get the date/time the picture was taken (These indexes contain the date and time metadata of when photo was taken)
            for (int i = 20; i < 54; i++)
            {
                
                if (fileInBytes[i] > 31) //bytes < 31 are ascii and are not relevant for our task
                {
                    dataString += $"{Convert.ToChar(fileInBytes[i])}";
                }
            }

            var split = dataString.Trim().Split(" ");
            var date = split[0].Split("/");

            //these are just print files for testing and correct formatting, can be ignored
            //File.WriteAllLines($"{convertedFilesDir}\\split.txt", split);
            //File.WriteAllLines($"{convertedFilesDir}\\date.txt", date);

            string month = date[0],
                day = date[1],
                year = date[2];
            var time = split[1].Split(":");
            string hour = time[0],
                minute = time[1],
                second = time[2];

            return $"{year}-{month}-{day} {hour}.{minute}.{second}";
        }
        #endregion

        /// <summary>
        /// Getting and Setting the default RDR2 directory where PRDR files are located
        /// </summary>
        #region Setters
        private void SetDefaultDirectory()
        {
            try
            {
                string[] fulldir = Directory.GetDirectories($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Rockstar Games\\Red Dead Redemption 2\\Profiles");
                defaultDirPRDR = fulldir[0];
                statusBarTextBlock.Text = "Status: Path set to default directory";
            }
            catch (Exception e)
            {
                MessageBox.Show($"EXCEPTION: SetDefaultDirectory method. NOTE TO USER: Select the 'use custom path' option on this application and paste in the path. See github page for more info and to reach out to the developer. RAW: {e.Message}");
            }

            Debug.WriteLine(defaultDirPRDR);
        }

        /// <summary>
        /// Creates RDR2 Photos and prdr Backups folders if they don't exist, this is where converted files and backups are stored
        /// </summary>
        private void SetAppDirectories()
        {
            string myPictures = "";
            try
            {
                myPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
            catch (Exception e)
            {
                MessageBox.Show($"EXCEPTION: SetAppDirectories Environment.GetFolderPath, USER NOTE: Please reach out to developer on github for assitance RAW: {e.Message}");
            }

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

        /// <summary>
        /// Backs up every file in prdrFiles to backupDirPRDR and checks for duplicates
        /// </summary>
        private string BackupPRDRs()
        {
            int backedUpFiles = 0;
            int duplicateFiles = 0;

            foreach (var file in prdrFiles)
            {
                fileName = file.Substring(activeDir.Length + 1);

                try
                {
                    File.Copy(Path.Combine(activeDir, fileName), Path.Combine(backupDirPRDR, $"{GetMetaData(file)} {file.Substring(activeDir.Length + 1)}"), false);
                    backedUpFiles++;
                }
                catch (IOException)
                {
                    duplicateFiles++;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"BackupPRDRs: {e.Message}");
                }
            }

            return $"{backedUpFiles} files backed up, {duplicateFiles} duplicates.";
        }
    }
}