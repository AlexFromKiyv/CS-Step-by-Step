using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Structs
{
    struct Point_v1
    {
        public int X { get; set; } = default;
        public int Y { get; set; } = default;

        public Point_v1(int x, int y)
        {
            X = x;
            Y = y;
        }
         public void ToConsole() => Console.WriteLine($"[{X},{Y}]"); 
    }
}
