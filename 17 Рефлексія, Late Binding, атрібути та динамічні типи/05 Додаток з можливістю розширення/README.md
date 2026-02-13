# Додаток з можливістю розширення

## Використання рефлексії, пізнього зв'язування та користувацьких атрибутів у перспективі.

Навіть якщо ви бачили численні приклади дії цих методів, ви все ще можете задаватися питанням, коли використовувати рефлексію, динамічне завантаження, пізнє зв'язування та користувацькі атрибути у своїх програмах. Звичайно, ці теми можуть здаватися дещо академічними (що може бути погано, а може й ні, залежно від вашої точки зору). Щоб зіставити ці теми з реальними ситуаціями, вам потрібен переконливий приклад. 
Припустимо на мить, що ви перебуваєте в команді програмістів, яка створює додаток з такою вимогою:

    Продукт повинен мати можливість розширення за допомогою додаткових інструментів сторонніх розробників.

Що саме мається на увазі під розширюваним? Що ж, розглянемо IDE Visual Studio або Visual Studio Code. Під час розробки цієї програми в кодову базу було вставлено різні «hooks»(перехоплювачі), щоб дозволити іншим постачальникам програмного забезпечення «snap»(вбудовувати) (або підключати) власні модулі в IDE. Очевидно, що команди розробників Visual Studio/Visual Studio Code не мали можливості встановлювати посилання на зовнішні збірки .NET, які ще не були розроблені (отже, не було раннього зв'язування), тож як саме застосунок мав би забезпечити необхідні перехоплювачі? Ось один із можливих способів вирішення цієї проблеми:

    1.По-перше, розширювана програма повинна забезпечувати певний механізм введення, щоб користувач міг вказати модуль для підключення (наприклад, діалогове вікно або прапорець командного рядка). Це вимагає динамічного завантаження.

    2.По-друге, розширювана програма повинна мати можливість визначати, чи підтримує модуль правильну функціональність (наприклад, набір необхідних інтерфейсів) для підключення до середовища. Це вимагає рефлексії.

    3.Нарешті, розширювана програма повинна отримати посилання на необхідну інфраструктуру (наприклад, набір типів інтерфейсів) та викликати членів для запуску базової функціональності. Це може вимагати пізнього зв'язування.

Простіше кажучи, якщо розширюваний застосунок був попередньо запрограмований для запиту певних інтерфейсів, він може визначити під час виконання, чи можна активувати тип. Після успішного проходження цього тесту перевірки, відповідний тип може підтримувати додаткові інтерфейси, що забезпечують поліморфну ​​структуру їхньої функціональності. Саме такий підхід обрала команда Visual Studio, і, попри те, що ви можете подумати, він зовсім не складний.

# Створення розширюваного застосунку

У наступних розділах я розгляну приклад, який ілюструє процес створення застосунку, який можна доповнити функціональністю зовнішніх збірок. Щоб слугувати дорожньою картою, розширювана програма включає такі збірки:

    CommonSnappableTypes.dll: Ця збірка містить визначення типів, які використовуватимуться кожним об'єктом шо вбудовуються та на які безпосередньо посилатиметься програма.

    CSharpSnapIn.dll: оснащення, написане на C#, яке використовує типи CommonSnappableTypes.dll.

    MyExtendableApp.exe: Консольна програма, яку можна розширити функціональністю кожної оснастки.

Ця програма використовуватиме динамічне завантаження, відображення та пізнє зв'язування для динамічного отримання функціональності збірок, про які вона не має попередніх знань.

    Бізнес-додатки, створені за допомогою C#, зазвичай належать до категорії інтелектуальних клієнтських веб-додатків/RESTful-сервісів (ASP.NET Core) або процесів (функції Azure). Ми використовуємо консольні застосунки, щоб зосередитися на конкретних концепціях прикладу, у цьому випадку динамічному завантаженні, відображенні та пізньому зв'язуванні. Далі ви дослідите «реальні» користувацькі застосунки, що використовують WPF та ASP.NET Core.

## Створення багатопроектного рішення ExtendableApp за допомогою CLI

До цього моменту більшість програм були окремими проектами, за кількома винятками. Це було зроблено для того, щоб приклади були простими та зосередженими. Однак у реальній розробці ви зазвичай працюєте з кількома проектами разом в одному рішенні.

## Створення рішення та проектів за допомогою CLI

Щоб розпочати використання CLI, введіть такі команди для створення нового рішення, бібліотек класів та консольної програми, а також посилань на проекти:

```
md ExtendableApp
cd ExtendableApp
dotnet new sln
dotnet new classlib -n CommonSnappableTypes
dotnet sln add .\CommonSnappableTypes
dotnet new classlib -n CSharpSnapIn
dotnet sln add .\CSharpSnapIn
dotnet add CSharpSnapIn reference CommonSnappableTypes
dotnet new console -n MyExtendableApp
dotnet sln add .\MyExtendableApp
dotnet add MyExtendableApp reference CommonSnappableTypes
```

### Додавання подій PostBuild до файлів проекту

Під час збірки проектів (з Visual Studio або з командного рядка) можна підключити певні події. Наприклад, ми хочемо скопіювати дві збірки оснащення до каталогу проекту консольної програми (під час налагодження за допомогою dotnet run) та каталогу виводу консольної програми (під час налагодження за допомогою Visual Studio) після кожної успішної збірки. Для цього ми скористаємося кількома вбудованими макросами. 
Якщо ви використовуєте Visual Studio, скопіюйте цей блок розмітки у файл CSharpSnapIn.csproj, який скопіює скомпільовану збірку у вихідний каталог MyExtendableApp (MyExtendableApp\bin\debug\netX.0):

```xml
  <Target Name="PostBuild" AfterTargets="PostBuildEvent">
    <Exec Command="copy $(TargetPath) $(SolutionDir)MyExtendableApp\$(OutDir)$(TargetFileName) /Y" />
  </Target>
```
Якщо ви використовуєте Visual Studio Code, скопіюйте цей блок розмітки у файл CSharpSnapIn.csproj, який скопіює скомпільовану збірку в каталог проекту MyExtendableApp:

```xml
<Target Name="PostBuild" AfterTargets="PostBuildEvent">
    <Exec Command="copy $(TargetPath) $(ProjectDir)..\MyExtendableApp\$(TargetFileName) /Y" />
</Target>
```
Тепер, коли кожен проект збирається, його збірка також копіюється в цільовий каталог MyExtendableApp.

## Створення рішення та проектів за допомогою Visual Studio

Нагадаємо, що за замовчуванням Visual Studio називає рішення так само, як і перший проект, створений у цьому рішенні. Однак ви можете легко змінити назву рішення.
Запустіть VS та виберіть Create new project. Виберіть Class Library ➤ ProjectName: CommonSnappableTypes Solution Name: ExtendableApp ➤ Next ➤ Create

Щоб додати ще один проект до рішення, клацніть правою кнопкою миші на назві рішення (ExtendableApp) у Solution Explorer (або натисніть File ➤ Add ➤ New Project) та виберіть Add ➤ New Project ➤ Class Library ➤ Project name: CSharpSnapIn ➤ Create 
Далі додайте посилання на проект CommonSnappableTypes з проекту CSharpSnapIn. Щоб зробити це у Visual Studio, клацніть правою кнопкою миші на проекті CSharpSnapIn та виберіть Add ➤ Project Reference. У діалоговому вікні «Диспетчер посилань» установіть прапорець поруч із пунктом CommonSnappableTypes.

Останній проект, який потрібно додати, – це консольний застосунок .NET з назвою MyExtendableApp. Додайте посилання на проект CommonSnappableTypes і встановіть консольну програму як стартовий проект для рішення. Для цього клацніть правою кнопкою миші проект MyExtendableApp у Solution Explorer і виберіть «Set as StartUp Project».

### Налаштування залежностей збірки проекту

Коли Visual Studio отримує команду на запуск рішення, запускаючі проекти та всі проекти, на які посилаються, збираються, якщо виявлено будь-які зміни; проте будь-які проекти, на які не посилаються, не збираються. Це можна змінити, встановивши залежності проекту. Щоб це зробити, клацніть правою кнопкою миші на рішенні в Solution Explorer, виберіть Project Build Order (Порядок складання проекту), а потім у діалоговому вікні, що відкриється, виберіть вкладку Dependencies (Залежності) та виберіть проект MyExtendableApp.
Зверніть увагу, що проект CommonSnappableTypes вже вибрано, а прапорець увімкнено. Це тому, що на нього посилаються безпосередньо. Також установіть прапорець проекту CSharpSnapIn.
Тепер, щоразу, коли збирається проект MyExtendableApp, також збирається проект CSharpSnapIn.

## Додавання подій після збірки

Відкрийте властивості проекту для CSharpSnapIn (клацніть правою кнопкою миші Solution Explorer та виберіть Properties) та перейдіть Build ➤ Events. Використовуючи макроси, MSBuild завжди використовуватиме правильний шлях відносно файлів *.csproj.
У полі PostBuild введіть наступне:

```
copy $(TargetPath) $(SolutionDir)MyExtendableApp\$(OutDir)$(TargetFileName) /Y
```
Після цього в файлі проекту зявиться

```xml
  <Target Name="PostBuild" AfterTargets="PostBuildEvent">
    <Exec Command="copy $(TargetPath) $(SolutionDir)MyExtendableApp\$(OutDir)$(TargetFileName) /Y" />
  </Target>
```
Можете додадт ще один рядок 

```
copy $(TargetPath) $(SolutionDir)MyExtendableApp\$(TargetFileName) /Y
```
Після додавання цих команд подій після збірки, кожна збірка буде скопійована до каталогів проекту та виводу MyExtendableApp щоразу під час компіляції.

# Збірка CommonSnappableTypes

У проекті CommonSnappableTypes видаліть файл Class1.cs за замовчуванням, додайте новий файл інтерфейсу з назвою IAppFunctionality.cs та оновіть файл до наступного:

```cs
namespace CommonSnappableTypes;

public interface IAppFunctionality
{
    void DoIt();
}
```
Додайте файл класу з назвою CompanyInfoAttribute.cs та оновіть його до наступного:

```cs
namespace CommonSnappableTypes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CompanyInfoAttribute : Attribute
{
    public string CompanyName { get; set; } = null!;
    public string CompanyUrl { get; set; } = null!;
}
```
Інтерфейс IAppFunctionality надає поліморфний інтерфейс для всіх оснасток, які може використовувати розширювана програма. Оскільки цей приклад є суто ілюстративним, ви надаєте один метод з назвою DoIt().
Тип CompanyInfoAttribute – це користувацький атрибут, який можна застосувати до будь-якого типу класу, що має бути прив’язаний до контейнера. Як видно з визначення цього класу, [CompanyInfo] дозволяє розробнику оснащення надати деякі основні відомості про точку походження компонента.

# Збірка оснащення C#

У проекті CSharpSnapIn видаліть файл Class1.cs та додайте новий файл з назвою CSharpModule.cs.

```cs
using CommonSnappableTypes;

namespace CSharpSnapIn;
[CompanyInfo(CompanyName = "FooSoft", CompanyUrl = "www.FooSoft.com")]
public class CSharpModule : IAppFunctionality
{
    void IAppFunctionality.DoIt()
    {
        Console.WriteLine("You have just used the C# snap-in!");
    }
}
```
Зверніть увагу, що я вирішив використовувати явну реалізацію інтерфейсу для підтримки інтерфейсу IAppFunctionality. Це не є обов'язковим; проте ідея полягає в тому, що єдиною частиною системи, яка повинна безпосередньо взаємодіяти з цим типом інтерфейсу, є додаток отримуючий функціональність. Завдяки явному впровадженню цього інтерфейсу, метод DoIt() метод не надається безпосередньо з типу CSharpModule.

# Проект ExtendableApp

Проект MyExtendableApp посилається на проект CommonSnappableTypes . Почніть з оновлення операторів using у верхній частині класу Program.cs до наступного:

```cs
using System.Reflection;
using CommonSnappableTypes;
```

Створимо загальну логіку програми. Під час запуску програми вводиться назва бібілотеки  CSharpSnapIn і програма працює з її функціональностю.

```cs
string? typeName = "";
do
{
    Console.Write("\nEnter a snapin to load:");
    // Get name of type.
    typeName = Console.ReadLine();
    Console.Clear();

    // Try to display type.
    try
    {
        LoadExternalModule(typeName);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Sorry, can't find snapin");
    }

} while (true);

void LoadExternalModule(string? assemblyName)
{
    Console.WriteLine($"\tI use {assemblyName}");
}

```
```
Enter a snapin to load:CSharpSnapIn
```
```
        I use CSharpSnapIn

Enter a snapin to load:

```


Метод LoadExternalModule() виконує такі завдання:

    Динамічно завантажує вибрану збірку в пам'ять
    
    Визначає, чи містить збірка будь-які типи, що реалізують IAppFunctionality

    Створює тип за допомогою пізнього зв'язування

Якщо знайдено тип, що реалізує IAppFunctionality, викликається метод DoIt(), а потім надсилається до методу DisplayCompanyData() для виведення додаткової інформації з рефлексованого типу.

```cs
void LoadExternalModule(string? assemblyName)
{
    Console.WriteLine($"\tI use {assemblyName}");

    Assembly theSnapInAsm = null!;

    try
    {
        // Dynamically load the selected assembly.
        theSnapInAsm = Assembly.LoadFrom(assemblyName);
        Console.WriteLine($"\tI loded assembly: {theSnapInAsm}"  );

        // Get all IAppFunctionality compatible classes in assembly.
        var theClassTypes =
            theSnapInAsm.GetTypes()
            .Where(t => t.IsClass && (t.GetInterface("IAppFunctionality") != null))
            .ToList();
        if (!theClassTypes.Any())
        {
            Console.WriteLine("Nothing implements IAppFunctionality!");
        }
        // Now, create the object and call DoIt() method.
        foreach (var type in theClassTypes)
        {
            Console.WriteLine($"\tI find class {type}");
            // Use late binding to create the type.
            IAppFunctionality itfApp = (IAppFunctionality)theSnapInAsm.CreateInstance(type.FullName, true);
            itfApp?.DoIt();

            // Show company info.
            DisplayCompanyData(type);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred loading the snapin: {ex.Message}");
        return;
    }
}
```
Створіть метод DisplayCompanyData() наступним чином. Зверніть увагу, що цей метод приймає один параметр System.Type.

```cs
void DisplayCompanyData(Type type)
{
    // Get [CompanyInfo] data.
    var compInfo = type
        .GetCustomAttributes(false)
        .Where(ci => (ci is CompanyInfoAttribute));

    // Show data.
    foreach (CompanyInfoAttribute c in compInfo)
    {
        Console.WriteLine($"More info about {c.CompanyName} can be found at {c.CompanyUrl}");
    }
}
```
Запуствть програму і введіть CSharpSnapIn. 

```
        I use CSharpSnapIn
        I loded assembly: CSharpSnapIn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
        I find class CSharpSnapIn.CSharpModule
You have just used the C# snap-in!
More info about FooSoft can be found at www.FooSoft.com
```
Зверніть увагу, що хоча C# чутливий до регістру, під час використання відображення регістр не має значення. Як CSharpSnapIn, так і csharpsnapin працюють однаково добре.

Ви можете додати ще одну бібліотеку наприклад з назвою MyModule. Додати клас з реалізацією інтерфейсу IAppFunctionality.

```cs
using CommonSnappableTypes;

namespace MyModule;
[CompanyInfo(CompanyName = "MySoft", CompanyUrl = "www.MySoft.com")]
public class MyModule : IAppFunctionality
{
    public void DoIt()
    {
        Console.WriteLine("I want to make it different."  );
    }
}
```
Змінити файл проекту

```xml
  <Target Name="PostBuild" AfterTargets="PostBuildEvent">
    <Exec Command="copy $(TargetPath) $(SolutionDir)MyExtendableApp\$(OutDir)$(TargetFileName) /Y" />
  </Target>
```
Перебудуйте бібліотеку.
Не змініючи розширюваний додаток використати вашу реалізацію.
```
        I use MyModule
        I loded assembly: MyModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null loded
        I find class MyModule.MyModule
I want to do it different.
More info about MySoft can be found at www.MySoft.com

Enter a snapin to load:CSharpSnapIn
        I use CSharpSnapIn
        I loded assembly: CSharpSnapIn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null loded
        I find class CSharpSnapIn.CSharpModule
You have just used the C# snap-in!
More info about FooSoft can be found at www.FooSoft.com

Enter a snapin to load:
```
