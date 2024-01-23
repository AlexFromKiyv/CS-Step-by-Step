
void SlowWork()
{
    Console.WriteLine(DoLongWork());
    Console.Write("You can enter something:");
    Console.ReadLine();

    static string DoLongWork()
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work! Thread:{threadId}");
        Thread.Sleep(5000);
        return "Done with work!";
    }
}
//SlowWork();

static async void DoSyncWork()
{
    int threadId = Thread.CurrentThread.ManagedThreadId;
    Console.WriteLine($"I star to do long work synchronous! Thread: {threadId}");
    Thread.Sleep(5000);
    Console.WriteLine($"Done with work. Thread: {threadId}");

}

static async Task<string> DoLongWorkAsync()
{

    return await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(5000);
        return $"\nDone with work. Thread: {threadId}";
    });
}

//DoSyncWork();
//string message = await DoLongWorkAsync();
//Console.WriteLine(message);
//Console.ReadLine();


async void UseAsyncAwait()
{
    string message = await DoLongWorkAsync();
    int id = Thread.CurrentThread.ManagedThreadId;
    await Console.Out.WriteLineAsync($"\nI call DoLongWorkAsync in Thread:{id}");
    Console.WriteLine(message);
}


UseAsyncAwait();
Console.WriteLine($"\tThread: {Thread.CurrentThread.ManagedThreadId} says: You can enter something");
Console.ReadLine();


