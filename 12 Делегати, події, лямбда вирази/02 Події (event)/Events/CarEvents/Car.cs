using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarEvents
{
    internal class Car
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

        public void ToConsole()
        {
            Console.WriteLine($"\tCar:{Name} Max speed:{MaxSpeed}");
            Console.WriteLine($"\tCurrent speed:{CurrentSpeed}");
        }

        //Define delegate type
        public delegate void CarEngineHandler(string? messageForCaller);

        // This car can send these events
        public event CarEngineHandler AboutToBlow;
        public event CarEngineHandler Exploded;

        //This is a method of changing the current speed
        public void Accelerate(int delta)
        {
            if (_isDead)
            {
                Exploded?.Invoke("Sorry, this car is dead!");
            }
            else
            {
                CurrentSpeed += delta;

                if (CurrentSpeed > MaxSpeed)
                {
                    _isDead = true;
                    CurrentSpeed = 0;
                    Exploded?.Invoke("Car dead!");
                }
                else
                {
                    Console.WriteLine($"\tCurrent speed {Name}: {CurrentSpeed}");
                }

                if ((MaxSpeed - CurrentSpeed) < 10 && !_isDead)
                {
                    AboutToBlow?.Invoke($"Careful buddy! Gonna blow! Current speed:{CurrentSpeed}");
                }
            }
        }
    }
}
