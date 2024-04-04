# Атрібути

Для початку зрозумійте шо метадані це дані, що характеризують або пояснюють інші дані. Однією з функцій компілятора .NET є генерація описів метаданих для всіх визначених і посилальних типів. На додаток до цих стандартних метаданих, що містяться в будь-якій збірці, платформа .NET надає програмістам можливість вбудовувати додаткові метадані в збірку за допомогою атрибутів. Атрибути — це не що інше, як анотації коду, які можна застосувати до заданого типу (класу, інтерфейсу, структури тощо), члена (властивості, методу тощо), збірки чи модуля.

Атрибути .NET — це типи класів, які розширюють абстрактний базовий клас System.Attribute. Досліджуючи простори імен .NET, ви знайдете багато попередньо визначених атрибутів, які можна використовувати у своїх програмах. Крім того, ви можете створювати власні атрибути для подальшої кваліфікації поведінки ваших типів, створюючи новий тип, похідний від Attribute. Бібліотека базових класів .NET надає атрибути в різних просторах імен.

Приклади вбудованих атрибутів.

[CLSCompliant] : Забезпечує відповідність анотованого елемента правилам загальномовної специфікації (CLS). Пам’ятайте, що CLS-сумісні типи гарантовано безпроблемно використовуватимуться в усіх мовах програмування .NET.

[DllImport] : Дозволяє коду .NET здійснювати виклики до будь-якої некерованої бібліотеки коду на основі C або C++, включаючи API основної операційної системи.

[Obsolete] : Позначає застарілий тип або член. Якщо інші програмісти спробують використати такий елемент, вони отримають попередження компілятора з описом помилки своїх шляхів.

Коли ви застосовуєте атрибути у своєму коді, вбудовані метадані, по суті, марні, доки інша частина програмного забезпечення явно не відобразить інформацію. Якщо це не так, анотація метаданих, вбудованих у збірку, ігнорується та є абсолютно нешкідливим.

### Що використовує атрібути.

Як ви здогадалися, .NET Framework постачається з численними утилітами, які справді шукають різні атрибути. Сам компілятор C# (csc.exe) був попередньо запрограмований на виявлення наявності різних атрибутів під час циклу компіляції. Наприклад, якщо компілятор C# зустрічає атрибут [CLSCompliant], він автоматично перевіряє атрибутований елемент, щоб переконатися, що він надає лише CLS-сумісні конструкції. Як інший приклад, якщо компілятор C# виявляє елемент, якому присвоєно атрибут [Obsolete], він відобразить попередження компілятора.
На додаток до засобів розробки, численні методи в бібліотеках базових класів .NET попередньо запрограмовані для відображення конкретних атрибутів. В наступних розділах представлено серіалізацію XML і JSON, обидва з яких використовують атрибути для керування процесом серіалізації.
Ви можете створювати програми, які запрограмовані на відображення ваших власних атрибутів, а також будь-яких атрибутів у бібліотеках базових класів .NET. Роблячи це, ви, по суті, можете створити набір «ключових слів», які розуміються певним набором збірок.
 
## Застосування атрибутів.

Створемо проект та додамо пакет NuGet System.Text.Json.  Припустімо, ви хочете створити клас під назвою Motorcycle які можна зберегти у форматі JSON. Якщо у вас є поле, яке не слід експортувати в JSON, ви можете застосувати атрибут [JsonIgnore].

ApplyingAttributes\Motorcycle.cs

```cs
internal class Motorcycle
{
    [JsonIgnore]
    public float weightOfCurrentPassengers; // This fild is unserializable
    // These fields are still serializable.
    public bool hasRadioSystem;
    public bool hasHeadSet;
    public bool hasSissyBar;
}

```
Атрибут застосовується до конкретного наступного елемента. Коли ви хочете застосувати атрибут, ім’я атрибута береться в квадратні дужки.
Один елемент може мати кілька атрибутів. Припустімо, що у вас є застарілий тип класу C# (HorseAndBuggy), якому було приписано власний простір імен XML. База коду змінювалася з часом, і тепер клас вважається застарілим для поточної розробки. Замість того, щоб видаляти визначення класу з бази коду (і ризикувати зламати існуюче програмне забезпечення), ви можете позначити клас атрибутом [Obsolete]. Щоб застосувати кілька атрибутів до одного елемента, просто використовуйте список, розділений комами або один над одним.

