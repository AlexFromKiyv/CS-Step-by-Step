using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Initializers
{
    class Manufacturer
    {
        public string Name { get; set; } = string.Empty;
    }
    class Car
    {
        public Manufacturer Manufacturer { get; set; }
        public string Model { get; set; }

        public Car()
        {
            Manufacturer = new Manufacturer();
            Model = string.Empty;
        }

        public Car(Manufacturer manufacturer, string model)
        {
            Manufacturer = manufacturer;
            Model = model;
        }

        public void ToConsole() => Console.WriteLine($"{Manufacturer.Name} - {Model}");

    }
}
