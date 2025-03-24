using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarEvents;

public class Car1
{
    public int CurrentSpeed { get; set; }
    public int MaxSpeed { get; set; }
    public string PetName { get; set; } = string.Empty;

    private bool _carIsDead;

    public Car1() { MaxSpeed = 100; }
    public Car1(string name, int maxSp, int currSp)
    {
        CurrentSpeed = currSp;
        MaxSpeed = maxSp;
        PetName = name;
    }

    public delegate void CarEngineHandler(object sender, CarEventArgs eventArgs );

    public event CarEngineHandler? Exploded;
    public event CarEngineHandler? AboutToBlow;

    public void Accelerate(int delta)
    {
        // If the car is dead, fire Exploded event.
        if (_carIsDead)
        {
            Exploded?.Invoke(this,new CarEventArgs("Sorry, this car is dead..."));
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
                AboutToBlow?.Invoke(this,new CarEventArgs("Careful buddy! Gonna blow!"));
            }
        }
    }

}
