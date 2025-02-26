using DisposalPattern;

static void UsingDisposalPatern()
{
    UsingDispose();
    UsingFinalizer();

    static void UsingDispose()
    {

        using MyResourceWrapper resource1 = new();

        Console.WriteLine("Use the members of resource.");
    }

    static void UsingFinalizer()
    {
        CreateObject();
        GC.Collect(0, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();

        static void CreateObject()
        {
            MyResourceWrapper resource = new();
        }
    }
}
UsingDisposalPatern();
