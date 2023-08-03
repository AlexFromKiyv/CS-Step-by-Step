
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

void ExplorationAnonimusTypes()
{
    var girl = new { Name = "Julia", Age = 35 };

    ReflectObjectContent(girl);

    void ReflectObjectContent(object @object)
    {
        Type type = @object.GetType();

        Console.WriteLine($"Object is instance of {type.Name}");
        Console.WriteLine($"Base class of {type.Name} is {type.BaseType}");
        Console.WriteLine($"object.ToString() == {@object}");
        Console.WriteLine($"@object.GetHashCode() == {@object.GetHashCode()}");
    }

    // girl.Name = "Olga"; It isn't works. it is readonly
}

ExplorationAnonimusTypes();