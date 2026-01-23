public partial class Program
{
    // These classes are intentionally empty for the purpose of this example.
    // They are simply marker classes for the purpose of demonstration,
    // contain no properties, and serve no other purpose.
    internal class HashBrown { }
    internal class Coffee { }
    internal class Egg { }
    internal class Juice { }
    internal class Toast { }


    private static Coffee PourCoffee()
    {
        Console.WriteLine("Pouring coffee");
        return new Coffee();
    }

    private static Egg FryEggs(int howMany)
    {
        Console.WriteLine("Starting fry eggs. Warming the egg pan...");
        Task.Delay(3000).Wait();
        Console.WriteLine($"cracking {howMany} eggs");
        Console.WriteLine("cooking the eggs ...");
        Task.Delay(3000).Wait();
        Console.WriteLine("Put eggs on plate");

        return new Egg();
    }

    private static HashBrown FryHashBrowns(int patties)
    {
        Console.WriteLine($"Starting fry hash browns. putting {patties} hash brown patties in the pan");
        Console.WriteLine("cooking first side of hash browns...");
        Task.Delay(3000).Wait();
        for (int patty = 0; patty < patties; patty++)
        {
            Console.WriteLine("flipping a hash brown patty");
        }
        Console.WriteLine("cooking the second side of hash browns...");
        Task.Delay(3000).Wait();
        Console.WriteLine("Put hash browns on plate");

        return new HashBrown();
    }

    private static Toast ToastBread(int slices)
    {
        Console.WriteLine("Start toasting...");
        for (int slice = 0; slice < slices; slice++)
        {
            Console.WriteLine("Putting a slice of bread in the toaster");
        }
        Task.Delay(3000).Wait();
        Console.WriteLine("Remove toast from toaster");

        return new Toast();
    }
    private static void ApplyJam(Toast toast) =>
    Console.WriteLine("Putting jam on the toast");
    private static void ApplyButter(Toast toast) =>
        Console.WriteLine("Putting butter on the toast");

    private static Juice PourOJ()
    {
        Console.WriteLine("Pouring orange juice");
        return new Juice();
    }

    //Async versions
    private static async Task<Egg> FryEggsAsync(int howMany)
    {
        Console.WriteLine("Starting fry eggs. Warming the egg pan...");
        await Task.Delay(3000);
        Console.WriteLine($"cracking {howMany} eggs");
        Console.WriteLine("cooking the eggs ...");
        await Task.Delay(3000);
        Console.WriteLine("Put eggs on plate");

        return new Egg();
    }

    private static async Task<HashBrown> FryHashBrownsAsync(int patties)
    {
        Console.WriteLine($"Starting fry hash browns. putting {patties} hash brown patties in the pan");
        Console.WriteLine("cooking first side of hash browns...");
        await Task.Delay(3000);
        for (int patty = 0; patty < patties; patty++)
        {
            Console.WriteLine("flipping a hash brown patty");
        }
        Console.WriteLine("cooking the second side of hash browns...");
        await Task.Delay(3000);
        Console.WriteLine("Put hash browns on plate");

        return new HashBrown();
    }
    private static async Task<Toast> ToastBreadAsync(int slices)
    {
        Console.WriteLine("Start toasting...");
        for (int slice = 0; slice < slices; slice++)
        {
            Console.WriteLine("Putting a slice of bread in the toaster");
        }
        await Task.Delay(3000);
        Console.WriteLine("Remove toast from toaster");

        return new Toast();
    }

    private static async Task<Toast> ToastBreadWithFireAsync(int slices, bool onFire)
    {
        Console.WriteLine("Start toasting...");
        for (int slice = 0; slice < slices; slice++)
        {
            Console.WriteLine("Putting a slice of bread in the toaster");
        }
        await Task.Delay(2000);
        if (onFire)
        {
            Console.WriteLine("Fire! Toast is ruined!");
            throw new InvalidOperationException("The toaster is on fire");
        }
        await Task.Delay(1000);
        Console.WriteLine("Remove toast from toaster");
        return new Toast();
    }

    static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
    {
        var toast = await ToastBreadWithFireAsync(number,false);
        ApplyButter(toast);
        ApplyJam(toast);

        return toast;
    }

}
