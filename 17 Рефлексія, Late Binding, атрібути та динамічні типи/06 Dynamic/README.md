# Dynamic

Ключове слово var дозволяє визначати локальні змінні таким чином, що базовий тип даних визначається під час компіляції на основі початкового призначення (це називається неявною типізацією). Після того, як це початкове призначення було зроблено, у вас є строго типізована змінна, і будь-яка спроба призначити несумісне значення призведе до помилки компілятора.

DynamicKeyword\Program.cs
```cs
static void InvestigationImplicitlyTypedValue()
{
    var list = new List<string> { "20" };

    list = "10"; //Cannot implicitly convert type "string" to ... 
}
```
Компілятор покаже помилку.

Неявна типізація корисна з LINQ, оскільки багато запитів LINQ повертають перерахування анонімних класів (через проекції), які ви не можете безпосередньо оголосити у своєму коді C#. Однак навіть у таких випадках неявно типізована змінна насправді є строго типізованою. У попередньому прикладі змінна a строго типізована як List<string>.

System.Object є найвищим батьківським класом у .NET і може представляти будь-що. Якщо ви оголошуєте змінну типу об’єкт, у вас є строго типізований фрагмент даних; однак те, на що він вказує в пам’яті, може відрізнятися залежно від вашого призначення посилання. Щоб отримати доступ до членів, на які в пам’яті вказує посилання на об’єкт, потрібно виконати явне приведення.

Припустимо у нас є клас.
```cs
internal class Person
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;   
}
```
І його використання.
```cs
static void UseObjectVariable()
{
    // Create a new instance of the Person class
    // and assign it to a variable of type System.Object
    object p = new Person() { FirstName = "Antoni", LastName = "Gaudi" };

    // Must cast object as Person to gain access
    // to the Person properties.
    Console.WriteLine($"{((Person)p).FirstName} {((Person)p).LastName}");
}
UseObjectVariable();
```
```
Antoni Gaudi
```
Отже для доступу до властивостей потрібна явне приведеня об'єкта

## Створення змінної з допомогою dynamic .

На високому рівні ви можете вважати ключове слово dynamic спеціалізованою формою System.Object, у якій будь-яке значення можна призначити динамічному типу даних. На перший погляд це може здатися жахливо заплутаним, оскільки здається, що тепер у вас є три способи визначення даних, базовий тип яких прямо не вказано у вашій базі коду.

```cs
static void TreeString()
{
    var str1 = "Go";
    object str2 = "to";
    dynamic str3 = "Home";

    Console.WriteLine(str1.GetType());
    Console.WriteLine(str2.GetType());
    Console.WriteLine(str3.GetType());
}
TreeString();
```
```
System.String
System.String
System.String
```
Що робить динамічну змінну значно відмінністю від змінної, оголошеної неявно або через посилання System.Object, це те, що вона не є строго типізованою. Іншими словами, динамічні дані не типізуються статично. Що стосується компілятора C#,  точці даних, оголошеній за допомогою динамічного ключового слова, можна призначити будь-яке початкове значення взагалі та можна перепризначити будь-яке нове значення протягом її життя.

```cs
static void UseDynamic()
{
    dynamic val;
    //object val; 
        
    val = "Hi";
    Console.WriteLine(val+" "+val.GetType());

    val = false;
    Console.WriteLine(val + " " + val.GetType());

    val = new List<int> { 1, 2, 3 };
    Console.WriteLine(val.GetType());

}
UseDynamic();
```
```
Hi System.String
False System.Boolean
System.Collections.Generic.List`1[System.Int32]
```
Попередній код буде скомпільований і виконаний ідентично, якщо ви оголосите змінну як System.Object. Алє, ключове слово dynamic пропонує багато додаткових функцій.

## Доступ до членів типу що оголошено як dynamic.

Якшо зміна визначеня як dynamic не треба виконувати явне приведеня до типу щоб отримати доступ до методу.

```cs
static void UseMemeberOfDynamicVariable()
{
    dynamic str = "hi";
    Console.WriteLine(str.ToUpper());

    try
    {
        Console.WriteLine(str.DoIt());
        Console.WriteLine(str.toupper());
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);    
    }
}
UseMemeberOfDynamicVariable();
```
```
HI
'string' does not contain a definition for 'DoIt'
```
Хоча код виконується, компілятор не перевіряє наявність члена DoIt у типі і таким чином знімає з себе відповідальність за це. Також при виконані не визначиться член toupper.
На відміну від змінної, визначеної як System.Object, динамічні дані не типізуються статично. Лише під час виконання ви дізнаєтесь, чи підтримують динамічні дані, які ви викликали, вказаний член, чи правильно ви передали параметри, чи правильно ви написали член тощо. Отже str має тип String і має доступ до його членів.
IDE дозволить вам ввести будь-яке ім’я члена, яке ви тільки можете придумати. Це означає, що вам потрібно бути надзвичайно обережними, коли ви вводите код C# у такі точки даних.

Звичайно, процес загортання всіх динамічних викликів методів у блок try/catch досить виснажливий. Якщо ви стежите за орфографією та передачею параметрів, це не обов’язково. Однак перехоплення винятків є зручним, коли ви можете не знати заздалегідь, чи буде член присутній у цільовому типі.

## Область застосування ключового слова dynamic.

Неявно типізовані дані, оголошені за допомогою ключового слова var, можливі лише для локальних змінних в області члена. Ключове слово var ніколи не можна використовувати як значення, що повертається, параметр або член класу або структури. Але це не стосується ключового слова dynamic

DynamicKeyword\VeryDynamic.cs
```cs
namespace DynamicKeyword;
internal class VeryDynamic
{
    private static dynamic _fild;

