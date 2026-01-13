
using SimpleMultiThreadApp;

static void Run()
{
    // Name the current thread.
    Thread primaryThread = Thread.CurrentThread;
    primaryThread.Name = "Primary";
    // Display Thread info.
    string threadInfo = $"{Thread.CurrentThread.Name}(id:{primaryThread.ManagedThreadId})";
    Console.WriteLine($"{threadInfo} is executing");

    Console.Write("Do you want 1 or 2 threads? Enter (1/2) : ");
    string? threadCount = Console.ReadLine();

    // Make worker class.
    Printer p = new Printer();

    switch (threadCount)
    {
        case "1":
            p.PrintNumbers();
            break;
        case "2":
            // Now make the thread.
            Thread backgroundThread = new Thread(new ThreadStart(p.PrintNumbers));
            backgroundThread.Name = "Secondary";
            backgroundThread.Start();
            break;
        default:
            goto case "1";
    }
    // Do some additional work.
    Console.WriteLine("\tThis is on the main thread, and we are on the finish. ");
    Console.ReadLine();
}
Run();