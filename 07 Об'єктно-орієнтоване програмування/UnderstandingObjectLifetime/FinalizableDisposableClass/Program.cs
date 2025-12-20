using FinalizableDisposableClass;

static void UsingDisposeAndFinalizer()
{
    Console.WriteLine("\tUsingDispose");
    UsingDispose();
    Console.WriteLine("\n\tUsingFinalizer");
    UsingFinalizer();

    static void UsingDispose()
    {

        using MyResourceWrapper resource1 = new();

        Console.WriteLine("Use the members of resource.");
        resource1.Work();
    }

    static void UsingFinalizer()
    {
        CreateObject();
        GC.Collect(0, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();

        static void CreateObject()
        {
            MyResourceWrapper resource2 = new();
            resource2.Work();
        }
    }
}
UsingDisposeAndFinalizer();
