using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMultiThreadApp;

public class Printer
{
    public int Counter { get; set; }
    public void PrintNumbers()
    {
        // Display Thread info.
        Console.WriteLine($"{Thread.CurrentThread.Name} is executing PrintNumbers()");
        
        // Print out numbers.
        Console.Write("Your numbers: ");
        for (int i = 0; i < 10; i++)
        {
            Console.Write($"{Counter} ");
            Thread.Sleep(2000);
            Counter++;
        }
        Console.WriteLine();
    }
}
