using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{

    interface IDrawable_v6
    {
        void Draw();
    }
    interface IPrintable_v6
    {
        void Print();
        void Draw();
    }
    interface IShape_v6 : IDrawable_v6, IPrintable_v6
    {
        int GetNumberOfSide();
    }

    class Rectangle_v6 : IShape_v6
    {
        public int GetNumberOfSide() => 4;
        public void Draw() => Console.WriteLine("Drawing ..."); 
        public void Print() => Console.WriteLine("Printing ...");
    }

    class Rectangle_v6_1 : IShape_v6
    {
        public int GetNumberOfSide() => 4;
        public void Print() => Console.WriteLine("Printing ...");

        void IDrawable_v6.Draw()
        {
            Console.WriteLine("Draw to screen");
        }
        void IPrintable_v6.Draw()
        {
            Console.WriteLine("Draw to print");
        }
    }


}
