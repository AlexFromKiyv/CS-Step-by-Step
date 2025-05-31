
using ThreadPoolApp;

static void PrintTheNumbers(object? obj)
{
    if (obj == null) return;
    Printer printer = (Printer)obj;
    printer.PrintNumbers();
}

Console.WriteLine($"Main thread id:{Environment.CurrentManagedThreadId} started.");
Printer p = new Printer();

WaitCallback workItem = new(PrintTheNumbers);

// Queue the method 10 times
for (int i = 0; i < 10; i++)
{
    ThreadPool.QueueUserWorkItem(workItem, p);
}
Console.WriteLine("All tasks queued");

Console.ReadLine();



