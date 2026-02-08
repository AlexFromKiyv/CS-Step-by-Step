using System.Reflection;

const string FILEPATH = @"D:\Temp\CarLibrary";
void Run()
{
    Assembly assembly = Assembly.LoadFrom(FILEPATH);
    WriteTypesInAssembly(assembly);
}
//Run();

void WriteTypesInAssembly(Assembly assembly)
{
    Console.WriteLine($"Assembly name:{assembly.FullName}");

    Type[] types = assembly.GetTypes();
    foreach (Type type in types)
    {
        Console.WriteLine(type);
    }
}

void UseAssemblyLoad()
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
UseAssemblyLoad();