using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    abstract class Shape
    {
        public string Name { get; set; }

        protected Shape(string name = "")
        {
            Name = name;
        }
        public abstract void Draw();
    }

    class Circle : Shape
    {
        public Circle() { }
        public Circle(string name) : base(name)
        {
        }
        public override void Draw()
        {
            Console.WriteLine($"Drawing {Name} the Circle");
        }
    }

    class Hexagon : Shape, IPointy
    {
        public Hexagon() { }
        public Hexagon(string name = "") : base(name)
        {
        }

        public int Points => 6;

        public override void Draw()
        {
            Console.WriteLine($"Drawing {Name} the Hexagon");
        }
    }

    class ThreeDCircle : Circle
    {

        public new void Draw()
        {
            Console.WriteLine($"Drawing {Name} 3D Circle");
        }
      
    }

    class Triangle : Shape, IPointy
    {
        public Triangle() { }
        public Triangle(string name = "") : base(name)
        {
        }

        public int Points => 3;

        public override void Draw()
        {
            Console.WriteLine($"Drawing {Name} Triangle");
        }
    }


    class Square: Shape, IRegularPointy
    {
        public Square() { }

        public Square(string name) : base(name)
        {
        }

        public override void Draw()
        {
            Console.WriteLine($"Drawing {Name} Square");
        }
        //This comes from the IPointy interface
        public int Points => 4;
        //These come from the IRegularPointy interface
        public int SideLength { get; set; }
        public int NumberOfSide { get; set; } = 4; 
    }


}
