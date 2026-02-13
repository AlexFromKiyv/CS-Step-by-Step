# Атрібути

Як показано на початку цієї глави, однією з ролей компілятора .NET є генерування описів метаданих для всіх визначених та згаданих типів. Окрім цих стандартних метаданих, що містяться в будь-якій збірці, платформа .NET надає програмістам можливість вбудовувати додаткові метадані в збірку за допомогою атрибутів. Коротко кажучи, атрибути — це не що інше, як додадкова анотації складу коду, які можна застосувати до заданого типу (клас, інтерфейс, структура тощо), члена (властивість, метод тощо), збірки або модуля.
Атрибути .NET – це типи класів, що розширюють абстрактний базовий клас System.Attribute. Досліджуючи простори імен .NET, ви знайдете багато попередньо визначених атрибутів, які можна використовувати у своїх застосунках. Крім того, ви можете створювати власні атрибути для подальшої кваліфікації поведінки ваших типів, створюючи новий тип, похідний від Attribute. Бібліотека базових класів .NET надає атрибути в різних просторах імен.

Приклад атрибутів

|Атрібут|Опис|
|-------|----|
|[CLSCompliant]|Забезпечує відповідність анотованого елемента правилам Специфікації загальної мови програмування (CLS). Нагадаємо, що типи, сумісні з CLS, гарантовано використовуватимуться безперешкодно в усіх мовах програмування .NET.|
|[DllImport]|Дозволяє коду .NET здійснювати виклики до будь-якої некерованої бібліотеки коду на основі C або C++, включаючи API базової операційної системи.|
|[Obsolete]|Позначає застарілий тип або член. Якщо інші програмісти спробують використати такий елемент, вони отримають попередження компілятора з описом помилки їхніх дій.|

Зрозумійте, що коли ви застосовуєте атрибути у своєму коді, вбудовані метадані по суті марні, доки інший програмний продукт явно не зробить рефлексію і викоритстає цю інформацію. Якщо це не так, то анотація метаданих, вбудована в збірку, ігнорується та є абсолютно нешкідливою.

## Споживачі атрибутів

Як ви могли здогадатися, .NET постачається з численними утилітами, які дійсно шукають різні атрибути. Сам компілятор C# (csc.exe) був попередньо запрограмований на виявлення наявності різних атрибутів під час циклу компіляції. Наприклад, якщо компілятор C# виявляє атрибут [CLSCompliant], він автоматично перевіряє елемент з атрибутом, щоб переконатися, що він надає доступ лише до конструкцій, сумісних з CLS. Іншим прикладом, якщо компілятор C# виявляє елемент з атрибутом [Obsolete], він відображатиме попередження компілятора.
Окрім інструментів розробки, численні методи в бібліотеках базових класів .NET попередньо запрограмовані для відображення певних атрибутів. У наступній главі представлено серіалізацію XML та JSON, обидві з яких використовують атрибути для керування процесом серіалізації.
Зрештою, ви можете вільно створювати застосунки, запрограмовані на врахування ваших власних атрибутів, а також будь-яких атрибутів у бібліотеках базових класів .NET. Роблячи це, ви по суті можете створити набір «ключових слів», які розуміються певним набором збірок.

# Застосування атрибутів у C#

Щоб проілюструвати процес застосування атрибутів у C#, створіть новий проект консольної програми з назвою ApplyingAttributes та додайте посилання на пакет System.Text.Json NuGet. Оновіть файл Program.cs, включивши до нього такі глобальні оператори using:

```cs
global using System.Text.Json.Serialization;
global using System.Xml.Serialization;
```
Припустимо, ви хочете створити клас з назвою Motorcycle, який можна зберігати у форматі JSON. Якщо у вас є поле, яке не слід експортувати у JSON, ви можете застосувати атрибут [JsonIgnore].

```cs
namespace ApplyingAttributes;

public class Motorcycle
{
    [JsonIgnore]
    public float weightOfCurrentPassengers;
    // These fields are still serializable.
    public bool hasRadioSystem;
    public bool hasHeadSet;
    public bool hasSissyBar;
}
```
    Атрибут застосовується до «наступного» елемента.

