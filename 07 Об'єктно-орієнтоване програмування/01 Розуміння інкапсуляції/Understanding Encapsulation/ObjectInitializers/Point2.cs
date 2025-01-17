using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInitializers;

class Point2
{
    public int X { get; set; }
    public int Y { get; set; }
    public PointColorEnum Color { get; set; }

    public Point2(int x, int y)
    {
        X = x;
        Y = y;
    }
    public Point2(PointColorEnum color)
    {
        Color = color;
    }
    public Point2() : this(PointColorEnum.BloodRed)
    {
    }
    public void DisplayState()
    {
        Console.WriteLine($"[{X},{Y},{Color}]");
    }
}
