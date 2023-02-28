using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClass
{
    class BadBus25
    {
        public int numberOfSeats;
        public string? driverName;

        public BadBus25()
        {
        }

        public BadBus25(int numberOfSeats)
        {
            if (numberOfSeats > 24)
            {
                numberOfSeats = 24;
            }
            this.numberOfSeats = numberOfSeats;
        }

        public BadBus25(int numberOfSeats, string driverName)
        {
            if (numberOfSeats > 24)
            {
                numberOfSeats = 24;
            }
            this.numberOfSeats = numberOfSeats;

            this.driverName = driverName;
        }
    }
}
