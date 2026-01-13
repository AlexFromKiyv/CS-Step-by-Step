namespace SimpleMultiThreadApp;

public class Printer
{
    public int Counter { get; set; }
    public void PrintNumbers()
    {
        // Display Thread info.
        string threadInfo = $"{Thread.CurrentThread.Name}(id:{Thread.CurrentThread.ManagedThreadId})";
        Console.WriteLine($"{threadInfo} is executing PrintNumbers()");
        
        // Print out numbers.
        Console.Write("Your numbers: ");
        for (int i = 0; i < 10; i++)
        {
            Console.Write($"{Counter} ");
            Thread.Sleep(2000);
            Counter++;
        }
        Console.WriteLine();
    }
}
