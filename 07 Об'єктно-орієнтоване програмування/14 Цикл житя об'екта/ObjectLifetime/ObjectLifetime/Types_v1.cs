using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLifetime
{
    class Car
    {
        public string Name { get; set; } = "";
        public int MaxSpeed { get; }
        public int CurrentSpeed { get; set; }


        public Car(string name, int maxSpeed, int currentSpeed)
        {
            Name = name;
            MaxSpeed = maxSpeed;
            CurrentSpeed = currentSpeed;
        }

        public Car() {}

        public override string? ToString() => $"{Name} is going {CurrentSpeed}";
      
    }
}
