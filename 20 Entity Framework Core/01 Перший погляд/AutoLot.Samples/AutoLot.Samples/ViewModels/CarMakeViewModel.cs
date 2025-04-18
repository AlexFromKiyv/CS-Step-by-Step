﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples.ViewModels;
[Keyless]
[EntityTypeConfiguration(typeof(CarMakeViewModelConfiguration))]
public class CarMakeViewModel
{
    public int MakeId { get; set; }
    public string Make { get; set; }
    public int CarId { get; set; }
    public bool IsDrivable { get; set; }
    public string Display { get; set; }
    public DateTime DateBuild { get; set; }
    public string Color { get; set; }
    public string PetName { get; set; }
    [NotMapped]
    public string FullDetail => $"The {Color} {Make} is named {PetName}";
    public override string? ToString() => FullDetail;
}
