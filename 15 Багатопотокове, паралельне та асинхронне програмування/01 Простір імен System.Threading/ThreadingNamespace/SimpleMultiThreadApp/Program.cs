using SimpleMultiThreadApp;

void WorkingWithThreadStart()
{
    Console.Write("Do you want 1 or 2 threads? [1/2]:");

    string? threadCount = Console.ReadLine();

    //Assigning the name of current thread.
    Thread primaryThread = Thread.CurrentThread;
    primaryThread.Name = "Primary";

    Console.WriteLine($"{Thread.CurrentThread.Name} is execution method in Top-level");

    Printer printer = new();

    switch (threadCount)
    {
        case "2":
            Thread backgroungThread = new Thread(new ThreadStart(printer.PrintNumbers));
            backgroungThread.Name = "Secondary";
            backgroungThread.Start();
            break;
        case "1":
        default:
            printer.PrintNumbers();
            break;
    }
    //Do some addition work.
    Console.WriteLine("For finishe prass End");
    ConsoleKey consoleKey = ConsoleKey.Home;
    while (consoleKey != ConsoleKey.End)
    {
        consoleKey = Console.ReadKey().Key;
        Console.Beep();
    }
    
    Console.WriteLine("\nThis is on the main thread, and we are finished.");
}
//WorkingWithThreadStart();


void Add(object? data)
{

    if (data is AddParams ap)
    {
        Console.WriteLine($"ID of thread in Add() method : {Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(4000);
        Console.WriteLine($"{ap.a} + {ap.b} is {ap.a + ap.b}");
    }
}


void WorkingWithParameterizedThreadStart()
{
    Console.WriteLine($"ID of main thread : {Thread.CurrentThread.ManagedThreadId}");

    AddParams addParams = new(1, 2);
    Thread thread = new(new ParameterizedThreadStart(Add));
    thread.Start(addParams);

    Thread.Sleep(2000);
    Console.WriteLine("The main thread is finished.");
}
WorkingWithParameterizedThreadStart();
