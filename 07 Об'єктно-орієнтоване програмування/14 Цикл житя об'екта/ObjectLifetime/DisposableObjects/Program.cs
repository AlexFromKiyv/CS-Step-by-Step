
using DisposableObjects;
//UsingDisposableObjects();
void UsingDisposableObjects()
{
    MyResourceWrapper myResourceWrapper = new MyResourceWrapper();

    // Use object.
    myResourceWrapper.Resource = "Here we are using unmanaged resource...";
    Console.WriteLine(myResourceWrapper.Resource);

    // Clean up.
    if (myResourceWrapper is IDisposable)
    {
        myResourceWrapper.Dispose();
    }

}

//DisposeInLibraries();
void DisposeInLibraries()
{
    FileStream fileStream = new("MyFile.txt", FileMode.OpenOrCreate);

    fileStream.Close();
    fileStream.Dispose();

}

//DisposeAndTry();
void DisposeAndTry()
{
    MyResourceWrapper myResource = new MyResourceWrapper();

    try
    {
        myResource.Resource = "My big resource";
        Console.WriteLine(myResource.Resource);
    }
    finally
    {
        myResource.Dispose();
    } 
}

//DisposeAndUsing();
void DisposeAndUsing()
{
    using(MyResourceWrapper myResource = new MyResourceWrapper()) 
    {
        myResource.Resource = "My big resource";
        Console.WriteLine(myResource.Resource);
    }
}

//DisposeAndUsingAndTwoVariable();
void DisposeAndUsingAndTwoVariable()
{
    using (MyResourceWrapper myResource1 = new(), myResource2 = new())
    {
        myResource1.Resource = "My big resource 1";
        Console.WriteLine(myResource1.Resource);

        myResource2.Resource = "My big resource 2 ";
        Console.WriteLine(myResource2.Resource);
    }
}

UsingAsDeclatation();
void UsingAsDeclatation()
{
    using MyResourceWrapper myResource = new();
    myResource.Resource = "Hi resource.";
    Console.WriteLine(myResource.Resource);
}