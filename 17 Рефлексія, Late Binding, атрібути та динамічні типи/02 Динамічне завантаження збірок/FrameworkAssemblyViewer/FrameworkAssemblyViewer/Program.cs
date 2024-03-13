using System.Reflection;

Assembly assembly = Assembly.Load("Microsoft.EntityFrameworkCore");

AssemblyName assemblyName = assembly.GetName(); 

Console.WriteLine(assemblyName.Name );
Console.WriteLine(assemblyName.FullName);
Console.WriteLine(assemblyName.Version);
Console.WriteLine(assemblyName.CultureInfo?.DisplayName);

Console.ReadKey();

var types = assembly.GetTypes().Where(t => t.IsPublic);

foreach (Type type in types)
{
    Console.WriteLine(type);
}



