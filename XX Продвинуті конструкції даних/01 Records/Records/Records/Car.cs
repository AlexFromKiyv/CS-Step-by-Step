using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Records
{
    internal class Car
    {
        public string Manufacturer { get; init; }
        public string Model { get; init; }
        public string Color { get; init; }

        public Car(string manufacturer, string model, string color)
        {
            Manufacturer = manufacturer;
            Model = model;
            Color = color;
        }

        public Car() : this("Not known.", "Not known.", "Not known.")
        {
        }
    }
}
