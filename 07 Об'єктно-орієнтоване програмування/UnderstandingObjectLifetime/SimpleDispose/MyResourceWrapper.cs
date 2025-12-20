namespace SimpleDispose;

class MyResourceWrapper : IDisposable
{
    public void Work()
    {
        Console.WriteLine("I'm object MyResourceWrapper. I'm working with unmanaged resources");
    }

    // The object user should call this method
    // when they finish with the object.
    public void Dispose()
    {
        // Clean up unmanaged resources...
        // Dispose other contained disposable objects...
        // Just for a test.
        Console.WriteLine("I'm cleaning up unmanaged resources in Dispose.");
    }
}
