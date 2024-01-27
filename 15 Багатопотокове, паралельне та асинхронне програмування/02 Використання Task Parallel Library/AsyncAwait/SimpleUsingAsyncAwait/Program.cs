// Асинхронні виклики з використанням шаблону async/await.
using Microsoft.VisualStudio.Threading;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;
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

// Асінхроні методи шо повертають void. 
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

//  Асінхроні методи шо повертають Task.
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

//try
//{
//    // MethodReturningVoidTaskAndExceptionAsync();
//    await MethodReturningVoidTaskAndExceptionAsync();
//    Console.ReadLine();
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}


// Асінхроний метод з багатьма await.

static async Task MultipleAwaits()
{
    await Task.Run(() => 
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(2000); 
    });
    Console.WriteLine("Done 1");

    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(2000);
    });
    Console.WriteLine("Done 2");

    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(2000);
    });
    Console.WriteLine("Done 3");
}

//await MultipleAwaits();

static async Task UseTaskWhenAll()
{
    Task[] tasks = [

        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(3000);
            Console.WriteLine("Done 1");
        }),
        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(6000);
            Console.WriteLine("Done 2");
        }),
        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(9000);
            Console.WriteLine("Done 3");
        }),
    ];
    await Task.WhenAll(tasks);
}

//await UseTaskWhenAll();
//Console.Write("Enter something:"); Console.ReadLine();

static async Task UseTaskWhenAny()
{
    Task[] tasks = [

        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(3000);
            Console.WriteLine("Done 1");
        }),
        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(6000);
            Console.WriteLine("Done 2");
        }),
        Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(9000);
            Console.WriteLine("Done 3");
        }),
    ];
    await Task.WhenAny(tasks);
}

//await UseTaskWhenAny();
//Console.Write("Enter something:"); Console.ReadLine();

// ## Виклик асінхронних методів з сінхронних.

//Task<string> task = DoLongWorkAsync();
//Console.WriteLine(task.Result);
//Console.ReadLine();


//JoinableTaskFactory joinableTaskFactory = new JoinableTaskFactory(new JoinableTaskContext());
//string message2 = joinableTaskFactory.Run(DoLongWorkAsync);
//Console.WriteLine(message2);

static async ValueTask<int> ReturnAnInt()
{
    await Task.Delay(3_000);
    return 5;
}

//int c = await ReturnAnInt();
//Console.WriteLine(c);


// Перевірка параметрів асінхроних методів.
static async Task MethodWithProblem(int t)
{
   await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
        Thread.Sleep(2000);
        t = 5 / t;
        Console.WriteLine(t);
    });
}

// MethodWithProblem(0);


static async Task MethodWithVerification(int t)
{
    if(!Verification(t))
    {
        Console.WriteLine("Bad parameter");
        return;
    }
    await Implementation();

    // privat function
    static bool Verification(int p) => (p == 0) ? false : true;

    async Task Implementation()
    {
        await Task.Run(() =>
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"I star to do long work asynchronous! Thread: {threadId}");
            Thread.Sleep(3000);
            t = 15 / t;
            Console.WriteLine($"Ok {t}");
        });
    }
}

MethodWithVerification(0);
await MethodWithVerification(0);
await MethodWithVerification(5);
