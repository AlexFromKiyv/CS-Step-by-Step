using StudyOfTheThreadPool;

static void UseQueueUserWorkItem()
{
    Thread primary = Thread.CurrentThread;
    primary.Name = "Primary";
    Console.WriteLine($"Main thread started. ThreadId:{primary.ManagedThreadId}");

    Printer printer = new();
    printer.PrintNumbersWithLock();
    Console.WriteLine();

    WaitCallback workItem = new WaitCallback(PrintTheNumbers);

    int length = 10;
    for (int i = 0; i < length; i++)
    {
        ThreadPool.QueueUserWorkItem(workItem,printer);
    }
    Console.WriteLine("All tasks queued");
    Console.ReadLine();

    static void PrintTheNumbers(object? state)
    {
        if (state is Printer task)
        {
            task.PrintNumbersWithLock();
        }
    }
}
UseQueueUserWorkItem();