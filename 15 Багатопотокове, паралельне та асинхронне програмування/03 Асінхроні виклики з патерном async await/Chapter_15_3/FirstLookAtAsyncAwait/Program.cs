using System.Diagnostics;
using Microsoft.VisualStudio.Threading;
static string GetThreadInfo()
{
    Thread thread = Thread.CurrentThread;
    return $"\tThreadId:{thread.ManagedThreadId}" +
        $"\tIsBackground:{thread.IsBackground}" +
        $"\tThreadState:{thread.ThreadState}";
}

static string DoWork()
{
    Console.WriteLine($"\tI star do work!\t{GetThreadInfo()}");
    Thread.Sleep(5000); // Emulation the long work
    return $"\tDone with work!";
}

//string result = DoWork();
//Console.WriteLine(result);
//Console.WriteLine($"Complited\t{GetThreadInfo()}");

//--
static async Task<string> DoWorkAsync()
{
    return await Task.Run(() => {
        Console.WriteLine($"\tI star do work!\t{GetThreadInfo()}");
        Thread.Sleep(5000); // Emulation the long work
        return $"\tDone with work!";
    });
}

//string message = await DoWorkAsync();
//Console.WriteLine(message);
//Console.WriteLine($"Complited\t{GetThreadInfo()}");

//--

//DoWorkAsync();

//--


//string message = await DoWorkAsync();
//Console.WriteLine($"0 - {message}");
//string message1 = await DoWorkAsync().ConfigureAwait(false);
//Console.WriteLine($"1 - {message1}");

//--

static async Task<string> MyWork()
{
    return await Task.Run(() => {

        string result = "The result:";
            //...Do long work to get result 
        return result;
    });
}

//string otherString = MyWork();

//string otherString = await MyWork();
//Console.WriteLine(otherString);

//--

static async void MethodReturningVoidAsync()
{
    await Task.Run(() =>
    {
        Console.WriteLine(GetThreadInfo());
        /* Do some work here... */
        Thread.Sleep(3000);

    });
    Console.WriteLine($"Fire and forget void method completed");
}

//MethodReturningVoidAsync();
//Console.WriteLine($"Completed.{GetThreadInfo()}");
//Console.ReadLine();

//--

static async void MethodReturningVoidWithExeptionAsync()
{
    await Task.Run(() =>
    {
        Console.WriteLine(GetThreadInfo());
        /* Do some work here... */
        Thread.Sleep(3000);
        throw new Exception("Something bad happened");
    });
    Console.WriteLine($"Fire and forget void method completed");
}

//try
//{
//    MethodReturningVoidWithExeptionAsync();
//}
//catch (Exception e)
//{
//    Console.WriteLine(e);
//}

//--

static async Task MethodReturningTaskAsync()
{
    await Task.Run(() =>
    {
        Console.WriteLine(GetThreadInfo());
        /* Do some work here... */
        Thread.Sleep(3000);
    });
    Console.WriteLine($"Void method completed");
}

//MethodReturningTaskAsync();
//Console.WriteLine($"Completed.{GetThreadInfo()}");
//Console.ReadLine();

//--

static async Task MethodReturningTaskWithExeptionAsync()
{
    await Task.Run(() =>
    {
        Console.WriteLine(GetThreadInfo());
        /* Do some work here... */
        Thread.Sleep(3000);
        throw new Exception("Something bad happened");
    });
    Console.WriteLine($"Void method completed");
}

//try
//{
//    //MethodReturningTaskWithExeptionAsync();
//    await MethodReturningTaskWithExeptionAsync();
//    Console.WriteLine($"Completed.{GetThreadInfo()}");
//    Console.ReadLine();
//}
//catch (Exception e)
//{
//    Console.WriteLine(e.Message);
//}

//--

static async Task MultipleAwaitsAsync()
{
    await Task.Run(() =>
    {
        Thread.Sleep(2000);
        Console.WriteLine($"Done with first task! {GetThreadInfo()}");
    });
    await Task.Run(() => 
    {
        Thread.Sleep(1000);
        Console.WriteLine($"Done with second task! {GetThreadInfo()}");
    });
    await Task.Run(() => 
    {
        Thread.Sleep(2000);
        Console.WriteLine($"Done with third task! {GetThreadInfo()}");
    });
}

//var watch = Stopwatch.StartNew();
//await MultipleAwaitsAsync();
//watch.Stop();
//Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");

//--

