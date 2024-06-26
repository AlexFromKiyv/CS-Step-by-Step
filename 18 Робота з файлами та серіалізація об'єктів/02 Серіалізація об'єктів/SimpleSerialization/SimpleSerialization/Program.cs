﻿
using SimpleSerialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

// Create objects for serialize.

Radio? radio = new()
{
    StationPresets = [89.3, 105.1, 97.1],
    HasTweeters  =true
};

TravelCar trevelCar = new()
{
    CanFly = true,
    CanSubmerge = false,
    Radio = new()
    {
        HasTweeters = true,
        StationPresets = [89.3, 105.1, 97.1]
    }
};

List<TravelCar> myCars =
[
    new TravelCar() { CanFly = true, CanSubmerge = true, Radio = radio },
    new TravelCar() { CanFly = true, CanSubmerge = false, Radio = radio },
    new TravelCar() { CanFly = false, CanSubmerge = false, Radio = radio },
];

Person person = new()
{
    FirstName = "James",
    IsAlive = true
};

void TestCreatedObject()
{
    Console.WriteLine(radio); 
    Console.WriteLine();

    Console.WriteLine(trevelCar);
    Console.WriteLine();

    Console.WriteLine("Object of "+myCars.ToString());
    foreach (var item in myCars)
    {
        Console.WriteLine(item);
    }

    Console.WriteLine(person);
}
//TestCreatedObject();


// Serialization to XML

static void SaveAsXMLFormat<T>(T objGrhaph, string fileName)
{
    //Must declare type in the constructor of the XmlSerializer
    XmlSerializer xmlSerializer = new(typeof(T));

    using Stream fileStream = new FileStream(fileName, FileMode.Create,
        FileAccess.Write,FileShare.None);
   
    xmlSerializer.Serialize(fileStream, objGrhaph);
}


void SerializingObjectsUsingXmlSerializer()
{
    SaveAsXMLFormat(trevelCar, @"D:\Temp\TrevalCar.xml");
    Console.WriteLine("Saved trevalCar in XML document.");

    SaveAsXMLFormat(person, @"D:\Temp\Person.xml");
    Console.WriteLine("Saved person in XML document.");
}
//SerializingObjectsUsingXmlSerializer();

void SerializingCollectionsOfObjects()
{
    SaveAsXMLFormat(myCars, @"D:\Temp\CarCollection.xml");
    Console.WriteLine("Saved list of cars in XML document.");
}
//SerializingCollectionsOfObjects();



// Deserialization from XML

static T? ReadAsXmlFormat<T>(string fileName)
{
    // Create a typed instance of the XmlSerializer
    XmlSerializer xmlSerializer = new(typeof(T));

    using Stream fileStream = new FileStream(fileName, FileMode.Open);

    if (xmlSerializer.Deserialize(fileStream) is T result)
    {
        return result;
    }
    else
    {
        return default;
    }
}

static void DeserializingObjectsAndCollectionsOfObjects()
{
 
    TravelCar? travelCar = ReadAsXmlFormat<TravelCar>(@"D:\Temp\TrevalCar.xml");

    Console.WriteLine("DeserializingObject\n");
    Console.WriteLine(travelCar);


    List<TravelCar>? savedCars = ReadAsXmlFormat<List<TravelCar>>(@"D:\Temp\CarCollection.xml");

    Console.WriteLine("\nDeserializingCollectionsOfObjects\n");
    if (savedCars == null) return;

    foreach (var car in savedCars)
    {
        Console.WriteLine(car);
    }
}
//DeserializingObjectsAndCollectionsOfObjects();


// Serialization to JSON

static void SaveAsJSONFormat<T>(T objGraph, string fileName)
{
    JsonSerializerOptions options = new()
    {
        NumberHandling = JsonNumberHandling.WriteAsString 
        | JsonNumberHandling.AllowReadingFromString,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = null,
        IncludeFields = true,
        WriteIndented = true
    };

    File.WriteAllText(
        fileName,
        //System.Text.Json.JsonSerializer.Serialize(objGrhaph)
        System.Text.Json.JsonSerializer.Serialize(objGraph, options)
        );
}

void SerializingObjectsUsingJsonSerializer()
{
    SaveAsJSONFormat(trevelCar, @"D:\Temp\TravelCar.json");
    Console.WriteLine("Saved trevalCar in JSON document.");

    SaveAsJSONFormat(person, @"D:\Temp\Person.json");
    Console.WriteLine("Saved person in JSON document.");
}
//SerializingObjectsUsingJsonSerializer();

