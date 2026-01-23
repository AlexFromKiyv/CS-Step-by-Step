using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

void MakeBreakastSync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tСoffee is ready");

    Egg eggs = FryEggs(2);
    Console.WriteLine("\tEggs are ready");

    HashBrown hashBrown = FryHashBrowns(3);
    Console.WriteLine("\tHash browns are ready");

    Toast toast = ToastBread(2);
    ApplyButter(toast);
    ApplyJam(toast);
    Console.WriteLine("\tToast is ready");

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");

}
//MakeBreakastSync();

async Task SimpleMakeBreakastAync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    Egg eggs = await FryEggsAsync(2);
    Console.WriteLine("\tEggs are ready");

    HashBrown hashBrown = await FryHashBrownsAsync(3);
    Console.WriteLine("\tHash browns are ready");

    Toast toast = await ToastBreadAsync(2);
    ApplyButter(toast);
    ApplyJam(toast);
    Console.WriteLine("\tToast is ready");

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
//await SimpleMakeBreakastAync();

async Task OtherSimpleMakeBreakastAync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    Task<Egg> eggsTask = FryEggsAsync(2);
    Egg eggs = await eggsTask;
    Console.WriteLine("\tEggs are ready");

    Task<HashBrown> hashBrownTask = FryHashBrownsAsync(3);
    HashBrown hashBrown = await hashBrownTask;
    Console.WriteLine("\tHash browns are ready");

    Task<Toast> toastTask = ToastBreadAsync(2);
    Toast toast = await toastTask;
    ApplyButter(toast);
    ApplyJam(toast);
    Console.WriteLine("\tToast is ready");

    Juice oj = PourOJ();
    Console.WriteLine("\tOj is ready");
    Console.WriteLine("\t\tBreakfast is ready!");

}
//await OtherSimpleMakeBreakastAync();

async Task FasterMakeBreakastAync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    Task<Egg> eggsTask = FryEggsAsync(2);
    Task<HashBrown> hashBrownTask = FryHashBrownsAsync(3);
    Task<Toast> toastTask = ToastBreadAsync(2);

    Toast toast = await toastTask;
    ApplyButter(toast);
    ApplyJam(toast);
    Console.WriteLine("\tToast is ready");
    
    Juice oj = PourOJ();
    Console.WriteLine("\tOj is ready");

    Egg eggs = await eggsTask;
    Console.WriteLine("\tEggs are ready");
    
    HashBrown hashBrown = await hashBrownTask;
    Console.WriteLine("\tHash browns are ready");

    Console.WriteLine("\t\tBreakfast is ready!");
}
//await FasterMakeBreakastAync();

//Toast otherToast = await MakeToastWithButterAndJamAsync(2);

async Task ImprovedFasterMakeBreakastAync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    var eggsTask = FryEggsAsync(2);
    var hashBrownTask = FryHashBrownsAsync(3);
    var toastTask = MakeToastWithButterAndJamAsync(2);

    var eggs = await eggsTask;
    Console.WriteLine("\tEggs are ready");

    var hashBrown = await hashBrownTask;
    Console.WriteLine("\tHash browns are ready");

    var toast = await toastTask;
    Console.WriteLine("\tToast is ready");

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
//await ImprovedFasterMakeBreakastAync();


async Task BreakastWithFireAync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    try
    {
        var eggsTask = FryEggsAsync(2);
        var hashBrownTask = FryHashBrownsAsync(3);
        var toastTask = MakeToastWithButterAndJamAsync(2);

        var eggs = await eggsTask;
        Console.WriteLine("\tEggs are ready");

        var hashBrown = await hashBrownTask;
        Console.WriteLine("\tHash browns are ready");

        var toast = await toastTask;
        Console.WriteLine("\tToast is ready");
    }catch(Exception ex)
    {
        Console.WriteLine(ex.Message.ToUpper());
    }

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
//await BreakastWithFireAync();


async Task BreakfastWithWhenAllAsync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    var eggsTask = FryEggsAsync(2);
    var hashBrownTask = FryHashBrownsAsync(3);
    var toastTask = MakeToastWithButterAndJamAsync(2);

    await Task.WhenAll(eggsTask, hashBrownTask, toastTask);
    Console.WriteLine("\tEggs are ready");
    Console.WriteLine("\tHash browns are ready");
    Console.WriteLine("\tToast is ready");

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
//await BreakfastWithWhenAllAsync();

async Task BreakfastWithWhenAnyAsync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    var eggsTask = FryEggsAsync(2);
    var hashBrownTask = FryHashBrownsAsync(3);
    var toastTask = MakeToastWithButterAndJamAsync(2);

    var breakfastTasks = new List<Task> { eggsTask, hashBrownTask, toastTask };
    while (breakfastTasks.Count > 0)
    {
        Task finishedTask = await Task.WhenAny(breakfastTasks);
        if (finishedTask == eggsTask)
        {
            Console.WriteLine("\tEggs are ready");
        }
        else if (finishedTask == hashBrownTask)
        {
            Console.WriteLine("\tHash browns are ready");
        }
        else if (finishedTask == toastTask)
        {
            Console.WriteLine("\tToast is ready");
        }
        await finishedTask;
        breakfastTasks.Remove(finishedTask);
    }

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
//await BreakfastWithWhenAnyAsync();

async Task MakeBreakfastAsync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    var eggsTask = FryEggsAsync(2);
    var hashBrownTask = FryHashBrownsAsync(3);
    var toastTask = MakeToastWithButterAndJamAsync(2);

    var breakfastTasks = new List<Task> { eggsTask, hashBrownTask, toastTask };
    while (breakfastTasks.Count > 0)
    {
        Task finishedTask = await Task.WhenAny(breakfastTasks);
        if (finishedTask == eggsTask)
        {
            Console.WriteLine("\tEggs are ready");
        }
        else if (finishedTask == hashBrownTask)
        {
            Console.WriteLine("\tHash browns are ready");
        }
        else if (finishedTask == toastTask)
        {
            Console.WriteLine("\tToast is ready");
        }
        await finishedTask;
        breakfastTasks.Remove(finishedTask);
    }

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
//await MakeBreakfastAsync();

long TimeOfSyncMethod()
{
    var watch = Stopwatch.StartNew();
    MakeBreakastSync();      
    watch.Stop();
    return watch.ElapsedMilliseconds;
}
async ValueTask<long> TimeOfAsyncMethod()
{
    var watch = Stopwatch.StartNew();
    await MakeBreakfastAsync();
    watch.Stop();
    return watch.ElapsedMilliseconds;
}

async Task CompareTime()
{
    double timeSyncMethod = TimeOfSyncMethod();
    double timeAsyncMethod = await TimeOfAsyncMethod();
    Console.WriteLine("\n\n");
    Console.WriteLine($"Sync  method time: {timeSyncMethod}");
    Console.WriteLine($"Async method time: {timeAsyncMethod}");
    Console.WriteLine($"\tCompare: {timeSyncMethod / timeAsyncMethod}");
}
//await CompareTime();
