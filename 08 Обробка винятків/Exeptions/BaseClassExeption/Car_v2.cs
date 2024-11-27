using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClassExeption;

public class Car_v2
{
    public const int MAXSPEED = 140;
    public string Name { get; set; } = "";
    public int CurrentSpeed { get; set; }

    private bool _carIsDead;

    public Car_v2(string name, int currentSpeed)
    {
        Name = name;
        CurrentSpeed = currentSpeed;
    }
    public Car_v2()
    {
    }

    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{Name} is out of order ...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MAXSPEED)
            {
                CurrentSpeed = 0;
                _carIsDead = true;
                throw new Exception($"{Name} has overheated!");
            }
            Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
        }
    }
}