AttributedCarLibrary\HorseAndBuggy.cs
```cs
//[XmlRoot(Namespace = "http://www.MyCompany.com"), Obsolete("Use another vehicle!")]
[XmlRoot(Namespace = "http://www.MyCompany.com")]
[Obsolete("Use another vehicle!")]
internal class HorseAndBuggy
{
    //...
}
```
Якщо ви переглядали документацію .NET, ви могли помітити, що фактичне ім’я класу атрибута [Obsolete] є ObsoleteAttribute, а не Obsolete. Відповідно до домовленості про найменування, усі атрибути .NET суфіксуються маркером Attribute. Однак, щоб спростити процес застосування атрибутів, мова C# не вимагає від вас введення суфікса Attribute. Враховуючи це, наступна ітерація типу HorseAndBuggy ідентична попередньому прикладу 

```cs
[XmlRootAttribute(Namespace = "http://www.MyCompany.com")]
[ObsoleteAttribute("Use another vehicle!")]
internal class HorseAndBuggy
{
    //...
}
```

### Параметри атрибутів. 

Зверніть увагу, що атрибут [Obsolete] може приймати те, що виявляється параметром конструктора. Якщо ви переглянете формальне визначення атрибута [Obsolete], то побачите, що цей клас справді надає конструктор, який отримує System.String

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
Коли ви надаєте параметри конструктора атрибуту, атрибут не виділяється в пам’ять, доки параметри не будуть відображені іншим типом або зовнішнім інструментом. Рядкові дані, визначені на рівні атрибутів, просто зберігаються в збірці як анотація метаданих.

### Атрібут [Obsolete] в дії.

Спробуємо створити примірник класу HorseAndBuggy який визначчений атрибутом [Obsolete].

AttributedCarLibrary\Program.cs
```cs
HorseAndBuggy horseAndBuggy = new();
```
Коли ви створете екземпляр класу компілятор виделить його з попередженням. 

```
CS0618: 'HorseAndBuggy' is obsolete: 'Use another vehicle!'
```

Таким чином.

Атрибути — це класи, які походять від System.Attribute.
Результатом атрибутів є вбудовані метадані класів або їх членів.
Атрибути застосовуються в C# за допомогою квадратних дужок.
Атрибути в основному марні, поки інший агент (включаючи IDE) не відобразить їх.

## Створення власних атрибутів.

При створені спеціального атрібуту створюється клас похідний від System.Attribute.

Стовримо проект AttributedCarLibrary. Ця збірка визначатиме кілька транспортних засобів, кожна з яких описана за допомогою спеціального атрибута з назвою VehicleDescriptionAttribute.

```cs
namespace AttributedCarLibrary;

public sealed class VehicleDescriptionAttribute :Attribute
{
    public string? Description { get; set; }

    public VehicleDescriptionAttribute(string description)
    {
        Description = description;
    }

    public VehicleDescriptionAttribute(){}
}
```
VehicleDescriptionAttribute зберігає частину рядкових даних за допомогою автоматичної властивості (Description).Крім того, що цей клас походить від System.Attribute, у цьому визначенні класу немає нічого унікального. З міркувань безпеки вважається найкращою практикою .NET розробляти всі спеціальні атрибути як sealed (запечатаний). 

Тепер коли ми маємо клас власного атрібути ми можемо використовувати його роблячи аннотації інших класів. 

```cs
namespace AttributedCarLibrary;

// Use "named property" 
[VehicleDescription(Description ="My rocking Harley")]
internal class Morotcycle
{
}
```
```cs
namespace AttributedCarLibrary;

[Obsolete("Use another vehicle!")]
[VehicleDescription("The old gray mare, she ain't what she used to be...")]
internal class HorseAndBuggy
{
}
```
```cs
namespace AttributedCarLibrary;

[VehicleDescription("A very long, slow, but feature-rich auto")]
internal class Winnebago
{
}
```
Зверніть увагу, що для Motorcycle призначається опис за допомогою нового фрагмента синтаксису атрибута, який називається іменованою властивістю. У конструкторі першого атрибута [VehicleDescription] ви встановлюєте основні рядкові дані за допомогою властивості Description. Типи HorseAndBuggy і Winnebago не використовують синтаксис іменованих властивостей і просто передають рядкові дані через спеціальний конструктор.


### Встановлення обмеження використання атрибутів.

За замовчуванням користувальницькі атрибути можна застосовувати майже до будь-якого аспекту вашого коду (методи, класи, властивості тощо). Таким чином, якщо це має сенс, ви можете використовувати VehicleDescription для визначення методів, властивостей або полів.

