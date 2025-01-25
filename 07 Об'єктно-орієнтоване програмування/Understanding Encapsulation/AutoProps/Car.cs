using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoProps;

class Car
{
    public string PetName { get; set; } = null!;
    public int Speed { get; set; }
    public string Color { get; set; } = null!;

    public void DisplayStats()
    {
        Console.WriteLine($"Car Name: {PetName}" );
        Console.WriteLine($"Speed: {Speed}");
        Console.WriteLine($"Color: {Color}");
    }
}
