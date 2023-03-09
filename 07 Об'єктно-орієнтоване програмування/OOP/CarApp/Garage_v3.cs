using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarApp
{
    internal class Garage_v3
    {
        public int NumberOfCars { get; set; } = 1;
        public Car_v1 MyCar { get; set; } = new Car_v1();

        public Garage_v3()
        {
        }

        public Garage_v3(int numberOfCars, Car_v1 myCar)
        {
            NumberOfCars = numberOfCars;
            MyCar = myCar;
        }
    }
}
