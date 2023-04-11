using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YourOwnExceptions
{
    class Car_v3    
    {
        public const int MAXSPEED = 140;
        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }

        private bool _carIsDead;

        public Car_v3(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }
        public Car_v3()
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
                    throw new CarIsDead_v3_Exception("Speed too high.", tempCurrentSpeed, $"{Name} has overhead");

                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }

    public class CarIsDead_v3_Exception : ApplicationException
    {
        public CarIsDead_v3_Exception()
        {
        }

        public CarIsDead_v3_Exception(string? message) : base(message)
        {
        }

        public CarIsDead_v3_Exception(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CarIsDead_v3_Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CarIsDead_v3_Exception(string? causeOfError, int speed, string message) :base(message) 
        {
            CauseOfError = causeOfError;
            Speed = speed;
        }

        public CarIsDead_v3_Exception( string? causeOfError, int speed, string? message, Exception? innerException) : base(message, innerException)
        {
            CauseOfError = causeOfError;
            Speed = speed;
        }

        public string? CauseOfError { get; }
        public int Speed { get; }
    }

}
