using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Initializers
{
    internal class Point_v2
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PointColorEnum Color { get; set; }
        public Point_v2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Point_v2(PointColorEnum color)
        {
            Color = color;
        }

        public Point_v2():this(PointColorEnum.Green)
        {
        }
        public void ToConsole() => Console.WriteLine($"[{X},{Y}] - {Color} \n");
    }
}
