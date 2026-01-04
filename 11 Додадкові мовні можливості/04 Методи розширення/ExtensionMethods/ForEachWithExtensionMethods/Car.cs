using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEachWithExtensionMethods;
class Car
{
    // Car properties.
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = string.Empty;

    // Constructors.
    public Car() { }

    public Car(string petName, int currentSpeed)
    {
        PetName = petName;
        CurrentSpeed = currentSpeed;
    }
}
