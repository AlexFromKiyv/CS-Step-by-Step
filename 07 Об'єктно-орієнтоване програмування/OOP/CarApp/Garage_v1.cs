using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarApp
{
    internal class Garage_v1
    {
        public int NumberOfCars { get; set; }
        public Car_v1 MyCar { get; set; } // Non-nullable prop must contain a non-null when exiting constructor. 
    }
}
