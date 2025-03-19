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

namespace SimpleWpfApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private void btnCanvas_Click(object sender, RoutedEventArgs e)
    {
        WindowCanvas windowCanvas = new WindowCanvas();
        windowCanvas.Show();
    }

    private void btnWrapPanel_Click(object sender, RoutedEventArgs e)
    {
        WindowWrapPanel windowWrapPanel = new WindowWrapPanel();
        windowWrapPanel.Show();
    }

    private void btnStackPanel_Click(object sender, RoutedEventArgs e)
    {
        WindowStackPanel windowStackPanel = new WindowStackPanel();
        windowStackPanel.Show();
    }

    private void btnGrid_Click(object sender, RoutedEventArgs e)
    {
        WindowGrid windowGrid = new();
        windowGrid.Show();
    }

    private void btnGridSplitter_Click(object sender, RoutedEventArgs e)
    {
        WindowGridSplitter windowGridSplitter = new();
        windowGridSplitter.Show();
    }

    private void btnDockPanel_Click(object sender, RoutedEventArgs e)
    {
        WindowDockPanel windowDockPanel = new();
        windowDockPanel.Show();
    }

    private void btnScrollViewer_Click(object sender, RoutedEventArgs e)
    {
        WindowScrollViewer windowScrollViewer = new();
        windowScrollViewer.Show();
    }
}