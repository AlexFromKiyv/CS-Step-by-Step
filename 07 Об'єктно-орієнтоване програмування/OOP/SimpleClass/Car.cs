using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClass
{
    internal class Car
    {
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }

        public Car(string? manufacturer = "", string? model="")
        {
            Manufacturer = manufacturer;
            Model = model;
        }

        public void Deconstruct(out string?  manufacturer, out string? model)
        {
            manufacturer = Manufacturer;
            model = Model;            
        }
    }
}
