using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarLibrary
{
    internal class InternalCar : Car
    {
        public override void TurboBoost()
        {
            Console.WriteLine("I'm internal Car");
        }
    }
}
