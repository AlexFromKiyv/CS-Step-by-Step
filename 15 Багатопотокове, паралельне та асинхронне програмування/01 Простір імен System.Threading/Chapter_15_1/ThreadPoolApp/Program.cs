
using ThreadPoolApp;

static void PrintTheNumbers(object state)
{
    Printer printer = (Printer)state;
    printer.PrintNumbers();
}

int id = Environment.CurrentManagedThreadId;
Console.WriteLine($"Main thread started. ThreadID = {id}");
Printer p = new Printer();

WaitCallback workItem = new(PrintTheNumbers);

// Queue the method 10 times
for (int i = 0; i < 10; i++)
{
    ThreadPool.QueueUserWorkItem(workItem, p);
}
Console.WriteLine("All tasks queued");

Console.ReadLine();


