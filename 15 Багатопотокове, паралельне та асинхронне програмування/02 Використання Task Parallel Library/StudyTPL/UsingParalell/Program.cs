void SimpleUsingInvoke()
{
    Parallel.Invoke(
        () => {
            Console.WriteLine($"Task:{Task.CurrentId}");
            Thread.Sleep(3000);
            Console.WriteLine("Hi");

        },        
        ()=>SayWithDelay("girl",5000),
        
        SayGoodbay       
        );

    void SayGoodbay()
    {
        SayWithDelay("Goodbay", 8000);
    }
    
    void SayWithDelay(string phrase, int delay) 
    {
        AboutTask();
        Thread.Sleep(delay);
        Console.WriteLine(phrase);
    }

    void AboutTask()
    {
        Console.WriteLine($"Task:{Task.CurrentId}");
    }
}
//SimpleUsingInvoke();


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

            Console.WriteLine(i);
            Thread.Sleep(500);
        }

    },cancellationToken);
    
    task.Start();

    Thread.Sleep(3000);

    cancellationTokenSource.Cancel();

    Thread.Sleep(500);

    Console.WriteLine($"Task status:{task.Status}");
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

            Console.WriteLine(i);
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
CancellationWithThrow();