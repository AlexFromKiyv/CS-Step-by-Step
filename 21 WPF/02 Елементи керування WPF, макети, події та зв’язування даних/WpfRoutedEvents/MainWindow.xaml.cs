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

namespace WpfRoutedEvents;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnClickMe_Clicked(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Clicked the button");
    }

    private void outerEllipse_MouseDown(object sender, MouseButtonEventArgs e)
    {

    }

    private void outerEllipse_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {

    }
}