using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClassExeption
{
    class Car_v1
    {
        public const int MAXSPEED = 140;

        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }


        private bool _carIsDead;

        public Car_v1(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }
        public Car_v1()
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
                    Console.WriteLine($"{Name} has overheated!");
                    CurrentSpeed = 0;
                    _carIsDead = true;
                }
                else
                {
                    Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
                }
            }
        }
    }

}
