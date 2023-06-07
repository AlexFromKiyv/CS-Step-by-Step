using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClass
{
    class Bus25
    {
        public int numberOfSeats;
        public string? driverName;

        public Bus25(int numberOfSeats, string? driverName)
        {
            Console.WriteLine("in main constructor");
            if (numberOfSeats > 24)
            {
                numberOfSeats = 24;
            }
            this.numberOfSeats = numberOfSeats;
            this.driverName = driverName;
        }

        public Bus25()
        {
            Console.WriteLine("in default constructor");
        }

        public Bus25(int numberOfSeats) : this(numberOfSeats, null)
        {
            Console.WriteLine("in constructor for numberOfSeats");
        }

        public Bus25(string? driverName) : this(default, driverName)
        {
            Console.WriteLine("in constructor for driverName");
        }

        public void StateToConsol() => Console.WriteLine($"driverName: {driverName}  numberOfSeats: {numberOfSeats}");
    }
}
