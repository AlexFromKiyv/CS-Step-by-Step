
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;

static string DoWork()
{
    int threadId = Thread.CurrentThread.ManagedThreadId;
    Console.WriteLine($"\n\tI star to do long work! Thread:{threadId}");
    Thread.Sleep(5000); // Emulation the long work
    return $"\tDone with work! Thread:{threadId}\n";
}


//while (true)
//{
//    Console.WriteLine(DoWork());
//    Console.Write($"Thread {Thread.CurrentThread.ManagedThreadId} says: Enter somthing:");
//    Console.ReadLine();
//}


static async Task<string> DoWorkAsync()
{
    return await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"\n\tI star to do long work asynchronous! Thread:{threadId}");
        Thread.Sleep(5000); // Emulation the long work
        return $"\tDone with work! Thread:{threadId}\n";
    });
}

//while (true)
//{
//    //string taskResult = await DoWorkAsync();
//    //Console.WriteLine(taskResult);

//    //_ = DoWorkAsync();

//    _ = CallDoWorkAsync();

//    Console.Write($"Thread {Thread.CurrentThread.ManagedThreadId} says: Enter somthing:");
//    Console.ReadLine();
//}

static async Task CallDoWorkAsync()
{
    string taskResult = await DoWorkAsync();
    Console.WriteLine(taskResult);
}





//string message = await DoWorkAsync().ConfigureAwait(true);
//Console.WriteLine($"0 - {message}");
//string message1 = await DoWorkAsync().ConfigureAwait(false);
//Console.WriteLine($"1 - {message1}");


//string message = DoWorkAsync();


static async void MethodReturningVoidAsync()
{

    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        /* Do some work here... */
        Thread.Sleep(4_000);
        Console.WriteLine($"Thread: {threadId}");
        throw new Exception("Something bad happened");
    });
    Console.WriteLine($"Fire and forget void method completed");
}

//MethodReturningVoidAsync();
//Console.WriteLine($"Completed Thread: {Thread.CurrentThread.ManagedThreadId}");
//Console.ReadLine();

//try
//{
//    MethodReturningVoidAsync();
//    Console.WriteLine($"Completed Thread: {Thread.CurrentThread.ManagedThreadId}");
//    Console.ReadLine();
//}
//catch (Exception e)
//{
//    Console.WriteLine(e.Message);
//}

static async Task MethodReturningVoidTaskAsync()
{

    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        /* Do some work here... */
        Thread.Sleep(4_000);
        Console.WriteLine($"Thread: {threadId}");
        //throw new Exception("Something bad happened");
    });
    Console.WriteLine($"Void method completed");
}

//MethodReturningVoidTaskAsync();
//Console.WriteLine($"Completed Thread: {Thread.CurrentThread.ManagedThreadId}");
//Console.ReadLine();

//try
//{
//    //MethodReturningVoidTaskAsync();
//    await MethodReturningVoidTaskAsync();
//    Console.WriteLine($"Completed Thread: {Thread.CurrentThread.ManagedThreadId}");
//    Console.ReadLine();
//}
//catch (Exception e)
//{
//    Console.WriteLine(e.Message);
//}


static async Task MultipleAwaits()
{
    await Task.Run(() => { Thread.Sleep(2_000); });
    Console.WriteLine($"Done with first task! {Thread.CurrentThread.ManagedThreadId}");
    await Task.Run(() => { Thread.Sleep(2_000); });
    Console.WriteLine($"Done with second task! {Thread.CurrentThread.ManagedThreadId}");
    await Task.Run(() => { Thread.Sleep(2_000); });
    Console.WriteLine($"Done with third task! {Thread.CurrentThread.ManagedThreadId}");
}

//var watch = Stopwatch.StartNew();
//await MultipleAwaits();
//watch.Stop();
//Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");

static async Task MultipleAwaitsAsync()
{
    await Task.WhenAll(
            Task.Run(() =>
            {
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with first task! {Thread.CurrentThread.ManagedThreadId}");
            }), 
            Task.Run(() =>
            {
                Thread.Sleep(1_000);
                Console.WriteLine($"Done with second task! {Thread.CurrentThread.ManagedThreadId}");
            }), 
            Task.Run(() =>
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with third task! {Thread.CurrentThread.ManagedThreadId}");
            })
     );
}

//var watch = Stopwatch.StartNew();
//await MultipleAwaitsAsync();
//watch.Stop();
//Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");

static async Task WhenAllDoWork()
{
    await Task.WhenAll(
        Task.Run(() => DoWork()),
        Task.Run(() => DoWork()),
        Task.Run(() => DoWork()));
}

//var watch = Stopwatch.StartNew();
//await WhenAllDoWork();
//watch.Stop();
//Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");


