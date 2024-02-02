using IssueOfConcurrency;

void OneThread()
{
    Thread.CurrentThread.Name = "Primary";
    Printer printer = new();
    printer.PrintNumbers();
}
//OneThread();

void WorkManyThreads()
{
    int length = 10;

    Printer printer = new();

    //Make many threads that are all pointing to
    //the same method on the same object
    Thread[] threads = new Thread[length];
    for (int i = 0; i < length; i++)
    {
        threads[i] = new Thread(new ThreadStart(printer.PrintNumbers)) 
        { Name = $"Work thread {i}" };
    }

    foreach (Thread thread in threads)
    {
        thread.Start();
    }
}
//WorkManyThreads();

void UseLock()
{
    int length = 3;

    Printer printer = new();

    Thread[] threads = new Thread[length];
    for (int i = 0; i < length; i++)
    {
        threads[i] = new Thread(new ThreadStart(printer.PrintNumbersWithLock))
        { Name = $"Work thread {i}" };
    }

    foreach (Thread thread in threads)
    {
        thread.Start();
    }
}
//UseLock();

void UseMonitor()
{
    int length = 3;

    Printer printer = new();

    Thread[] threads = new Thread[length];
    for (int i = 0; i < length; i++)
    {
        threads[i] = new Thread(new ThreadStart(printer.PrintNumbersWithMonitor))
        { Name = $"Work thread {i}" };
    }

    foreach (Thread thread in threads)
    {
        thread.Start();
    }
}
//UseMonitor();


void AssigningWithLock()
{
    int intValue = 5;
    object lockTocken = new();
    lock(lockTocken)
    {
        intValue++;
    }
    Console.WriteLine(intValue);
}
//AssigningWithLock();

void UseInterlockedIncrement()
{
    int intValue = 5;
    intValue = Interlocked.Increment(ref intValue);
    Console.WriteLine(intValue);
}
//UseInterlockedIncrement();

void UseInterlockedExchange()
{
    int intValue = 5;
    Interlocked.Exchange(ref intValue,10);
    Console.WriteLine(intValue);
}
//UseInterlockedExchange();

void UseInterlockedCompareExchange()
{
    int intValue = 5;
    Interlocked.CompareExchange(ref intValue,15,5);
    Console.WriteLine(intValue);

    Interlocked.CompareExchange(ref intValue, 5, 10);
    Console.WriteLine(intValue);
}
//UseInterlockedCompareExchange();