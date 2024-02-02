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
    Console.WriteLine($"{Thread.CurrentThread.Name} is almost complete. Press Enter."  );
    Console.ReadLine();
}
//WorkingWithThreadStart();


void Add(object? data)
{
    if (data is AddParams ap)
    {
        Console.WriteLine($"Start work in method Add() into thread with ID : {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine($"{ap.a} + {ap.b} is {ap.a + ap.b}");
    }
    Console.WriteLine("The method Add is finished.");
}

void WorkingWithParameterizedThreadStart()
{
    Console.WriteLine($"Start work in main thread with ID : {Thread.CurrentThread.ManagedThreadId}");

    AddParams addParams = new(1, 2);
    Thread thread = new(new ParameterizedThreadStart(Add));
    thread.Start(addParams);
    //Thread.Sleep(5);
    Console.WriteLine("The main thread is finished.");
}
//WorkingWithParameterizedThreadStart();

AutoResetEvent _waitHandler = new AutoResetEvent(false);
void AddWithSet(object? data)
{
    if (data is AddParams ap)
    {
        Console.WriteLine($"Start work in method Add() into thread with ID : {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine($"{ap.a} + {ap.b} is {ap.a + ap.b}");
    }
    Console.WriteLine("The method Add is finished.");
    _waitHandler.Set();
}

void WorkingWithClassAutoResetEvent()
{
    

    Console.Write("Wait for finish second thread (Y/N):");
    string? toWait = Console.ReadLine();


    Console.WriteLine($"Start work method from main thread with ID : {Thread.CurrentThread.ManagedThreadId}");
    AddParams addParams = new(1, 2);
    Thread thread = new(new ParameterizedThreadStart(AddWithSet));
    thread.Start(addParams);

    if(toWait != null && (toWait == "Y" || toWait == "y"))
    {
        _waitHandler.WaitOne();
    }
    Console.WriteLine("The main thread is finished.");
}
//WorkingWithClassAutoResetEvent();

void UseIsBackground()
{
    Console.Write("Do you want make worker thread backgrounded? (Y/N):");
    string? isBackgrounded = Console.ReadLine();

    Console.WriteLine($"Start the method from primary thread with ID : {Thread.CurrentThread.ManagedThreadId}");

    Printer printer = new();
    
    Thread workThread = new Thread(new ThreadStart(printer.PrintNumbers));
    workThread.Name = "Worker thread";
    workThread.IsBackground = (isBackgrounded == "Y" || isBackgrounded == "y");
    workThread.Start();

    Console.ReadLine();

    Console.WriteLine("The primary thread is finished.");
}

//UseIsBackground();