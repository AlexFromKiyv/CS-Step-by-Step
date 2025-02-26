namespace SimpleFinalize;

class MyResourceWrapper
{
    // Override System.Object.Finalize() via finalizer syntax.
    ~MyResourceWrapper()
    {
        // Clean up unmanaged resources here.

        // Beep when destroyed (testing purposes only!)
        Console.Beep();
    }
}
