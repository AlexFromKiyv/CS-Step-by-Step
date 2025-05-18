
using AddWithThreads;

AutoResetEvent _waitHandle = new AutoResetEvent(false);

void Add(object? data)
{
    if (data is AddParams ap)
    {
        Console.WriteLine($"ID of thread in Add(): {Environment.CurrentManagedThreadId}");
        Console.WriteLine($"{ap.a} + {ap.b} is {ap.a + ap.b}");
        
        _waitHandle.Set();
    }
}


Console.WriteLine($"ID of thread in Main(): {Environment.CurrentManagedThreadId}");

// Make an AddParams object to pass to the secondary thread.
AddParams ap = new AddParams(10, 10);
Thread t = new Thread(new ParameterizedThreadStart(Add));
t.Start(ap);

// Wait here until you are notified!
_waitHandle.WaitOne();
Console.WriteLine("Other thread is done!");