static async Task MultipleAwaitsWhenAnyAsync()
{
    await Task.WhenAny(
        Task.Run(() =>
        {
            Thread.Sleep(2_000);
            Console.WriteLine("Done with first task!");
        }), 
        Task.Run(() =>
        {
            Thread.Sleep(1_000);
            Console.WriteLine("Done with second task!");
        }), 
        Task.Run(() =>
        {
            Thread.Sleep(2_000);
            Console.WriteLine("Done with third task!");
        })
    );
}

//var watch = Stopwatch.StartNew();
//await MultipleAwaitsWhenAnyAsync();
//watch.Stop();
//Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");


static async Task MultipleAwaitsWithListTaskAsync()
{
    var tasks = new List<Task>();
    tasks.Add(Task.Run(() =>
        {
            Thread.Sleep(2_000);
            Console.WriteLine("Done with first task!");
        }));
    tasks.Add(Task.Run(() =>
        {
            Thread.Sleep(1_000);
            Console.WriteLine("Done with second task!");
        }));
    tasks.Add(Task.Run(() =>
        {
            Thread.Sleep(2_000);
            Console.WriteLine("Done with third task!");
        }));
    //await Task.WhenAny(tasks);
    await Task.WhenAll(tasks);
}

//var watch = Stopwatch.StartNew();
//await MultipleAwaitsWithListTaskAsync();
//watch.Stop();
//Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");



//_ = DoWorkAsync().Result;
//_ = DoWorkAsync().GetAwaiter().GetResult();

//JoinableTaskFactory joinableTaskFactory = new(new JoinableTaskContext());

//string message = joinableTaskFactory.Run(async () => await DoWorkAsync());
//Console.WriteLine(message);

//try
//{

//    joinableTaskFactory.Run(async () =>
//    {
//        await MethodReturningVoidTaskAsync();
//        //await SomeOtherAsyncMethod();
//    });
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
static async ValueTask<int> ReturnAnInt()
{
    await Task.Delay(3_000);
    return 5;
}

//Console.WriteLine(ReturnAnInt());
//Console.WriteLine(await ReturnAnInt());

static async Task MethodWithProblems(int firstParam, int secondParam)
{
    await Task.Run(() =>
    {
        //Call long running method
        Thread.Sleep(4_000);
        Console.WriteLine("First Complete");
        //Call another long running method that fails because
        //the second parameter is out of range
        Console.WriteLine("Something bad happened");
    });
}
//await MethodWithProblems(1, -2);

static async Task MethodWithProblemsFixed(int firstParam, int secondParam)
{
    if (secondParam < 0)
    {
        Console.WriteLine("Bad data");
        return;
    }
    await actualImplementation();
    async Task actualImplementation()
    {
        await Task.Run(() =>
        {
            //Call long running method
            Thread.Sleep(4_000);
            Console.WriteLine("First Complete");
            //Call another long running method that fails because
            //the second parameter is out of range
            Console.WriteLine("Something bad happened");
        });
    }
}

//await MethodWithProblemsFixed(1, -2);

async Task UsingWaitAsync()
{
    CancellationTokenSource cancellationTokenSource = new();

    try
    {
        string message = await DoWorkAsync().WaitAsync(TimeSpan.FromSeconds(12));
        await Console.Out.WriteLineAsync(message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        string message = await DoWorkAsync().WaitAsync(TimeSpan.FromSeconds(2));
        await Console.Out.WriteLineAsync(message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        string message = await DoWorkAsync().WaitAsync(cancellationTokenSource.Token);
        await Console.Out.WriteLineAsync(message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }


    cancellationTokenSource.Cancel();

    try
    {
        _ = await DoWorkAsync().WaitAsync(cancellationTokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        _ = await DoWorkAsync().WaitAsync(TimeSpan.FromSeconds(2), cancellationTokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}
//await UsingWaitAsync();

void UsingWait()
{
    CancellationTokenSource tokenSource = new CancellationTokenSource();
    MethodReturningVoidTaskAsync().Wait(tokenSource.Token);
    MethodReturningVoidTaskAsync().Wait(10000, tokenSource.Token);

    tokenSource.Cancel();

    try
    {
       MethodReturningVoidTaskAsync().Wait(tokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        MethodReturningVoidTaskAsync().Wait(2000, tokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//UsingWait();

void UsingWaitAsyncInSync()
{
    JoinableTaskFactory joinableTaskFactory = new JoinableTaskFactory(new JoinableTaskContext());
    CancellationTokenSource tokenSource = new CancellationTokenSource();

    try
    {
        joinableTaskFactory.Run(async () =>
        {
            await MethodReturningVoidTaskAsync().WaitAsync(tokenSource.Token);
            await MethodReturningVoidTaskAsync().WaitAsync(TimeSpan.FromSeconds(2), tokenSource.Token);
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//UsingWaitAsyncInSync();

static async IAsyncEnumerable<int> GenerateSequence()
{
    for (int i = 0; i < 20; i++)
    {
        await Task.Delay(100);
        yield return i;
    }
}

await foreach (var number in GenerateSequence())
{
    Console.WriteLine(number);
}