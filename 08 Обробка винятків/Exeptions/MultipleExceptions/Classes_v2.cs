using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MultipleExceptions
{
    class Radio
    {
        public void Switch(bool on)
        {
            Console.WriteLine(on ? "Jamming ..." : "Quiet time...");
        }
    }

    class Car_v2
    {
        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; } 
        public int MaximumSpeed { get; }

        private readonly Radio radio = new Radio();

        private bool _carIsDead;

        public Car_v2()
        {
        }

        public Car_v2(string name, int currentSpeed, int maximumSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
            MaximumSpeed = maximumSpeed;
        }

        public void RadioSwitch(bool state)
        {
            radio.Switch(state);
        }

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
                if (CurrentSpeed > MaximumSpeed)
                {
                    int tempCurrentSpeed = CurrentSpeed;
                    CurrentSpeed = 0;
                    _carIsDead = true;
                    throw new CarIsDead_v2_Exception("Speed too high.",DateTime.Now, tempCurrentSpeed, $"{Name} broke down.");

                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }


    public class CarIsDead_v2_Exception : ApplicationException
    {
        public CarIsDead_v2_Exception()
        {
        }
        public CarIsDead_v2_Exception(string? cause, DateTime time,int speed) : this(cause,time,speed,string.Empty)
        {
        }
        public CarIsDead_v2_Exception(string? cause, DateTime time, int speed, string? message) : this(cause,time,speed,message,null)
        {
            Cause = cause;
            Speed = speed;
        }

        public CarIsDead_v2_Exception(string? cause, DateTime time, int speed, string? message, Exception? innerException) : base(message, innerException)
        {
            Cause = cause;
            Speed = speed;
            ErrorTimeStamp = time;
        }

        public string? Cause { get; }
        public int Speed { get; }
        public DateTime ErrorTimeStamp { get; set; }
    }

}
