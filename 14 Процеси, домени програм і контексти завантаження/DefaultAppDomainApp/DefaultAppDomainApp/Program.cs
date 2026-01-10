using ClassLibrary1;
using System.Reflection;
using System.Runtime.Loader;

static void DisplayDefaulAppDomainStats()
{
    // Get access to the AppDomain for the current thread.
    AppDomain defaultAD = AppDomain.CurrentDomain;

    // Print out various stats about this domain.
    Console.WriteLine($"Name of this domain: {defaultAD.FriendlyName}");
    Console.WriteLine($"ID of domain in this process: {defaultAD.Id}");
    Console.WriteLine($"Is this the default domain?: {defaultAD.IsDefaultAppDomain()}");
    Console.WriteLine($"Base directory of this domain: {defaultAD.BaseDirectory}");
    Console.WriteLine($"Setup Information for this domain:");
    Console.WriteLine($"\tApplication Base: {defaultAD.SetupInformation.ApplicationBase}");
    Console.WriteLine($"\tTarget Framework: {defaultAD.SetupInformation.TargetFrameworkName}");
}
//DisplayDefaulAppDomainStats();

static void ListAllAssembliesInAppDomain()
{
    // Get access to the AppDomain for the current thread.
    AppDomain defaultAD = AppDomain.CurrentDomain;

    // Now get all loaded assemblies in the default AppDomain.
    //var loadedAssemblies = defaultAD.GetAssemblies();
    var loadedAssemblies = defaultAD.GetAssemblies().OrderBy(a => a.GetName().Name);
    
    Console.WriteLine($"***** Here are the assemblies loaded in {defaultAD.FriendlyName} *****\n");
    foreach (Assembly a in loadedAssemblies)
    {
        Console.WriteLine($"\t{a.GetName().Name}:{a.GetName().Version}");
    }
}
ListAllAssembliesInAppDomain();

static void LoadAdditionalAssembliesDifferentContexts()
{
    string? path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassLibrary1.dll");
    AssemblyLoadContext lc1 = new AssemblyLoadContext("NewContext1", false);
    Assembly assembly1 = lc1.LoadFromAssemblyPath(path);
    object? car1 = assembly1.CreateInstance("ClassLibrary1.Car");
    AssemblyLoadContext lc2 = new AssemblyLoadContext("NewContext2", false);
    Assembly assembly2 = lc2.LoadFromAssemblyPath(path);
    object? car2 = assembly2.CreateInstance("ClassLibrary1.Car");
    Console.WriteLine("*** Loading Additional Assemblies in Different Contexts ***");
    Console.WriteLine($"Assembly1.Equals(Assembly2) {assembly1.Equals(assembly2)}");
    Console.WriteLine($"Assembly1 == Assembly2 {assembly1 == assembly2}");
    Console.WriteLine($"Class1.Equals(Class2) {car1.Equals(car2)}");
    Console.WriteLine($"Class1 == Class2 {car1 == car2}");
}
//LoadAdditionalAssembliesDifferentContexts();

static void LoadAdditionalAssembliesSameContext()
{
    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassLibrary1.dll");
    AssemblyLoadContext lc1 = new AssemblyLoadContext(null, false);
    var cl1 = lc1.LoadFromAssemblyPath(path);
    var c1 = cl1.CreateInstance("ClassLibrary1.Car");
    var cl2 = lc1.LoadFromAssemblyPath(path);
    var c2 = cl2.CreateInstance("ClassLibrary1.Car");
    Console.WriteLine("*** Loading Additional Assemblies in Same Context ***");
    Console.WriteLine($"Assembly1.Equals(Assembly2) {cl1.Equals(cl2)}");
    Console.WriteLine($"Assembly1 == Assembly2 {cl1 == cl2}");
    Console.WriteLine($"Class1.Equals(Class2) {c1.Equals(c2)}");
    Console.WriteLine($"Class1 == Class2 {c1 == c2}");
}
//LoadAdditionalAssembliesSameContext();

