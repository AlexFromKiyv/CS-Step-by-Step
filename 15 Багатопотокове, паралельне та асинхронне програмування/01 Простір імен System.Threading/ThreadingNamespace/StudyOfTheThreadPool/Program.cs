using StudyOfTheThreadPool;

static void UseQueueUserWorkItem()
{
    Thread primary = Thread.CurrentThread;
    primary.Name = "Primary";

    Console.WriteLine($"Main thread started.\n" +
        $" Thread.CurrentThread.ManagedThreadId:{primary.ManagedThreadId}\n" +
        $" Environment.CurrentManagedThreadId:{Environment.CurrentManagedThreadId}");

    Printer printer = new();
    printer.PrintNumbersWithLock();

    WaitCallback workItem = new WaitCallback(PrintTheNumbers);

    int length = 5;
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