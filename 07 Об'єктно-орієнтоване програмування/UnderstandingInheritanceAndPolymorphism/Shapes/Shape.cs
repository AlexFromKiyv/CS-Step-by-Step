using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes;
// The abstract base class of the hierarchy.
abstract class Shape
{
    public string PetName { get; set; } 

    protected Shape(string petName = "NoName")
    {
        PetName = petName;
    }
    // A single virtual method.
    public virtual void Draw()
    {
        Console.WriteLine("Inside Shape.Draw()");
    }
}

