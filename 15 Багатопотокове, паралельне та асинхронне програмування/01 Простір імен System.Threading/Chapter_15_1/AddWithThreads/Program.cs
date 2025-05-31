
using AddWithThreads;

void Add(object? data)
{
    if (data is AddParams ap)
    {
        Console.WriteLine($"Id of thread in Add(): {Environment.CurrentManagedThreadId}");
        Console.WriteLine($"{ap.a} + {ap.b} is {ap.a + ap.b}");
    }
}

void SimpleUseParameterizedThreadStart()
{
    Console.WriteLine($"Id of thread in top-lavel: {Environment.CurrentManagedThreadId}");

    // Make an AddParams object to pass to the secondary thread.
    AddParams ap = new AddParams(10, 10);
    Thread t = new Thread(new ParameterizedThreadStart(Add));
    t.Start(ap);
    // Force a wait to let other thread finish.
    //Thread.Sleep(5);
    Console.WriteLine(t.ThreadState);
}
//SimpleUseParameterizedThreadStart();

void UseParameterizedThreadStartWithAutoResetEvent()
{
    Console.WriteLine($"Id of thread in top-lavel: {Environment.CurrentManagedThreadId}");

    AutoResetEvent _waitHandle = new AutoResetEvent(false);
    
    AddParams ap = new AddParams(10, 10);
    Thread thread = new Thread(new ParameterizedThreadStart(AddWithAutoResetEvent));
    thread.Start(ap);

    //Wait for the wait handle to complete
    //_waitHandle.WaitOne();
    Console.WriteLine(thread.ThreadState);


    // New version Add
    void AddWithAutoResetEvent(object? data)
    {
        if (data is AddParams ap)
        {
            //Add in sleep to show the background thread getting terminated
            Thread.Sleep(2000);

            Console.WriteLine($"Id of thread in AddWithAutoResetEvent(): {Environment.CurrentManagedThreadId}");
            Console.WriteLine($"{ap.a} + {ap.b} is {ap.a + ap.b}");

            // Tell other thread we are done.
            _waitHandle.Set();
        }
    }
}
UseParameterizedThreadStartWithAutoResetEvent();


