using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Records
{
    public record struct Point_v2
    {
        public int X { get; set; } = default;
        public int Y { get; set; } = default;

        public Point_v2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void ToConsole() => Console.WriteLine($"[{X},{Y}]");
    }
}
