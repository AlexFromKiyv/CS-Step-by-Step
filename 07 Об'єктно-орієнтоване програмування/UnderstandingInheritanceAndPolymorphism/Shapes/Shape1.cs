using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes;
abstract class Shape1
{
    public string PetName { get; set; } 

    protected Shape1(string petName = "NoName")
    {
        PetName = petName;
    }
    // Force all child classes to define how to be rendered.
    public abstract void Draw();
}

