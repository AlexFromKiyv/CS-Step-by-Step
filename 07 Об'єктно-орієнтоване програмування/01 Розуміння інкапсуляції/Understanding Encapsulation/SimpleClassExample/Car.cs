using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassExample;

class Car
{
    // The 'state' of the Car.
    public string petName;
    public int currSpeed;
  
    // A custom default constructor.
    public Car()
    {
        petName = "No name";
    }

    public Car(string name)
    {
        petName = name;
    }

    public Car(string name, int speed)
    {
        petName = name;
        currSpeed = speed;
    }

    public Car(string name, int speed, out bool inDanger)
    {
        petName = name;
        currSpeed = speed;
        inDanger = (speed > 100) ? true : false;
    }



    // The functionality of the Car.
    public void PrintState()
    {
        Console.WriteLine($"{petName} is going {currSpeed} MPH.");
    }
    public void SpeedUp(int delta)
    {
        currSpeed += delta;
    }


}
