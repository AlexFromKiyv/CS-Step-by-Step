using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    static class AnnoyingExtensions
    {
        public static void Print(this System.Collections.IEnumerable iterator)
        {
            foreach (var item in iterator) 
            {
                Console.WriteLine(item);
                Console.Beep();
            }
        }
    }
}
