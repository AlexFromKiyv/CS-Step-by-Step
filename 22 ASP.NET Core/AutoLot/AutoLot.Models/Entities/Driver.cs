namespace AutoLot.Models.Entities;

[EntityTypeConfiguration(typeof(DriverConfiguration))]
public class Driver : BaseEntity
{
    public Person PersonInformation { get; set; } = new Person();

    [InverseProperty(nameof(Car.Drivers))]
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    [InverseProperty(nameof(CarDriver.DriverNavigation))]
    public ICollection<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();
}
