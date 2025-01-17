using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithRecords;

class Car
{
    public string Make { get; init; } = null!;
    public string Model { get; init; } = null!;
    public string Color { get; init; } = null!;

    public Car()
    {
    }

    public Car(string make, string model, string color)
    {
        Make = make;
        Model = model;
        Color = color;
    }
}
