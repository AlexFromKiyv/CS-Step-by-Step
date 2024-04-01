using DynamicKeyword;
using System.Reflection;

static void InvestigationImplicitlyTypedValue()
{
    var list = new List<string> { "20" };

    //list = "10"; //Cannot implicitly convert type "string" to ... 
}


static void UseObjectVariable()
{
    // Create a new instance of the Person class
    // and assign it to a variable of type System.Object
    object p = new Person() { FirstName = "Antoni", LastName = "Gaudi" };

    // Must cast object as Person to gain access
    // to the Person properties.
    Console.WriteLine($"{((Person)p).FirstName} {((Person)p).LastName}");
}
//UseObjectVariable();

static void TreeString()
{
    var str1 = "Go";
    object str2 = "to";
    dynamic str3 = "Home";

    Console.WriteLine(str1.GetType());
    Console.WriteLine(str2.GetType());
    Console.WriteLine(str3.GetType());
}
//TreeString();

static void UseDynamic()
{
    dynamic val;
    //object val; 
        
    val = "Hi";
    Console.WriteLine(val+" "+val.GetType());

    val = false;
    Console.WriteLine(val + " " + val.GetType());

    val = new List<int> { 1, 2, 3 };
    Console.WriteLine(val.GetType());

}
//UseDynamic();

static void UseMemeberOfDynamicVariable()
{
    dynamic str = "hi";
    Console.WriteLine(str.ToUpper());

    try
    {
        Console.WriteLine(str.DoIt());
        Console.WriteLine(str.toupper());
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);    
    }
}
//UseMemeberOfDynamicVariable();

static void UseVeryDynamic()
{
    VeryDynamic veryDynamic = new();

    veryDynamic.Property = 10;
    dynamic result = veryDynamic.Method(veryDynamic.Property);
    Write(result);

    veryDynamic.Property = "Julia";
    result = veryDynamic.Method(veryDynamic.Property);
    Write(result);

    static void Write(dynamic value)
    {
        Console.WriteLine(value + " " + value.GetType());
    }
}
//UseVeryDynamic();

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
                // Create object
                obj = Activator.CreateInstance(miniVan);
                Console.WriteLine($"Created a {obj} using late binding!");

                //Invoke method without parameters
                MethodInfo? methodInfoTurboBoost = miniVan.GetMethod("TurboBoost");
                methodInfoTurboBoost?.Invoke(obj, null);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
//Run();


void RunWithDynamic()
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
        Type typeMiniVan = assembly.GetType("CarLibrary.MiniVan");

        try
        {
            dynamic miniVan = Activator.CreateInstance(typeMiniVan);
            Console.WriteLine($"Created a {miniVan} using late binding!");
            miniVan.TurboBoost();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
//RunWithDynamic();