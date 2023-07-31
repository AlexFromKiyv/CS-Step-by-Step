using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static void DisplayDefiningAssembly(this object obj)
        {
            Console.WriteLine($"\n" +
                $"{obj.GetType()} " +
                $"lives here: " +
                $"{Assembly.GetAssembly(obj.GetType())?.GetName().Name}");
        } 

        public static int ReverseDigits(this int i)
        {
            char[] chars = i.ToString().ToCharArray();

            Array.Reverse(chars);

            string newStringGigit = new string(chars);

            return int.Parse(newStringGigit);
        }

        public static string ReverseChars(this string s)
        {
            char[] chars = s.ToCharArray();

            Array.Reverse(chars);

            string newString = new string(chars);

            return newString;
        }
    }
}
