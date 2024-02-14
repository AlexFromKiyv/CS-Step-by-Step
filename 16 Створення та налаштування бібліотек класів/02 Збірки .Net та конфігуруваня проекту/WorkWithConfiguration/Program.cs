using Microsoft.Extensions.Configuration;
using WorkWithConfiguration;
using static System.Collections.Specialized.BitVector32;

void GetDataFromConfigFile()
{
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json",true,true)
        .Build();

       
    Console.WriteLine(configuration["BaseCurrency"]);
    
    Console.WriteLine(configuration.GetValue<string>("CarName"));
    Console.WriteLine(configuration.GetValue("BaseCurrency", "USD"));
    Console.WriteLine(configuration.GetValue("Currency", "USD"));
    Console.WriteLine(configuration.GetValue<string>("BusName"));
    Console.WriteLine(configuration.GetValue("BusName","Mercedes Sprinter"));
    
    Console.ReadLine();
}
//GetDataFromConfigFile();

void MoreThanOneConfigFile()
{
    IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile("appsettings.development.json", true, true)
    .Build();

    Console.WriteLine(configuration.GetValue<string>("BaseCurrency"));
}
//MoreThanOneConfigFile();

IConfiguration GetConfiguration()
{
    IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .Build();

    return configuration;
}

void SimpleGetMultilevelData()
{
    IConfiguration configuration = GetConfiguration();

    Console.WriteLine(configuration["Car:Make"]);

    IConfigurationSection section = configuration.GetSection("Car");

    Console.WriteLine(section["Color"]);
}
//SimpleGetMultilevelData();

void GetMultilevelData()
{
    IConfiguration configuration = GetConfiguration();

    IConfigurationSection section = configuration.GetSection("Car");

    Car car = new();
    section.Bind(car);
    Console.WriteLine(car.Make);
    Console.WriteLine(car.Color);
    Console.WriteLine(car.EngineType);
}
//GetMultilevelData();




void CreateConfigurationObject()
{
    IConfiguration configuration = GetConfiguration();

    var configurationObject_1 = configuration.GetSection(nameof(Car)).Get(typeof(Car)) as Car;

    Console.WriteLine(configurationObject_1?.Make);
    Console.WriteLine(configurationObject_1?.Color);
    Console.WriteLine(configurationObject_1?.EngineType);
    Console.WriteLine();

    var configurationObject_2 = configuration.GetSection(nameof(Car)).Get<Car>();
    Console.WriteLine(configurationObject_2?.Make);
    Console.WriteLine(configurationObject_2?.Color);
    Console.WriteLine(configurationObject_2?.EngineType);
}
//CreateConfigurationObject();

void BindAndGetAndReflection()
{
    
    IConfiguration configuration = GetConfiguration();

    Car configurationObject_1 = new();
    
    configuration.GetSection("car").Bind(configurationObject_1);
       
    Console.WriteLine(configurationObject_1?.Make);
    Console.WriteLine(configurationObject_1?.Color);
    Console.WriteLine(configurationObject_1?.EngineType);
    
    Console.WriteLine();

    var configurationObject_2 = configuration.GetSection(nameof(Car)).Get<Car>();
    Console.WriteLine(configurationObject_2?.Make);
    Console.WriteLine(configurationObject_2?.Color);
    Console.WriteLine(configurationObject_2?.EngineType);
}
//BindAndGetAndReflection();


void BindValidation()
{
    IConfiguration configuration = GetConfiguration();

    try
    {
        IConfigurationSection section = configuration.GetSection(nameof(Car));
        Car? configurationObject = section.Get<Car>(t => t.ErrorOnUnknownConfiguration = true);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//BindValidation();

void PrivatePropertyFromConfiguration()
{
    IConfiguration configuration = GetConfiguration();
    IConfigurationSection section = configuration.GetSection(nameof(Car));

    Car? configurationObject = section.Get<Car>();
    Console.WriteLine(configurationObject?.GetWarCode());

    Car? configurationObject_1 = section.Get<Car>(t=>t.BindNonPublicProperties=true);
    Console.WriteLine(configurationObject_1?.GetWarCode());
}
//PrivatePropertyFromConfiguration();

void UseGetRequiredSection()
{
    try
    {
        IConfiguration configuration = GetConfiguration();
        IConfigurationSection section = configuration.GetRequiredSection("Bus");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        throw;
    }
}
//UseGetRequiredSection();
