using System.Diagnostics;
using System.Threading;

void DoWork(int time, int taskId)
{
    Thread.Sleep(time);

    Thread thread = Thread.CurrentThread;

    string threadInfo = "\n"+
        $"\tManagedThreadId: {thread.ManagedThreadId}\t" +
        $"\tIsBackground: {thread.IsBackground}" +
        $"\tIsAlive:{thread.IsAlive}" +
        $"\tThreadState:{thread.ThreadState}";

    Console.WriteLine(threadInfo);

    int id = Thread.CurrentThread.ManagedThreadId;
    Console.WriteLine($"Task {taskId} in thread:{id} finished.\n");
}

void RunTasks()
{
    Task task1 = new Task(() => DoWork(2000,1));
    task1.Start();

    Task task2 = Task.Factory.StartNew(() => DoWork(2000,2));

    Task task3 = Task.Run(() => DoWork(1000,3));

    Console.WriteLine($"All tasks are working.");

    //task1.Wait();
    //task2.Wait();
    //task3.Wait();
}
//RunTasks();

void PropertyOfTask()
{

    Task task = new(() =>
    {
        //Console.WriteLine($"Task Id: {Task.CurrentId}");
        //Thread.Sleep(2000);
        DoWork(2000, 2);
    });
    task.Start();

    Console.WriteLine($"Id: {task.Id}");
    Console.WriteLine($"IsCompleted: {task.IsCompleted}");
    Console.WriteLine($"IsFaulted: {task.IsFaulted}");
    Console.WriteLine($"IsCanceled: {task.IsCanceled}");
    Console.WriteLine($"Status: {task.Status}");
    Console.WriteLine($"IsCompletedSuccessfully:{task.IsCompletedSuccessfully}");

    task.Wait();

    Console.WriteLine($"Id: {task.Id}");
    Console.WriteLine($"IsCompleted: {task.IsCompleted}");
    Console.WriteLine($"IsFaulted: {task.IsFaulted}");
    Console.WriteLine($"IsCanceled: {task.IsCanceled}");
    Console.WriteLine($"Status: {task.Status}");
    Console.WriteLine($"IsCompletedSuccessfully:{task.IsCompletedSuccessfully}");

}
//PropertyOfTask();

void InnerTask()
{
    Console.WriteLine("Start main");
    Task outer = Task.Factory.StartNew(() =>
    {
        Console.WriteLine("Outer task starting.");

        Task inner = Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Inner task starting.");
            DoWork(1000, 2);
            Console.WriteLine("Inner task finished.");
        });
        //inner.Wait();
        Console.WriteLine("Outer task finished.");
    });
    outer.Wait();
    Console.WriteLine("End main");
}
//InnerTask();

void InnerTaskAsPartOuter()
{
    Console.WriteLine("Start main");
    Task outer = Task.Factory.StartNew(() =>
    {
        Console.WriteLine("Outer task starting.");

        Task inner = Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Inner task starting.");
            DoWork(2000,2);
            Console.WriteLine("Inner task finished.");
        }, TaskCreationOptions.AttachedToParent);
        //inner.Wait();
        Console.WriteLine("Outer task finished.");
    });
    outer.Wait();
    Console.WriteLine("End main");
}
//InnerTaskAsPartOuter();

void ArrayOfTasks()
{
    Task[] tasks =
    [
        new Task(() => DoWork(3000,1)),
        new Task(() => DoWork(3000,2)),
        new Task(() => DoWork(3000,3))
    ];

    for (int i = 0; i < 3; i++)
    {
        tasks[i].Start();
    }
    Console.WriteLine($"All tasks are working.");
    Task.WaitAll(tasks);
}
//ArrayOfTasks();

int SlowlyGetSquare(int x)
{
    DoWork(2000, 1);
    return x*x;
}

void ObtainingTheResultOfTheTask()
{
    Console.Write($"Primary thread:{Thread.CurrentThread.ManagedThreadId}\nEnter number :");

    int.TryParse(Console.ReadLine(), out int x);

    Task<int> squareTask = new(() =>
    {
        Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId}");
        return SlowlyGetSquare(x);
    });

    squareTask.Start();

    int result = squareTask.Result;

    Console.WriteLine($"Result:{result}");
}
//ObtainingTheResultOfTheTask();

