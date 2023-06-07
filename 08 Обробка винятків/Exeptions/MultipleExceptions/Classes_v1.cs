using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MultipleExceptions
{
    class Car_v1
    {

        public const int MAXSPEED = 140;
        private bool _carIsDead;

        public Car_v1(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }
        public Car_v1() { }

        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }


        public void Accelerate(int delta)
        {

            if (delta < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(delta), "Acceleration must be greater than zero.");
            }


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
                    throw new CarIsDead_v1_Exception("Speed too high.",tempCurrentSpeed,$"{Name} broke down.");

                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }

    public class CarIsDead_v1_Exception : ApplicationException
    {
        public CarIsDead_v1_Exception()
        {
        }

        public CarIsDead_v1_Exception(string? message) : base(message)
        {
        }

        public CarIsDead_v1_Exception(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CarIsDead_v1_Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CarIsDead_v1_Exception(string? cause, int speed,  string? message) : base(message)
        {
            Cause = cause;
            Speed = speed;
        }

        public CarIsDead_v1_Exception(string? cause, int speed, string? message, Exception? innerException) : base(message,innerException)
        {
            Cause = cause;
            Speed = speed;
        }

        public string? Cause { get; }
        public int Speed { get; }
    }

}
