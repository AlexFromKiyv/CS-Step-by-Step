using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassExample;

public class Motorcycle3
{
    public int driverIntensity;
    public string driverName = null!;
    public Motorcycle3()
    {
    }
    public Motorcycle3(int driverIntensity) : this(driverIntensity,"")
    {
    }
    public Motorcycle3(string driverName) :this(0,driverName) 
    {
    }
    public Motorcycle3(int driverIntensity, string driverName)
    {
        if (driverIntensity > 10)
        {
            driverIntensity = 10;
        }
        this.driverIntensity = driverIntensity;
        this.driverName = driverName;
    }
}
