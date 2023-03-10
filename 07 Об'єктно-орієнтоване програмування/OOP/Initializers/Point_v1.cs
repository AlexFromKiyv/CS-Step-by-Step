using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Initializers
{
    internal class Point_v1
    {
        public int X { get; init; }
        public int Y { get; init; }

        public Point_v1()
        {
        }

        public Point_v1(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void ToConsole() => Console.WriteLine($"[{X},{Y}]\n");
        
    }
}
