using System.Windows;
using System.Windows.Controls;

namespace SvoyaIgra.Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CockpitWindow : Window
    {
        public CockpitWindow()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var lv = (ListView)sender;
            lv.ScrollIntoView(lv.SelectedItem);
        }
    }
}
