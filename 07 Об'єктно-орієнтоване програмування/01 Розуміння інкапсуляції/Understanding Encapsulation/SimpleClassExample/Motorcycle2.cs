using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassExample;

public class Motorcycle2
{
    public int driverIntensity;
    public string driverName;

    // Constructors.
    public Motorcycle2() { }
    public Motorcycle2(int intensity)
    {
        SetIntensity(intensity);
    }
    public Motorcycle2(int intensity, string name)
    {
        SetIntensity(intensity);
        driverName = name;
    }
    public void SetIntensity(int intensity)
    {
        if (intensity > 10)
        {
            intensity = 10;
        }
        driverIntensity = intensity;
    }
}
