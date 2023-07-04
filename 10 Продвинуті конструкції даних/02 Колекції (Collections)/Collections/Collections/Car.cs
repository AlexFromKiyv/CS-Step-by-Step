using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Collections
{
    public class Car
    {
        public string Manufacturer { get; set; } = "Undefined";
        public string Model { get; set; } = "Undefined";
        public int Year { get; set; }

        public Car(string manufacturer, string model, int year)
        {
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
        }

        public Car()
        {
        }

        public void ToConsole() => Console.WriteLine($"{Manufacturer}\t{Model}\t{Year}");
    }

    public class CarCollection : IEnumerable
    {
        private ArrayList arrayCars = new();
        public void Add( Car car ) => arrayCars.Add(car);
        public Car? Get(int index) => (Car?)arrayCars[index];
        public int Count => arrayCars.Count;
        public void Clear() => arrayCars.Clear();
        public IEnumerator GetEnumerator() => arrayCars.GetEnumerator();
    }
}
