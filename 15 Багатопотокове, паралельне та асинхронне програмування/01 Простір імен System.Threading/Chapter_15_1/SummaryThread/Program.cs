void DoWork(int delay) 
{ 
    Thread.Sleep(delay);
}

void MethodForWork()
{
    // Display Thread info.
    string threadInfo = $"{Thread.CurrentThread.Name} (id:{Thread.CurrentThread.ManagedThreadId})";
    Console.WriteLine($"{threadInfo} is executing DoWork 5000 ms.");
    DoWork(5000);
}

void Run()
{
    // Display Thread info.
    string threadInfo = $"Primary (id:{Thread.CurrentThread.ManagedThreadId})";
    Console.WriteLine(threadInfo);

    Thread thread = new Thread(new ThreadStart(MethodForWork));
    thread.Name = "Secondary";
    thread.IsBackground = true;
    thread.Start();

    Console.ReadLine();
}
Run();
