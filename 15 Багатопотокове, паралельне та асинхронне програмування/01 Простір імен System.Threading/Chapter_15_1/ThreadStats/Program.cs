static void PrimaryThread()
{
    // Obtain and name the current thread.
    Thread primaryThread = Thread.CurrentThread;
    primaryThread.Name = "Primary";

    // Print out some stats about this thread.
    Console.WriteLine($"ID of current thread: {primaryThread.ManagedThreadId}");
    Console.WriteLine($"Thread Name: {primaryThread.Name}");
    Console.WriteLine($"Has thread started?: {primaryThread.IsAlive}");
    Console.WriteLine($"Priority Level: {primaryThread.Priority}");
    Console.WriteLine($"Thread State: {primaryThread.ThreadState}");

}
PrimaryThread();
