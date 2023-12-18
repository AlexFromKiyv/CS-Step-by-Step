static void ExtractExecutingThread()
{
    Thread thread = Thread.CurrentThread;
    Console.WriteLine(thread.ManagedThreadId);

}
//ExtractExecutingThread();

static void ExtractAppDomainHostingThread()
{
    // Obtain the AppDomain hosting the current thread.
    AppDomain appDomain = Thread.GetDomain();
    Console.WriteLine(appDomain.FriendlyName);
}
//ExtractAppDomainHostingThread();

static void ExtractCurrentThreadExecutionContext()
{
    ExecutionContext executionContext = Thread.CurrentThread.ExecutionContext;
    Console.WriteLine(executionContext);
}
//ExtractCurrentThreadExecutionContext();

void ExplorationTheThread()
{
    Thread primaryThread = Thread.CurrentThread;
    primaryThread.Name = "ThePrimaryThread";

    Console.WriteLine($"Name :{primaryThread.Name}");
    Console.WriteLine($"ManagedThreadId :{primaryThread.ManagedThreadId}");
    Console.WriteLine($"IsAlive :{primaryThread.IsAlive}");
    Console.WriteLine($"Priority :{primaryThread.Priority}");
    Console.WriteLine($"ThreadState :{primaryThread.ThreadState}"); 
    Console.WriteLine($"IsThreadPoolThread :{primaryThread.IsThreadPoolThread}");
    Console.WriteLine($"CurrentCulture :{primaryThread.CurrentCulture}");
}

ExplorationTheThread();