﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudyOfTheThreadPool;
public class Printer
{

    //Lock token
    private object threadLock = new object();
    public void PrintNumbersWithLock()
    {
        lock (threadLock) 
        { 
            // Display Thread info.
            Console.WriteLine($"{Thread.CurrentThread.Name} is executing PrintNumbers()");

            //Print out numbers.
            for (int i = 0; i < 10; i++)
            {
                Random random = new();
                Thread.Sleep(50 * random.Next(5));
                Console.Write($"{i} ");
            }
            Console.WriteLine();
        }
    }
}
