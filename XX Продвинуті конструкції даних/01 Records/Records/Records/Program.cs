
using Records;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
//UsingCar();
void UsingCar()
{
    Car car1 = new Car("VW", "Polo", "Red");
    Car car2 = new()
    {
        Manufacturer = "VW",
        Model = "Polo",
        Color = "Red"
    };

    Console.WriteLine(car1.GetType());
    Console.WriteLine(car1);

    Console.WriteLine( car1 == car2 );
}

//UsingCarRecord_v1();
void UsingCarRecord_v1()
{
    CarRecord_v1 car1 = new("VW","Polo","Red");
    CarRecord_v1 car2 = new()
    {
        Manufacturer = "VW",
        Model = "Polo",
        Color = "Red"
    };

    Console.WriteLine(car1.GetType());
    Console.WriteLine(car1);
    Console.WriteLine( car1 == car2 );
}

//UsingCarRecord_v2();
void UsingCarRecord_v2()
{
    CarRecord_v2 car1 = new("VW", "Polo", "Red");
    CarRecord_v2 car2 = new CarRecord_v2("VW", "Polo", "Red");

   // car1.Manufacturer = "Volks Wagen"; // don't work  immutable


    Console.WriteLine(car1.GetType());
    Console.WriteLine(car1);
    Console.WriteLine(car1 == car2);

    CarRecord_v2 car3 = new CarRecord_v2(null, null, null);
    Console.WriteLine(car3.Manufacturer);
    Console.WriteLine(car3.Model);
    Console.WriteLine(car3.Color);

}

UsingDeconstuct();
void UsingDeconstuct()
{
    CarRecord_v1 car1 = new("VW", "Polo", "Red");
    
    // car1.Deconstruct( It's no here this method

    CarRecord_v2 car2 = new("VW", "Polo", "Red");

    string color;

    car2.Deconstruct(out string manufacturer, out string model, out color);


    Console.WriteLine($"{manufacturer} {model} {color}");

    var (manufacturer_1, model_1, color_1) = car2;

    Console.WriteLine($"{manufacturer_1} {model_1} {color_1}");

}