using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoProps;

class Garage
{
    // The hidden backing field is set to zero!
    public float Temperature { get; set; }
    // The hidden Car backing field is set to null!
    public Car MyCar { get; set; }

    public Garage()
    {
        MyCar = new Car();
    }
    public Garage(float temperature, Car myCar)
    {
        Temperature = temperature;
        MyCar = myCar;
    }
}
