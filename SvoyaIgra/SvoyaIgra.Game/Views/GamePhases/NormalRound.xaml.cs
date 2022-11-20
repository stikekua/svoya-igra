using System.Windows;
using System.Windows.Controls;

namespace SvoyaIgra.Game.Views.GamePhases
{
    /// <summary>
    /// Interaction logic for NormalRound.xaml
    /// </summary>
    public partial class NormalRound : UserControl
    {


        public object WhichScreenLocation
        {
            get { return (object)GetValue(WhichScreenLocationProperty); }
            set { SetValue(WhichScreenLocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCockpit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WhichScreenLocationProperty =
            DependencyProperty.Register("WhichScreenLocation", typeof(object), typeof(NormalRound), new PropertyMetadata(0));



        public NormalRound()
        {
            InitializeComponent();
        }
    }
}
