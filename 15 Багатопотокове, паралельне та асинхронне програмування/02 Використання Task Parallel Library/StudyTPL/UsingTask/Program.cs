using System.Runtime.CompilerServices;

void RunTasks()
{
    Task task1 = new Task(() => { Console.WriteLine($"Task 1 on {Thread.CurrentThread.ManagedThreadId}");});
    task1.Start();

    Task task2 = Task.Factory.StartNew(() => { Console.WriteLine($"Task 2 on {Thread.CurrentThread.ManagedThreadId}"); ; ;});

    Task task3 = Task.Run(() => { Console.WriteLine($"Task 3 on {Thread.CurrentThread.ManagedThreadId} "); });

    task1.Wait();
    task2.Wait();
    task3.Wait();

}
//RunTasks();

void UsingWait()
{
    Console.WriteLine("Main thread.");
    
    Task task = new(()=>
    {
        Console.WriteLine("Start Task.");
        Thread.Sleep(1000);
        Console.WriteLine("End Task.");

    });
    task.Start();
    task.Wait();

    Console.WriteLine("End main thread."  );
}
//UsingWait();
void PropertyOfTask()
{
    Console.WriteLine("Main thread.");

    Task task = new(() =>
    {
        Console.WriteLine($"Id: {Task.CurrentId}");
        Thread.Sleep(1000);
    });
    task.Start();

    Console.WriteLine($"Id: {task.Id}");
    Console.WriteLine($"IsCompleted: {task.IsCompleted}");
    Console.WriteLine($"IsFaulted: {task.IsFaulted}");
    Console.WriteLine($"IsCanceled: {task.IsCanceled}");
    Console.WriteLine($"Status: {task.Status}");

    task.Wait();

    Console.WriteLine("End main thread.");
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
            Thread.Sleep(1000);
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
            Thread.Sleep(1000);
            Console.WriteLine("Inner task finished.");
        },TaskCreationOptions.AttachedToParent);
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
        new Task( () => 
        {
            Console.WriteLine("Task 1 started.");
            Thread.Sleep(3000); 
            Console.WriteLine("Task 1 finished.");}),

        new Task(() =>
        {
            Console.WriteLine("Task 2 started.");
            Thread.Sleep(3000);
            Console.WriteLine("Task 2 finished.");
        }),
        new Task(() =>
        {
            Console.WriteLine("Task 3 started.");
            Thread.Sleep(3000);
            Console.WriteLine("Task 3 finished.");
        }),

    ];

    for (int i = 0; i < 3; i++)
    {
        tasks[i].Start();
    }

    Task.WaitAll(tasks);
}
//ArrayOfTasks();

void ObtainingTheResultOfTheTask()
{
    Console.Write($"Thread:{Thread.CurrentThread.ManagedThreadId} Enter number :");
    
    int.TryParse(Console.ReadLine(),out int x);

    Task<int> squareTask = new(() =>
    {
        Console.WriteLine($"Thread:{Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(3000);
        return x*x;
    });

    squareTask.Start();

    int result = squareTask.Result;

    Console.WriteLine($"Result:{result}");
}
//ObtainingTheResultOfTheTask();


void ContinuationTask()
{
    Task firstTask = new Task(() => Console.WriteLine($"Task Id:{Task.CurrentId}") );

    Task nextTask = firstTask.ContinueWith(AboutTask);

    firstTask.Start();

    nextTask.Wait();

    void AboutTask(Task task)
    {
        Console.WriteLine($"Current Task Id:{Task.CurrentId}");
        Console.WriteLine($"Previous Task Id:{task.Id}");
    }
}
//ContinuationTask();

void ContinuationTaskWithValue()
{
    int t = 5000;

    Task<int> squareTask = new(() =>
    {
        Console.WriteLine($"Task {Task.CurrentId} says: I'm starting"  );
        Thread.Sleep(t);
        return t;
    });

    Task nextTask = squareTask.ContinueWith(task => PrintResult(task.Result));

    squareTask.Start();

    Console.ReadLine();

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
