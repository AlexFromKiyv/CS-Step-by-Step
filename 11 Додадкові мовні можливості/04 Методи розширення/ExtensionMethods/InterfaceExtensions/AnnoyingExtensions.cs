using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceExtensions;

static class AnnoyingExtensions
{
    public static void PrintDataAndBeep(this System.Collections.IEnumerable collection)
    {
        foreach (var item in collection)
        {
            Console.WriteLine(item);
            Console.Beep();
        }
    }
}
