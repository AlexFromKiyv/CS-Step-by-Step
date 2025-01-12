using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassExample;

public class Motorcycle
{
    public int speed;

    public string driverName;

    public void Drive()
    {
        Console.WriteLine($"I diving {speed}");
    }
    public Motorcycle()
    {
    }
    public Motorcycle(int speed)
    {
        this.speed = speed;
    }

    public void SetName(string name)
    {
        //name = name;
        //this.name = name;
        driverName = name;
    }

}