    public dynamic Property { get; set; }

    public dynamic Method(dynamic parameter)
    {
        dynamic dynamicVariable = parameter;

        if (parameter is string)
        {
            return dynamicVariable.ToUpper();
        }
        else
        {
            return dynamicVariable;
        }

    }
}
```
```cs
static void UseVeryDynamic()
{
    VeryDynamic veryDynamic = new();

    veryDynamic.Property = 10;
    dynamic result = veryDynamic.Method(veryDynamic.Property);
    Write(result);

    veryDynamic.Property = "Julia";
    result = veryDynamic.Method(veryDynamic.Property);
    Write(result);

    static void Write(dynamic value)
    {
        Console.WriteLine(value + " " + value.GetType());
    }
}
UseVeryDynamic();
```
```
10 System.Int32
JULIA System.String
```
Тепер ви можете викликати публічних учасників, як очікувалося; однак, оскільки ви працюєте з динамічними методами та властивостями, ви не можете бути повністю впевнені, яким буде тип даних! Хоча визначення VeryDynamicClass може бути некорисним у реальній програмі, воно ілюструє область, де можна застосувати це ключове слово C#.

## Обмеженя на викорситання dynamic.

Елементи dynamic не може використовувати лямбда-вирази або анонімні методи C# під час виклику методу.

```cs
dynamic a = GetDynamicObject();
// Error! Methods on dynamic data can't use lambdas!
a.Method(arg => Console.WriteLine(arg));
```
Щоб обійти це обмеження, вам потрібно буде працювати безпосередньо з базовим делегатом. Іншим обмеженням є те, що динамічна точка даних не може зрозуміти жодних методів розширення. На жаль, це також включатиме будь-які методи розширення, які надходять з API LINQ. Таким чином, змінна, оголошена за допомогою ключового слова dynamic, має обмежене використання в LINQ to Objects та інших технологіях LINQ.

```cs
dynamic a = GetDynamicObject();
// Error! Dynamic data can't find the Select() extension method!
var data = from d in a select d;
```

## Де краше використовувати.

Зважаючи на те, що динамічні дані не є строго типізованими, не перевіряються під час компіляції, не мають можливості запускати IntelliSense і не можуть бути метою запиту LINQ, тому використання ключового слова dynamic без особливих причин це є пагана практика.
Однак у деяких випадках ключове слово dynamic може радикально зменшити кількість коду, який потрібно створити вручну. Зокрема, якщо ви створюєте програму .NET, яка активно використовує пізнє зв’язування (через рефлексію), ключове слово dynamic може заощадити час на введення. Крім того, якщо ви створюєте програму .NET, яка потребує зв’язку зі застарілими бібліотеками COM (такими як продукти Microsoft Office), ви можете значно спростити свою кодову базу за допомогою ключового слова dynamic. Як останній приклад, веб-програми, створені з використанням ASP.NET Core, часто використовують тип ViewBag, до якого також можна отримати доступ у спрощений спосіб за допомогою ключового слова dynamic.
Використання ключового слова dynamic є компромісом між стислістю коду та безпекою типу. Хоча C# є суворо типізованою мовою, ви можете вибрати (або відмовитися) від динамічної поведінки на основі виклику за викликом. Завжди пам’ятайте, що вам ніколи не потрібно використовувати ключове слово dynamic. Ви завжди можете отримати той самий кінцевий результат, створивши альтернативний код вручну (і зазвичай набагато більше).

### Dynamic Language Runtime.

Середовище CLR (Common Language Runtime) доповнено додатковим середовищем виконання під назвою Dynamic Language Runtime. Концепція «динамічного середовища виконання», звичайно, не нова. Фактично, багато мов програмування, такі як JavaScript, LISP, Ruby та Python, використовували його роками. Динамічне середовище виконання дозволяє динамічній мові повністю виявляти типи під час виконання без перевірок під час компіляції.
Ідея такого середовища виконання може здатися небажаною. Зрештою, ви зазвичай хочете отримувати помилки під час компіляції, а не під час виконання, де це можливо. Тим не менш, динамічні мови/середовища виконання надають деякі цікаві функції, зокрема такі:

    Надзвичайно гнучка кодова база. Ви можете рефакторювати код, не вносячи численних змін у типи даних.

    Простий спосіб взаємодії з різними типами об’єктів, створених на різних платформах і мовах програмування.

    Спосіб додавати або видаляти членів типу в пам’яті під час виконання.

Одна з ролей DLR полягає в тому, щоб дозволити різноманітним динамічним мовам працювати з .NET Runtime і надати їм спосіб взаємодії з іншим кодом .NET. Ці мови живуть у динамічному всесвіті, де тип виявляється виключно під час виконання. І все ж ці мови мають доступ до багатства бібліотек базових класів .NET. Навіть краще, їхні бази коду можуть взаємодіяти з C# (або навпаки) завдяки включенню ключового слова dynamic.

### Дерево виразів.

DLR використовує "дерева виразів", щоб отримати значення динамічного виклику в нейтральних термінах.

```cs
dynamic d = GetSomeData();
d.SuperMethod(12);
```
У цьому прикладі DLR автоматично створить дерево виразів, яке, по суті, говорить: «Викличте метод під назвою SuperMethod для об’єкта d, передаючи число 12 як аргумент».
Ця інформація (офіційно названа корисним навантаженням) потім передається до правильного зв’язувача часу виконання, який знову може бути динамічним зв’язувачем C# або навіть (як коротко пояснено) застарілими об’єктами COM. 
Звідси запит відображається в необхідну структуру виклику для цільового об’єкта. Приємна річ у цих деревах виразів (крім того, що вам не потрібно створювати їх вручну) полягає в тому, що вони дозволяють вам писати фіксований оператор коду C# і не турбуватися про те, що насправді є основною метою.

### Динамічний пошук дерев виразів під час виконання.

Як пояснено, DLR передасть дерева виразів цільовому об’єкту; однак на цю диспетчеризацію впливатиме кілька факторів. Якщо динамічний тип даних вказує в пам’яті на об’єкт COM, дерево виразів надсилається до низькорівневого інтерфейсу COM під назвою IDispatch. Як ви, можливо, знаєте, цей інтерфейс був способом COM включити власний набір динамічних служб. Однак COM-об’єкти можна використовувати в програмі .NET без використання ключового слова DLR або C# dynamic. Однак це (як ви побачите) призводить до набагато складнішого кодування C#.
Якщо динамічні дані не вказують на COM-об’єкт, дерево виразів може бути передано об’єкту, що реалізує інтерфейс IDynamicObject. Цей інтерфейс використовується за лаштунками, щоб дозволити мові, такій як IronRuby, взяти дерево виразів DLR і зіставити його зі специфікою Ruby.
Нарешті, якщо динамічні дані вказують на об’єкт, який не є об’єктом COM і не реалізує IDynamicObject, об’єкт є звичайним повсякденним об’єктом .NET. У цьому випадку дерево виразів надсилається до зв’язувача середовища виконання C# для обробки. Процес зіставлення дерева виразів зі специфікою .NET включає служби рефлексії.
Після того, як дерево виразів буде оброблено даним зв’язувачем, динамічні дані будуть розв’язані до реального типу даних у пам’яті, після чого викликається правильний метод із будь-якими необхідними параметрами.

## Практичне використання dynamic. Спрощення пізнього зв'язування.

Один із випадків, коли ви можете вирішити використовувати ключове слово dynamic, це коли ви працюєте зі службами рефлексії, зокрема під час викликів методів із пізнім зв’язуванням. Раніше в цій главі ви бачили кілька прикладів того, коли цей тип виклику методу може бути корисним, найчастіше, коли ви створюєте якийсь тип розширюваної програми. Ми використовували метод Activator.CreateInstance() для створення об’єкта, про який ви не знаєте під час компіляції (крім його відображуваного імені). Також ми використовувати типи простору імен System.Reflection для виклику членів через пізнє зв’язування. 

```cs

