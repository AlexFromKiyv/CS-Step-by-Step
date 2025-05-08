using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEachWithExtensionMethods;

class Garage
{
    public Car[] CarsInGarage { get; set; }

    // Fill with some Car objects upon startup.
    public Garage()
    {
        CarsInGarage = new Car[4];
        CarsInGarage[0] = new Car("Rusty", 30);
        CarsInGarage[1] = new Car("Clunker", 55);
        CarsInGarage[2] = new Car("Zippy", 30);
        CarsInGarage[3] = new Car("Fred", 30);
    }
}
