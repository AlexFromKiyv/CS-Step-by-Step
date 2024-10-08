﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples.Models;

[EntityTypeConfiguration(typeof(DriverConfiguration))]
public class Driver : BaseEntity
{
    //[Required, StringLength(50)]
    //public string FirstName { get; set; }
    //[Required, StringLength(50)]
    //public string LastName { get; set; }
    //[InverseProperty(nameof(Car.Drivers))]
    public Person PersonInfo { get; set; } = new Person();
    public IEnumerable<Car> Cars { get; set; } = new List<Car>();
    [InverseProperty(nameof(CarDriver.DriverNavigation))]
    public IEnumerable<CarDriver> CarDrivers { get; set; } = new List<CarDriver>();
}
