using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Partial
{
    partial class Person
    {
        public void ToConsole()
        {
            Console.WriteLine($"Id:{Id}\nName:{Name}\n");
        }
        public double BodyMassIndex() => Weight / (Height * Height);

        public void BodyMassIndexToConsol() => Console.WriteLine(BodyMassIndex());    

    }
}
