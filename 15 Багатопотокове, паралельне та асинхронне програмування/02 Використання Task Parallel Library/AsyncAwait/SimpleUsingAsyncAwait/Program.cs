
void SlowWork()
{
    Console.WriteLine(DoLongWork());
    Console.Write("You can enter something:");
    Console.ReadLine();

    static string DoLongWork()
    {
        Console.WriteLine("I star to do long work!");
        Thread.Sleep(5000);
        return "Done with work!";
    }
}
//SlowWork();

static async Task<string> DoLongWorkAsync()
{
    return await Task.Run(() =>
    {
        Console.WriteLine($"I star to do long work! Thread:{Environment.CurrentManagedThreadId}");
        Thread.Sleep(5000);
        return $"\nDone with work! Thread: {Thread.CurrentThread.ManagedThreadId}";
    });
}

static async void UseAsyncAwait()
{
    string message = await DoLongWorkAsync();
    Console.WriteLine(message);
}

void TestingUsingAsyncAwait()
{
    UseAsyncAwait();
    Console.WriteLine($"\tThread: {Thread.CurrentThread.ManagedThreadId} says: You can enter something");
    Console.ReadLine();
}
TestingUsingAsyncAwait();



