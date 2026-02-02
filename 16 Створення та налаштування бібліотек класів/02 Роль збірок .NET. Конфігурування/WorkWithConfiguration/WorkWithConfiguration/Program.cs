using Microsoft.Extensions.Configuration;
using WorkWithConfiguration;

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile("appsettings.development.json", true, true)
    .Build();


void ReadingFromConfiguration_1()
{
    Console.WriteLine($"My car's name is {config["CarName"]}");
}
//ReadingFromConfiguration_1();

void ReadingFromConfiguration_2()
{
    Console.WriteLine($"My car's name is {config["CarName2"]}");
    Console.WriteLine($"CarName2 is null? {config["CarName2"] == null}");
}
//ReadingFromConfiguration_2();

void ReadingFromConfiguration_3()
{
    Console.WriteLine($"My car's name is {config.GetValue(typeof(string), "CarName")}");
    Console.WriteLine($"My car's name is {config.GetValue<string>("CarName")}");
}
//ReadingFromConfiguration_3();

void ReadingFromConfiguration_4()
{
    Console.WriteLine($"My car's name is {config.GetValue<int>("CarName2")}");
}
//ReadingFromConfiguration_4();

void ReadingFromConfiguration_5()
{
    try
    {
        Console.WriteLine($"My car's name is {config.GetValue<int>("CarName")}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"An exception occurred: {ex.Message}");
    }
}
//ReadingFromConfiguration_5();

void ReadingFromConfiguration_6()
{
    Console.Write($"My car object is a {config["Car:Color"]} ");
    Console.WriteLine($"{config["Car:Make"]} named {config["Car:PetName"]}");
}
ReadingFromConfiguration_6();
void ReadingFromConfiguration_7()
{
    IConfigurationSection section = config.GetSection("Car");
    Console.Write($"My car object is a {section["Color"]} ");
    Console.WriteLine($"{section["Make"]} named {section["PetName"]}");
}
//ReadingFromConfiguration_7();

void ReadingFromConfiguration_8()
{
    IConfigurationSection section = config.GetSection("Car");

    var c = new Car();
    section.Bind(c);

    Console.Write($"My car object is a {c.Color} ");
    Console.WriteLine($"{c.Make} named {c.PetName}");
}
//ReadingFromConfiguration_8();

void ReadingFromConfiguration_9()
{
    var notFoundCar = new Car
    {
        Color = "Red"
    };

    config.GetSection("Car2").Bind(notFoundCar);
    Console.Write($"My car object is a {notFoundCar.Color} ");
    Console.WriteLine($"{notFoundCar.Make} named {notFoundCar.PetName}");
}
//ReadingFromConfiguration_9();

void ReadingFromConfiguration_10()
{
    var carFromGet = config.GetSection(nameof(Car)).Get(typeof(Car)) as Car;
    Console.Write($"My car object (using Get()) is a {carFromGet.Color} ");
    Console.WriteLine($"{carFromGet.Make} named {carFromGet.PetName}");

}
//ReadingFromConfiguration_10();

void ReadingFromConfiguration_11()
{
    var notFoundCarFromGet = config.GetSection("Car2").Get(typeof(Car));
    Console.WriteLine($"The not found car is null? {notFoundCarFromGet == null}");
}
//ReadingFromConfiguration_11();

void ReadingFromConfiguration_12()
{
    var carFromGet = config.GetSection(nameof(Car)).Get<Car>();
    Console.Write($"My car object (using Get()) is a {carFromGet.Color} ");
    Console.WriteLine($"{carFromGet.Make} named {carFromGet.PetName}");

    var notFoundCarFromGet = config.GetSection("Car2").Get<Car>();
    Console.WriteLine($"The not found car is null? {notFoundCarFromGet == null}"); ;
}
//ReadingFromConfiguration_12();

void ReadingFromConfiguration_13()
{
    try
    {
        _ = config
            .GetSection(nameof(Car))
            .Get<Car>(bo => bo.ErrorOnUnknownConfiguration = true);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"An exception occurred: {ex.Message}");
    }
}
//ReadingFromConfiguration_13();

void ReadingFromConfiguration_14()
{
    var notFoundCar = new Car();

    try
    {
        config.GetRequiredSection("Car2").Bind(notFoundCar);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"An exception occurred: {ex.Message}");
    }
}
//ReadingFromConfiguration_14();

