
using System.Text.Json;
using System.Text.Json.Serialization;

void DefiningAnonymousType()
{
    var girl = new { Name = "Alice", Age = 25 };
    Console.WriteLine(girl+"\n"+ girl.GetType()+"\n\n");


    Console.WriteLine(GetCar("VW", "Käfer", 1938));

    static string GetCar(string manufacturer, string model, int year)
    {
        var car = new { Manufacturer = manufacturer, Model = model, Year = year };
        Console.Write(car+"\n"+ car.GetType()+"\n\n"); 

        string carString = JsonSerializer.Serialize(car);
        return carString;
    }
}

//DefiningAnonymousType();


void ReflectObjectContent(object @object)
{
    Type type = @object.GetType();

    Console.WriteLine($"\nObject is instance of {type.Name}");
    Console.WriteLine($"Base class of {type.Name} is {type.BaseType}");
    Console.WriteLine($"object.ToString() == {@object}");
    Console.WriteLine($"@object.GetHashCode() == {@object.GetHashCode()}");
}

void ExplorationAnonimusTypes()
{
    var girl = new { Name = "Julia", Age = 35 };

    ReflectObjectContent(girl);

    // girl.Name = "Olga"; It isn't works. it is readonly
}

//ExplorationAnonimusTypes();

void MethodEqualsIntoAnonymousType()
{
    var girl1 = new { Name = "Olga", Age = 35 };
    var girl2 = new { Name = "Olga", Age = 35 };

    ReflectObjectContent(girl1);
    ReflectObjectContent(girl2);

    Console.WriteLine();

    Console.WriteLine($"girl1.Equals(girl2): {girl1.Equals(girl2)} ");
    Console.WriteLine($"girl1 == girl2: {girl1 == girl2}");

}
MethodEqualsIntoAnonymousType();