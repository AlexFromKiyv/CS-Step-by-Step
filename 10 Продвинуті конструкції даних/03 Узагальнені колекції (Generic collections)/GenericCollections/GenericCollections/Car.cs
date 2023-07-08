using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics
{
    public class Car : IComparable
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = "Undefined";
        public string Model { get; set; } = "Undefined";
        public int Year { get; set; }

        public Car(int id, string manufacturer, string model, int year)
        {
            Id = id;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
        }

        public Car()
        {
        }

        public void ToConsole() => Console.WriteLine($"{Id}\t{Manufacturer}\t{Model}\t{Year}");

        public int CompareTo(object? obj)
        {
            if (obj == null) return 1; 

            if (obj is Car tempCar)
            {
                return Id.CompareTo(tempCar.Id);
            }
            throw new ArgumentException("Parameter is not a car.");
        }
    }
}
