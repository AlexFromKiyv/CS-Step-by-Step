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
UseLock();


