using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples.Models;
[Table("Inventory",Schema ="dbo")]
[Index(nameof(MakeId),Name = "IX_Inventory_MakeId")]
[EntityTypeConfiguration(typeof(CarConfiguration))]
public class Car : BaseEntity
{
    private string _color;
    [Required,StringLength(50)]
    public string Color { get => _color; set => _color = value; }
    [Required,StringLength(50)]
    public string PetName { get; set; }
    public int MakeId { get; set; }
    [ForeignKey(nameof(MakeId))]
    public Make MakeNavigation { get; set; }
    public Radio RadioNavigation { get; set; }
    [InverseProperty(nameof(Driver.Cars))]
    public IEnumerable<Driver> Drivers { get; set; }
    public DateTime? DateBuild { get; set; }
    //public bool IsDrivable { get; set; }
    private bool? _IsDrivable;
    public bool IsDrivable
    {
        get => _IsDrivable ?? true;
        set => _IsDrivable = value;
    }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string Display { get; set; }

    [InverseProperty(nameof(CarDriver.CarNavigation))]
    public IEnumerable<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();

}
