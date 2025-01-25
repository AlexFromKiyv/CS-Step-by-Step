using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstData;

class MyMathClass
{
    //public const double PI = 3.14;

    public static readonly double PI;

    static MyMathClass()
    {
        PI = 3.14;
    }

    // Error!
    //public void ChangePI()
    //{ PI = 3.14444; }
}
