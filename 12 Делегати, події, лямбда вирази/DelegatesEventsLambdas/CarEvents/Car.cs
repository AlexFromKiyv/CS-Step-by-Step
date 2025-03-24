using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarEvents;

public class Car
{
    // Internal state data.
    public int CurrentSpeed { get; set; }
    public int MaxSpeed { get; set; }
    public string PetName { get; set; } = string.Empty;

    // Is the car alive or dead?
    private bool _carIsDead;

    // Class constructors.
    public Car() { MaxSpeed = 100; }
    public Car(string name, int maxSp, int currSp)
    {
        CurrentSpeed = currSp;
        MaxSpeed = maxSp;
        PetName = name;
    }

    // This delegate works in conjunction with the
    // Car's events.
    public delegate void CarEngineHandler(string msgForCaller);

    // This car can send these events.
    public event CarEngineHandler? Exploded;
    public event CarEngineHandler? AboutToBlow;

    public void Accelerate(int delta)
    {
        // If the car is dead, fire Exploded event.
        if (_carIsDead)
        {
            Exploded?.Invoke("Sorry, this car is dead...");
        }
        else
        {
            CurrentSpeed += delta;

            // Check speed
            if (CurrentSpeed >= MaxSpeed)
            {
                _carIsDead = true;
            }
            else
            {
                Console.WriteLine($"CurrentSpeed = {CurrentSpeed}");
            }

            //Almost dead
            if ((MaxSpeed-CurrentSpeed) <= 10)
            {
                AboutToBlow?.Invoke("Careful buddy! Gonna blow!");
            }
        }
    }

}
