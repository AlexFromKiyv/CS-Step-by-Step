namespace MyBusLibrary;

public class Bus
{
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int? Year { get; set; }

    public Bus(string manufacturer, string model, int? year)
    {
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
    }
    public Bus(string manufacturer, string model)
    {
        Manufacturer = manufacturer;
        Model = model;
    }

    public Bus()
    {
        Manufacturer = "Undefined";
        Model = "Undefined";
    }

    public void ToConsole() => Console.WriteLine($"{Manufacturer}\t{Model}\t{Year}");
}
