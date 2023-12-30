
static void PrintTime(object? state)
{

    Console.Write($" {DateTime.Now.ToLongTimeString()} " +
        $"ThreadID : {Thread.CurrentThread.ManagedThreadId} " +
        $"{state?.ToString()}");

    Console.SetCursorPosition(0, 1);
    Console.CursorVisible = false;
}
//PrintTime(null);

void UseTimer()
{
    Console.WriteLine($"ThreadID : {Thread.CurrentThread.ManagedThreadId}");
    // The TimerCallback delegate object.
    TimerCallback timerCallback = new(PrintTime);

    Timer timer = new Timer(
        timerCallback,// The TimerCallback delegate object.
        null,         // Any info to pass into the called method (null for no info).
        0,            // Amount of time to wait before starting (in milliseconds).
        1000);        // Interval of time between calls (in milliseconds).

    Console.ReadLine();
}
//UseTimer();

void UseTimerWithInformation()
{
    Console.WriteLine($"ThreadID : {Thread.CurrentThread.ManagedThreadId}");

    TimerCallback timerCallback = new(PrintTime);

    _ = new Timer(timerCallback,
        "Good moment",  // Any info to pass into the called method (null for no info).
        0,1000);

    Console.ReadLine();
}
UseTimerWithInformation();