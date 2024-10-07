using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples.Models;

[Table("InventoryToDrivers",Schema ="dbo")]
public class CarDriver : BaseEntity
{
    public int DriverId { get; set; }
    [ForeignKey(nameof(DriverId))]
    public Driver DriverNavigation { get; set; }

    [Column("InvertoryId")]
    public int CarId { get; set; }
    [ForeignKey(nameof(CarId))]
    public Car CarNavigation { get; set; }
}