Коли ви хочете застосувати атрибут, ім'я атрибута затиснуте в квадратних дужках.
Як ви можете здогадатися, одному елементу можна приписати кілька атрибутів. Припустимо, у вас є застарілий тип класу C# (HorseAndBuggy), якому було присвоєно власний простір імен XML. Кодова база з часом змінилася, і тепер клас вважається застарілим для поточної розробки. Замість того, щоб видаляти визначення класу з вашої кодової бази (і ризикувати зламати існуюче програмне забезпечення), ви можете позначити клас атрибутом [Obsolete]. Щоб застосувати кілька атрибутів до одного елемента, просто використовуйте список, розділений комами, ось так:

```cs
namespace ApplyingAttributes;

[XmlRoot(Namespace = "http://www.MyCompany.com"), Obsolete("Use another vehicle!")]
public class HorseAndBuggy
{
    // ...
}
```
Як альтернативу, ви також можете застосувати кілька атрибутів до одного елемента, об'єднавши кожен атрибут наступним чином:

```cs
namespace ApplyingAttributes;

[XmlRoot(Namespace = "http://www.MyCompany.com")]
[Obsolete("Use another vehicle!")]
public class HorseAndBuggy
{
    // ...
}
```
## Скорочена нотація атрибутів у C#

Якщо ви переглядали документацію .NET, ви могли помітити, що фактична назва класу атрибута [Obsolete] — ObsoleteAttribute, а не Obsolete. Згідно з домовленістю про іменування, всі атрибути .NET (включно з користувацькими атрибутами, які ви можете створити самостійно) мають суфікс токена Attribute. Однак, щоб спростити процес застосування атрибутів, мова C# не вимагає введення суфікса Attribute. З огляду на це, наступна ітерація типу HorseAndBuggy ідентична попередньому прикладу (вона лише включає кілька додаткових натискань клавіш):

```cs
[XmlRootAttribute(Namespace = "http://www.MyCompany.com")]
[ObsoleteAttribute("Use another vehicle!")]
public class HorseAndBuggy
{
  // ...
}
```
Майте на увазі, що це люб'язність, надана C#. Не всі мови .NET підтримують цей скорочений синтаксис атрибутів.

## Визначення параметрів конструктора для атрибутів

Зверніть увагу, що атрибут [Obsolete] може приймати те, що виглядає як параметр конструктора. Якщо ви переглянете формальне визначення атрибута [Obsolete], то побачите, що цей клас справді надає конструктор, який отримує System.String.

```cs
public sealed class ObsoleteAttribute : Attribute
{
  public ObsoleteAttribute();
  public ObsoleteAttribute(string? message);
  public ObsoleteAttribute(string? message, bool error);
  public string? Message { get; }
  public bool IsError { get; }
  public string DiagnosticId { get; set; }
  public string UrlFormat { get; set; }
}
```
Зрозумійте, що коли ви надаєте атрибуту параметри конструктора, атрибут не виділяється в пам'яті, доки параметри не будуть оброблені іншим типом або зовнішнім інструментом. Рядкові дані, визначені на рівні атрибутів, просто зберігаються в збірці як анотація метаданих.

## Атрибут Obsolete у дії

Тепер, коли HorseAndBuggy позначено як застарілий, якщо ви виділите екземпляр цього типу:

```cs
using ApplyingAttributes;

HorseAndBuggy mule = new HorseAndBuggy();
```
ви побачите, що видається попередження компілятора. Попередження стосується конкретно CS0618, і повідомлення містить інформацію, передану в атрибут. 
Visual Studio та Visual Studio Code також допомагають з IntelliSense, яка отримує інформацію через рефлексію.

В ідеалі, на цьому етапі ви повинні розуміти такі ключові моменти щодо атрибутів .NET:

    Атрибути – це класи, що походять від System.Attribute.
    Атрибути призводять до вбудованих метаданих.
    Атрибути фактично марні, доки інший агент (включаючи IDE) не використає їх.
    Атрибути застосовуються в C# за допомогою квадратних дужок.

