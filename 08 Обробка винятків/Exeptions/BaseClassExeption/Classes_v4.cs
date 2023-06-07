using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClassExeption
{
    class Car_v4
    {
        public const int MAXSPEED = 140;
        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }

        private bool _carIsDead;

        public Car_v4(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }
        public Car_v4()
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
                    int tempSpeed = CurrentSpeed;
                    CurrentSpeed = 0;
                    _carIsDead = true;
                    throw new Exception($"{Name} has overheated!") 
                    { 
                        Data =
                        {
                            {"TimeStamp",$"The car exploded at {DateTime.Now}" },
                            {"Clause",$"The speed is too high {tempSpeed}. Maximum speed is {MAXSPEED}" }
                        }
                    };
                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }
}
