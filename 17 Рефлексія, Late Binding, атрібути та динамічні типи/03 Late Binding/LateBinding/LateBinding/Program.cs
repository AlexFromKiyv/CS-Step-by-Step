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
                obj = Activator.CreateInstance(miniVan);
                Console.WriteLine($"Created a {obj} using late binding!");

                MethodInfo? methodInfo = miniVan.GetMethod("TurboBoost");
                methodInfo?.Invoke(obj, null);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
Run();
