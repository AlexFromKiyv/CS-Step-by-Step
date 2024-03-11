using System.Collections;
using System.Reflection;

void StartViewer()
{
	string enteredType = string.Empty;
	do
	{	//Get name of type.
        Console.Clear();
        Console.Write("Enter Type or q:");
		enteredType = Console.ReadLine()!;

		if (enteredType.Equals("Q",StringComparison.OrdinalIgnoreCase))
		{
			break;
		}
		Console.Clear();	
		InvestigateTheType(enteredType);
        Console.ReadKey();
    } while (true);
}
StartViewer();


void InvestigateTheType(string typeName)
{
    Type? type = Type.GetType(typeName);
    if (type == null)
    {
        Console.WriteLine("Sorry, can't find type!");
        return;
    }
    Console.WriteLine($"We want to investigate the type:{type.FullName}");

    //AboutType(type);
    //ListFilds(type);
    //ListProperties(type);
    ListMethods(type);
}

void AboutType(Type type)
{
    Console.WriteLine();
    Console.WriteLine($"Is type class:{type.IsClass}");
    Console.WriteLine($"Is type abstract:{type.IsAbstract}");
    Console.WriteLine($"Is type generic:{type.IsGenericType}");
    Console.WriteLine($"Is type sealed:{type.IsSealed}");
    Console.WriteLine($"Base type:{type.BaseType}");
}

void ListFilds(Type type)
{
    Console.WriteLine("\nFilds");

    var filds = from f in type.GetFields()
                orderby f.Name
                select f;
    foreach (var item in filds)
    {
        Console.WriteLine("\t"+item.Name);
    }  
}

void ListProperties(Type type)
{
    Console.WriteLine("\nProperties");

    var properties = from p in type.GetProperties()
                     orderby p.Name
                     select p;
    foreach (var p in properties)
    {
        Console.WriteLine("\t"+p.Name);
    }
}

void ListMethods(Type type)
{
    Console.WriteLine("Methods");

    var methods =from t in type.GetMethods()
                 orderby t.Name
                 select t;
    
    foreach (MethodInfo method in methods)
    {
        Console.WriteLine($"\t{method.Name}");
    }
}