void Run()
{
    Assembly? assembly = null;
    try
    {
        assembly = Assembly.LoadFrom(@"D:\CarLibrary");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    if (assembly != null)
    {
        CreateUsingLateBinding(assembly);
    }

    void CreateUsingLateBinding(Assembly assembly)
    {
        object? obj;
        try
        {
            Type? miniVan = assembly.GetType("CarLibrary.MiniVan");
            if (miniVan != null)
            {
                // Create object
                obj = Activator.CreateInstance(miniVan);
                Console.WriteLine($"Created a {obj} using late binding!");

                //Invoke method without parameters
                MethodInfo? methodInfoTurboBoost = miniVan.GetMethod("TurboBoost");
                methodInfoTurboBoost?.Invoke(obj, null);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
Run();
```
```
Created a CarLibrary.MiniVan using late binding!
Eek! Your engine block exploded!
```
Хоча цей код працює належним чином, ви можете погодитися, що він трохи незграбний. Ви повинні вручну використовувати клас MethodInfo, вручну запитувати метадані тощо. Спробуємо використати dynamic

```cs
void RunWithDynamic()
{
    Assembly? assembly = null;
    try
    {
        assembly = Assembly.LoadFrom(@"D:\CarLibrary");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    if (assembly != null)
    {
        CreateUsingLateBinding(assembly);
    }

    void CreateUsingLateBinding(Assembly assembly)
    {
        Type typeMiniVan = assembly.GetType("CarLibrary.MiniVan");

        try
        {
            dynamic miniVan = Activator.CreateInstance(typeMiniVan);
            Console.WriteLine($"Created a {miniVan} using late binding!");
            
            miniVan.TurboBoost();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
RunWithDynamic();
```
```
Created a CarLibrary.MiniVan using late binding!
Eek! Your engine block exploded!
```
Оголошуючи змінну за допомогою ключового слова dynamic, від вашого імені виконується важка робота рефлексії, завдяки DRL. 

## Dynamic для передачи параметрів.

Корисність DLR стає ще більш очевидною, коли вам потрібно зробити виклики методів після пізнього зв'язування, які приймають параметри. Коли ви використовуєте такі виклики після рефлексії, аргументи потрібно запакувати у вигляді масиву object[], які передаються в метод Invoke() методу MethodInfo.

Створемо рішеня з проектом LateBindingWithDynamic та бібіліотекою класів MyMath. В MyMath переіменуйте клас Class1 на SimpleMath

LateBindingWithDynamic\MyMath.cs
```cs
namespace MyMath;
public class SimpleMath
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}

```
В файл проекут додамо 

```
  <Target Name="PostBuild" AfterTargets="PostBuildEvent">
    <Exec Command="copy $(TargetPath) $(SolutionDir)LateBindingWithDynamic\$(OutDir)$(TargetFileName) /Y" />
  </Target>
```
Побачите в папці виконувальних файлів (... Debug\netX.0) проекту файл MyMath.dll

Тепер повернемося до проекту LateBindingWithDynamic. Додайте метод , який викликає метод Add() за допомогою типових викликів API рефлексії. 

LateBindingWithDynamic\Program.cs

```cs

using System.Reflection;

static void AddWithReflection()
{
    Assembly assembly = Assembly.LoadFrom("MyMath");
	try
	{
        Type simpleMath = assembly.GetType("MyMath.SimpleMath");

        object objSimpleMath = Activator.CreateInstance(simpleMath);

        MethodInfo methodInfoAdd = simpleMath.GetMethod("Add");

        object[] objects = { 1, 2 };

        Console.WriteLine(methodInfoAdd.Invoke(objSimpleMath,objects));

    }
	catch (Exception ex)
	{
        Console.WriteLine(ex.Message);
    }
}
AddWithReflection();

```
```
3
```
Тепер спростимо код використавши dynamic.

```cs
static void AddWithDynamic()
{
    Assembly assembly = Assembly.LoadFrom("MyMath");
    try
    {
        Type simpleMath = assembly.GetType("MyMath.SimpleMath");

        dynamic objSimpleMath = Activator.CreateInstance(simpleMath);

        Console.WriteLine(objSimpleMath.Add(1,2));

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
AddWithDynamic();
```
```
3
```
Використовуючи ключове слово dynamic, ви заощадили собі чимало роботи. З динамічно визначеними даними вам більше не потрібно вручну пакувати аргументи як масив об’єктів, запитувати метадані збірки або встановлювати інші подібні деталі. Якщо ви створюєте програму, яка активно використовує динамічне завантаження та пізнє зв’язування,економія коду збільшиться.

## ExpandoObject та DynamicObject.

В просторі імен System.Dynamic є класи яки дозволяють стоврювати код в манері схожій на Javascript.

### ExpandoObject

Існує можливість створювати динамічні об'єкти.

UsingExpandoObject\Program.cs

```cs
void CreateDynamicObject()
{
    dynamic girl = new System.Dynamic.ExpandoObject();

    // properties
    girl.Name = "Lucy";
    girl.Age = 31;
    girl.Languages = new List<string> { "ukrainian", "russian" };
    
    //method
    girl.IncreaseAge = (Action<int>)(a =>  girl.Age += a);



    //invoke
    girl.IncreaseAge(2);

    //write
    Console.WriteLine($"{girl.Name} {girl.Age}");
    foreach (string language in girl.Languages)
    {
        Console.WriteLine($"\t{language}");
    }
}
CreateDynamicObject();
```
```
Lucy 33
        ukrainian
        russian
```
Властивості та методи створюються на льоту. 

### DynamicObject

Цей клас також дозаоляє створювати динамічні об'єкти але з посиленим контролем. Для використаня треба створити клас нашадок від DynamicObject та реалізувати низьку методів.

```cs
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingDynamicObject;

internal class Person : DynamicObject
{

    Dictionary<string, object> members = new Dictionary<string, object>();

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        if (value is not null)
        {
            members[binder.Name] = value;
            return true;
        }
        return false;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        result = null;
        if (members.ContainsKey(binder.Name))
        {
            result = members[binder.Name];
            return true;
        }
        return false;
    }

    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        result = null;
        if (args?[0] is int number)
        {
            dynamic method = members[binder.Name];
            result = method(number);
        }
        return result != null;
    }
}

```
Перевизначені методи повертають чи виконалась операція. Перший параметр зв'язувач. Якшо метод є метод об'єкта другий параметр масив для параметрів object[]. 

Тепер цей клас можна використати.
```cs
void TestOurClass()
{

    dynamic person = new Person();

    person.Name = "George"; // invoked TrySetMember
    person.Age = 25;

    Func<int,int> increaseAge  = (int y) => { person.Age += y; return person.Age; };
    person.IncreaseAge = increaseAge;


    person.IncreaseAge(10); // invoked TryInvokeMember

    Console.WriteLine($"{person.Name} {person.Age}"); // invoked TryGetMember

}
TestOurClass();
```

## DLR та IronPython

### Запуск скрипта на Python.

Існують сфери в який використаня динамічних язиків програмуваня корисно. Наприклад написання клієнських сценарієв. Крім того інуючи бібіліотеки на Phayton які мають функціонал якого нема в .Net.

Створемо проект UsingIronPython і додамо пакети DynamicLanguageRuntime та IronPython (правий клік на Dependencies > Manage NuGet Packages >)

UsingIronPython\Program.cs
```cs
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;


void RunPythonScript()
{
    ScriptEngine engine = Python.CreateEngine();
    engine.Execute("print('Hi, warrior')");
    engine.ExecuteFile("D://hi.py");
}
RunPythonScript();
```
D:\Hi.py
```py
print('Hi, girl')
```
```console
Hi, warrior
Hi, girl
```

Об'єкт який виконує скрип має тип ScriptEngine і приналежить простору імен Microsoft.Scripting.Hosting

### Взаємодія з скриптом. ScriptScope

За допомогою класу ScriptScope є можливість взаємодіяти з скриптом Python

```cs
void UseScriptScope()
{
    int a = 10;

    ScriptEngine engine = Python.CreateEngine();
    ScriptScope scope = engine.CreateScope();

    scope.SetVariable("x", a);
    engine.ExecuteFile("D://square.py", scope);
    
    dynamic y = scope.GetVariable("y");
    Console.WriteLine(y);
}
UseScriptScope();
```
D:\square.py
```py
y = x
x = x * x
y = x / y
print(x)
```
```
100
10
```
Методи GetVariable, SetVariable дозволяє отримати та встановити змінні.

Якшо в скрипті Python визначено функцію її можна виклкикати
```cs
void CallPythonFunction()
{

    ScriptEngine engine = Python.CreateEngine();
    ScriptScope scope = engine.CreateScope();

    engine.ExecuteFile("D://squares.py", scope);

    dynamic square = scope.GetVariable("square");
    dynamic result = square(10);

    Console.WriteLine(result);
}
CallPythonFunction();
```
D:\squares.py
```py
def square(x):
    return x * x
```
```console
100
```
Функцію можна отримати як зміну за домоммогою GetVariable. Отримавши фунцію її можна використовувати як звичайну.


