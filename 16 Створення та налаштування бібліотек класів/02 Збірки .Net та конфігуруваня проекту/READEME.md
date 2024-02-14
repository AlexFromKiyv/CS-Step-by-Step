# Збірки .Net та конфігуруваня проекту

Програми .NET створюються шляхом збирання з різних частин разом в об’єднання будь-якої кількості збірок. Простіше кажучи, збірка — це бінарний файл з версіями, що описує себе, і розміщений у середовищі виконання .NET. Формат збірки надає деякі переваги.

### Збірки сприяють повторному використанню коду.

Оскільки ви створювали проекти консольних додатків у попередніх розділах, могло здатися, що всі функціональні можливості додатків містяться у виконуваній збірці, яку ви створювали. Ваші програми використовували численні типи, що містяться в завжди доступних бібліотеках базових класів .NET. 
Як ви, можливо, знаєте, бібліотека коду (також називається бібліотекою класів) — це 
*.dll, яка містить типи, призначені для використання зовнішніми програмами. Коли ви створюєте виконувані збірки, ви, безсумнівно, будете використовувати численні поставляємі системою та спеціальні бібліотеки коду під час створення програми. Але бібліотека коду не обов’язково має мати розширення *.dll. Для виконуваної збірки цілком можливо (хоча, звичайно, не часто) використовувати типи, визначені в зовнішньому виконуваному файлі. У цьому світлі *.exe, на який посилається, також можна вважати бібліотекою коду. 
Незалежно від того, як упакована бібліотека коду, платформа .NET дозволяє повторно використовувати типи незалежно від мови. Наприклад, ви можете створити бібліотеку коду на C# та повторно використовувати цю бібліотеку в будь-якій іншій мові програмування .NET.Можна не тільки розподіляти типи між мовами, але й виводити з них. Інтерфейси, визначені в F#, можуть бути реалізовані за допомогою структур, визначених у C# і так далі. Справа в тому, що коли ви починаєте розбивати єдиний монолітний виконуваний файл на численні збірки .NET, ви досягаєте нейтральної щодо мови форми повторного використання коду.

### Збірки встановлюють межу типу.

Повне ім'я тпу скаладестья з назви простору імен та назви типу.

```cs
System.Console.WriteLine("Hi");
```
Збірка, в якій знаходиться тип, додатково встановлює ідентичність типу. Наприклад, якщо у вас є дві збірки з унікальними іменами (скажімо, MyCars.dll і YourCars.dll), які визначають простір імен (CarLibrary), що містить клас під назвою SportsCar, вони вважаються унікальними типами у всесвіті .NET.

### Збірка як одниця має версію.

Для збірки .Net присвоюється номер версії з чотирьох частин у формі <major>.<minor>.<build>.<revision>. ( Якшо ви явно не вказали за замовчуванням встановлюється версія 1.0.0.0). Це число дозволяє кільком версіям однієї збірки гармонійно співіснувати на одній машині.

### Збірки самі себе описують.

Збірки вважаються такими, що описують себе, частково тому, що вони записують у маніфест збірки кожну зовнішню збірку, до якої вони повинні мати доступ, щоб правильно функціонувати. Маніфест — це блок метаданих, який описує саму збірку (назва, версія, необхідні зовнішні збірки тощо).
На додаток до даних маніфесту збірка містить метадані, які описують склад (імена членів, реалізовані інтерфейси, базові класи, конструктори тощо) кожного типу, що міститься. Оскільки збірка задокументована в таких подробицях, .NET Runtime не звертається до системного реєстру Windows, щоб визначити її розташування (досить радикальний відхід від застарілої моделі програмування Microsoft COM). Це відокремлення від реєстру є одним із факторів, який дозволяє програмам .NET працювати в інших операційних системах, окрім Windows, а також підтримувати кілька версій .NET на одній машині.

## Фомат збірки .Net.

Структурно збірка склажається з такіх елементів.

    Файловий заголовок операційної системи (наприклад, Windows).

    Заголовок CLR

    Код CIL

    Метадані типів

    Маніфест

    Додаткові вбудовані ресурси

Для розгляду збірки потрібна утіліта dumpbin яку можна отримати вставновивши C++ profiling tools.  

Для цього в строчці пошуку з основного меню Visual Studio введіть C++ profiling tools.

Також потрібна бібліотека для дослідження. Можна взяти рішеня з цього розділу CreateLibrary яке має бібліотеку CarLibrary.dll.

### Файловий заголовок операційної системи.

