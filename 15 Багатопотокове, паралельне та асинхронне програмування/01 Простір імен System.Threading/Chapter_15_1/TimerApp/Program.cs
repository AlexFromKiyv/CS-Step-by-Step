static void PrintTime(object? state)
{
    Thread thread = Thread.CurrentThread;

    string message =
        $"Time is: {DateTime.Now.ToLongTimeString()}\t" +
        $"Param is {state}\t" +
        $"ManagedThreadId: {thread.ManagedThreadId}\t" +
        $"IsBackground: {thread.IsBackground}";

    Console.WriteLine(message);
}

void Run()
{
    Console.WriteLine($"Primary thread id: {Thread.CurrentThread.ManagedThreadId}");

    // Create the delegate for the Timer type.
    TimerCallback timerCallback = new TimerCallback(PrintTime);

    // Establish timer settings.
    Timer t = new Timer(
      timerCallback,     // The TimerCallback delegate object.
      "It's noon",       // Any info to pass into the called method (null for no info).
      0,          // Amount of time to wait before starting (in milliseconds).
      1000);      // Interval of time between calls (in milliseconds).

    Console.WriteLine("Hit Enter key to terminate...");
    Console.ReadLine();
}
Run();

