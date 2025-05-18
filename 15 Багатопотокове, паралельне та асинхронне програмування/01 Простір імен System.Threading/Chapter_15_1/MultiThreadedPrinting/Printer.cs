using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreadedPrinting;

public class Printer
{
    // Lock token.
    private object threadLock = new();
    public void PrintNumbers()
    {
        lock (threadLock)
        {
            // Display Thread info.
            Console.WriteLine($"\t{Thread.CurrentThread.Name} is executing PrintNumbers()");

            // Print out numbers.
            for (int i = 0; i < 10; i++)
            {
                // Put thread to sleep for a random amount of time.
                Random r = new Random();
                Thread.Sleep(1000 * r.Next(5));
                Console.Write($"{i}, ");
            }
            Console.WriteLine();
        }
    }
}
