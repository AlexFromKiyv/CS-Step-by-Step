# Визначення власних namespaces.

Перш ніж створювати бібліотеку треба упаковати ваші користувацьки типи в namespace(простір імен) .Net. Коли створюеться масштабний проект з багатьма типами може бути корисним згрупувати повєязані типи в спеціальні простори імен. Це досягаеться за допомогою ключового слова <b>namespace</b>. Явне визначення спеціальних namespace ще важливіше під час створення спільних збірок, оскільки іншим розробникам потрібно буде посилатися на бібліотеку і імпортувати ваші namespace, щоб використовувати ваші типи. Кастомні namespaces також запобігають суперечності імен, відокремлюючи ваші кастомні класи, які можуть мати таке саме ім'я.
Нехай нам потрібні стоврювати геометрічни класи Square, Circle та Hexagon. Враховуючи їх природу і схожість ви хотіли б згрупувати їх в унікальний простір імен під назвою CustomNamespaces.MyShapes в збірці CustomNamespaces.exe

Хоча ви можете вільно використовувати будь-яке ім’я, яке ви виберете для своїх просторів імен, угода про іменування зазвичай подібна до CompanyName.ProductName.AssemblyName.Path.

Хоча компілятору не важливо чи визначаєте ви власні типи в одному файлі або кожен тип в окремому, це може бути важливо при роботі в команді. Якщо ви працюєте над класом Circle, а вашому колезі потрібно попрацювати над класом Hexagon, вам доведеться по черзі працювати над монолітним файлом або зіткнутися зі складними для вирішення (ну, принаймні трудомісткими) конфліктами злиття. Кращим підходом є розміщення кожного класу в окремому файлі з визначенням простору імен. Щоб гарантувати, що кожен тип упаковано в ту саму логічну групу, просто оберніть дані визначення класу в одну область namespace.

В файлі типу просто оголошуеться namespace , і все, що йде після цього оголошення простору імен, буде включено в простір імен.

CustomNamespaces\Circle.cs
```cs
namespace CustomNamespaces.MyShape;
public class Circle
{
    public Circle()
    {
    }

    public Circle(int radius)
    {
        Radius = radius;
    }

    public int Radius { get; set; }
    // Here are its members.

    public void InfoAboutShape()
    {
        Console.WriteLine($"The shape is circle with radius: {Radius}");
    }
}
```
CustomNamespaces\Hexagon.cs
```cs
namespace CustomNamespaces.MyShape;
public class Hexagon
{
    // Here are its members.
}
```
CustomNamespaces\Square.cs
```cs
namespace CustomNamespaces.MyShape;
public class Square
{
    // Here are its members.
}
```
CustomNamespaces\Program.cs
```cs
// Make use of types defined the MyShapes namespace.
using CustomNamespaces.MyShape;

void CreateCircle()
{
    Circle circle = new Circle();
    circle.InfoAboutShape();
}
CreateCircle();
```
```
The shape is circle with radius: 0
```
Зверніть увагу, як простір імен CustomNamespaces.MyShapes діє як концептуальний «контейнер» цих класів. Коли інший простір імен (наприклад, CustomNamespaces) хоче використовувати типи в окремому просторі імен, ви використовуєте ключове слово using так само, як і під час використання просторів імен бібліотек базових класів .NET.
У цьому прикладі передбачається, що файли C#, які визначають простір імен CustomNamespaces.MyShapes, є частиною того самого проекту консольної програми; іншими словами, усі файли компілюються в одну збірку. Якщо ви визначили простір імен CustomNamespaces.MyShapes у зовнішній збірці, вам також потрібно буде додати посилання на цю бібліотеку, перш ніж ви зможете успішно компілювати.

## Вирішення конфліктів імен.

Для того щоб посилатися на тип визначений в зовнішньому namespace не обов'язково використовувати using. Можна використати повне ім'я.

```cs
// Make use of types defined the MyShapes namespace.
////using CustomNamespaces.MyShape;

...

void CreateSquare()
{
    // Note we are not importing CustomNamespaces.MyShapes anymore!
    CustomNamespaces.MyShape.Circle circle = new CustomNamespaces.MyShape.Circle(5);
    CustomNamespaces.MyShape.Square square = new CustomNamespaces.MyShape.Square();
    CustomNamespaces.MyShape.Hexagon hexagon = new CustomNamespaces.MyShape.Hexagon();
    Console.WriteLine(circle);
    Console.WriteLine(square);
    Console.WriteLine(hexagon);
}
CreateSquare();
```
```
CustomNamespaces.MyShape.Circle
CustomNamespaces.MyShape.Square
CustomNamespaces.MyShape.Hexagon

```
Як правило, немає необхідності використовувати повне ім’я. Немає різнийці як визначати це не впливає на кількість коду і швидкість. Вкористання using просто економить час.

