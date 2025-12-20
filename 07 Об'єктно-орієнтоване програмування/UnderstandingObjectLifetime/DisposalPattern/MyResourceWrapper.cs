namespace DisposalPattern;

public class MyResourceWrapper : IDisposable
{
    // Used to determine if Dispose() has already been called.
    private bool disposed = false;

    private void CleanUp(bool disposing)
    {
        // Be sure we have not already been disposed!
        if (!disposed)
        {
            // If disposing equals true, dispose all managed resources.
            if (disposing)
            {
                // Dispose managed resources.
                Console.WriteLine("Dispose managed resources.");
            }
            // Clean up unmanaged resources here.
            Console.WriteLine("Clean up unmanaged resources here");
        }
        disposed = true;
    }

    ~MyResourceWrapper()
    {
        // Call our helper method.
        // Specifying 'false' signifies that the GC triggered the cleanup.
        Console.Beep();
        CleanUp(false);
    }

    public void Work()
    {
        Console.WriteLine("I'm object MyResourceWrapper. I'm working with unmanaged resources");
    }

    public void Dispose()
    {
        // Call our helper method.
        // Specifying 'true' signifies that the object user triggered the cleanup.
        CleanUp(true);
        // Now suppress finalization.
        GC.SuppressFinalize(this);
    }
}
