
using DisposableObjects;
UsingDisposableObjects();
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
