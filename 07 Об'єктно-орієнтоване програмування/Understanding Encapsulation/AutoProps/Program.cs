
using AutoProps;
using System.Security.Cryptography;

void UsingAutoProperties()
{
    Car car = new Car();
    car.PetName = "Framk";
    car.Speed = 55;
    car.Color = "Red";

    Console.WriteLine($"Your car is named {car.PetName}? That\'s odd...");
    car.DisplayStats();
}
//UsingAutoProperties();

void AutomaticPropertiesAndDefaultValues()
{
    Garage garage = new();
    // OK, prints default value of zero.
    Console.WriteLine(garage.Temperature);

    // Runtime error! Backing field is currently null!
    Console.WriteLine(garage.MyCar.PetName);

    Car car = new() { PetName = "Lasivka" };
    garage.MyCar = car;
    Console.WriteLine(garage.MyCar.PetName);
    Console.WriteLine(garage.MyCar.Color);
    Console.WriteLine(garage.MyCar.Speed);
}
AutomaticPropertiesAndDefaultValues();