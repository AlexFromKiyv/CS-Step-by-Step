# Додаток з можливістю розширення

Створимо програму, яка має можливість розширення функціональністю зовнішніх збірок.

Програму будемо складати з наступних частин.

    CommonSnappableTypes.dll: ця збірка містить визначення типів, які використовуватимуться кожним об’єктом оснащення, і на які безпосередньо посилатиметься програма.

    CSharpSnapIn.dll: оснащення, написане на C#, яке використовує типи CommonSnappableTypes.dll.

    MyExtendableApp.exe: консольна програма, яку можна розширити за допомогою функцій оснастки.

Ця програма використовуватиме динамічне завантаження, рефлексію та пізнє зв’язування, щоб динамічно отримати функціональні можливості збірок, про які вона не знає раніше.

Ми використовуємо консольні програми, щоб зосередитися на конкретних концепціях прикладу, у цьому випадку динамічного завантаження, відображення та пізнього зв’язування.

## Створення каркасy рішення рішення ExtendableApp.

У реальній розробці зазвичай є кілька проектів разом у рішенні.

1. Стовремо рішення ExtendableApp з проектом типу Console App з назвою MyExtendableApp. 
2. Додамо до рішення проект типу Class Library з назвою CommonSnappableTypes.(Правий клік на рішені > Add project)
3. Додамо посилання на проект CommonSnappableTypes із проекту MyExtendableApp. (Правий клік на проекті > Project Reference ... )
4. Додамо до рішення проект типу Class Library з назвою CSharpSnapIn.
5. Додамо посилання на проект CommonSnappableTypes із проекту CSharpSnapIn. 
 
### Налаштування залежностей при будувані проекту.

Коли Visual Studio отримує команду запустити рішення, проекти запуску та всі проекти, на які посилаються, будуються, якщо виявлено будь-які зміни; однак будь-які проекти без посилань не будуються. Це можна змінити, встановивши залежності проекту.

Для цього правий-клік на рішені, виберіть Project Build Order, а в діалоговому вікні, що з’явиться, виберіть вкладку Dependencies. Тут можна поставити прапорець на CSharpSnapIn.(він не має посилання безпосередньо). Тепер щоразу, коли створюється проект MyExtendableApp, також будуються проект CSharpSnapIn.

### Додавання дії після події будування рішення.

Відкрийте властивості проекту для CSharpSnapIn (клацніть правою кнопкою миші Solution Explorer і виберіть Properties) і перейдіть на сторінку Build > Events.
Перевага використання цих команд у подіях збірки полягає в тому, що вони не залежать від машини та працюють на відносних шляхах.

У полі Post-build event введіть наступне (у двох рядках):

```console
copy $(TargetPath) $(SolutionDir)MyExtendableApp\$(OutDir)$(TargetFileName) /Y
copy $(TargetPath) $(SolutionDir)MyExtendableApp\$(TargetFileName) /Y
```
Після того, як ви додасте ці команди подій після збірки, кожна збірка буде скопійована в каталоги проекту та виводу MyExtendableApp кожного разу, коли вони компілюються. Можете запустити додаток і переконатися шо в каталозі ..\ ExtendableApp\MyExtendableApp\bin\Debug\netX.0 є відповідні бібіліотеки.

## Створення складових. 

### Збірка CommonSnappableTypes.

У проекті CommonSnappableTypes  додайте новий файл інтерфейсу під назвою IAppFunctionality.cs і оновіть файл.
```cs
namespace CommonSnappableTypes;

public interface IAppFunctionality
{
    void DoIt();
}
```

Інтерфейс IAppFunctionality забезпечує поліморфний інтерфейс для всіх оснасток, які можуть використовуватися розширюваною програмою. Враховуючи, що цей приклад є суто ілюстративним, ви надаєте один метод під назвою DoIt().

Додамо клас з назвою CompanyInfoAttribute.

```cs
namespace CommonSnappableTypes;

[AttributeUsage(AttributeTargets.Class)]
internal class CompanyInfoAttribute : Attribute
{
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyUrl { get; set; } = string.Empty;
}
```
Тип CompanyInfoAttribute — це спеціальний атрибут, який можна застосувати до будь-якого типу класу, який потрібно закріпити в контейнері. Як ви можете зрозуміти з визначення цього класу, [CompanyInfo] дозволяє розробнику оснастки надати деякі основні відомості про точку походження компонента.

