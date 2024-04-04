# Динамічне завантаження збірок.

Бувають випадки, коли вам потрібно буде програмно завантажити збірки під час виконання. Акт завантаження зовнішніх збірок за потребою відомий як динамічне завантаження(dynamic load).
System.Reflection визначає клас під назвою Assembly. Використовуючи цей клас, ви можете динамічно завантажувати збірку, а також відкривати властивості самої збірки. Клас Assembly надає методи, які дозволяють вам програмно завантажувати збірки.


ExternalAssemblyReflector\Program.cs
```cs
using System.Reflection;

void Start()
{
    string? assemblyName = "";
    Assembly? assembly = null;
    do
    {
        Console.Clear();
        Console.Write("Enter the name of the assembly to evaluate: ");
        assemblyName = Console.ReadLine();

        if (assemblyName == null) assemblyName = "";

        try
        {

            assembly = Assembly.LoadFrom(assemblyName);
            WriteTypesInAssembly(assembly);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadKey();
        }

    } while (true);
}
Start();

void WriteTypesInAssembly(Assembly assembly)
{
    Console.WriteLine($"Assembly name:{assembly.FullName}");

    Type[] types = assembly.GetTypes();
    foreach (Type type in types)
    {
        Console.WriteLine(type);
    }

    Console.ReadKey();
}

```
Для тестування скопіюємо файл CarLibrary.dll на D:\
```
Enter the name of the assembly to evaluate: D:\CarLibrary
Assembly name:CarLibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
CarLibrary.Car
CarLibrary.EngineStateEnum
CarLibrary.InternalCar
CarLibrary.MiniVan
CarLibrary.MusicMediaEnum
CarLibrary.SportCar

```

## Рефлексія збірок фреймворка.

Крім метода LoadFrom в класі Аssembly є метод Load який має декілька перезавантажень. Один варіант дозволяє вказати значення культури (для локалізованих збірок), а також номер версії та значення токена відкритого ключа (для збірок каркаса). У сукупності набір елементів, що ідентифікують збірку, називається відображуваним іменем.Формат відображуваного імені — це розділений комами рядок пар ім’я-значення, який починається зі зрозумілої назви збірки, за якою слідують необов’язкові кваліфікатори (які можуть з’являтися в будь-якому порядку). Ось шаблон, якого слід дотримуватися (необов’язкові елементи вказано в дужках):

Name (,Version = major.minor.build.revision) (,Culture = culture token) (,PublicKeyToken= public key token)

ExternalAssemblyReflector\Program.cs
```cs
void UseAsseblyLoad()
{
    try
    {
        Assembly? assembly = Assembly.Load("CarLibrary, Version=1.0.0.1");
        WriteTypesInAssembly(assembly);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
UseAsseblyLoad();
```
Щоб цей код працював потрібно шоб в проекті було посилянна на проект бібліотеки.

```
Assembly name:CarLibrary, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
CarLibrary.Car
CarLibrary.EngineStateEnum
CarLibrary.InternalCar
CarLibrary.MiniVan
CarLibrary.MusicMediaEnum
CarLibrary.SportCar
```
Також можна використати тип Assembly.AssemblyName

```cs
void UseAsseblyLoad()
{
    try
    {
        //Assembly? assembly = Assembly.Load("CarLibrary, Version=1.0.0.1");
        AssemblyName assemblyName = new()
        {
            Name = "CarLibrary",
            Version = new Version("1.0.0.1"),
        };

        Assembly? assembly = Assembly.Load(assemblyName);

        WriteTypesInAssembly(assembly);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
UseAsseblyLoad();
```
Створемо проект FrameworkAssemblyViewer і додамо в проект пакет Microsoft.EntityFrameworkCore

Отримаємо типи цієї збірки.
```cs
using System.Reflection;

Assembly assembly = Assembly.Load("Microsoft.EntityFrameworkCore");

AssemblyName assemblyName = assembly.GetName(); 

Console.WriteLine(assemblyName.Name );
Console.WriteLine(assemblyName.FullName);
Console.WriteLine(assemblyName.Version);
Console.WriteLine(assemblyName.CultureInfo?.DisplayName);

Console.ReadKey();

var types = assembly.GetTypes().Where(t => t.IsPublic);

foreach (Type type in types)
{
    Console.WriteLine(type);
}

```
```
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
9.0.0.0
Invariant Language (Invariant Country)
System.Transactions.TransactionsDatabaseFacadeExtensions
Microsoft.Extensions.DependencyInjection.EntityFrameworkServiceCollectionExtensions
Microsoft.EntityFrameworkCore.AutoTransactionBehavior
Microsoft.EntityFrameworkCore.ChangeTrackingStrategy
Microsoft.EntityFrameworkCore.ChangeTrackerExtensions
Microsoft.EntityFrameworkCore.DbContext
Microsoft.EntityFrameworkCore.DbContextId
Microsoft.EntityFrameworkCore.DbContextOptions
Microsoft.EntityFrameworkCore.DbContextOptionsBuilder
Microsoft.EntityFrameworkCore.DbContextOptionsBuilder`1[TContext]
Microsoft.EntityFrameworkCore.DbContextOptions`1[TContext]
Microsoft.EntityFrameworkCore.DbFunctions

```
Коли ви посилаєтеся на іншу збірку, копія цієї збірки копіюється в вихідний каталог проекту, на який посилається.
Вам, швидше за все, не потрібно буде створювати власні браузери об’єктів часто (якщо взагалі буде). Однак служби відображення є основою для багатьох поширених дій програмування, включаючи пізнє зв’язування.