using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
        private string fileName;

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
            dirInputTextBox.Text = defaultDirPRDR;
        }

        private void OnCustomPathClicked(object sender, RoutedEventArgs e)
        {
            dirInputTextBox.Text = "Paste your custom path here";
        }

        private void OnDblClickTextBox(object sender, MouseButtonEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void OnSetDirectoryClick(object sender, RoutedEventArgs e)
        {
            if (myDefaultPathRadioButton.IsChecked == true)
            {
                statusBarTextBlock.Text = $"Valid Default Path found";
            }
            else if (myCustomPathRadioButton.IsChecked == true)
            {
                GetCustomDir();
                activeDir = customDirPRDR;
            }
        }

        private void OnConvertFilesClick(object sender, RoutedEventArgs e)
        {
            GetValidFiles(activeDir);

            if (myBackupCheckbox.IsChecked == true)
            {
                BackupPRDRs();
            }

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

                File.WriteAllBytes($"{convertedFilesDir}\\{fileName}.jpg", fileInBytesTemp);

                if (myDeleteCheckbox.IsChecked == true)
                {
                    File.Delete(file);
                }
            }

            statusBarTextBlock.Text += $"{prdrFiles.Count} files converted into images.";

            prdrFiles.Clear();

            MessageBox.Show($"{statusBarTextBlock.Text} All done!");
        }

        private void OnMyTwitterClick(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://twitter.com/SneakyAzWhat",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void OnMyGithubClick(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://github.com/SneakyAzWhat",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        #endregion

        #region Getters

        private void GetCustomDir()
        {
            customDirPRDR = dirInputTextBox.Text;
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

        private string GetMetaData(string file)
        {
            byte[] fileInBytes = File.ReadAllBytes(file);

            string dataString = "";

            //Iterating through the indexes 20-47 to get the date/time the picture was taken (These indexes contain the most pertinent information)
            for (int i = 20; i < 48; i++)
            {
                if (fileInBytes[i] > 31) //bytes < 31 are ascii and are not relevant for our task
                {
                    switch (fileInBytes[i])
                    {
                        case 47:
                            dataString += "-"; // Replacing / with -
                            break;
                        case 58:
                            dataString += ""; //Replacing : with nothing
                            break;
                        default:
                            dataString += $"{Convert.ToChar(fileInBytes[i])}";
                            break;
                    }
                }
            }
            return dataString;
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

        private void BackupPRDRs()
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
                catch (Exception)
                {
                    duplicateFiles++;
                }
            }

            statusBarTextBlock.Text = $"{backedUpFiles} files backed up, {duplicateFiles} duplicates. ";
        }
    }
}
