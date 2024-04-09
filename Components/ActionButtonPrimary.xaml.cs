using System.Windows;
using System.Windows.Controls;

namespace RDR2PhotoConverter.Components
{
    /// <summary>
    /// Interaction logic for ActionButtonPrimary.xaml
    /// </summary>
    public partial class ActionButtonPrimary : UserControl
    {
        // Event handler for when the custom button is clicked
        public event RoutedEventHandler Click;
        public ActionButtonPrimary()
        {
            InitializeComponent();
        }

        private void ActionButtonPrimary_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // call passed method if exists
            Click?.Invoke(this, e);
        }

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof(string), typeof(ActionButtonPrimary), new PropertyMetadata("Action Button Primary Content"));

        // Dependency property for button content
        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }
    }
}
