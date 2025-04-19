using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ObjectResourcesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            // Get the brush and make a change.
            var b = (RadialGradientBrush)Resources["MyBrush"];
            b.GradientStops[1] = new GradientStop(Colors.WhiteSmoke, 0.0);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Put a totally new brush into the myBrush slot.
            Resources["MyBrush"] = new SolidColorBrush(Colors.Red);
        }
    }
}