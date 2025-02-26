namespace FinalizableDisposableClass;

// A sophisticated resource wrapper.
public class MyResourceWrapper : IDisposable
{
    // The garbage collector will call this method
    // if the object user forgets to call Dispose().
    ~MyResourceWrapper()
    {
        // Clean up any internal unmanaged resources.
        // Do **not** call Dispose() on any managed objects.
        Console.Beep();
    }

    // The object user will call this method
    // to clean up resources ASAP.
    public void Dispose()
    {
        // Clean up unmanaged resources here.
        // Call Dispose() on other contained disposable objects.
        // No need to finalize if user called Dispose(), so suppress finalization.
        Console.WriteLine("I'm cleaning up unmanaged resources in Dispose.");
        GC.SuppressFinalize(this);
    }
}