```cs
internal class Winnebago
{
    [VehicleDescription("My rocking CD player")]
    public void PlayMusic(bool On)
    {
        //...
    }
}
```
У деяких випадках це саме та поведінка, яка вам потрібна. Однак в інших випадках ви можете створити спеціальний атрибут, який можна застосувати лише до вибраних елементів коду.Якщо ви хочете обмежити область дії спеціального атрибута, вам потрібно буде застосувати атрибут [AttributeUsage] до визначення вашого спеціального атрибута.

```cs
// This time, we are using the AttributeUsage attribute
// to annotate our custom attribute.
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class VehicleDescriptionAttribute :Attribute
{
    public string? Description { get; set; }

    public VehicleDescriptionAttribute(string description)
    {
        Description = description;
    }

    public VehicleDescriptionAttribute(){}
}

```
Якщо ви хочете обмежити область дії спеціального атрибута, вам потрібно буде застосувати атрибут [AttributeUsage] до визначення вашого спеціального атрибута. Атрибут [AttributeUsage] дозволяє надавати будь-яку комбінацію значень (за допомогою операції OR) з enum AttributeTargets.

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
Крім того, [AttributeUsage] також дозволяє додатково встановити іменовану властивість (AllowMultiple), яка вказує, чи можна застосувати атрибут більше одного разу до того самого елемента (за замовчуванням false). Крім того, [AttributeUsage] дозволяє встановити, чи слід атрибут успадковувати похідними класами за допомогою властивості Inherited (за умовчанням встановлено true).

При цьому, якщо розробник намагався застосувати атрибут [VehicleDescription] до будь-чого, окрім класу чи структури, йому видається помилка під час компіляції. В класі Winnebago виникне помилка ослількі атребут не годиться для полів. 

## Атрібути рівня збірки.

Також можна застосувати атрибути до всіх типів у певній збірці за допомогою тегу [assembly:]. Наприклад, припустимо, що ви хочете переконатися, що кожен публічний член кожного публічного типу, визначеного у вашій збірці, сумісний із CLS. Для цього просто додайте наступний атрибут рівня складання у верхній частині будь-якого файлу вихідного коду C#, як це (поза будь-якими оголошеннями простору імен):

```cs
[assembly: CLSCompliant]
namespace AttributedCarLibrary;
///...
```
Усі атрибути рівня збірки або модуля мають бути перераховані попереду і за межами будь-якої області простору імен.

### Використання окремого файлу для атрибутів збірки.

Інший підхід полягає в додаванні нового файлу до вашого проекту з іменем, подібним до AssemblyAttributes.cs (буде працювати будь-яке ім’я, але це ім’я передає призначення файлу) і розміщення ваших атрибутів рівня збірки в цьому файлі. Розміщення атрибутів в окремому файлі полегшить іншим розробникам пошук атрибутів, застосованих до проекту. 
У .NET є дві важливі зміни. По-перше, файл AssemblyInfo.cs тепер автоматично генерується з властивостей проекту і не призначений для використання розробником. Друга (і пов’язана) зміна полягає в тому, що багато попередніх атрибутів рівня збірки (версія, компанія тощо) було замінено властивостями у файлі проекту.

Створемо файл AssemblyAttributes.cs

```cs
// List 'using' statements first.
// Now list any assembly- or module-level attributes.
// Enforce CLS compliance for all public types in this
// assembly.
[assembly: CLSCompliant(true)]
```
Якщо ви зараз додасте фрагмент коду, який виходить за межі специфікації CLS,  вам буде видано попередження компілятора.

```cs
public  class Winnebago
{
    //[VehicleDescription("My rocking CD player")]

    public ulong notCompliant; // ... Type is not CLS-compiant
    public void PlayMusic(bool On)
    {
        //...
    }

}
```

### Використання файлу проекту для атрибутів збірки.

Ми вже розлядали використання атрибутів на рівні проекту коли розглядали відкритя доступу до внутрішнього типу зовнішнім збіркам.

```
<ItemGroup>
  <AssemblyAttribute Include="System.Runtime.CompilerServices.InternalsVisibleToAttribute">
    <_Parameter1>CarClient</_Parameter1>
  </AssemblyAttribute>
</ItemGroup>
```
Існує підступ, оскільки таким чином можна використовувати лише однорядкові атрибути параметрів. Це стосується властивостей, які можна встановити на вкладці «Package» у властивостях проекту.

Встановимо деякі властивості (наприклад, Authors, Description), клацнувши правою кнопкою миші  проект у провіднику рішень, вибравши «Properties,», а потім клацнувши «Package».

Файл проекту вигляджатиме.
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <Authors>Alex</Authors>
    <Company>MySoft</Company>
    <Description>This is a simple car library with attributes</Description>
  </PropertyGroup>

