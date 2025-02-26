using SimpleDispose;

static void InvokeDispose()
{
    // Create a disposable object and call Dispose()
    // to free any internal resources.
    MyResourceWrapper myResource = new();
    myResource.Dispose();
}
//InvokeDispose();

static void InvokeDisposeWithChecking()
{
    MyResourceWrapper myResource = new();
    if (myResource is IDisposable)
    {
        myResource.Dispose();
    }
}
//InvokeDisposeWithChecking();

static void DisposeFileStream()
{
    FileStream fileStream = new FileStream("myFile.txt", FileMode.OpenOrCreate);
    // Confusing, to say the least!
    // These method calls do the same thing!
    fileStream.Close();
    fileStream.Dispose();
}

static void TryAndDispose()
{
    MyResourceWrapper resource = new();

    try
    {
        // Use the members of resource.
    }
    finally
    {
        resource.Dispose();
    }
}


static void KeywordUsing1()
{
    // Dispose() is called automatically when the using scope exits.
    using (MyResourceWrapper resource = new())
    {
        Console.WriteLine("Use the members of resource.");
    }
}
//KeywordUsing1();

static void KeywordUsing2()
{
    // Use a comma-delimited list to declare multiple objects to dispose.
    using (MyResourceWrapper resource1 = new MyResourceWrapper(),resource2 = new() )
    {
        Console.WriteLine("Use the members of resource1.");
        Console.WriteLine("Use the members of resource2.");
    }
}
//KeywordUsing2();

static void UsingDeclaration()
{
    using MyResourceWrapper resource = new();

    Console.WriteLine("Use the members of resource.");
}
UsingDeclaration();