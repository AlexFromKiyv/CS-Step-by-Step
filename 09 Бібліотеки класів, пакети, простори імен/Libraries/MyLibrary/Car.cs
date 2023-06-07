namespace MyLibrary
{
    public class Car
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }

        public Car(string manufacturer, string model, int? year)
        {
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
        }
        public Car(string manufacturer, string model)
        {
            Manufacturer = manufacturer;
            Model = model;
        }

        public Car()
        {
            Manufacturer = "Undefined";
            Model = "Undefined";
        }

        public void ToConsole() => Console.WriteLine($"{Manufacturer}\t{Model}\t{Year}");
    }
}