void ContinuationTask()
{
    Task firstTask = new Task(() => Console.WriteLine($"Task Id:{Task.CurrentId}"));

    Task nextTask = firstTask.ContinueWith(AboutTask);

    firstTask.Start();

    firstTask.Wait();

    void AboutTask(Task task)
    {
        Console.WriteLine($"Current Task Id:{Task.CurrentId}");
        Console.WriteLine($"Previous Task Id:{task.Id}");
    }
}
//ContinuationTask();

void ContinuationTaskWithValue()
{
    int x = 5;

    Task<int> squareTask = new(() =>
    {
        Console.WriteLine($"Task {Task.CurrentId} says: I'm starting");
        return SlowlyGetSquare(x);
    });

    Task nextTask = squareTask.ContinueWith(task => PrintResult(task.Result));

    squareTask.Start();

    squareTask.Wait();

    void PrintResult(int result)
    {
        Console.WriteLine($"Task {Task.CurrentId} says: I waited {result} ");
    }
}
//ContinuationTaskWithValue();

void ChainOfTask()
{
    Task firstTask = new(() => AboutTask(null));

    Task secondTask = firstTask.ContinueWith(AboutTask);

    Task thirdTask = secondTask.ContinueWith(AboutTask);

    firstTask.Start();

    Console.ReadLine();

    void AboutTask(Task? task) =>
        Console.WriteLine($"Current Task:{Task.CurrentId}  Previous Task:{task?.Id}");
}
//ChainOfTask();

void CancellationWitoutThrow()
{
    using CancellationTokenSource cancellationTokenSource = new();
    CancellationToken cancellationToken = cancellationTokenSource.Token;

    Task task = new(() =>
    {
        for (int i = 0; i < 10; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Task canceled");
                return;
            }
            Console.Write($"{i} ");
            Thread.Sleep(500);
        }
    }, cancellationToken);
    task.Start();

    Thread.Sleep(3000);

    cancellationTokenSource.Cancel();

    Console.ReadLine();
}
//CancellationWitoutThrow();

void CancellationWithThrow()
{
    using CancellationTokenSource cancellationTokenSource = new();
    CancellationToken cancellationToken = cancellationTokenSource.Token;

    Task task = new(() =>
    {
        for (int i = 0; i < 10; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Console.Write($"{i} ");
            Thread.Sleep(500);
        }

    }, cancellationToken);

    try
    {
        task.Start();
        Thread.Sleep(3000);
        cancellationTokenSource.Cancel();
        task.Wait();
    }
    catch (AggregateException ae)
    {
        foreach (Exception ex in ae.InnerExceptions)
        {
            if (ex is TaskCanceledException)
                Console.WriteLine("Task canceled");
            else
                Console.WriteLine(ex.Message);
        }
    }
    Console.WriteLine($"Task status:{task.Status}");
}
//CancellationWithThrow();

void SimpleUsingInvoke_1()
{
    Parallel.Invoke(
        () => DoWork(3000, 1),
        () => DoWork(1000, 2),
        () => DoWork(3000, 3),
        () => Console.WriteLine("Hi")
        );
}
//SimpleUsingInvoke_1();


void SimpleUsingInvoke_2()
{
    var watch = Stopwatch.StartNew();

    Parallel.Invoke(
        () => DoWork(3000, 1),
        () => DoWork(2000, 2),
        () => DoWork(3000, 3),
        () => Console.WriteLine("Hi")
        );

    watch.Stop();
    Console.WriteLine($"\tTime: {watch.ElapsedMilliseconds}");

    watch = Stopwatch.StartNew();

    DoWork(3000, 1);
    DoWork(2000, 2);
    DoWork(3000, 3);
    Console.WriteLine("Hi");

    watch.Stop();
    Console.WriteLine($"\tTime: {watch.ElapsedMilliseconds}");
}
//SimpleUsingInvoke_2();
