using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClass
{
    class Bus
    {
        public int numberOfSeats;
        public string? driverName;

        public Bus(int numberOfSeats = 20 , string? driverName = "Someone")
        {
            if (numberOfSeats > 30)
            {
                numberOfSeats = 30;
            }

            this.numberOfSeats = numberOfSeats;
            this.driverName = driverName;
        }
        public void ToConsol() => Console.WriteLine($"driverName: {driverName}  numberOfSeats: {numberOfSeats}");
    }
}
