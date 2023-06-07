# Бібліотеки класів, пакети, простори імен.

## Створення бібіліотеки класів в VS.

Класи можна помістити в власну бібліотеку аби потім використовувати і ділитись.

1. Створемо рішення Libraries з проектом ProjectWithMyLibrary типу Console App.
2. На рішені правиц клік Add > New Project > Class Library > Next > Project name: MyLibrary > Next > Create
3. Змінемо клас Class1.cs на Car.cs
```cs
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
```
4. Build > Rebuild MyLibrary.
5. В проекті ProjectWihtMyLibrary на папці Dependencies правиц клік > Add Project Reference > MyLybrary > OK
6. Для використання змінемо клас Program.cs
```cs
using MyLibrary;

UsingClassCar();
void UsingClassCar()
{
    Car car = new Car("Nissan","Leaf",2005);
    car.ToConsole();
}
```
7. Run
```
Nissan  Leaf    2005
```

## Створення бібіліотеки класів за допомогою .NET CLI.

1. В папці Libraries відкриваемо командний рядок.(Cmd)
2. dotnet new list
3. dotnet new -h
4. dotnet new classlib -n MyBusLibrary
5. Міняємо Class1.cs на Bus.cs
```cs
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
```
6. cd MyBusLibrary
7. dotnet build
8. cd ..
9. dir
10. cd ProjectWithMyLibrary
11. dotnet add ProjectWithMyLibrary.csproj reference ..\MyBusLibrary\MyBusLibrary.csproj
12. cd ..
13. dir
14. dotnet sln Libraries.sln add MyBusLibrary\MyBusLibrary.csproj
15. Міняем файл Program.cs
```cs
using MyBusLibrary;
using MyLibrary;

UsingClassCar();
void UsingClassCar()
{
    Car car = new Car("Nissan", "Leaf", 2005);
    car.ToConsole();
}

UsingClassBus();
void UsingClassBus()
{
    Bus bus = new();
    bus.ToConsole();
}
```
16. cd ProjectWithMyLibrary
17. dotnet run

```cs
Nissan  Leaf    2005
Undefined       Undefined
```

