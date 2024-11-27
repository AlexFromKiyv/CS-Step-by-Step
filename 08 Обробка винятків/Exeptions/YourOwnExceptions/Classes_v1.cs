using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourOwnExceptions;

public class CarIsDead_v1_Exception : ApplicationException
{
    private string? _messageDetails;
    public string? CauseOfError { get; }
    public int Speed { get; }

    public CarIsDead_v1_Exception()
    {
    }
    public CarIsDead_v1_Exception(string? message, string? cause, int speed)
    {
        _messageDetails = message;
        CauseOfError = cause;
        Speed = speed;
    }
    public override string Message => $"Car error message:\t{_messageDetails}";
}

public class Car_v1
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
                int tempCurrentSpeed = CurrentSpeed;
                CurrentSpeed = 0;
                _carIsDead = true;
                throw new CarIsDead_v1_Exception($"{Name} has overheated!", "Speed too high.", tempCurrentSpeed);
            }
            Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
        }
    }
}


