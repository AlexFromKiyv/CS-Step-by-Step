using System.Reflection;
using CommonSnappableTypes;

string? typeName = "";
do
{
    Console.Write("\nEnter a snapin to load:");
    // Get name of type.
    typeName = Console.ReadLine();
    Console.Clear();

    // Try to display type.
    try
    {
        LoadExternalModule(typeName);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Sorry, can't find snapin");
    }

} while (true);

void LoadExternalModule(string? assemblyName)
{
    Console.WriteLine($"\tI use {assemblyName}");

    Assembly theSnapInAsm = null!;

    try
    {
        // Dynamically load the selected assembly.
        theSnapInAsm = Assembly.LoadFrom(assemblyName);
        Console.WriteLine($"\tI loded assembly: {theSnapInAsm} loded");

        // Get all IAppFunctionality compatible classes in assembly.
        var theClassTypes =
            theSnapInAsm.GetTypes()
            .Where(t => t.IsClass && (t.GetInterface("IAppFunctionality") != null))
            .ToList();
        if (!theClassTypes.Any())
        {
            Console.WriteLine("Nothing implements IAppFunctionality!");
        }
        // Now, create the object and call DoIt() method.
        foreach (var type in theClassTypes)
        {
            Console.WriteLine($"\tI find class {type}");
            // Use late binding to create the type.
            IAppFunctionality itfApp = (IAppFunctionality)theSnapInAsm.CreateInstance(type.FullName, true);
            itfApp?.DoIt();

            // Show company info.
            DisplayCompanyData(type);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred loading the snapin: {ex.Message}");
        return;
    }
}

void DisplayCompanyData(Type type)
{
    // Get [CompanyInfo] data.
    var compInfo = type
        .GetCustomAttributes(false)
        .Where(ci => (ci is CompanyInfoAttribute));

    // Show data.
    foreach (CompanyInfoAttribute c in compInfo)
    {
        Console.WriteLine($"More info about {c.CompanyName} can be found at {c.CompanyUrl}");
    }
}