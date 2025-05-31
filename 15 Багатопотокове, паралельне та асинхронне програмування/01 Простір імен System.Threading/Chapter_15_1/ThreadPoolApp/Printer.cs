using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPoolApp;

public class Printer
{
    // Lock token.
    private object threadLock = new();
    public void PrintNumbers()
    {
        lock (threadLock)
        {
            // Display Thread info.
            Console.WriteLine($"Thread id:{Thread.CurrentThread.ManagedThreadId} is executing PrintNumbers()");

            // Print out numbers.
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{i}, ");
                Thread.Sleep(50);
            }
            Console.WriteLine();
        }
    }

}
