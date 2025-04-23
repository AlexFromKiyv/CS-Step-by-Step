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

namespace AnimationExample
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

        private void btnLabel_Click(object sender, RoutedEventArgs e)
        {
            WindowLabelAnimation windowLabelAnimation = new();
            windowLabelAnimation.Show();
        }

        private void btnKeyFrames_Click(object sender, RoutedEventArgs e)
        {
            WindowKeyFrames windowKeyFrames = new();
            windowKeyFrames.Show();
        }
    }
}