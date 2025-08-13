
namespace AutoLot.Models.ViewModels;

[Keyless]
[EntityTypeConfiguration(typeof(CustomerOrderViewModelConfiguration))]
public partial class CustomerOrderViewModel : INonPersisted
{
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string Color { get; set; } = null!;

    [StringLength(50)]
    public string PetName { get; set; } = null!;

    [StringLength(50)]
    public string Make { get; set; } = null!;

    public bool? IsDrivable { get; set; }
    public string Display { get; set; } = null!;
    public string? Price { get; set; }
    public DateTime? DateBuilt { get; set; }

    [NotMapped]
    public string FullDetail =>
    $"{FirstName} {LastName} ordered a {Color} {Make} named {PetName}";

    public override string ToString() => FullDetail;
}
