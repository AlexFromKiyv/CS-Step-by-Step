using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordInheritance;

public record Car1
{
    public string Make { get; init; } = null!;
    public string Model { get; init; } = null!;
    public string Color { get; init; } = null!;

    public Car1(string make, string model, string color)
    {
        Make = make;
        Model = model;
        Color = color;
    }
}
