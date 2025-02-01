using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes;

class Circle : Shape
{
    public Circle()
    {        
    }
    public Circle(string name) : base(name)
    {
    }

    // Circle Does not override Draw().
}

