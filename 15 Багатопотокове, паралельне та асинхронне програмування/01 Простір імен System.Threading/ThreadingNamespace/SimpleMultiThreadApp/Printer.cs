using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMultiThreadApp
{
    public class Printer
    {
        public void PrintNumbers()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} is executing PrintNumbers()");

            Console.WriteLine("Starting slow work: : ");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"\nIt's done step {i}");
                Thread.Sleep(2000);
            }
            Console.WriteLine();
        }
    }
}
