using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PolymorphicInterface
{
    abstract class Shape_v1
    {
        public string Name { get; set; }
        protected Shape_v1(string name = "No name")
        {
            Name = name;
        }
        public virtual void Draw()
        {
            Console.WriteLine("Work method Shape.Draw()");
        }
        public virtual void  ToConsole() => Console.WriteLine($"\n {Name}");
    }

    class Circle_v1 : Shape_v1
    {
        public Circle_v1(string name = "No name") : base(name)
        {
        }
    }

    class Hexagon_v1 : Shape_v1
    {
        public Hexagon_v1(string name = "No name") : base(name)
        {
        }
        public override void Draw() => Console.WriteLine($"Hexogen - {Name}");
        
    }
}