</Project>
```

Після компіляції відкриємо файл AttributedCarLibrary.AssemblyInfo.cs file. в папці ...obj\Debug\netX.0

```
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Reflection;

[assembly: System.Reflection.AssemblyCompanyAttribute("MySoft")]
[assembly: System.Reflection.AssemblyConfigurationAttribute("Debug")]
[assembly: System.Reflection.AssemblyDescriptionAttribute("This is a simple car library with attributes")]
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.0.0")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.0+aa91760d8aee83c77f36eb3f33746f4da60a1baa")]
[assembly: System.Reflection.AssemblyProductAttribute("AttributedCarLibrary")]
[assembly: System.Reflection.AssemblyTitleAttribute("AttributedCarLibrary")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]

// Generated by the MSBuild WriteCodeFragment class.
```
Ви побачите властивості проекту як атрибути (на жаль, не дуже читабельні в цьому форматі). Як останнє заключне зауваження щодо атрибутів збірки, ви можете вимкнути створення класу AssemblyInfo.cs, якщо хочете самостійно керувати процесом.

## Рефлексія атрибутів за допомогою раннього зв’язування.

Атрибут є абсолютно марним, доки інше програмне забезпечення не рефлексує його значення. Після виявлення даного атрибута ця частина програмного забезпечення може вжити будь-які необхідні дії. Тепер, як і будь-яка програма, ця «інша частина програмного забезпечення» може виявити наявність спеціального атрибута за допомогою раннього або пізнього зв’язування. Якщо ви хочете використовувати раннє зв’язування, вам знадобиться, щоб клієнтська програма мала визначення атрибута під час компіляції (у цьому випадку VehicleDescriptionAttribute). З огляду на те, що збірка AttributedCarLibrary визначила цей спеціальний атрибут як публічний клас, раннє зв’язування є найкращим варіантом.

Створимо кліенську частину VehicleDescriptionAttributeReader та додаво посиланя на бібіліотеку AttributedCarLibrary.

```cs
using AttributedCarLibrary;

void ReflectOnAttributesUsingEarlyBinding()
{
    Type type = typeof(Winnebago);
    Console.WriteLine($"We have type {type}\n");

    object[] customAttributes = type.GetCustomAttributes(false);

    foreach (VehicleDescriptionAttribute item in customAttributes)
    {
        Console.WriteLine("\t"+item.Description);
    }

}
ReflectOnAttributesUsingEarlyBinding();
```
```
We have type AttributedCarLibrary.Winnebago

        A very long, slow, but feature-rich auto

```
Метод Type.GetCustomAttributes() повертає масив об’єктів, який представляє всі атрибути, застосовані до члена, представленого типом (параметр Boolean контролює, чи повинен пошук продовжуватися вгору по ланцюжку успадкування).

## Рефлексія атрибутів за допомогою пізнього зв’язування.

У попередньому прикладі використовувалося раннє зв’язування для друку даних опису автомобіля для типу Winnebago. Це стало можливим, оскільки тип класу VehicleDescriptionAttribute було визначено як відкритий член у збірці AttributedCarLibrary. Також можна використовувати динамічне завантаження та пізнє зв’язування для відображення атрибутів.

Створимо нове рішеня з проектом AttributeReaderLateBinding

```cs
using System.Reflection;

void ReflectAttributesUsingLateBinding()
{
	try
	{
		Assembly assembly = Assembly.LoadFrom("AttributedCarLibrary");

		Type? vehicleDesctiption = assembly.GetType("AttributedCarLibrary.VehicleDescriptionAttribute");
		Console.WriteLine(vehicleDesctiption);

        PropertyInfo? propertyInfoVehileDesc = vehicleDesctiption?.GetProperty("Description");
        Console.WriteLine(propertyInfoVehileDesc);

        Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			if (vehicleDesctiption == null) { return;}

			object[] objects = type.GetCustomAttributes(vehicleDesctiption, false);
			foreach (object obj in objects)
			{
                Console.WriteLine($"{type} - {propertyInfoVehileDesc?.GetValue(obj,null)}");
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
AttributedCarLibrary.VehicleDescriptionAttribute
System.String Description
AttributedCarLibrary.HorseAndBuggy - The old gray mare, she ain't what she used to be...
AttributedCarLibrary.Morotcycle - My rocking Harley
AttributedCarLibrary.Winnebago - A very long, slow, but feature-rich auto
```

Єдиним цікавим моментом є використання методу PropertyInfo.GetValue(), який використовується для запуску засобу доступу до властивості.
