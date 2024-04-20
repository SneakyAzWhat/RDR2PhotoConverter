using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RDR2PhotoConverter.Components
{
    /// <summary>
    /// Interaction logic for CustomToggle.xaml
    /// </summary>
    public partial class CustomToggle : UserControl
    {
        private bool _isChecked = false;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                UpdateVisualState();
            }
        }

        public CustomToggle()
        {
            InitializeComponent();
            UpdateVisualState();
        }

        private void ToggleGrid_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            IsChecked = !IsChecked;
        }

        private void UpdateVisualState()
        {
            if (IsChecked)
            {
                var animation = new DoubleAnimation(-20, 20, TimeSpan.FromSeconds(0.2));
                NoText.Visibility = Visibility.Collapsed;
                YesText.Visibility = Visibility.Visible;
                ToggleGrid.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 193, 0));
                ToggleTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            }
            else
            {
                var animation = new DoubleAnimation(20, -20, TimeSpan.FromSeconds(0.2));
                NoText.Visibility = Visibility.Visible;
                YesText.Visibility = Visibility.Collapsed;
                ToggleGrid.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(211, 211, 211));
                ToggleTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            }
        }
    }
}
