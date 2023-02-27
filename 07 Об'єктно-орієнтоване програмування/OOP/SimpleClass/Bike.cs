using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SimpleClass
{
    class Bike
    {
        public string ownerName;
        public int currentSpeed;

        public void StateToConsol() => Console.WriteLine($"{ownerName} is driving at speed {currentSpeed}");
        public void SpeedUp(int speedChange) => currentSpeed += speedChange;
    }
}
