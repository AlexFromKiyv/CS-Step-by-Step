using SimpleGC;

static void CreateInstanceOfType()
{
    // Create a new Car object on the managed heap.
    // We are returned a reference to the object 
    Car refToMyCar = new Car("Zippy",50);

    // The C# dot operator (.) is used to invoke members
    // on the object using our reference variable.
    Console.WriteLine(refToMyCar.CurrentSpeed);
    Console.WriteLine(refToMyCar.ToString());
}
//CreateInstanceOfType();

static void MakeACar()
{
    // If myCar is the only reference to the Car object,
    // it *may* be destroyed when this method returns.

    Car myCar = new();
    myCar = null;
}

void MemebersSystemGC()
{
    Console.WriteLine($"Estimated bytes on heap: {GC.GetTotalMemory(false)}");
    Console.WriteLine($"This OS has {GC.MaxGeneration+1} object generations.");
    Car car = new("Zippy", 50);
    Console.WriteLine(car);
    Console.WriteLine($"Generation of car is: {GC.GetGeneration(car)}");
}
//MemebersSystemGC();

void ForcingAGarbageCollection()
{
    Console.WriteLine($"Estimated bytes on heap: {GC.GetTotalMemory(false)}");
    Console.WriteLine($"This OS has {GC.MaxGeneration + 1} object generations.");
    Car car = new("Zippy", 50);
    Console.WriteLine(car);
    Console.WriteLine($"Generation of car is: {GC.GetGeneration(car)}");
    Console.WriteLine();

    // Make a ton of objects for testing purposes.
    object[] tonsOfObjects = new object[50000];
    for (int i = 0; i < 50000; i++)
    {
        tonsOfObjects[i] = new object();
    }

    // Collect only gen 0 objects.
    Console.WriteLine("Force Garbage Collection");
    GC.Collect(0, GCCollectionMode.Forced);
    GC.WaitForPendingFinalizers();
    Console.WriteLine();

    // Print out generation of car.
    Console.WriteLine($"Generation of car is: {GC.GetGeneration(car)}");

    // See if tonsOfObjects[9000] is still alive.
    if (tonsOfObjects[9000]!=null)
    {
        Console.WriteLine($"Generation of tonsOfObjects[9000] is: " +
            $"{GC.GetGeneration(tonsOfObjects[9000])}");
    }
    else
    {
        Console.WriteLine("tonsOfObjects[9000] is no longer alive.");
    }
    Console.WriteLine();

    // Print out how many times a generation has been swept.
    Console.WriteLine($"Gen 0 has been swept {GC.CollectionCount(0)} times");
    Console.WriteLine($"Gen 1 has been swept {GC.CollectionCount(1)} times");
    Console.WriteLine($"Gen 2 has been swept {GC.CollectionCount(2)} times");
    Console.WriteLine();

    Console.WriteLine($"Estimated bytes on heap: {GC.GetTotalMemory(false)}");
}
ForcingAGarbageCollection();