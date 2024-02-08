using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNamespaces.MyShape;
public class Circle
{
    public Circle()
    {
    }

    public Circle(int radius)
    {
        Radius = radius;
    }

    public int Radius { get; set; }
    // Here are its members.

    public void InfoAboutShape()
    {
        Console.WriteLine($"The shape is circle with radius: {Radius}");
    }
}
