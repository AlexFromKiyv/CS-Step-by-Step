using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics
{
    public class Car_v1 : IComparable<Car_v1>
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = "Undefined";
        public string Model { get; set; } = "Undefined";
        public int Year { get; set; }

        public Car_v1(int id, string manufacturer, string model, int year)
        {
            Id = id;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
        }

        public Car_v1()
        {
        }

        public void ToConsole() => Console.WriteLine($"{Manufacturer}\t{Model}\t{Year}");

        public int CompareTo(Car_v1? other)
        {
            if (other == null) 
            {
                return 1;
            }
            else
            {
                return Id.CompareTo(other.Id);
            }
        }
    }
}
