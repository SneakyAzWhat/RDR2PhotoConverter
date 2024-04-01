using RDR2PhotoConverter.Frames;
using System.Windows;
using System.Windows.Input;

namespace RDR2PhotoConverter.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DirectorySelect DirectorySelectPage { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DirectorySelectPage = new DirectorySelect();
            ParentContainer.Content = DirectorySelectPage;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TitleBar_Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
