using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SvoyaIgra.Game.Views
{
    /// <summary>
    /// Interaction logic for PlayScreenWindow.xaml
    /// </summary>
    public partial class PlayScreenWindow : Window
    {
        public PlayScreenWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/4189660/why-does-wpf-mediaelement-not-work-on-secondary-monitor
            var hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                var hwndTarget = hwndSource.CompositionTarget;
                if (hwndTarget != null) hwndTarget.RenderMode = RenderMode.SoftwareOnly;
            }
        }
    }
}
