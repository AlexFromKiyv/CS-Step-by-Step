using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalizableОbjects;

class MyResourceWrapper
{
    // Work with unmanaged resources 
    // 
    ~MyResourceWrapper() => Console.Beep();
}