### Збірка CSharpSnapIn.

У проекті CSharpSnapIn видаліть файл Class1.cs і додайте новий файл із назвою CSharpModule.cs.
```cs
using CommonSnappableTypes;

namespace CSharpSnapIn;
[CompanyInfo(CompanyName ="MuCompany",CompanyUrl ="www.mycompany.com")]
public class CSharpModule : IAppFunctionality
{
    void IAppFunctionality.DoIt()
    {
        Console.WriteLine("You have just used the C# snap-in!");
    }
}

```
Зверніть увагу на явну реалізацію інтерфейсу під час реалізації інтерфейсу IAppFunctionality. Ідея полягає в тому, що єдиною частиною системи, яка повинна безпосередньо взаємодіяти з цим типом інтерфейсу, є програма шо пряцює з класом.Завдяки явній реалізації цього інтерфейсу метод DoIt() не відображається безпосередньо від типу CSharpModule.

### Проект MyExtendableApp.

При створені каркасу і консольного додадку ми встановили пряме посилання на збірку CommonSnappableTypes. Прямого зв'язку з збіркою яка розширяє додаток CSharpSnapIn немає.

Створимо частину програми яка буде отримувати назву збірки шо буде розширювати можливості програми.

MyExtendableApp\Program.cs

```cs
static void Run()
{
    string typeName = string.Empty;
    do
    {
        Console.WriteLine("\tResearch your type.\n");
        Console.Write("Enter a spanin to load:");
        typeName = Console.ReadLine()!;

        try
        {
            LoadExternalModule(typeName);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.Clear();
    } while (true);
}
Run();

static void LoadExternalModule(string assemblyName)
{
    Console.WriteLine(assemblyName);
    Console.ReadKey();
}

```
Протестуємо з використанням будьякої назви.


Змінемо метод LoadExternalModule() який буде виконувати наступні завдання:

1. Динамічно завантажує вибрану збірку в пам'ять.
2. Визначає, чи містить збірка будь-які типи, що реалізують IAppFunctionality
3. Створює тип за допомогою пізнього зв’язування
4. Якщо знайдено тип, що реалізує IAppFunctionality, викликається метод DoIt(), а потім  виводимо додаткову інформації про цей типу.

```cs
static void LoadExternalModule(string assemblyName)
{
    Console.Write($"\nFinding and loading the assembly {assemblyName}. ");
    Assembly? snapInAssembly = null;
    try
    {
        snapInAssembly = Assembly.LoadFrom(assemblyName);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    if (snapInAssembly != null)
    {
        Console.WriteLine("Ok. The assembly found.");
    }
    else
    {
        Console.WriteLine("Assembly is null");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("\nGet all IAppFunctionality compatible classes in assembly");

    List<Type> iAppFuncClasses = snapInAssembly
        .GetTypes()
        .Where(t => t.IsClass && t.GetInterface("IAppFunctionality") != null)
        .ToList();

    if (!iAppFuncClasses.Any())
    {
        Console.WriteLine("Nothing implements IAppFunctionality!");
        Console.ReadKey();
        return;
    }

    foreach (var type in iAppFuncClasses)
    {
        Console.WriteLine($"Here is type:{type}");
        // Use late binding to create the type.
        IAppFunctionality appFunc = (IAppFunctionality)snapInAssembly.CreateInstance(type.FullName!, true)!;
        appFunc.DoIt();
        DisplayCompanyData(type);
    }

    Console.ReadKey();
}

static void DisplayCompanyData(Type type)
{
    var companyInfos = type.GetCustomAttributes(false)
        .Where(ci => (ci is CompanyInfoAttribute));
    foreach (CompanyInfoAttribute ci in companyInfos)
    {
        Console.WriteLine($"\nCompany {ci.CompanyName} {ci.CompanyUrl}" );
    }
}
```
Під час запуску програми введіть CSharpSnapIn і подивіться, як програма працює. 

Зауважте, що хоча C# не чутливий до регістру, під час використання рефлексії регістр не має значення. І CSharpSnapIn, і csharpsnapin працюють однаково добре.

Таким чином можна використовувати пізне зв'язування.