Далі давайте розглянемо, як можна створювати власні атрибути та програмне забезпечення, яке відображає вбудовані метадані.

# Створення власних атрибутів

Першим кроком у створенні власного атрибута є створення нового класу, похідного від System.Attribute. Дотримуючись автомобільної теми, створимо новий проект C# Class Library з назвою AttributedCarLibrary в рішені AttributeExploration. 
Ця збірка визначить кілька транспортних засобів, кожен з яких описується за допомогою спеціального атрибута з назвою VehicleDescriptionAttribute, наступним чином:

```cs
namespace AttributedCarLibrary;
// A custom attribute.
public sealed class VehicleDescriptionAttribute : Attribute
{
    public string Description { get; set; } = null!;

    public VehicleDescriptionAttribute(string description)
    {
        Description = description;
    }

    public VehicleDescriptionAttribute()
    {
    }
}
```
Як бачите, VehicleDescriptionAttribute зберігає фрагмент рядкових даних за допомогою автоматичної властивості (Description). Окрім того факту, що цей клас походить від System.Attribute, у визначенні цього класу немає нічого унікального.

    З міркувань безпеки вважається найкращою практикою .NET розробляти всі користувацькі атрибути як sealed. Фактично, як Visual Studio, так і Visual Studio Code надають фрагмент коду з назвою Attribute, який створюватиме новий клас, похідний від System.Attribute, у вашому вікні коду. Ви можете розгорнути будь-який фрагмент, ввівши його назву та натиснувши клавішу Tab.

## Застосування користувацьких атрибутів

Оскільки VehicleDescriptionAttribute походить від System.Attribute, тепер ви можете анотувати свої транспортні засоби на свій розсуд. Оскільки VehicleDescriptionAttribute походить від System.Attribute, тепер ви можете анотувати свої транспортні засоби на свій розсуд. Для тестування додайте такі класи до вашої нової бібліотеки класів:

```cs
namespace AttributedCarLibrary;

// Assign description using a "named property."
[VehicleDescription(Description = "My rocking Harley")]
public class Motorcycle
{
}
```
```cs
namespace AttributedCarLibrary;

[Obsolete("Use another vehicle!")]
[VehicleDescription("The old gray mare, she ain't what she used to be...")]
public class HorseAndBuggy
{
}
```
```cs

[VehicleDescription("A very long, slow, but feature-rich auto")]
public class Winnebago
{
}
```

## Синтаксис іменованої властивості
Зверніть увагу, що опису мотоцикла присвоюється опис за допомогою нового фрагмента синтаксису атрибута, який називається іменованою властивістю. У конструкторі першого атрибута [VehicleDescription] ви встановлюєте базові рядкові дані за допомогою властивості Description. Якщо зовнішній агент використовує цей атрибут, значення передається у властивість Description (синтаксис іменованої властивості є допустимим, лише якщо атрибут надає властивість .NET, на яку можна записувати). 
На відміну від цього, типи HorseAndBuggy та Winnebago не використовують синтаксис іменованих властивостей і просто передають рядкові дані через власний конструктор. У будь-якому випадку, після компіляції збірки AttributedCarLibrary ви можете використовувати ildasm.exe для перегляду введених описів метаданих для вашого типу. Наприклад, нижче показано вбудований опис класу Winnebago:

```
// CustomAttribute #1
// -------------------------------------------------------
//   CustomAttribute Type: 06000005
//   CustomAttributeName: AttributedCarLibrary.VehicleDescriptionAttribute :: instance void .ctor(class System.String)
//   Length: 45
//   Value : 01 00 28 41 20 76 65 72  79 20 6c 6f 6e 67 2c 20 >  (A very long, <
//         : 73 6c 6f 77 2c 20 62 75  74 20 66 65 61 74 75 72 >slow, but feature<
//         : 65 2d 72 69 63 68 20 61  75 74 6f 00 00          >e-rich auto     <
//   ctor args: ('A very long, slow, but feature-rich auto')                                                                                    
```

# Обмеження використання атрибутів

За замовчуванням, власні атрибути можна застосовувати практично до будь-якого аспекту вашого коду (методів, класів, властивостей тощо). Таким чином, якщо це мало сенс, ви могли б використовувати VehicleDescription для кваліфікації методів, властивостей або полів (серед іншого).

