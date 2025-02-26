using SimpleFinalize;

void FinalizerExecutes()
{
    Console.WriteLine("Hit return to create the objects ");
    Console.WriteLine("then force the GC to invoke Finalize()");
    Console.ReadLine();
    CreateObject(10);
    //Artificially inflate the memory pressure
    GC.AddMemoryPressure(2147483647);
    GC.Collect(0, GCCollectionMode.Forced);
    GC.WaitForPendingFinalizers();

    static void CreateObject(int count)
    {
        MyResourceWrapper[]? tonsOfObjects = new MyResourceWrapper[count];
        for (int i = 0; i < count; i++)
        {
            tonsOfObjects[i] = new MyResourceWrapper();
        }
        //tonsOfObjects = null;
    }
}
FinalizerExecutes();


