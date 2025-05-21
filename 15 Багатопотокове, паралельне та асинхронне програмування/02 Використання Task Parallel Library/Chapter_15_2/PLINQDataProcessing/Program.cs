using System.Diagnostics;
using System.Threading;

void UsingPLINQ()
{
    Console.WriteLine("Processing");
    Task.Factory.StartNew(ProcessingIntData);
    //Task.Factory.StartNew(ProcessingIntDataWithPLINQ);
    Console.ReadLine();


    void ProcessingIntData()
    {
        // Get a very large array of integers.
        int[] ints = Enumerable.Range(0, 50_000_000).ToArray();
        // Find the numbers where num % 3 == 0 is true, returned
        // in descending order.

        var query = from number in ints
                    where number % 3 == 0
                    orderby number descending
                    select number;

        var watch = Stopwatch.StartNew();

        int[] modThreeIsZero = query.ToArray();

        watch.Stop();
        Console.WriteLine($"Time:{watch.ElapsedMilliseconds}");
        Console.WriteLine($"Found {modThreeIsZero.Count()} numbers that match query!");
    }

    void ProcessingIntDataWithPLINQ()
    {
        // Get a very large array of integers.
        int[] ints = Enumerable.Range(0, 50_000_000).ToArray();
        // Find the numbers where num % 3 == 0 is true, returned
        // in descending order.

        var query = from number in ints.AsParallel()
                    where number % 3 == 0
                    orderby number descending
                    select number;

        var watch = Stopwatch.StartNew();

        int[] modThreeIsZero = query.ToArray();

        watch.Stop();
        Console.WriteLine($"With Paralell\nTime:{watch.ElapsedMilliseconds}");
        Console.WriteLine($"Found {modThreeIsZero.Count()} numbers that match query!");
    }

}
//UsingPLINQ();

void UsingPLINQWithCancellation()
{
    CancellationTokenSource _cancelToken = new CancellationTokenSource();
    do
    {
        Console.WriteLine("Start any key to start processing");
        Console.ReadKey();
        Console.WriteLine("Processing");
        Task.Factory.StartNew(ProcessingIntDataWithPLINQAndCancellation);
        Console.Write("Enter Q to quit: ");
        string? answer = Console.ReadLine();
        // Does user want to quit?
        if (answer != null && answer.Equals("Q",StringComparison.OrdinalIgnoreCase))
        {
            _cancelToken.Cancel();
            break;
        }
    }
    while (true);

    Console.ReadLine();

    void ProcessingIntDataWithPLINQAndCancellation()
    {

        int[] ints = Enumerable.Range(0, 50_000_000).ToArray();

        var query = from number in ints.AsParallel().WithCancellation(_cancelToken.Token)
                    where number % 3 == 0
                    orderby number descending
                    select number;

        try
        {
            var watch = Stopwatch.StartNew();
            Thread.Sleep(1000);
            int[] modThreeIsZero = query.ToArray();

            watch.Stop();
            Console.WriteLine($"\nWith Paralell\nTime:{watch.ElapsedMilliseconds}");
            Console.WriteLine($"\tAmount:{modThreeIsZero.Count()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
UsingPLINQWithCancellation();