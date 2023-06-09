using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDesigner
{
    public class Car : Vehile
    {
        public Car(string name, string producer) : base(name, producer)
        {
        }

        public int Price { get; set; }

        public string GetInfo()
        {
            return $"Car:{Producer} - {Name} Price:{Price}";
        }
    }
}