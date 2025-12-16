using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes;

class ThreeDCircle1 : Circle1
{
    // Hide the PetName property above me.
    public new string PetName { get; set; }

    //public void Draw()
    //{
    //    Console.WriteLine("Drawing a 3D Circle");
    //}

    // Hide any Draw() implementation above me.
    public new void Draw()
    {
        Console.WriteLine("Drawing a 3D Circle");
    }
}
