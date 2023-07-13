using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenericMethods
{
    internal static class SwapFunctions
    {
        static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        static void Swap(ref Person personA,ref Person personB)
        { 
            Person tempPerson = personA;
            personA = personB;
            personB = tempPerson;
        }

        internal static void Swap<T>(ref T a,  ref T b)
        {
            Console.WriteLine($"\nI am swaping two {typeof(T)}\n");
            T temp = a;
            a = b;
            b = temp;
        } 

        internal static void BaseClass<T>()
        {
            Console.WriteLine(typeof(T));
        }
    }

}
