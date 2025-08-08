
namespace AutoLot.Models.Entities;

[Table("Inventory")]
[Index("MakeId", Name = "IX_Inventory_MakeId")]
[EntityTypeConfiguration(typeof(CarConfiguration))]
public partial class Car : BaseEntity
{
    [Required]
    [DisplayName("Make")]
    public int MakeId { get; set; }

    private bool? _isDrivable;

    [Required]
    [DisplayName("Is Drivable")]
    public bool IsDrivable
    {
        get => _isDrivable ?? true;
        set => _isDrivable = value;
    }

    [StringLength(50)]
    public string Color { get; set; } = null!;

    [StringLength(50)]
    [DisplayName("Pet Name")]
    public string PetName { get; set; } = null!;
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string Display { get; set; }
    public string? Price { get; set; }
    public DateTime? DateBuilt { get; set; }

    [InverseProperty(nameof(Driver.Cars))]
    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    [InverseProperty(nameof(CarDriver.CarNavigation))]
    public virtual ICollection<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();

    [ForeignKey("MakeId")]
    [InverseProperty(nameof(Make.Cars))]
    public virtual Make MakeNavigation { get; set; } = null!;

    [InverseProperty(nameof(Radio.CarNavigation))]
    public virtual Radio RadioNavigation { get; set; } = null!;

    [InverseProperty(nameof(Order.CarNavigation))]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [NotMapped]
    public string MakeName => MakeNavigation?.Name ?? "Unknown";

    public override string? ToString()
    {
        return $"{Id}\t{PetName ?? "No name"}\t{Color}\t{MakeNavigation?.Name}";
    }
}
