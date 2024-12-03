using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoLot.Models.Entities.Configuration;
using Microsoft.EntityFrameworkCore;


[Table("Inventory",Schema = "dbo")]
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

    [Required]
    [StringLength(50)]
    public string Color { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [DisplayName("Pet Name")]
    public string PetName { get; set; } = null!;
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string Display { get; set; }
    public string? Price { get; set; }
    public DateTime? DateBuild { get; set; }


    [ForeignKey(nameof(MakeId))]
    [InverseProperty(nameof(Make.Cars))]
    public virtual Make MakeNavigation { get; set; } = null!;

    [InverseProperty(nameof(Order.CarNavigation))]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    
    //many-to-many
    [InverseProperty(nameof(Driver.Cars))]
    public virtual IEnumerable<Driver> Drivers { get; set; } = new List<Driver>();
    [InverseProperty(nameof(CarDriver.CarNavigation))]
    public IEnumerable<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();
    //many-to many

    [InverseProperty(nameof(Radio.CarNavigation))]
    public virtual Radio RadioNavigation { get; set; }


    [NotMapped]
    public string MakeName => MakeNavigation?.Name ?? "Unknown";

    public override string? ToString()
    {
        return $"{PetName ?? "No name"} is a {Color} {MakeNavigation?.Name} with Id:{Id}";
    }
}
