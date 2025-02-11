using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomException;

class Car
{
    private bool _carIsDead;

    public const int MaxSpeed = 100;
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = "NoName";

    private readonly Radio _radio = new Radio();

    public Car()
    {
    }
    public Car(string petName, int currentSpeed)
    {
        CurrentSpeed = currentSpeed;
        PetName = petName;
    }

    public void CrankTunes(bool state)
    {
        // Delegate request to inner object.
        _radio.TurnOn(state);
    }
    //Change current speed.
    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{PetName} is out of order...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = 0;
                _carIsDead = true;

                throw new CarIsDeadException3(
                    $"{PetName} has overheated!",DateTime.Now, "You have a lead foot");
            }
            Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
        }
    }
}


