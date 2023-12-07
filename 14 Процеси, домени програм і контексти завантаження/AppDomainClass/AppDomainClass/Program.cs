using System.Reflection;
using System.Runtime.Loader;

static void InvestigationAppDomain()
{
    AppDomain appDomain = AppDomain.CurrentDomain;

    Console.WriteLine($"FriendlyName: {appDomain.FriendlyName}");
    Console.WriteLine($"Id: {appDomain.Id}");
    Console.WriteLine($"IsDefaultAppDomain: {appDomain.IsDefaultAppDomain()}");
    Console.WriteLine($"BaseDirectory: {appDomain.BaseDirectory}");
    Console.WriteLine($"SetupInformation.ApplicationBase: {appDomain.SetupInformation.ApplicationBase}");
    Console.WriteLine($"SetupInformation.TargetFrameworkName: {appDomain.SetupInformation.TargetFrameworkName}");
}
//InvestigationAppDomain();

void GetAssemliesOfAppDomain()
{
    AppDomain appDomain = AppDomain.CurrentDomain;

    Assembly[] assemblies = appDomain.GetAssemblies();

    Console.WriteLine($"Assemlies of {appDomain.FriendlyName}\n");

    foreach (var assembly in assemblies)
    {
        Console.WriteLine($"{assembly.GetName().Name} {assembly.GetName().Version}");
    }
  
}
//GetAssemliesOfAppDomain();

void GetAssemliesOfAppDomainWithChange()
{
    AppDomain appDomain = AppDomain.CurrentDomain;

    Assembly[] assemblies = appDomain.GetAssemblies();

    Array.Reverse(assemblies);

    Console.WriteLine($"\nAssemlies of {appDomain.FriendlyName}\n");

    foreach (var assembly in assemblies)
    {
        Console.WriteLine($"{assembly.GetName().Name} {assembly.GetName().Version}");
    }
}
//GetAssemliesOfAppDomain();
//GetAssemliesOfAppDomainWithChange();

static void LoadAdditionalAssembliesDifferentContexts()
{
    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyLibrary.dll");
  
    AssemblyLoadContext assemblyLoadContext1 = new("NewContext1", false);
    var assembly1 = assemblyLoadContext1.LoadFromAssemblyPath(path);
    var class1 = assembly1.CreateInstance("MyLibrary.Car");

    AssemblyLoadContext assemblyLoadContext2 = new("NewContext2", false);
    var assembly2 = assemblyLoadContext2.LoadFromAssemblyPath(path);
    var class2 = assembly2.CreateInstance("MyLibrary.Car");

    Console.WriteLine($"assembly1.Equals(assembly2) : {assembly1.Equals(assembly2)}");
    Console.WriteLine($"assembly1 == assembly2 : {assembly1 == assembly2}");
   
    Console.WriteLine($"class1.Equals(class2) : {class1?.Equals(class2)}");
    Console.WriteLine($"class1 == class2 : {class1 == class2}");
}

//LoadAdditionalAssembliesDifferentContexts();


static void LoadAdditionalAssembliesSameContext()
{
    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyLibrary.dll");

    AssemblyLoadContext assemblyLoadContext1 = new(null, false);
    var assembly1 = assemblyLoadContext1.LoadFromAssemblyPath(path);
    var class1 = assembly1.CreateInstance("MyLibrary.Car");
       
    var assembly2 = assemblyLoadContext1.LoadFromAssemblyPath(path);
    var class2 = assembly2.CreateInstance("MyLibrary.Car");

    Console.WriteLine($"assembly1.Equals(assembly2) : {assembly1.Equals(assembly2)}");
    Console.WriteLine($"assembly1 == assembly2 : {assembly1 == assembly2}");

    Console.WriteLine($"class1.Equals(class2) : {class1?.Equals(class2)}");
    Console.WriteLine($"class1 == class2 : {class1 == class2}");
}

//LoadAdditionalAssembliesSameContext();