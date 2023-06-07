using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarApp
{
    internal class Garage_v2
    {
        public int NumberOfCars { get; set; }
        public Car_v1 MyCar { get; set; }

        public Garage_v2()
        {
            NumberOfCars = default;
            MyCar = new Car_v1();
        }
    }
}
