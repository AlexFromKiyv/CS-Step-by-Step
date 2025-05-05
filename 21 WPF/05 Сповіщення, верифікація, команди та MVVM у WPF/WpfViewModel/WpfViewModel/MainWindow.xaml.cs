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

using WpfViewModel.ViewModels;

namespace WpfViewModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public IList<Car> _cars = new ObservableCollection<Car>();
        //private ICommand _changeColorCommand = null;
        //public ICommand ChangeColorCmd =>
        //    _changeColorCommand ??= new ChangeColorCommand();

        //private ICommand _addCarCommand = null;
        //public ICommand AddCarCmd
        //  => _addCarCommand ??= new AddCarCommand();

        public MainWindowViewModel ViewModel { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            //_cars.Add(new Car { Id = 1, Color = "Blue", Make = "Chevy", PetName = "Kit", IsChanged = false });
            //_cars.Add(new Car { Id = 2, Color = "Red", Make = "Ford", PetName = "Red Rider", IsChanged = false });
            //cboCars.ItemsSource = _cars;
        }

        //private RelayCommand<Car> _deleteCarCommand = null;
        //public RelayCommand<Car> DeleteCarCmd
        //    => _deleteCarCommand ??= new RelayCommand<Car>(DeleteCar, CanDeleteCar);
        //private bool CanDeleteCar(Car car) => car != null;
        //private void DeleteCar(Car car)
        //{
        //    _cars.Remove(car);
        //}

    }

}