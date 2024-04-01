
using System.Reflection;

static void AddWithReflection()
{
    Assembly assembly = Assembly.LoadFrom("MyMath");
	try
	{
        Type simpleMath = assembly.GetType("MyMath.SimpleMath");

        object objSimpleMath = Activator.CreateInstance(simpleMath);

        MethodInfo methodInfoAdd = simpleMath.GetMethod("Add");

        object[] objects = { 1, 2 };

        Console.WriteLine(methodInfoAdd.Invoke(objSimpleMath,objects));

    }
	catch (Exception ex)
	{
        Console.WriteLine(ex.Message);
    }
}
//AddWithReflection();

static void AddWithDynamic()
{
    Assembly assembly = Assembly.LoadFrom("MyMath");
    try
    {
        Type simpleMath = assembly.GetType("MyMath.SimpleMath");

        dynamic objSimpleMath = Activator.CreateInstance(simpleMath);

        Console.WriteLine(objSimpleMath.Add(1,2));

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
AddWithDynamic();