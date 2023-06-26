using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Records
{
    public record struct Point_v4
    {
        public int X { get; init; }
        public int Y { get; init; }

        public Point_v4(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void ToConsole() => Console.WriteLine($"[{X},{Y}]");
    }
}
