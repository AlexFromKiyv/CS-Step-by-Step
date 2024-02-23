using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarLibrary;

public class SportCar : Car
{
    public SportCar()
    {
    }
    public SportCar(string name, int currentSpeed, int maxSpeed) : base(name, currentSpeed, maxSpeed)
    {
    }
    public override void TurboBoost()
    {
        Console.WriteLine("Ramming speed! Faster is better...");
    }
}