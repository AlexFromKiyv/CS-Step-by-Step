using CommonSnappableTypes;
using System.Reflection;

static void Run()
{
    string typeName = string.Empty;
    do
    {
        Console.WriteLine("\tResearch your type.\n");
        Console.Write("Enter a spanin to load:");
        typeName = Console.ReadLine()!;

        try
        {
            LoadExternalModule(typeName);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.Clear();
    } while (true);
}
Run();

static void LoadExternalModule(string assemblyName)
{
    Console.Write($"\nFinding and loading the assembly {assemblyName}. ");
    Assembly? snapInAssembly = null;
    try
    {
        snapInAssembly = Assembly.LoadFrom(assemblyName);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    if (snapInAssembly != null)
    {
        Console.WriteLine("Ok. The assembly found.");
    }
    else
    {
        Console.WriteLine("Assembly is null");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("\nGet all IAppFunctionality compatible classes in assembly");

    List<Type> iAppFuncClasses = snapInAssembly
        .GetTypes()
        .Where(t => t.IsClass && t.GetInterface("IAppFunctionality") != null)
        .ToList();

    if (!iAppFuncClasses.Any())
    {
        Console.WriteLine("Nothing implements IAppFunctionality!");
        Console.ReadKey();
        return;
    }

    foreach (var type in iAppFuncClasses)
    {
        Console.WriteLine($"Here is type:{type}");
        // Use late binding to create the type.
        IAppFunctionality appFunc = (IAppFunctionality)snapInAssembly.CreateInstance(type.FullName!, true)!;
        appFunc.DoIt();
        DisplayCompanyData(type);
    }

    Console.ReadKey();
}

static void DisplayCompanyData(Type type)
{
    var companyInfos = type.GetCustomAttributes(false)
        .Where(ci => (ci is CompanyInfoAttribute));
    foreach (CompanyInfoAttribute ci in companyInfos)
    {
        Console.WriteLine($"\nCompany {ci.CompanyName} {ci.CompanyUrl}" );
    }
}