Файловий заголовок операційної системи встановлює факт що збірку можна завантажувати і виконувати цільовою операційною системою. Ці дані заголовка також ідентифікують тип програми для розміщення в операційній системі.

Запустіть tools > Command line > Developer Command Prompt
Зайдіть в каталог CarLibrary\bin\Debug\netX.0> і превірте що там є бібіліотека (dir).
Запустіть команду 
```console
dumpbin /headers CarLibrary.dll
```
Це відображає інформацію про заголовок операційної системи збірки (показано нижче, якщо створено для Windows).Ось (часткова) інформація заголовка Windows для CarLibrary.dll:

```console
Dump of file CarLibrary.dll

PE signature found

File Type: DLL

FILE HEADER VALUES
             14C machine (x86)
               3 number of sections
        BE7C76DA time date stamp
               0 file pointer to symbol table
               0 number of symbols
              E0 size of optional header
            2022 characteristics
                   Executable
                   Application can handle large (>2GB) addresses
                   DLL

```
Більшості програмістів .NET ніколи не доведеться турбуватися про формат даних заголовка, вбудованих у збірку .NET.
Ця інформація використовується, коли операційна система завантажує двійковий образ у пам’ять.

### Файловий заголовок CLR.

Заголовок CLR — це блок даних, який усі збірки .NET повинні підтримувати (і підтримують завдяки компілятору C#), щоб розмістити їх у середовищі виконання .NET. У двох словах, цей заголовок визначає численні позначки, які дозволяють середовищу виконання розуміти макет керованого файлу. Наприклад, існують прапорці, які ідентифікують розташування метаданих і ресурсів у файлі, версію середовища виконання, на основі якого було створено збірку, значення (необов’язкового) відкритого ключа тощо.

```console
dumpbin /clrheader CarLibrary.dll
```

```console
Dump of file CarLibrary.dll

File Type: DLL

  clr Header:

              48 cb
            2.05 runtime version
            217C [     BE4] RVA [size] of MetaData Directory
               1 flags
                   IL Only
               0 entry point token
               0 [       0] RVA [size] of Resources Directory
               0 [       0] RVA [size] of StrongNameSignature Directory
               0 [       0] RVA [size] of CodeManagerTable Directory
               0 [       0] RVA [size] of VTableFixups Directory
               0 [       0] RVA [size] of ExportAddressTableJumps Directory
               0 [       0] RVA [size] of ManagedNativeHeader Directory


  Summary

        2000 .reloc
        2000 .rsrc
        2000 .text
```
Як розробнику .NET, вам не потрібно буде турбуватися про особливі деталі інформації заголовка CLR збірки. Просто зрозумійте, що кожна збірка .NET містить ці дані, які використовуються за лаштунками середовищем виконання .NET під час завантаження даних зображення в пам’ять.

### Код CIL, метадані типів та маніфест збірки.

У своїй основі збірка містить код CIL, який, як ви пам’ятаєте, є проміжною мовою, незалежною від платформи та ЦП. Під час виконання внутрішній CIL компілюється на льоту за допомогою своєчасного компілятора (just-in-time JIT) відповідно до інструкцій, що стосуються платформи та ЦП. Враховуючи цю структуру, збірки .NET справді можуть виконуватися на різних архітектурах, пристроях і операційних системах.

Збірка також містить метадані, які повністю описують формат типів, що містяться, а також формат зовнішніх типів, на які посилається ця збірка. .NET Runtime використовує ці метадані для визначення розташування типів (та їхніх членів) у двійковому файлі, розміщення типів у пам’яті та полегшення віддаленого виклику методів.

Збірка також повинна містити пов’язаний маніфест (також відомий як метадані складання).Маніфест документує кожен модуль у складі, встановлює версію збірки та документує будь-які зовнішні збірки, на які посилається поточна збірка.

### Додаткові ресурси для збірки.

Збірка .NET може містити будь-яку кількість вбудованих ресурсів, таких як піктограми програм, файли зображень, звукові кліпи або таблиці рядків. .NET підтримує супутникові збірки, які містять лише локалізовані ресурси. Це може бути корисним, якщо ви хочете розділити свої ресурси на основі певної культури (англійська, німецька тощо) для створення міжнародного програмного забезпечення.

### Що краще консольна програма або бібліотека?

Консольні програми мають єдину точку входу (або визначений метод Main(), або оператори верхнього рівня), можуть взаємодіяти з консоллю та запускатися безпосередньо з операційної системи.
Бібліотеки класів, з іншого боку, не мають точки входу і тому не можуть бути запущені безпосередньо. Вони використовуються для інкапсуляції логіки, користувацьких типів тощо, і на них посилаються інші бібліотеки класів та/або консольні програми.Іншими словами, бібліотеки класів використовуються для зберігання речей, про які щойно йшлось.

## Налаштування програм за допомогою файлів конфігурації.

Хоча можна зберегти всю інформацію, необхідну для вашої програми .NET, у вихідному коді, можливість змінювати певні значення під час виконання є життєво важливою для більшості програм. Одним із найпоширеніших варіантів є використання одного або кількох файлів конфігурації, надісланих (або розгорнутих) разом із виконуваним файлом програми.
Найпоширенішим методом налаштування програм .NET є файли JavaScript Object Notation (JSON). Це формат пари ім’я-значення, де кожен об’єкт укладено у фігурні дужки. Значення також можуть бути об’єктами, що використовують той самий формат пари ім’я-значення.

Створемо новий проект і додамо посилання на пакет .

```console
 dotnet new console -n WorkWithConfiguration -o .\WorkWithConfiguration
 dotnet add WorkWithConfiguration package Microsoft.Extensions.Configuration
 dotnet add WorkWithConfiguration package Microsoft.Extensions.Configuration.Binder
 dotnet add WorkWithConfiguration package Microsoft.Extensions.Configuration.Json
```
Це додає посилання на підсистему конфігурації, підсистему конфігурації .NET на основі файлів JSON і зв’язувальні розширення для конфігурації у вашому проекті. Почніть із додавання нового файлу JSON у свій проект під назвою <b>appsettings.json.</b>. Оновіть файл проекту, щоб переконатися, що файл завжди копіюється до вихідного каталогу під час створення проекту.
```
<ItemGroup>
  <None Update="appsettings.development.json">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </None>
  <None Update="appsettings.json">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </None>
</ItemGroup>
```   
Змінемо файл appsettings.json
```json
{
  "BaseCurrency": "Euro",
  "CarName": "Id.4"
}
```
Тепер данні з  конфігураційного можна отримати в програмі під час виконання.

```cs
using Microsoft.Extensions.Configuration;

void GetDataFromConfigFile()
{
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json",true,true)
        .Build();

       
    Console.WriteLine(configuration["BaseCurrency"]);
    
    Console.WriteLine(configuration.GetValue<string>("CarName"));
    Console.WriteLine(configuration.GetValue("BaseCurrency", "USD"));
    Console.WriteLine(configuration.GetValue("Currency", "USD"));
    Console.WriteLine(configuration.GetValue<string>("BusName"));
    Console.WriteLine(configuration.GetValue("BusName","Mercedes Sprinter"));
    
    Console.ReadLine();
}
GetDataFromConfigFile();

```
```console
Euro
Id.4
Euro
USD

Mercedes Sprinter

```
Тут у об'єкта ConfigurationBuilder викликається декілька методів. Шлях, за яким збірка конфігурації почне шукати додані файли, встановлюється за допомогою методу SetBasePath. Потім файл конфігурації додається за допомогою методу AddJsonFile(), який приймає три параметри. Перший параметр - це шлях і ім'я файлу. Оскільки цей файл знаходиться в тому самому місці, що й основний шлях, у рядку немає інформації про шлях, лише ім’я файлу. Другий параметр визначає, чи є файл необов’язковим (true) чи обов’язковим (false), а останній параметр визначає, чи має конфігурація використовувати засіб спостереження за файлами для пошуку будь-яких змін у файлі (true) чи ігнорувати будь-які зміни під час виконання (false). Створення конфігурації з екземпляром IConfiguration виконується за допомогою методу Build(). Цей екземпляр надає доступ до всіх кофігураційних значень.

Маючи екземпляр IConfiguration, ви можете отримати значення з файлів конфігурації декількома способами. Якщо назва запиту не існує в конфігурації, результат буде null.
Метод GetValue() розроблено для отриманя не простих значень які не є об'єктами.

## Кілька файлів конфігурації.

У систему конфігурації можна додати більше одного файлу конфігурації. Якщо використовується більше одного файлу, властивості конфігурації є адитивними, якщо жодні імена пар ім’я-значення не конфліктують. Коли виникає конфлікт імен, виграє останній.
Додамо ще один файл конфігурації.

Файл проекту.
```
<ItemGroup>
  <None Update="appsettings.development.json">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </None>
  <None Update="appsettings.json">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </None>
</ItemGroup>
```
Файл конфігурації appsettings.development.json.

```json
{
  "BaseCurrency": "USD",
  "Shop": "ElectroHit",
}
```
```cs
void MoreThanOneConfigFile()
{
    IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile("appsettings.development.json", true, true)
    .Build();

    Console.WriteLine(configuration.GetValue<string>("BaseCurrency"));
}
MoreThanOneConfigFile();
```
```
USD
```
Як бачимо бачимо значеня конфігурації береться з другого доданого файла.

## Об'єкти конфігурації.

У реальних проектах конфігурація програми зазвичай складніша, ніж окрема властивість.
Оновимо файл конфігурації

```json
{
  "BaseCurrency": "Euro",
  "CarName": "Id.4",
  "Car": {
    "Make": "VW",
    "Color": "Gray",
    "EngineType": "Electric"
  }
}
```
Додамо метод який повертає конфігурацію з одного файла.

```cs
IConfiguration GetConfiguration()
{
    IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .Build();

    return configuration;
}
```
Щоб отримати доступ до багаторівневих значень JSON, ключем для пошуку є ієрархія JSON, кожен рівень розділений двокрапками (:).

```cs
void SimpleGetMultilevelData()
{
    IConfiguration configuration = GetConfiguration();

    Console.WriteLine(configuration["Car:Make"]);

    IConfigurationSection section = configuration.GetSection("Car");

    Console.WriteLine(section["Color"]);
}
SimpleGetMultilevelData();
```
```
VW
Gray
```
Замість обходу ієрархії імен цілі розділи можна отримати за допомогою методу GetSection.Отримавши розділ, ви можете отримати значення з розділу за допомогою простого формату імені.

Також можно отримати відображеня об'єкта конфігурації в об'єкті програми. Можна використовувати метод Bind(), щоб зв’язати значення конфігурації з існуючим екземпляром об’єкта, або метод Get(), щоб створити новий екземпляр об’єкта. Вони схожі на метод GetValue(), але працюють з складними типами.

Створимо клас та застосуємо метод Bind.

Car.cs
```cs
namespace WorkWithConfiguration
{
    internal class Car
    {
        public string? Make { get; set; }
        public string? Color { get; set; }
        public string? EngineType { get; set; }
    }
}
```
```cs
void GetMultilevelData()
{
    IConfiguration configuration = GetConfiguration();

    IConfigurationSection section = configuration.GetSection("Car");

    Car car = new();
    section.Bind(car);
    Console.WriteLine(car.Make);
    Console.WriteLine(car.Color);
    Console.WriteLine(car.EngineType);
}
GetMultilevelData();
```
```
VW
Gray
Electric
```
Якщо розділ не конфігуровано, метод Bind() не оновлюватиме екземпляр об'єкту, але залишить усі властивості такими, якими вони існували до виклику Bind().

Створити об'єкт конфігурації можна інакше.

```cs
void CreateConfigurationObject()
{
    IConfiguration configuration = GetConfiguration();

    var configurationObject_1 = configuration.GetSection(nameof(Car)).Get(typeof(Car)) as Car;

    Console.WriteLine(configurationObject_1?.Make);
    Console.WriteLine(configurationObject_1?.Color);
    Console.WriteLine(configurationObject_1?.EngineType);
    Console.WriteLine();

    var configurationObject_2 = configuration.GetSection(nameof(Car)).Get<Car>();
    Console.WriteLine(configurationObject_2?.Make);
    Console.WriteLine(configurationObject_2?.Color);
    Console.WriteLine(configurationObject_2?.EngineType);

}
CreateConfigurationObject();
```
```
VW
Gray
Electric

VW
Gray
Electric
```
Метод Get створює новий екземпляр зазначеного типу з розділу конфігурації. Неузагальнена версія методу повертає тип object?, тому значення, що повертається, має бути приведене до певного типу перед використанням. Узагальнена версія повертає екземпляр зазначеного типу без необхідності виконувати приведення. Якщо розділ не знайдено, метод повертає значення null.

### Як виявити помилки при роботі з об'єктом конфігурації.


Оновимо файл конфігурації.

```json
{
  "BaseCurrency": "Euro",
  "CarName": "Id.4",
  "Car": {
    "Made": "VW",
    "color": "Gray",
    "engineType": "Electric",
    "PowerReserve": "425"
  }
}
```
Зверніть увагу замість Make - Made.
```cs
void BindAndGetAndReflection()
{
    
    IConfiguration configuration = GetConfiguration();

    Car configurationObject_1 = new();
    
    configuration.GetSection("car").Bind(configurationObject_1);
       
    Console.WriteLine(configurationObject_1?.Make);
    Console.WriteLine(configurationObject_1?.Color);
    Console.WriteLine(configurationObject_1?.EngineType);
    
    Console.WriteLine();

    var configurationObject_2 = configuration.GetSection(nameof(Car)).Get<Car>();
    Console.WriteLine(configurationObject_2?.Make);
    Console.WriteLine(configurationObject_2?.Color);
    Console.WriteLine(configurationObject_2?.EngineType);
}
BindAndGetAndReflection();
```
```

Gray
Electric


Gray
Electric
```
Методи Bind() і Get()/Get<T>() використовують reflection, щоб зіставити імена загальнодоступних властивостей класу з іменами в розділі конфігурації без урахування регістру. Якщо властивість у конфігурації не існує в класі (або ім’я написано по-іншому), тоді це конкретне значення конфігурації (за замовчуванням) ігнорується.

Методи Bind(), Get() і Get<T>() додатково можуть виконувати Action<BinderOptions> для подальшого вдосконалення процесу оновлення (Bind()) або створення (Get()/Get<T>()) екземпляра класу.
```cs
//
// Summary:
//     Options class used by the Microsoft.Extensions.Configuration.ConfigurationBinder.
public class BinderOptions
{
    //
    // Summary:
    //     When false (the default), the binder will only attempt to set public properties.
    //     If true, the binder will attempt to set all non read-only properties.
    public bool BindNonPublicProperties { get; set; }

    //
    // Summary:
    //     When false (the default), no exceptions are thrown when trying to convert a value
    //     or when a configuration key is found for which the provided model object does
    //     not have an appropriate property which matches the key's name. When true, an
    //     System.InvalidOperationException is thrown with a description of the error.
    public bool ErrorOnUnknownConfiguration { get; set; }
}
```
Якщо для параметра ErrorOnUnknownConfiguration встановлено значення true, тоді виникне виняткова ситуація InvalidOperationException, якщо конфігурація містить назву, яка не існує в моделі.

```cs
void BindValidation()
{
    IConfiguration configuration = GetConfiguration();

    try
    {
        IConfigurationSection section = configuration.GetSection(nameof(Car));
        Car? configurationObject = section.Get<Car>(t => t.ErrorOnUnknownConfiguration = true);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
BindValidation();

```
```
'ErrorOnUnknownConfiguration' was set on the provided BinderOptions, but the following properties were not found on the instance of WorkWithConfiguration.Car: 'Made', 'PowerReserve'
```

BindNonPublicProperties контролює прив’язування непублічні властивостей.
Змінемо клас Car
```cs
    internal class Car
    {
        private string? war_code { get; set; }
        public string? Make { get; set; }
        public string? Color { get; set; }
        public string? EngineType { get; set; }
        public string? GetWarCode() => war_code;
    }
```
Змінемо файл конфігурації.
```json
{
  "BaseCurrency": "Euro",
  "CarName": "Id.4",
  "Car": {
    "Made": "VW",
    "color": "Gray",
    "engineType": "Electric",
    "PowerReserve" : "425",
    "war_code" : "23345678"
  }
}
```
```cs
void PrivatePropertyFromConfiguration()
{
    IConfiguration configuration = GetConfiguration();
    IConfigurationSection section = configuration.GetSection(nameof(Car));

    Car? configurationObject = section.Get<Car>();
    Console.WriteLine(configurationObject?.GetWarCode());

    Car? configurationObject_1 = section.Get<Car>(t=>t.BindNonPublicProperties=true);
    Console.WriteLine(configurationObject_1?.GetWarCode());
}
PrivatePropertyFromConfiguration();
```
```

23345678
```
Таким чином якшо з конфігурації треба зчитувати непублічні властивості це треба вказувати.

Також є метод GetRequiredSection якій створить виняток, якщо розділ не налаштовано.

```cs
void UseGetRequiredSection()
{
    try
    {
        IConfiguration configuration = GetConfiguration();
        IConfigurationSection section = configuration.GetRequiredSection("Bus");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        throw;
    }
}
UseGetRequiredSection();
```
```
Section 'Bus' not found in configuration.
Unhandled exception. System.InvalidOperationException: Section 'Bus' not found in configuration.
```

## Додаткові параметри конфігурації.

Окрім використання конфігурації на основі файлів, є варіанти використання змінних середовища, сховища ключів Azure, аргументів командного рядка та багато іншого. Багато з них використовуються в ASP.NET Core.
