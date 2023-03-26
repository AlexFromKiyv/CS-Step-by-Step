using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicInterface
{
    abstract class Shape_v2
    {
        public string Name { get; set; }
        protected Shape_v2(string name = "No name")
        {
            Name = name;
        }
        public abstract void Draw();
    }

    class Circle_v2 : Shape_v2
    {
        public Circle_v2(string name = "No name") : base(name)
        {
        }

        public override void Draw() => Console.WriteLine($"Circle({Name})\n");
    }

    class Hexagon_v2 : Shape_v2
    {
        public Hexagon_v2(string name = "No name") : base(name)
        {
        }

        public override void Draw()
        {
            Console.WriteLine($"This is Hexogen -> {Name}\n");
        }
    }
}
