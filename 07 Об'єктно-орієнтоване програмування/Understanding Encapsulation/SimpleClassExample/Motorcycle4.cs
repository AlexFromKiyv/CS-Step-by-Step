using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassExample;

public class Motorcycle4
{
    public int driverIntensity;
    public string? driverName;

    // Constructor chaining.
    public Motorcycle4()
    {
        Console.WriteLine("In default constructor");
    }
    public Motorcycle4(int driverIntensity) : this(driverIntensity,null)
    {
        Console.WriteLine("In constructor taking an int");
    }
    public Motorcycle4(string driverName) : this(default,driverName) 
    {
        Console.WriteLine("In constructor taking a string");
    }
    // This is the 'main' constructor that does all the real work.
    public Motorcycle4(int driverIntensity, string? driverName)
    {
        Console.WriteLine("In main constructor");
        if (driverIntensity > 10)
        {
            driverIntensity = 10;
        }
        this.driverIntensity = driverIntensity;
        this.driverName = driverName;
    }
    public void Drive()
    {
        Console.WriteLine($"I am {driverName}");
        for (int i = 0; i < driverIntensity; i++)
        {
            Console.WriteLine($"Yeeeeeee Haaaaaeewww!");
        }        
    }
    public void SetName(string driverName)
    {
        this.driverName = driverName;
    }
}