static async Task MultipleAwaitsWhenAllAsync()
{
    await Task.WhenAll(
            Task.Run(() =>
            {
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with first task! {GetThreadInfo()}");
            }),
            Task.Run(() =>
            {
                Thread.Sleep(1_000);
                Console.WriteLine($"Done with second task! {GetThreadInfo()}");
            }),
            Task.Run(() =>
            {
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with third task! {GetThreadInfo()}");
            })
     );
}

//var watch = Stopwatch.StartNew();
//await MultipleAwaitsWhenAllAsync();
//watch.Stop();
//Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");

static async Task MultipleAwaitsWhenAnyAsync()
{
    await Task.WhenAny(
            Task.Run(() =>
            {
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with first task! {GetThreadInfo()}");
            }),
            Task.Run(() =>
            {
                Thread.Sleep(1_000);
                Console.WriteLine($"Done with second task! {GetThreadInfo()}");
            }),
            Task.Run(() =>
            {
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with third task! {GetThreadInfo()}");
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
        Console.WriteLine($"Done with first task! {GetThreadInfo()}");
    }));
    tasks.Add(Task.Run(() =>
    {
        Thread.Sleep(1_000);
        Console.WriteLine($"Done with second task! {GetThreadInfo()}");
    }));
    tasks.Add(Task.Run(() =>
    {
        Thread.Sleep(2_000);
        Console.WriteLine($"Done with third task! {GetThreadInfo()}");
    }));
    await Task.WhenAll(tasks);
    //await Task.WhenAny(tasks);

}

//var watch = Stopwatch.StartNew();
//await MultipleAwaitsWithListTaskAsync();
//watch.Stop();
//Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");

//--

//_ = DoWorkAsync().Result;
//_ = DoWorkAsync().GetAwaiter().GetResult();

//--

//JoinableTaskFactory joinableTaskFactory = new JoinableTaskFactory(
//    new JoinableTaskContext());

//string message = joinableTaskFactory.Run(async () => await DoWorkAsync());
//Console.WriteLine(message);

//--

static async ValueTask<int> ReturnAnIntAsync()
{
    await Task.Delay(3_000);
    return 5;
}

////Console.WriteLine(ReturnAnIntAsync()); // You won't see anything.
//Console.WriteLine(await ReturnAnIntAsync());

//--
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

//--

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

//--

async Task UsingWaitAsync()
{
    CancellationTokenSource cancellationTokenSource = new();

    //try
    //{
    //    string message = await DoWorkAsync().WaitAsync(TimeSpan.FromSeconds(12));
    //    await Console.Out.WriteLineAsync(message);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}

    //try
    //{
    //    string message = await DoWorkAsync().WaitAsync(TimeSpan.FromSeconds(2));
    //    await Console.Out.WriteLineAsync(message);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}

    //try
    //{
    //    string message = await DoWorkAsync().WaitAsync(cancellationTokenSource.Token);
    //    await Console.Out.WriteLineAsync(message);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}


    cancellationTokenSource.Cancel();

    //try
    //{
    //    _ = await DoWorkAsync().WaitAsync(cancellationTokenSource.Token);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}

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

//--
void UsingWait()
{
    CancellationTokenSource tokenSource = new CancellationTokenSource();
    //MethodReturningTaskAsync().Wait(tokenSource.Token);
    //MethodReturningTaskAsync().Wait(10000, tokenSource.Token);

    tokenSource.Cancel();

    //try
    //{
    //    MethodReturningTaskAsync().Wait(tokenSource.Token);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}

    try
    {
        MethodReturningTaskAsync().Wait(2000, tokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//UsingWait();

//--

void UsingWaitAsyncInSync()
{
    JoinableTaskFactory joinableTaskFactory = new JoinableTaskFactory(new JoinableTaskContext());
    CancellationTokenSource tokenSource = new CancellationTokenSource();
    tokenSource.Cancel();
    try
    {
        joinableTaskFactory.Run(async () =>
        {
            //await MethodReturningTaskAsync().WaitAsync(tokenSource.Token);
            await MethodReturningTaskAsync().WaitAsync(TimeSpan.FromSeconds(2), tokenSource.Token);
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//UsingWaitAsyncInSync();

//--

static async IAsyncEnumerable<int> GenerateSequence()
{
    for (int i = 0; i < 20; i++)
    {
        await Task.Delay(100);
        yield return i;
    }
}

//await foreach (var number in GenerateSequence())
//{
//    Console.Write($"{number} ");
//}