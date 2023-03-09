using CarApp;

//UsingCar_v1();
void UsingCar_v1()
{
    Car_v1 car = new();
    car.ToConsole();

    car.Manufacturer = "VW";
    car.Model = "Golf 7";
    car.Year = 2018;
    car.ToConsole();

    Car_v1 yourCar = new("Mercedes", "Sprinter", 2018);
    yourCar.ToConsole();
}

//UsingGarage_v1();

void UsingGarage_v1()
{
    Garage_v1 garage = new();

    Console.WriteLine(garage.NumberOfCars);
    Console.WriteLine(garage.MyCar.Model);
}


UsingGarage_v2();

void UsingGarage_v2()
{
    Garage_v2 garage = new();

    Console.WriteLine(garage.NumberOfCars);
    Console.WriteLine(garage.MyCar.Model);
}
