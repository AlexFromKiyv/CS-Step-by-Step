using System.Reflection;

void Start()
{
    string? assemblyName = "";
    Assembly? assembly = null;
    do
    {
        Console.Clear();
        Console.Write("Enter the name of the assembly to evaluate: ");
        assemblyName = Console.ReadLine();

        if (assemblyName == null) assemblyName = "";

        try
        {

            assembly = Assembly.LoadFrom(assemblyName);
            WriteTypesInAssembly(assembly);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadKey();
        }

    } while (true);
}
//Start();

void WriteTypesInAssembly(Assembly assembly)
{
    Console.WriteLine($"Assembly name:{assembly.FullName}");

    Type[] types = assembly.GetTypes();
    foreach (Type type in types)
    {
        Console.WriteLine(type);
    }

    Console.ReadKey();
}

void UseAsseblyLoad()
{
    try
    {
        //Assembly? assembly = Assembly.Load("CarLibrary, Version=1.0.0.1");
        AssemblyName assemblyName = new()
        {
            Name = "CarLibrary",
            Version = new Version("1.0.0.1"),
        };

        Assembly? assembly = Assembly.Load(assemblyName);

        WriteTypesInAssembly(assembly);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//UseAsseblyLoad();