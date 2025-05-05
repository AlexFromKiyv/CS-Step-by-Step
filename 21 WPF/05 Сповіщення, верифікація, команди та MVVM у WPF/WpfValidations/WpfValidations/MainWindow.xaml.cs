using System.Collections.ObjectModel;
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
using WpfValidations.Models;

namespace WpfValidations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IList<Car> _cars = new ObservableCollection<Car>();
        public MainWindow()
        {
            InitializeComponent();
            _cars.Add(new Car { Id = 1, Color = "Blue", Make = "Chevy", PetName = "Kit", IsChanged = false });
            _cars.Add(new Car { Id = 2, Color = "Red", Make = "Ford", PetName = "Red Rider", IsChanged = false });
            cboCars.ItemsSource = _cars;
        }

        private void btnChangeColor_Click(object sender, RoutedEventArgs e)
        {
            _cars.First(x => x.Id == ((Car)cboCars.SelectedItem)?.Id).Color = "Pink";
        }

        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {
            int maxId = _cars?.Max(c => c.Id) ?? 0;
            Car car = new()
            {
                Id = ++maxId,
                Color = "Yellow",
                Make = "VW",
                PetName = "Birdie",
                IsChanged = false
            };
            _cars?.Add(car);
        }
    }

}