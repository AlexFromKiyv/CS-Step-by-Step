using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoProps;

class Garage1
{
    private float temperature = 18;
    public float Temperature { get => temperature; set => temperature = value; }
    public Car MyCar { get; set; } = new Car();

    public Garage1() {}
    public Garage1(float temperature, Car myCar)
    {
        Temperature = temperature;
        MyCar = myCar;
    }
}
