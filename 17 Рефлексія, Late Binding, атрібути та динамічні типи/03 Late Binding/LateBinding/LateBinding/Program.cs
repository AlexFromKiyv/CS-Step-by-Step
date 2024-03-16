// This program will load an external library,
// and create an object using late binding.
using System.Reflection;

void Run()
{
    Assembly? assembly = null;
    try
    {
        assembly = Assembly.LoadFrom(@"D:\CarLibrary");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    if (assembly != null)
    {
        CreateUsingLateBinding(assembly);
    }

    void CreateUsingLateBinding(Assembly assembly)
    {
        object? obj;
        try
        {
            Type? miniVan = assembly.GetType("CarLibrary.MiniVan");
            if (miniVan != null)
            {
                // Create object
                obj = Activator.CreateInstance(miniVan);
                Console.WriteLine($"Created a {obj} using late binding!");

                //Invoke method without parameters
                MethodInfo? methodInfoTurboBoost = miniVan.GetMethod("TurboBoost");
                methodInfoTurboBoost?.Invoke(obj, null);

                //Invoke method with parameters
                MethodInfo? methodInfoTurnOnRadio = miniVan.GetMethod("TurnOnRadio");
                methodInfoTurnOnRadio?.Invoke(obj, new object[] {true,2});

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
Run();
