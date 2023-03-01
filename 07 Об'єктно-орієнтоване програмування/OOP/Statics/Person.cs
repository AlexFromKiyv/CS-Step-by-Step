using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statics
{
    class Person
    {
        public static double maxWeight = 635;
        public static double avarageWeight = 81.5;

        public string name;
        public double? height;
        public double? weight;

        public Person(string name, double? height, double? weight)
        {
            this.name = name;
            this.height = height;
            this.weight = weight;
        }

        public Person(string name)
        {
            this.name = name;
        }

        public void ToConsole() => Console.WriteLine($"Name:{name} Height:{height} Weight:{weight}");

        // Static method
        public static double GetAvarageWeight() => avarageWeight;
        public static void SetAvarageWeight(double newAvarageWeight) => avarageWeight = newAvarageWeight;
    }
}
