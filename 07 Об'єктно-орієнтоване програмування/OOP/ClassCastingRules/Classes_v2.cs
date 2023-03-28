using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassCastingRules
{
    abstract class Shape
    {
        public string? Name { get; set; }
        protected Shape(string? name = "No name")
        {
            Name = name;
        }
        public abstract void Draw();
    }

    class Hexagon : Shape
    {
        public Hexagon(string? name = "No name") : base(name)
        {
        }
        public override void Draw()
        {
            Console.WriteLine($"\n Hexagone {Name}"); 
        }
    }
}
