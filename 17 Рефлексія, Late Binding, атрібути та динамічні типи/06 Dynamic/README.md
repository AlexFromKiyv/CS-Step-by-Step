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

### Створення змінної.

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
Що робить динамічну змінну значно відмінністю від змінної, оголошеної неявно або через посилання System.Object, це те, що вона не є строго типізованою. Іншими словами, динамічні дані не типізуються статично. Що стосується компілятора C#, точці даних, оголошеній за допомогою динамічного ключового слова, можна призначити будь-яке початкове значення взагалі та можна перепризначити будь-яке нове (і, можливо, не пов’язане) значення протягом її життя.

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

### Доступ до членів типу що оголошено як dynamic.

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
Хоча код виконується, компілятор не перевіряє наявність члена DoIt у типі і таким чином знімає з себе відповідальність за це. Також при виконані не визначиться члена toupper.
На відміну від змінної, визначеної як System.Object, динамічні дані не типізуються статично. Лише під час виконання ви дізнаєтесь, чи підтримують динамічні дані, які ви викликали, вказаний член, чи правильно ви передали параметри, чи правильно ви написали член тощо. Отже str має тип String і має доступ до його членів.
IDE дозволить вам ввести будь-яке ім’я члена, яке ви тільки можете придумати. Це означає, що вам потрібно бути надзвичайно обережними, коли ви вводите код C# у такі точки даних.

Звичайно, процес загортання всіх динамічних викликів методів у блок try/catch досить виснажливий. Якщо ви стежите за орфографією та передачею параметрів, це не обов’язково. Однак перехоплення винятків є зручним, коли ви можете не знати заздалегідь, чи буде член присутній у цільовому типі.

### Область застосування ключового слова dynamic.

Неявно типізовані дані, оголошені за допомогою ключового слова var, можливі лише для локальних змінних в області члена. Ключове слово var ніколи не можна використовувати як значення, що повертається, параметр або член класу або структури. Але це не стосується ключового слова dynamic

```cs
namespace DynamicKeyword;
internal class VeryDynamic
{
    private static dynamic _fild;

    public dynamic Property { get; set; }

    public dynamic Method(dynamic parameter)
    {
        dynamic dynamicVariable = "10";

        if (parameter is int)
        {
            return dynamicVariable;
        }
        else
        {
            return 10;
        }
    }
}
```
```cs
static void UseVeryDynamic()
{
    VeryDynamic veryDynamic = new();

    dynamic result =  veryDynamic.Method(10);
    Write(result);

    result = veryDynamic.Method("10");
    Write(result);

    static void Write(dynamic value)
    {
        Console.WriteLine(value + " " + value.GetType());
    }
}
UseVeryDynamic();
```
```
10 System.String
10 System.Int32
```
Тепер ви можете викликати публічних учасників, як очікувалося; однак, оскільки ви працюєте з динамічними методами та властивостями, ви не можете бути повністю впевнені, яким буде тип даних! Хоча визначення VeryDynamicClass може бути некорисним у реальній програмі, воно ілюструє область, де можна застосувати це ключове слово C#.

### Обмеженя на викорситання dynamic.

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
