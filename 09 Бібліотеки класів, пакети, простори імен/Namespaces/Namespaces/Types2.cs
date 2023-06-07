using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle
{

    namespace Cars
    {
        class Car
        {
            public string Manufacturer { get; set; } = "Undefined";
            public string Model { get; set; } = "Undefinred";

            public void ToConsole() => Console.WriteLine($"{Manufacturer}\t{Model}");
        }
    }
}



