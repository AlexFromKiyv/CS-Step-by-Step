using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassExample;

public class Motorcycle5
{
    public int driverIntensity;
    public string? driverName;

    public Motorcycle5(int driverIntensity = 0, string? driverName = null)
    {
        if (driverIntensity > 10)
        {
            driverIntensity = 10;
        }
        this.driverIntensity = driverIntensity;
        this.driverName = driverName;
    }
    public void Drive()
    {
        Console.WriteLine($"\tI am {driverName}");
        for (int i = 0; i < driverIntensity; i++)
        {
            Console.WriteLine($"Yeeeeeee Haaaaaeewww!");
        }
        Console.WriteLine();
    }
}
