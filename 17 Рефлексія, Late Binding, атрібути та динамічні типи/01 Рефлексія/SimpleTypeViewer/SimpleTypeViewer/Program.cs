using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Xml;

static void Run()
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
Run();


static void InvestigateTheType(string typeName)
{
    Type? type = Type.GetType(typeName);
    if (type == null)
    {
        Console.WriteLine("Sorry, can't find type!");
        return;
    }
    Console.WriteLine($"We want to investigate the type:{type.FullName}");

    AboutType(type);
    ListFilds(type);
    ListProperties(type);
    ListMethods(type);
    ListInterfaces(type);
}

static void AboutType(Type type)
{
    Console.WriteLine();
    Console.WriteLine($"Is type class:{type.IsClass}");
    Console.WriteLine($"Is type abstract:{type.IsAbstract}");
    Console.WriteLine($"Is type generic:{type.IsGenericType}");
    Console.WriteLine($"Is type sealed:{type.IsSealed}");
    Console.WriteLine($"Base type:{type.BaseType}");
}

static void ListFilds(Type type)
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

static void ListProperties(Type type)
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


static void ListMethods(Type type)
{
    Console.WriteLine("Methods");

    var methods =from t in type.GetMethods()
                 orderby t.Name
                 select t;

    //// Example 1

    //foreach (MethodInfo method in methods)
    //{
    //    Console.WriteLine($"\t{method.Name}");
    //}

    //// Example 2

    //foreach (MethodInfo methodInfo in methods)
    //{
    //    //AboutMethod(methodInfo);
    //    Console.WriteLine("\t" + methodInfo);
    //}
}

static void AboutMethod(MethodInfo methodInfo)
{
    string? nameOfTheReturnType = methodInfo.ReturnType.FullName;
    string nameOfTheParameters = "(";
    foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
    {
        nameOfTheParameters += $"{paramInfo.ParameterType} {paramInfo.Name}";
    }
    nameOfTheParameters += ")";

    Console.WriteLine($"{nameOfTheReturnType} {methodInfo.Name} {nameOfTheParameters}");
}

static void ListInterfaces(Type type)
{
    Console.WriteLine("Interfaces");

    var interfaces = from i in type.GetInterfaces()
                     orderby i.Name
                     select i;

    foreach (var item in interfaces)
    {
        Console.WriteLine("\t"+item.Name);
    }
}

static void HowGetStaticType()
{
    Type type = typeof(Console);
    Console.WriteLine($"We want to investigate the type:{type}");
    ReflectionOfType(type);
}
//HowGetStaticType();

static void ReflectionOfType(Type type)
{
    AboutType(type);
    ListFilds(type);
    ListProperties(type);
    ListMethods(type);
    ListInterfaces(type);
}