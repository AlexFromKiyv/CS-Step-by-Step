using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourOwnExceptions
{
    class Car_v2
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
                    int tempCurrentSpeed = CurrentSpeed;
                    CurrentSpeed = 0;
                    _carIsDead = true;
                    throw new CarIsDead_v2_Exception("Speed too high.", tempCurrentSpeed, $"{Name} has overheated!");

                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }

    public class CarIsDead_v2_Exception : ApplicationException
    {
        public CarIsDead_v2_Exception(string? cause, int speed, string message) :base(message)
        {
            CauseOfError = cause;
            Speed = speed;
        }
        public string? CauseOfError { get; }
        public int Speed { get; }

    }
}
