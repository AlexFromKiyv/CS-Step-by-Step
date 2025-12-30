# Бібліотеки класів, пакети, простори імен.

# Створення бібіліотеки класів в VS.

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
6. Для використання змінемо Program.cs
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
Undefined       Undefined
```
## Сумістність бібліотеки.

Якшо ви відкриєте файл проекту MyLibrary ви зможете побачити строку наприклад

```xml
    <TargetFramework>net7.0</TargetFramework>
```
Якшо е потреба можна зробити бібліотеку сумісною з стандартом.

```xml
    <TargetFramework>netstandard2.0</TargetFramework>
    <LangVersion>11</LangVersion>
```


# Пакети Nuget.

Існує багато готових бібліотек яки можна використовувати. Для цого існує менеджер пакетів Nuget.
Nuget-пакет це архівний файл з dll та іншими файлами для роботи коду. Пакети також мають інформацію про версію. Глобальний репозіторій nuget.org туди розробники додають і завантажують звідти пакети.

## Додавання пакетів.

Створемо рішення Packages з проектом UsingPackages.

Додамо пакет.

1. В Solution Explorer на назві проекту правий клік.
2. Manage NuGet Packages...
3. Перейти на закладку Browse.
4. Ввести Newtonsoft.Json
5. Вибрати пакет Newtonsoft.Json нажати Install.

 Після додавання в файлі проекту буде 
 ```
  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
  </ItemGroup>
 ```
Якшо використовувати .NET CLI додати пакет в проект, можна за допомогою команди
```
dotnet add package Newtonsoft.Json
```

## Використання пакету

Program.cs
```cs
using Newtonsoft.Json;

UsingSerializeObject();
void UsingSerializeObject()
{
    Car car = new("Tesla", "Model 3");

    string carJson = JsonConvert.SerializeObject(car);

    Console.WriteLine(carJson);
}


class Car
{
    public string Manufacturer { get; set; } = "Undefined";
    public string Model { get; set; } = "Undefined";

    public Car(string manufacturer, string model)
    {
        Manufacturer = manufacturer;
        Model = model;
    }
}
```
```
{"Manufacturer":"Tesla","Model":"Model 3"}
```
З прикладу видно коли пакет включено в проект його можно використовувати за допомогою using. При потребі в пакеті можна знайти документацію.

## Простір імен.

Зазвичай класи та інші типи об'єднуються в логічні блоки згуповані по призначенню.
Наприклад у випадку System.Console простір імен це System а Console це клас в ньому. Для того шоб ми могли визивати методи цього класу компілятоу потривна вказівка де цей клас може бути. 

Namespaces\Types1.cs
```cs
namespace Account;
internal class Person
{
    string name;

    public Person(string name)
    {
        this.name = name;
    }

    public void ToConsole() => Console.WriteLine(name);
}
```
Namespaces\Types2.cs
```cs
namespace Vehicle
{

    namespace Cars
    {
        class Car
        {
            public string Manufacturer { get; set; } = "Undefined";
            public string Model { get; set; } = "Undefinred";

            public void ToConsole() => Console.WriteLine($"{Manufacturer}\t{Model}");
        }
    }
}
```
```cs

using Account;
using Vehicle.Cars;

ToConnectNamespace();
void ToConnectNamespace()
{
    Person person = new("Viktory");
    person.ToConsole();

    Car car = new Car();
    car.ToConsole();
}
```
```
Viktory
Undefined       Undefinred
```
Простір імен може мати вкладений простір і тому терба вказувати повний ланцюг до нього.

Можна підключити простір імен до всіх файлів проекту 
```cs
global using Vehicle.Cars;
```

## Підключені простори за замовчуванням.

Файл проекту може мати такий рядок.

```
    <ImplicitUsings>enable</ImplicitUsings>
```
При компфляції коду генерується файл obj\Debug\netX.0\<ProjectName>.GlobalUsings.g.cs в якому вказуються ти простори імен які потрібні майже в кожному файлі программи. 
Якшо в Solution Explorer нажати Show All Files і подивитись obj\Debug\netX.0\Namespaces.GlobalUsings.g.cs то він буде виглядати:
```cs
// <auto-generated/>
global using global::System;
global using global::System.Collections.Generic;
global using global::System.IO;
global using global::System.Linq;
global using global::System.Net.Http;
global using global::System.Threading;
global using global::System.Threading.Tasks;
```
Це вказує що кожен файл проекту може використовувати класи ціх просторів.
Якшо деякі простори не потрібні їх можна відімкнути.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <Using Remove='System.Collections.Generic;' />
    <Using Remove='System.IO' />
    <Using Remove='System.Linq' />
    <Using Remove='System.Net.Http' />
    <Using Remove='System.Threading' />
    <Using Remove='System.Threading.Tasks;' />
    <Using Include='System.Numerics' />
  </ItemGroup>
</Project>
```
Коли ми змінемо файл проекту,після компіляції, змінеться GlobalUsings.g.cs

```cs
// <auto-generated/>
global using global::System;
global using global::System.Numerics;
```

## Викорстання псевдоніма(alias) простору імен.

```cs
using Env = System.Environment;

UsingAlias();
void UsingAlias()
{
    Console.WriteLine(Env.OSVersion);
    Console.WriteLine(Env.Version);
    Console.WriteLine(Env.ProcessorCount);
 }
```
```
Microsoft Windows NT 10.0.19045.0
7.0.5
4
```


