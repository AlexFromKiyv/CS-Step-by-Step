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
            if (numberOfSeats > 24)
            {
                numberOfSeats = 24;
            }
            this.numberOfSeats = numberOfSeats;
            this.driverName = driverName;
        }

        public Bus25(int numberOfSeats) : this(numberOfSeats, null)
        {
        }

        public Bus25(string? driverName) : this(default, driverName)
        {
        }
    }
}
