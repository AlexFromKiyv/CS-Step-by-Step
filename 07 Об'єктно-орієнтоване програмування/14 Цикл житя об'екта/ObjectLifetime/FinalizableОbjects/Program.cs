
using FinalizableОbjects;

HowInvokeFinalize();
void HowInvokeFinalize()
{

    CreateObjects(10);
    //Artificially inflate the memory pressure
    GC.AddMemoryPressure(2147483647);
    GC.Collect(0, GCCollectionMode.Forced);
    GC.WaitForPendingFinalizers();



    void CreateObjects(int count)
    {
        MyResourceWrapper[]? tonsOfObjects = new MyResourceWrapper[count];

        for (int i = 0; i < count; i++)
        {
            tonsOfObjects[i] = new();
        }

        tonsOfObjects = null;
    }
}