using RDR2PhotoConverter.Frames;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using TextBox = System.Windows.Controls.TextBox;
using Serilog;
using Microsoft.Extensions.Logging;

namespace RDR2PhotoConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger<MainWindow> _logger;

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

            // Setup logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RDR2PhotoConverter.log"))
                .CreateLogger();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog());
            _logger = loggerFactory.CreateLogger<MainWindow>();

            directorySelectPage = new DirectorySelect();
            directorySelectPage.defaultButton.Click += OnDefaultPathClicked;
            directorySelectPage.browseButton.Click += OnBrowseClick;
            directorySelectPage.convertButton.Click += OnConvertFilesClick;
            parentContainer.Content = directorySelectPage;
            titleBar.Text = "RDR2 Photo Converter";
            try
            {
                userName = Environment.UserName;
                _logger.LogInformation("Application started by user: {UserName}", userName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception when assigning userName");
                MessageBox.Show($"Exception when assigning userName = Environment.UserName, USER NOTE: Please reach out to developer on github for assistance, RAW:{e.Message}");
            }

            SetDefaultDirectory();
            SetAppDirectories();

            activeDir = defaultDirPRDR ?? throw new InvalidOperationException("Default directory could not be set.");
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
            statusBarTextBlock.Text = "Status: Path set to default directory.";
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
        /// Converts list of prdrFiles into images asynchronously, if you checked the 'backup' or 'delete' boxes those things will happen here too
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private async void OnConvertFilesClick(object sender, RoutedEventArgs e)
        {
            if (!GetCustomDir()) return;

            GetValidFiles(activeDir);

            string backupInfo = "";
            if (directorySelectPage.backupToggle == true)
            {
                backupInfo = await BackupPRDRsAsync();
            }

            foreach (var file in prdrFiles)
            {
                await ConvertFileAsync(file);
            }

            statusBarTextBlock.Text = $"Status: {backupInfo} {prdrFiles.Count} files converted into images.";

            prdrFiles.Clear();

            MessageBox.Show($"{statusBarTextBlock.Text} All done!");
        }

        /// <summary>
        /// Converts a single PRDR file to JPG asynchronously.
        /// </summary>
        /// <param name="filePath">The path to the PRDR file.</param>
        private async Task ConvertFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                _logger.LogWarning("File path is null or empty.");
                return;
            }

            string metadata = GetMetaData(filePath);
            fileName = $"{metadata} {Path.GetFileName(filePath)}";

            try
            {
                byte[] fileInBytes = await File.ReadAllBytesAsync(filePath);
                if (fileInBytes.Length <= 300)
                {
                    _logger.LogWarning("File {FilePath} is too small to contain image data.", filePath);
                    return;
                }

                // Use Span<T> for performance
                Span<byte> span = fileInBytes.AsSpan(300);
                byte[] fileInBytesTemp = span.ToArray();

                string outputPath = Path.Combine(convertedFilesDir, $"{fileName}.jpg");
                await File.WriteAllBytesAsync(outputPath, fileInBytesTemp);
                _logger.LogInformation("Converted {FilePath} to {OutputPath}", filePath, outputPath);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception during file conversion for {FilePath}", filePath);
                MessageBox.Show($"EXCEPTION: File conversion, USER NOTE: chances are you just tried to convert the same files back to back OR some type of AntiVirus program is blocking the program from running properly. You can try restarting the application to see if that fixes the problem.\n\n{exception.Message} ");
            }

            if (directorySelectPage.deleteToggle == true)
            {
                try
                {
                    File.Delete(filePath);
                    _logger.LogInformation("Deleted original file {FilePath}", filePath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to delete {FilePath}", filePath);
                }
            }
        }

        /// <summary>
        /// Clicking Bluesky button in bottom right corner opens your browser and directs you to my Bluesky account
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void OnMyBlueskyClick(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://bsky.app/profile/sneakyazwhat.bsky.social",
                UseShellExecute = true
            };
            Process.Start(psi);
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
        /// <returns>True if the path is valid; otherwise, false.</returns>
        private bool GetCustomDir()
        {
            customDirPRDR = directorySelectPage?.dirInputTextBox?.Text;
            if (string.IsNullOrWhiteSpace(customDirPRDR))
            {
                _logger.LogWarning("Custom directory path is null or empty.");
                statusBarTextBlock.Text = "Status: Invalid Path entered, path is empty.";
                MessageBox.Show("Invalid Custom Path entered, path is empty. Please enter a valid path.");
                return false;
            }

            if (!Directory.Exists(customDirPRDR))
            {
                _logger.LogWarning("Custom directory {Path} does not exist.", customDirPRDR);
                statusBarTextBlock.Text = "Status: Invalid Path entered, please double check your entered path and try again.";
                MessageBox.Show("Invalid Custom Path entered, please double check your entered path and try again \n\n Example of a valid path: \n I:\\SomeFolder\\AnotherFolder");
                return false;
            }

            activeDir = customDirPRDR;
            statusBarTextBlock.Text = "Status: Valid Path entered.";
            _logger.LogInformation("Custom directory set to {Path}", customDirPRDR);
            return true;
        }

        /// <summary>
        /// Searching the activeDir for PRDR files and adding them to prdrFiles List
        /// </summary>
        /// <param name="path">The directory path to search for PRDR files.</param>
        private void GetValidFiles(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                _logger.LogError("Path is null or empty in GetValidFiles.");
                return;
            }

            if (!Directory.Exists(path))
            {
                _logger.LogError("Directory {Path} does not exist.", path);
                return;
            }

            string[] files;
            try
            {
                files = Directory.GetFiles(path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get files from {Path}", path);
                return;
            }

            foreach (var file in files)
            {
                if (Path.GetFileName(file).Contains("PRDR", StringComparison.OrdinalIgnoreCase))
                {
                    prdrFiles.Add(file);
                }
            }

            statusBarTextBlock.Text = $"Status: {prdrFiles.Count} PRDR files retrieved, ready to convert.";
            _logger.LogInformation("Retrieved {Count} PRDR files from {Path}", prdrFiles.Count, path);
        }

        /// <summary>
        /// Reading the MetaData of each PRDR file to get the date and time when the picture was taken
        /// </summary>
        /// <param name="filePath">The path to the PRDR file.</param>
        /// <returns>The formatted date and time string.</returns>
        private static string GetMetaData(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return "unknown-date unknown-time";
            }

            byte[] fileInBytes = File.ReadAllBytes(filePath);
            if (fileInBytes.Length < 55)
            {
                return "unknown-date unknown-time";
            }

            // Use Span<T> for performance
            Span<byte> span = fileInBytes.AsSpan(20, 34); // 20 to 53 inclusive is 34 bytes
            string dataString = "";
            foreach (byte b in span)
            {
                if (b > 31) // bytes < 31 are ascii and not relevant
                {
                    dataString += Convert.ToChar(b);
                }
            }

            var parts = dataString.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                return "unknown-date unknown-time";
            }

            var dateParts = parts[0].Split("/", StringSplitOptions.RemoveEmptyEntries);
            if (dateParts.Length < 3)
            {
                return "unknown-date unknown-time";
            }

            string month = dateParts[0],
                day = dateParts[1],
                year = dateParts[2];
            var timeParts = parts[1].Split(":", StringSplitOptions.RemoveEmptyEntries);
            if (timeParts.Length < 3)
            {
                return "unknown-date unknown-time";
            }

            string hour = timeParts[0],
                minute = timeParts[1],
                second = timeParts[2];

            return $"{year}-{month}-{day} {hour}.{minute}.{second}";
        }
        #endregion

        /// <summary>
        /// Getting and Setting the default RDR2 directory where PRDR files are located
        /// </summary>
        private void SetDefaultDirectory()
        {
            try
            {
                string profilesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Rockstar Games", "Red Dead Redemption 2", "Profiles");
                if (!Directory.Exists(profilesPath))
                {
                    _logger.LogWarning("Default RDR2 profiles directory does not exist: {Path}", profilesPath);
                    throw new DirectoryNotFoundException($"Profiles directory not found: {profilesPath}");
                }

                string[] fulldir = Directory.GetDirectories(profilesPath);
                if (fulldir.Length == 0)
                {
                    _logger.LogWarning("No profile directories found in {Path}", profilesPath);
                    throw new DirectoryNotFoundException("No profile directories found.");
                }

                defaultDirPRDR = fulldir[0];
                statusBarTextBlock.Text = "Status: Path set to default directory";
                _logger.LogInformation("Default directory set to {Path}", defaultDirPRDR);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception in SetDefaultDirectory");
                MessageBox.Show($"EXCEPTION: SetDefaultDirectory method. NOTE TO USER: Select the 'use custom path' option on this application and paste in the path. See github page for more info and to reach out to the developer. RAW: {e.Message}");
            }
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
                if (string.IsNullOrEmpty(myPictures))
                {
                    throw new InvalidOperationException("My Pictures folder path is null or empty.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception in SetAppDirectories");
                MessageBox.Show($"EXCEPTION: SetAppDirectories Environment.GetFolderPath, USER NOTE: Please reach out to developer on github for assistance RAW: {e.Message}");
                return;
            }

            convertedFilesDir = Path.Combine(myPictures, "RDR2 Photos");
            backupDirPRDR = Path.Combine(convertedFilesDir, "prdr backups");

            try
            {
                if (!Directory.Exists(convertedFilesDir))
                {
                    Directory.CreateDirectory(convertedFilesDir);
                    _logger.LogInformation("Created directory {Path}", convertedFilesDir);
                }
                if (!Directory.Exists(backupDirPRDR))
                {
                    Directory.CreateDirectory(backupDirPRDR);
                    _logger.LogInformation("Created directory {Path}", backupDirPRDR);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create app directories");
            }
        }
        #endregion

        /// <summary>
        /// Backs up every file in prdrFiles to backupDirPRDR asynchronously and checks for duplicates
        /// </summary>
        /// <returns>A string summarizing the backup operation.</returns>
        private async Task<string> BackupPRDRsAsync()
        {
            int backedUpFiles = 0;
            int duplicateFiles = 0;

            foreach (var file in prdrFiles)
            {
                string fileName = Path.GetFileName(file);
                string destFileName = $"{GetMetaData(file)} {fileName}";
                string destPath = Path.Combine(backupDirPRDR, destFileName);

                try
                {
                    await Task.Run(() => File.Copy(file, destPath, false));
                    backedUpFiles++;
                    _logger.LogInformation("Backed up {File} to {DestPath}", file, destPath);
                }
                catch (IOException)
                {
                    duplicateFiles++;
                    _logger.LogWarning("Duplicate file {File}, skipping backup.", file);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Exception during backup for {File}", file);
                    MessageBox.Show($"BackupPRDRs: {e.Message}");
                }
            }

            return $"{backedUpFiles} files backed up, {duplicateFiles} duplicates.";
        }
    }
}