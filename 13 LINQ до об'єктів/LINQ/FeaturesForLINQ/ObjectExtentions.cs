using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FeaturesForLINQ
{
    internal static class ObjectExtentions
    {
        public static void DisplayDefiningAssembly(this object  obj)
        {
            Console.WriteLine($"{obj.GetType().Name} live here: {Assembly.GetAssembly(obj.GetType())}");
        }
    }
}
