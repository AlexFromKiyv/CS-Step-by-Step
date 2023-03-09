using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarApp
{
    internal class Car_v1
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int SerialNumber { get; }

        public Car_v1(string manufacturer, string model, int year)
        {
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
        }

        public Car_v1() : this("Not known", "Not known", default)
        {
        }

        public void ToConsole()
        {
            Console.WriteLine($"Manufacturer: {Manufacturer}");
            Console.WriteLine($"Modle: {Model}");
            Console.WriteLine($"Year: {Year}");
            Console.WriteLine("\n");
        }
    }
}
