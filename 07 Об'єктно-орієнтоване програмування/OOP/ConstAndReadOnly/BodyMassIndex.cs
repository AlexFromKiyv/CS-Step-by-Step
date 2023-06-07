using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstAndReadOnly
{
    internal class BodyMassIndex
    {
        public const double LESS_THEN_NORM = 18.5;
        public const double OVER_THEN_NORM = 25; 

        public void Greeting()
        {
            const string hello = "Hello\n";
            const string greeting = $"{hello}This is Body mass index calculator \n\n";
            Console.WriteLine(greeting);
        }

    }
}
