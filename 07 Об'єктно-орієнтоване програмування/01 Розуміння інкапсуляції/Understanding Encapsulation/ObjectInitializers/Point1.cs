using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInitializers;

class Point1
{
    public int X { get; init; }
    public int Y { get; init; }

    public Point1(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point1()
    {
    }

    public void DisplayState()
    {
        Console.WriteLine($"[{X},{Y}]");
    }
}

