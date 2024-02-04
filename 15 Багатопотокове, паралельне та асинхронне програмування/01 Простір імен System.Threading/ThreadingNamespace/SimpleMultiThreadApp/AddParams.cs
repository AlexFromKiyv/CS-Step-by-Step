using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMultiThreadApp
{
    internal class AddParams
    {
        public int a, b;
        public AddParams(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
        public void AddPrint() => Console.WriteLine($"Sum is {a+b}");
    }
}
