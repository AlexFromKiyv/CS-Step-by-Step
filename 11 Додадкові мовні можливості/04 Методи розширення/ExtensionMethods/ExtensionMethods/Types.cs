using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods;

internal class Car
{
    public string Name { get; set; } = "";
    public int CurrentSpeed { get; set; }

    public Car(string name, int currentSpeed)
    {
        Name = name;
        CurrentSpeed = currentSpeed;
    }
    public Car()
    {
    }

    public override string? ToString() => $"{Name} {CurrentSpeed}"; 

}

class Garage
{
    public Car[] Cars { get; set; }

    public Garage(Car[] cars)
    {
        Cars = cars;
    }
}

static class GarageExtentions
{
    public static IEnumerator GetEnumerator(this Garage garage)
        => garage.Cars.GetEnumerator();
}