void SerializingRadio()
{
    JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = null,
        IncludeFields = true,
        WriteIndented = true,
        NumberHandling =
        JsonNumberHandling.AllowNamedFloatingPointLiterals
        | JsonNumberHandling.WriteAsString,

    };

    string result = JsonSerializer.Serialize(radio,options);

    Console.WriteLine(result);

}
//SerializingRadio();

// Streaming Serialize Async.

static async IAsyncEnumerable<int> PrintNumbers(int n)
{
    for (int i = 0; i < n; i++)
    {
        yield return i;
    }
}

static async void SerializeAsync()
{
    using Stream stream = Console.OpenStandardOutput();

    var data = new { Data = PrintNumbers(10) };

    await JsonSerializer.SerializeAsync(stream, data);
}

//SerializeAsync();

// Streaming Deserialize Async.

async static void DeserializeAsync()
{
    var strem = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("[0,1,2,3,4]"));

    await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<int>(strem))
    {
        Console.WriteLine(item);
    }
}
//DeserializeAsync();


// Performance Issues Using JsonSerializerOptions


JsonSerializerOptions globalOptions = new()
{
    ReferenceHandler = ReferenceHandler.IgnoreCycles,
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = null,
    IncludeFields = true,
    WriteIndented = true
};

static void SaveAsJson<T>(JsonSerializerOptions options,T objGraph, string fileName)
{
    File.WriteAllText(
    fileName,
    System.Text.Json.JsonSerializer.Serialize(objGraph, options)
    );
}

void SerializeWithGlobalOptions()
{
    SaveAsJson(globalOptions, radio, "Radio.json");
    Console.WriteLine("Saved radio in JSON document.");
}
//SerializeWithGlobalOptions();


// JsonSerializerDefaults.Web
void UseOptionsGeneralAndForWeb()
{
    Console.WriteLine("General options");
    JsonSerializerOptions? options = new(JsonSerializerDefaults.General);
    ShowOptions(options);


    Console.WriteLine("\nOptions for Web");
    JsonSerializerOptions webOptions = new(JsonSerializerDefaults.Web) 
    { 
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };
    ShowOptions(webOptions);
}
//UseOptionsGeneralAndForWeb();

void ShowOptions(JsonSerializerOptions? options)
{
    Console.WriteLine($"PropertyNameCaseInsensitive: {options?.PropertyNameCaseInsensitive}");
    Console.WriteLine($"PropertyNamingPolicy: {options?.PropertyNamingPolicy}");
    Console.WriteLine($"NumberHandling: {options?.NumberHandling}");
    Console.WriteLine($"WriteIndented: {options?.WriteIndented}");
    Console.WriteLine($"ReferenceHandler: {options?.ReferenceHandler}");
}


// Serializing a collection of objects into JSON

JsonSerializerOptions optionsWithWriteIndented = new(JsonSerializerDefaults.General)
{ WriteIndented = true };

void SerializingCollectionToJson()
{
    SaveAsJson(optionsWithWriteIndented, myCars, @"D:\Temp\CarCollection.json");
    Console.WriteLine("The collection is serialized.");
}
//SerializingCollectionToJson();


// Deserialization from JSON

static T? ReadAsJsonFormat<T>(JsonSerializerOptions options, string fileName) =>
    JsonSerializer.Deserialize<T>(File.ReadAllText(fileName), options);


JsonSerializerOptions optionsWithNumberHandling = new(JsonSerializerDefaults.General)
{ NumberHandling = JsonNumberHandling.AllowReadingFromString };

void DeserializeObjectAndCollectionFromJson()
{
    TravelCar? travelCar = ReadAsJsonFormat<TravelCar>(
        optionsWithNumberHandling,
        @"D:\Temp\TravelCar.json");
  
    Console.WriteLine("Object TravelCar in memory.\n");
    Console.WriteLine(travelCar);

    List<TravelCar>? travelCars = ReadAsJsonFormat<List<TravelCar>>(
        optionsWithNumberHandling,
        @"D:\Temp\CarCollection.json"     
        );

    if (travelCars == null) return;
    
    Console.WriteLine("\nCollections of TravelCar objects in memory.\n");
    foreach (TravelCar car in travelCars)
    {
        Console.WriteLine(car);
    }
}
//DeserializeObjectAndCollectionFromJson();

void UseCustomConverter()
{
    JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = null,
        IncludeFields = true,
        WriteIndented = true,
        Converters = { new JsonStringNullToEmptyConverter()}

    };

    // Object with null

    Radio radio1 = new() 
    {
        HasTweeters = true,
        HasSubWoofers = true,
        RadioId = null
    };

    var jsonRadio1 = JsonSerializer.Serialize(radio1, options);

    Console.WriteLine(jsonRadio1);

    Console.WriteLine(JsonSerializer.Serialize(radio1, globalOptions));

}
//UseCustomConverter();