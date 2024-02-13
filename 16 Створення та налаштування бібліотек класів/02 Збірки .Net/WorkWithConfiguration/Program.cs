using Microsoft.Extensions.Configuration;
using WorkWithConfiguration;

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
CreateConfigurationObject();