Припустімо, що у вас є новий простір імен під назвою CustomNamespaces.My3DShapes, який визначає наступні три класи, здатні відтворювати форму в 3D.

Circle3D.cs
```cs
namespace CustomNamespaces.My3DShape;
public class Circle
{
    public Circle()
    {
    }

    public Circle(int radius)
    {
        Radius = radius;
    }

    public int Radius { get; set; }
    // Here are its members.

    public void InfoAboutShape()
    {
        Console.WriteLine($"The shape is 3d circle with radius: {Radius}");
    }
}
```
Square3D.cs
```cs
namespace CustomNamespaces.My3DShape;
public class Square
{
    // Here are its members.
}
```
Тоді при визначені типа може винивкнути комфлікт імен.

```cs
using CustomNamespaces.MyShape;
using CustomNamespaces.My3DShape;

void CreateCircle()
{
    Circle circle = new Circle(); // Compiler error!
    circle.InfoAboutShape();
}
CreateCircle();
```

Повні імена можуть бути корисними (і іноді необхідними), щоб уникнути потенційних конфліктів імен під час використання кількох просторів імен, які містять однакові імена типів.

```cs
void CreateAnyCircle()
{
    CustomNamespaces.MyShape.Circle circle = new();
    CustomNamespaces.My3DShape.Circle circle_3d = new();

    circle.InfoAboutShape();
    circle_3d.InfoAboutShape();
}
CreateAnyCircle();
```
```
The shape is circle with radius: 0
The shape is 3d circle with radius: 0
```
Ключове слово C# using також дозволяє створити псевдонім для повного імені типу.Це визначає маркер, який замінює повне ім’я типу під час компіляції.
```cs
using The3DCircle = CustomNamespaces.My3DShape.Circle;
```
Існує інший (частіше використовуваний) синтаксис використання, який дозволяє створити псевдонім для простору імен замість типу.
```cs
// Resolve the ambiguity for a type using a custom alias.

using ThreeDShapes = CustomNamespaces.My3DShape;

// ...

void CreateCircleWithAlias()
{
    ThreeDShapes.Circle circle1 = new();
    circle1.InfoAboutShape();
}
CreateCircleWithAlias();
```
```
The shape is 3d circle with radius: 0
```
Майте на увазі, що надмірне використання псевдонімів C# для типів може призвести до заплутаної бази коду. Якщо інші програмісти у вашій команді не знають про ваші спеціальні псевдоніми для типів, вони можуть мати труднощі з пошуком справжніх типів у проекті(ах).

## Створення вкладених namespaces.

Упорядковуючи типи, ви можете визначати простори імен в інших просторах імен. Бібліотеки базових класів роблять це в багатьох місцях, щоб забезпечити більш глибокі рівні організації типів. Наприклад, простір імен IO вкладено в System, щоб отримати System.IO. Фактично, ви вже створили вкладені простори імен у попередньому прикладі. Багатокомпонентні простори імен CustomNamespaces.MyShapes та CustomNamespaces.My3DShapes вкладено в кореневий простір імен CustomNamespaces.
Файл Program.cs неявно належить до простору імен який називають корневим. За замовчуванням він співпадає з назвою проекту. У нашому поточному прикладі кореневим простором імен, створеним шаблоном .NET, є CustomNamespaces.
Вкладення одного просторо в інший можна декількома способами.

```cs
namespace CustomNamespaces;
namespace MyShapes
{
 
}
```

Кращий варіант.

```cs
namespace CustomNamespaces.MyShapes;

public class Circle
{

}
```
Загальною практикою є групування файлів у просторі імен за каталогами. Розташування файлу в структурі каталогів не впливає на простори імен. Таким чином, багато розробників і інструменти для лінінгування коду очікують, що простори імен відповідатимуть структурам папок.

Iм’я кореневого простору імен вашої програми за замовчуванням є ідентичним імені проекту. З цього моменту, коли ви використовуєте Visual Studio для вставлення нових файлів коду за допомогою пункту меню Project ➤ Add New Item , типи автоматично розміщуватимуться в кореневому просторі імен і додадуть шлях до каталогу.

В файл проекту, в данному прикладі \CustomNamespaces.csproj можна додати рядок

<RootNamespace>PerformeShape</RootNamespace>

Тоді будуть додаватись нове значення простору імен.
