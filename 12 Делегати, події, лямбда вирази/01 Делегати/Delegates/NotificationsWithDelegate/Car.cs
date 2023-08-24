using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsWithDelegate
{
    internal class Car
    {
        // State data
        public string Name { get; set; } = string.Empty;
        public int MaxSpeed { get; set; } = 100;
        public int CurrentSpeed { get; set; }

        private bool _isDead;

        //Constructors
        public Car(string name, int maxSpeed, int currentSpeed )
        {
            Name = name;
            MaxSpeed = maxSpeed;
            CurrentSpeed = currentSpeed;
        }
        public Car()
        {
        }

        // For delegate work

        //Define delegate type
        public delegate void CarEngineHandler(string messageForCaller);

        //Variable for delegate
        private CarEngineHandler _listOfHandlers;

        //For external caller allows register method for call 
        public void RegisterCarEngineHandler(CarEngineHandler methodToCall)
        {
            _listOfHandlers = methodToCall;
        } 
        
        public void Accelerate(int delta)
        {
            if (_isDead)
            {
                _listOfHandlers?.Invoke("Sorry, this car is dead!");
            }
            else
            {
                CurrentSpeed += delta;
                

                if (CurrentSpeed > MaxSpeed)
                {
                    _isDead = true;
                    _listOfHandlers?.Invoke("Car dead!");
                }
                else
                {
                    Console.WriteLine($"Current speed {Name}: {CurrentSpeed}");
                }

                if ((MaxSpeed - CurrentSpeed) < 10 && !_isDead )
                {
                    _listOfHandlers?.Invoke($"Careful buddy! Gonna blow! Current speed:{CurrentSpeed}");
                }
            }
        }
    }
}
