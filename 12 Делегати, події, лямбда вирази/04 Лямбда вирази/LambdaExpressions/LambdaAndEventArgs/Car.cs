using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaAndEventArgs
{
    class Car
    {
        // State data
        public string Name { get; set; } = string.Empty;
        public int MaxSpeed { get; set; } = 100;
        public int CurrentSpeed { get; set; }

        private bool _isDead;

        //Constructors
        public Car(string name, int maxSpeed, int currentSpeed)
        {
            Name = name;
            MaxSpeed = maxSpeed;
            CurrentSpeed = currentSpeed;
        }
        public Car()
        {
        }

        public override string? ToString() => Name;

        // This car can send these events
        public event EventHandler<CarEventArgs> AboutToBlow;
        public event EventHandler<CarEventArgs> Exploded;

        //This is a method of changing the current speed
        public void Accelerate(int delta)
        {
            if (_isDead)
            {
                Exploded?.Invoke(this, new CarEventArgs("Sorry, this car is dead!"));
            }
            else
            {
                CurrentSpeed += delta;

                if (CurrentSpeed > MaxSpeed)
                {
                    _isDead = true;
                    CurrentSpeed = 0;
                    Exploded?.Invoke(this, new CarEventArgs("Car dead!"));
                }
                else
                {
                    Console.WriteLine($"\tCurrent speed {Name}: {CurrentSpeed}");
                }

                if ((MaxSpeed - CurrentSpeed) < 10 && !_isDead)
                {
                    AboutToBlow?.Invoke(this, new CarEventArgs($"Careful buddy! Gonna blow! Current speed:{CurrentSpeed}"));
                }
            }
        }
    }
}
