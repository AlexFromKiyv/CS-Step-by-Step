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

    class Hexagon : Shape, IPointy, IDraw3D
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

        public void Draw3D() 
        {
            Console.WriteLine("Drawing Hexagon in 3D!");
        }
    }

    class ThreeDCircle : Circle, IDraw3D
    {

        public new void Draw()
        {
            Console.WriteLine($"Drawing {Name} 3D Circle");
        }

        public void Draw3D()
        {
            Console.WriteLine("Drawing Circle in 3D!");
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


    class Fork : IPointy
    {
        public int Points => 5;
    }

    class Knife : IPointy
    {
        public int Points => 2;
    }

    class PichFork : IPointy
    {
        public int Points => 6;
    }

    class Octagon_v1 : IDrawToForm, IDrawToMemory, IDrawToPrinter
    {
        public void Draw()
        {
            Console.WriteLine("Drawing the Octagon...");
        }
    }

    class Octagon_v2 : IDrawToForm, IDrawToMemory, IDrawToPrinter
    {
        void IDrawToForm.Draw()
        {
            Console.WriteLine("Drawing the Octagon to Form.");
        }

        void IDrawToMemory.Draw()
        {
            Console.WriteLine("Drawing the Octagon to Memory.");
        }

        void IDrawToPrinter.Draw()
        {
            Console.WriteLine("Drawing the Octagon to Printer.");
        }
    }

}
