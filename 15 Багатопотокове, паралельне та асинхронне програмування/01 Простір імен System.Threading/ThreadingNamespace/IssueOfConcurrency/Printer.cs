using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueOfConcurrency;
public class Printer
{
    public void PrintNumbers()
    {
        // Display Thread info.
        Console.WriteLine($"{Thread.CurrentThread.Name} is executing PrintNumbers()");

        //Print out numbers.
        for (int i = 0; i < 10; i++)
        {
            Random random = new();
            Thread.Sleep(100 * random.Next(5));
            Console.Write($"{i} ");
        }
        Console.WriteLine();
    }
}
