using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicInheritance;

sealed class MiniVan1 : Car1
{

    public void LowSpeed()
    {
        // Error! Cannot access private
        // members of parent within a derived type.
        //_speed = 10;

        // OK! Can access public members
        // of a parent within a derived type.
        Speed = 10;
    }
}

//// Error! Cannot extend
//// a class marked with the sealed keyword!
//class DeluxeMiniVan
//  : MiniVan1
//{
//}