```cs
[VehicleDescription("A very long, slow, but feature-rich auto")]
public class Winnebago
{
    [VehicleDescription("My rocking CD player")]
    public void PlayMusic(bool On)
    {
    }
}
```
У деяких випадках саме така поведінка вам потрібна. Однак, в інших випадках вам може знадобитися створити власний атрибут, який можна застосовувати лише до вибраних елементів коду. Якщо ви хочете обмежити область дії власного атрибута, вам потрібно застосувати атрибут [AttributeUsage] до визначення вашого власного атрибута. Атрибут [AttributeUsage] дозволяє вам передавати будь-яку комбінацію значень (через операцію OR) з переліку AttributeTargets :

```cs
// This enumeration defines the possible targets of an attribute.
public enum AttributeTargets
{
  All, Assembly, Class, Constructor,
  Delegate, Enum, Event, Field, GenericParameter,
  Interface, Method, Module, Parameter,
  Property, ReturnValue, Struct
}
```
Крім того, [AttributeUsage] також дозволяє вам за бажанням встановити іменовану властивість (AllowMultiple), яка вказує, чи можна застосовувати атрибут більше одного разу до одного елемента (значення за замовчуванням — false). Також, [AttributeUsage] дозволяє встановити, чи має атрибут успадковуватися похідними класами, використовуючи властивість Inherited named (за замовчуванням – true).
Щоб встановити, що атрибут [VehicleDescription] може бути застосований лише один раз до класу або структури, можна оновити визначення VehicleDescriptionAttribute наступним чином:

```cs
// This time, we are using the AttributeUsage attribute
// to annotate our custom attribute.
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class VehicleDescriptionAttribute : System.Attribute
{
...
}
```
Таким чином, якщо розробник намагався застосувати атрибут [VehicleDescription] до чогось, окрім класу чи структури, йому виникала помилка під час компіляції. В класі Winnebago виникне помилка ослількі атребут не годиться для методів.

## Використання атрибутів для валідації.

Атрібути це додадкові метадані яки можна додадти до існуючих і потім використовувати. Створемо новий проект консольного додадку під назвою AttributeForValidation. Додамо клас атрібута який характеризуватиме вікові рамки.

```cs
namespace AttributeForValidation;

internal class AgeValidationAttribute : Attribute
{
    public int From { get; set; } = 0;
    public int To { get; set; } = 130;

    public AgeValidationAttribute(int from, int to)
    {
        From = from;
        To = to;
    }

    public AgeValidationAttribute()
    {
    }
}
```
Додамо клас до якого застосовується атрібут.

```cs
namespace AttributeForValidation;

[AgeValidation(25, 60)]
internal class Warrior
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Warrior(string name, int age)
    {
        Name = name;
        Age = age;
    }
}
```
Створимо в Program.cs метод який буде перевіряти чи відповідає властивість Age даним встановленим в атрибуті.

```cs
using AttributeForValidation;
using System.Reflection;

static bool AgeValidationForWarrior(Warrior warrior)
{
    Type typeWarrior = warrior.GetType();

    AgeValidationAttribute? attribute = typeWarrior.GetCustomAttribute<AgeValidationAttribute>();    

    if (attribute != null)
    {
        return attribute.From <= warrior.Age && warrior.Age < attribute.To;
    }
    return true;
}

```

Додамо метода який буде робить запит і відповідати.

```cs
static void IsWarriorAgeAppropriate(Warrior warrior)
{
    string result = AgeValidationForWarrior(warrior) ? "is" : "is not";

    Console.WriteLine($"{warrior.Name} {warrior.Age} as a warrior {result} age appropriate.");
}
```
Тепер перевіремо ці методи на об'єктах.

```cs
void WarriorValidation()
{
    Warrior max = new("Max", 58);

    IsWarriorAgeAppropriate(max);

    Warrior julia = new("Julia", 23);

    IsWarriorAgeAppropriate(julia);
}
WarriorValidation();
```
```
Max 58 as a warrior is age appropriate.
Julia 23 as a warrior is not age appropriate.
```
Таким чином стоврюючи власні атрибути і використовуючи GetCustomAttributes можна виконувати валідацію об'єктів.

