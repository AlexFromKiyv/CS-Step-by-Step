using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInitializers;

class Rectangle
{
    private Point2 topLeft = new();
    private Point2 bottomRight = new();

    public Point2 TopLeft 
    {
        get => topLeft; 
        set => topLeft = value; 
    }
    public Point2 BottomRight 
    { 
        get => bottomRight; 
        set => bottomRight = value; 
    }

    public void DisplayState()
    {
        Console.WriteLine($"[[{topLeft.X},{topLeft.Y},{topLeft.Color}] " +
            $"[{bottomRight.X},{bottomRight.Y},{bottomRight.Color}]]");
    }
}
