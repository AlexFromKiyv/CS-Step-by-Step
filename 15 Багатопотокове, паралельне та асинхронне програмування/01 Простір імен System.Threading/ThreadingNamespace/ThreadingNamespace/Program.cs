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