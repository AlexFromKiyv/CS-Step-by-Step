using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoLot.Models.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Models.Entities;
[Table("Makes",Schema ="dbo")]
[EntityTypeConfiguration(typeof(MakeConfiguration))]
public partial class Make :BaseEntity
{

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty(nameof(Car.MakeNavigation))]
    public virtual IEnumerable<Car> Cars { get; set; } = new List<Car>();
}
