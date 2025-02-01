using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes;

class Circle1 : Shape1
{
    public Circle1() {}
    public Circle1(string name) : base(name) {}
    public override void Draw()
    {
        Console.WriteLine($"Drawing {PetName} the Circle"); 
    }
}

