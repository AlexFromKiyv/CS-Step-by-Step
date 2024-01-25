// Асинхронні виклики з використанням шаблону async/await.
using System.Diagnostics;
using System.Threading;

void SlowWork()
{
    while (true)
    {
        Console.Clear();

        Console.WriteLine(DoLongWork());
        Console.WriteLine(DoLongWork());
        Console.WriteLine(DoLongWork());

        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} says: Enter somthing");
        Console.ReadLine();
    }
 
    static string DoLongWork()
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work! Thread:{threadId}");
        Thread.Sleep(3000); // Emulation the long work
        return "Done with work!";
    }
}
//SlowWork();

static async Task<string> DoLongWorkAsync()
{
    return await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(3000);
        return $"\nDone with work. Thread: {threadId}";
    });
}

async void CallAsyncMethod()
{
    string taskResult = await DoLongWorkAsync();
    Console.WriteLine(taskResult);
}

//while (true)
//{
//    Console.Clear();

//    CallAsyncMethod();
//    CallAsyncMethod();
//    CallAsyncMethod();

//    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} says: Enter somthing");
//    Console.ReadLine();
//}


// 

async void UseConfigureAsync()
{
    Stopwatch stopwatch1 = Stopwatch.StartNew();
    string message1 = await DoLongWorkAsync();
    Console.WriteLine($"\tmessage1 {message1}");
    stopwatch1.Stop();
    Console.WriteLine(stopwatch1.ElapsedMilliseconds);


    Stopwatch stopwatch2 = Stopwatch.StartNew();
    string message2 = await DoLongWorkAsync().ConfigureAwait(false);
    Console.WriteLine($"\tmessage2 {message2}");
    stopwatch2.Stop();
    Console.WriteLine(stopwatch2.ElapsedMilliseconds);
}

//UseConfigureAsync();
//Console.ReadLine();


//string result = DoLongWorkAsync();

//Асінхроні методи шо повертають void. 
static async void MethodReturningVoidAsync()
{
    await Task.Run(() => 
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(3000); // Emulation the long work 
    });
    Console.WriteLine("Fire and forget void method completed");
}

//MethodReturningVoidAsync();
//Console.WriteLine("The work after calling the method.");
//Console.ReadLine();

static async void MethodReturningVoidWithExceptionAsync()
{
    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(3000); // Emulation the long work 
        throw new Exception("Smomething bad happend!");
    });
    Console.WriteLine("Fire and forget void method completed");
}

//try
//{
//    MethodReturningVoidWithExceptionAsync();
//    Console.ReadLine();
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}

static async Task MethodReturningVoidTaskAsync()
{
    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(3000); // Emulation the long work 
    });
    Console.WriteLine("Method with Task completed");
}

//MethodReturningVoidTaskAsync();
//Console.WriteLine("The work after calling the method.");
//Console.ReadLine();

static async Task MethodReturningVoidTaskAndExceptionAsync()
{
    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(3000); // Emulation the long work 
        throw new Exception("Smomething bad happend!");
    });
    Console.WriteLine("Method with Task completed");
}

try
{
    // MethodReturningVoidTaskAndExceptionAsync();
    await MethodReturningVoidTaskAndExceptionAsync();
    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}