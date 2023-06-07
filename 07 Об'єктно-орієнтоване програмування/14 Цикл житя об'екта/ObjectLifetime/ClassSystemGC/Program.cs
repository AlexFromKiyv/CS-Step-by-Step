using ClassSystemGC;

//UsingSystemGC();
void UsingSystemGC()
{
    Car car = new();

    Console.WriteLine($"Estimated bytes on heap {GC.GetTotalMemory(false)}");
    Console.WriteLine($"This OS has {GC.MaxGeneration+1} object generations."  );
    Console.WriteLine($"Generation of car is:{GC.GetGeneration(car)}");
}

//ForcingGC();
void ForcingGC()
{

    Console.WriteLine($"Estimated bytes on heap {GC.GetTotalMemory(false)}");
    Car car = new("VW Beetle",140,20);
    Console.WriteLine($"Generation of car is:{GC.GetGeneration(car)}");

    for (int i = 0; i < 10; i++)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine($"Estimated bytes on heap {GC.GetTotalMemory(false)}");
        Console.WriteLine($"Generation of car is:{GC.GetGeneration(car)}");
    }

}

//ForcingGCWithGenetation();
void ForcingGCWithGenetation()
{
    Console.WriteLine($"Estimated bytes on heap {GC.GetTotalMemory(false)}");

    Car[] cars = new Car[1000000];
    for (int i = 0; i < 1000000; i++)
    {
        cars[i] = new("VW",i,i);
    }


    Console.WriteLine($"Estimated bytes on heap {GC.GetTotalMemory(false)}");
    Console.WriteLine($"Generation of cars is:{GC.GetGeneration(cars)}");

    GC.Collect(0);
    GC.WaitForPendingFinalizers();

    Console.WriteLine($"Estimated bytes on heap {GC.GetTotalMemory(false)}");
    Console.WriteLine($"Generation of cars is:{GC.GetGeneration(cars)}");

}

ForcingGCWithGenetationAndMode();
void ForcingGCWithGenetationAndMode() 
{
    Car car = new Car();
    Console.WriteLine($"Generation of car is:{GC.GetGeneration(car)}");

    object[] tonsOfObjects = new object[50000];

    for (int i = 0; i < tonsOfObjects.Length; i++)
    {
        tonsOfObjects[i] = new();
    }

    long before = GC.GetTotalMemory(false);

    Console.WriteLine($"Estimated bytes on heap before: {before}");

    //GC.Collect(0, GCCollectionMode.Default);
    //GC.Collect(0, GCCollectionMode.Forced);
    //GC.Collect(0, GCCollectionMode.Optimized);
    GC.Collect(2, GCCollectionMode.Aggressive, true, true);
    GC.WaitForPendingFinalizers();

    long after = GC.GetTotalMemory(false);

    Console.WriteLine($"Estimated bytes on heap after: {after}");

    Console.WriteLine($"before - after : {before - after}");

    

    Console.WriteLine($"Generation of car is:{GC.GetGeneration(car)}");

    Console.WriteLine($"Generation of tonsOfObjects[9000] is:{GC.GetGeneration(tonsOfObjects[9000])}");

    Console.WriteLine($"Generation 0 has ben swapt: {GC.CollectionCount(0)}");
    Console.WriteLine($"Generation 1 has ben swapt: {GC.CollectionCount(1)}"); 
    Console.WriteLine($"Generation 2 has ben swapt: {GC.CollectionCount(2)}");

}
