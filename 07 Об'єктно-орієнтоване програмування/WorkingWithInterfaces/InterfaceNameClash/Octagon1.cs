using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceNameClash;

class Octagon1 : IDrawToForm, IDrawToMemory, IDrawToPrinter
{
    void IDrawToForm.Draw()
    {
        Console.WriteLine("Drawing to form...");
    }

    void IDrawToMemory.Draw()
    {
        Console.WriteLine("Drawing to memory...");
    }

    void IDrawToPrinter.Draw()
    {
        Console.WriteLine("Drawing to a printer...");
    }
}
