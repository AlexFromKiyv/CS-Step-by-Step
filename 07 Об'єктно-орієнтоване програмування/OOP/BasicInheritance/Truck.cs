using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicInheritance
{
    sealed class Truck :Car
    {
        public int CarryingCapacity { get; set; }
    }

    //class SuperTruck : Truck { } // cannot derive from sealed type

    //class MyString : String // cannot derive from sealed type
}