# Атрибути рівня збірки

Також можна застосовувати атрибути до всіх типів у заданій збірці за допомогою тегу [assembly:]. Наприклад, припустимо, що ви хочете переконатися, що кожен публічний член кожного публічного типу, визначеного у вашій збірці, сумісний з CLS. Для цього просто додайте наступний атрибут рівня збірки на початку будь-якого файлу вихідного коду C#, ось так (за будь-якими оголошеннями простору імен):

```cs
[assembly: CLSCompliant(true)]
namespace AttributedCarLibrary;
```
    Усі атрибути рівня збірки або модуля мають бути перелічені поза межами будь-якої області видимості простору імен.

## Використання окремого файлу для атрибутів збірки

Інший підхід полягає в тому, щоб додати до вашого проекту новий файл з назвою, подібною до AssemblyAttributes.cs (будь-яка назва підійде, але вона передає призначення файлу) та розмістити атрибути рівня збірки в цьому файлі. Розміщення атрибутів в окремому файлі полегшить іншим розробникам пошук атрибутів, застосованих до проєкту. Створіть новий файл з назвою AssemblyAttributes.cs та оновіть його відповідно до наступного:

```cs
// Now list any assembly- or module-level attributes.
// Enforce CLS compliance for all public types in this
// assembly.
[assembly: CLSCompliant(true)]
```
Якщо ви тепер додасте фрагмент коду, який виходить за межі специфікації CLS (наприклад, відкриту точку непідписаних даних), вам буде видано попередження компілятора.

```cs
public class Winnebago
{
    public ulong NotCompliant;
    //...
}
```

## Використання файлу проекту для атрибутів збірки

Як показано в главі "Створення та налаштування бібліотек класів" з InternalsVisibleToAttribute, атрибути збірки також можна додавати до файлу проекту. Є один нюанс: таким чином можна використовувати лише атрибути параметрів з одним рядком. Це стосується властивостей, які можна встановити на вкладці «Packeg» у властивостях проекту.
Спробуйте встановити деякі властивості (наприклад, Автори, Опис), клацнувши правою кнопкою миші на проекті в Solution Explorer, вибравши Properties, а потім клацнувши Package. Також додайте InternalsVisibleToAttribute, як ви робили. Тепер ваш файл проекту виглядатиме приблизно так:
```xml
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <Title>IamExploringAttributes</Title>
    <Authors>Alex Po</Authors>
    <Company>BusinessSoft</Company>
    <Product>VehileLibrary</Product>
  </PropertyGroup>
```
Після компіляції проєкту перейдіть до каталогу \obj\Debug\netX.0 та знайдіть файл AttributedCarLibrary.AssemblyInfo.cs. Відкрийте його, і ви побачите ці властивості як атрибути (на жаль, у цьому форматі вони не дуже читабельні).

```cs
[assembly: System.Reflection.AssemblyCompanyAttribute("BusinessSoft")]
[assembly: System.Reflection.AssemblyConfigurationAttribute("Debug")]
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.0.0")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.0+73f108779031b863507e5f6bf900f2d723b1d047")]
[assembly: System.Reflection.AssemblyProductAttribute("VehileLibrary")]
[assembly: System.Reflection.AssemblyTitleAttribute("AttributedCarLibrary")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
```
Як останнє зауваження щодо атрибутів збірки, ви можете вимкнути генерацію класу AssemblyInfo.cs, якщо хочете керувати процесом самостійно.

# Рефлексія атрибутів за допомогою раннього зв'язування.

Пам’ятайте, що атрибут є абсолютно марним, доки інший програмний компонент не визначить його значення. Після виявлення певного атрибута ця програма може вжити будь-яких необхідних дій. Тепер, як і будь-яка програма, цей «інший фрагмент програмного забезпечення» може виявити наявність спеціально створенного атрибута за допомогою раннього або пізнього зв'язування. Якщо ви хочете використовувати раннє зв'язування, вам знадобиться, щоб клієнтська програма мала визначення відповідного атрибута під час компіляції (у цьому випадку VehicleDescriptionAttribute). Враховуючи, що збірка AttributedCarLibrary визначила цей користувацький атрибут як публічний клас, раннє зв'язування є найкращим варіантом.

