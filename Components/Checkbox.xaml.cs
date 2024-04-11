using System.Windows;
using System.Windows.Controls;

namespace RDR2PhotoConverter.Components
{
    /// <summary>
    /// Interaction logic for Checkbox.xaml
    /// </summary>
    public partial class Checkbox : UserControl
    {
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(Checkbox), new PropertyMetadata(false));

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(string), typeof(Checkbox), new PropertyMetadata("Checkbox"));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public Checkbox()
        {
            InitializeComponent();
        }
    }
}