Щоб проілюструвати процес рефлексії користувацьких атрибутів, додайте до рішення новий проект консольного застосунку C# з назвою VehicleDescriptionAttributeReader. Далі додайте посилання на проект AttributedCarLibrary. 
Це все можно зробити за допомогою CLI в каталозі рішення:

```
dotnet new console -n VehicleDescriptionAttributeReader
dotnet sln add .\VehicleDescriptionAttributeReader
dotnet add VehicleDescriptionAttributeReader reference .\AttributedCarLibrary
```
Оновіть файл Program.cs за допомогою наступного коду:

```cs
using AttributedCarLibrary;
static void ReflectOnAttributesUsingEarlyBinding()
{
    // Get a Type representing the Winnebago.
    Type type = typeof(HorseAndBuggy);

    object[] customAttributes = type.GetCustomAttributes(false);

    // Print the description.
    foreach (var customAttribute in customAttributes)
    {
        Console.Write(customAttribute);

        if (customAttribute is VehicleDescriptionAttribute vehicleDescriptionAttribute )
        {
            Console.Write($"\t{vehicleDescriptionAttribute.Description}");
        }
        else
        {
            Console.WriteLine();
        }    
    }
}
ReflectOnAttributesUsingEarlyBinding();
```
```
System.ObsoleteAttribute
AttributedCarLibrary.VehicleDescriptionAttribute        The old gray mare, she ain't what she used to be...
```
Метод Type.GetCustomAttributes() повертає масив об'єктів, який представляє всі атрибути, застосовані до члена, представленого типом Type (логічний параметр визначає, чи слід розширювати пошук угору по ланцюжку успадкування). Після отримання списку атрибутів, переберіть кожен екземпляр класу VehicleDescriptionAttribute та виведіть значення, отримане властивістю Description.

# Рефлексія атрибутів за допомогою пізнього зв'язування

У попередньому прикладі використовувалося раннє зв'язування для виведення даних опису транспортного засобу для типу. Це стало можливим завдяки тому, що тип класу VehicleDescriptionAttribute був визначений як публічний член у збірці AttributedCarLibrary. Також можливо використовувати динамічне завантаження та пізнє зв'язування для відображення атрибутів.

Додайте до рішення новий проект під назвою AttributeReaderLateBinding запустіть і скопіюйте AttributedCarLibrary.dll до папки проекту (або \bin\Debug\netX.0, якщо використовується Visual Studio).

```cs
using System.Reflection;

static void ReflectAttributesUsingLateBinding()
{
    try
    {
        // Load the local copy of AttributedCarLibrary.
        Assembly asm = Assembly.LoadFrom("AttributedCarLibrary");

        // Get type info of VehicleDescriptionAttribute.
        Type vehicleDesc = asm.GetType("AttributedCarLibrary.VehicleDescriptionAttribute");

        // Get type info of the Description property.
        PropertyInfo propDesc = vehicleDesc.GetProperty("Description");

        // Get all types in the assembly.
        Type[] types = asm.GetTypes();

        foreach (var type in types)
        {
            object[] objects = type.GetCustomAttributes(vehicleDesc, true);
            // Iterate over each VehicleDescriptionAttribute and print
            // the description using late binding.
            foreach (object o in objects)
            {
                Console.WriteLine($"{type.Name}: {propDesc?.GetValue(o, null)}\n");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}
ReflectAttributesUsingLateBinding();
```
```
HorseAndBuggy: The old gray mare, she ain't what she used to be...

Motorcycle: My rocking Harley

Winnebago: A very long, slow, but feature-rich auto
```
Якщо ви змогли слідкувати за прикладами в цієї глави, цей код має бути (більш-менш) зрозумілим без пояснень. Єдиним цікавим моментом є використання методу PropertyInfo.GetValue(), який використовується для запуску аксессора властивості.