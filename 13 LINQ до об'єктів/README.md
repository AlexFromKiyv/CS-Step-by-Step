# LINQ до об'єктів.

Більша частина програм обробляє дані в певних формах при виконанні. Це може бути массив, коллекція в пам'яті, база даних, файл XML. Доступ до ціх даних схожий але довгий час виористовувався різний API. Набір технологій LINQ забезпечує стислий, симетричний і строго типізований спосіб доступу до широкого спектру сховищ даних.

## Спеціфічні конструкції програмування LINQ.

LINQ можна розглядати як вбудована в C# строго типізована мова запитів до даних. Про це говорить розшифровка абрівіатура LINQ Language Integreted Quary.
Використовуючи LINQ можна створювати вирази у вигляді запитів до даних схожі на SQL. Вони можуть бути застосована для різних сховищ, які не мають нічого спільного з реляційними базами даних. Синтаксис SQL і LINQ не індентичний. Краще сприймати запити LINQ як унікальні оператори.

В LINQ використовується:

    Неявно типізовані локальні змінні.

    Синтаксис ініціалізації об'єкта та коллекції.

    Лямбда-вирази.

    Методи розширення.

    Анонімні типи.

Переглянемо ці можливості коротоко в проекті FeaturesForLINQ. 

### Неявна типізація локальних змінних.
```cs
void ImpicitlyTypedLocalVariables()
{
    var variable1 = 2;
    var variable2 = "Hi";
    var variable3 = false; 
    
    Console.WriteLine($"{nameof(variable1)}\t{variable1}\t{variable1.GetType()}");
    Console.WriteLine($"{nameof(variable2)}\t{variable2}\t{variable2.GetType()}");
    Console.WriteLine($"{nameof(variable3)}\t{variable3}\t{variable3.GetType()}");
}
ImpicitlyTypedLocalVariables();
```
```
variable1       2       System.Int32
variable2       Hi      System.String
variable3       False   System.Boolean
```
Ключеве слово var дозволяє визначити зміну без явного вказування типу на якому вона базується. Однак зміна є строго типізованою оскільки тип визначив компілятор з контексту.
Ця розумність компілятора зручна для LINQ, оскільки деякі типи бувають дуже громізьки а іноді їх визначити складно. Деякі типи, які поверає запит LinQ, не відомі до моменту компіляції. Не знаючи тип ви не можете визначити зміну явно.

### Синтаксис ініціалізації об'єкта та коллекції.

В систеній бібліотеці System.Drawing є визначена структура Rectangle.

```cs

void ObjectAndCollectionInitializationSyntax()
{
    List<Rectangle> rectangles = new() 
    {
        new Rectangle()
        {
            X = 0,Y = 0,Width = 10, Height = 10,
        },
        new Rectangle()
        {
            X = 10,Y = 10,Width = 20, Height = 20
        },
        new Rectangle()
        {
            X = 30,Y = 30,Width = 40, Height = 40
        },
    };
}
```
Синтаксис ініціалізації компактний варіант створення об'єктів на льоту. Таким же чином можна створювати коллекції. Компактність коду основана користь від цього. 
Цей синтаксис у поєдняні з неявною типізацією дозволяє оголосити анонімний тип, що корисний при створені проекції LINQ. 

### Лямбда-вирази.

Лямбда-вираз можна використовувати будь-коли, коли викликаете метод, який вимагає строго типізований делегат як аргумент. Вони значно спрощують роботу з делегатами, оскільки зменьшують кількість коду який треба робити вручну. 
Лямда-вирази схематично виглядають так.
```cs
( ArgumentsToProcess ) => { StatementsToProcessThem }
``` 
Для об'єктів коллекції List<T> є метод FindAll(Predicate<T> match) в якому для делегата зручно використовувати лямбда-вираз.

```cs
void LambdaExpressions()
{
    List<int> ints = new();

    ints.AddRange(new[] { 12, 31, 23, 21, 34 });

    List<int> evenNumbers = ints.FindAll( x => (x%2) == 0);

    foreach (var item in evenNumbers)
    {
        Console.WriteLine(item);
    }
}
LambdaExpressions();
```
```
12
34
```
Лямбда-вирази дуже корисні для роботи з лежачимі в основі об'єктів моделями LINQ. Оператори запитів LINQ це скорочена нотація для визову методів класу System.Linq.Enumerable. Для ціх методів зазвичай як параметри потрібні делегати (зазвичай Func<>), які використовуються для обробки даних для отримання правільного наботу результатів. За допомогою лямбда-виразу ви можете оптімізувати ваш код даючи компілятору можливість зробити висновок як виглядає об'єкт делегату.

### Методи розширення.

Методи розширеня дозволяють додавати нову функціональність до існуючих класів без необхідності створювати підклас. Вони дозволяють церобити і для класів та структур які не дозволяють успадкування.  

```cs
    namespace FeaturesForLINQ;

    internal static class ObjectExtentions
    {
        public static void DisplayDefiningAssembly(this object  obj)
        {
            Console.WriteLine($"{obj.GetType().Name} live here: {Assembly.GetAssembly(obj.GetType())}");
        }
    }
```
Коли створюється метод розширення, перший параметр доповнюється словом this і позначає тип шо розширюється. Крім того методи розширення повині оперовати на рівні класу і тому вони мають бути static і визначені в статичному класі.

```cs
void UseExtentionMethod()
{
    int variable1 = 1;
    variable1.DisplayDefiningAssembly();

    System.Data.DataSet dataSet = new();
    dataSet.DisplayDefiningAssembly();
}
UseExtentionMethod();

```
```
Int32 live here: System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
DataSet live here: System.Data.Common, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
```
При роботі з LINQ рідко коли треба робити їх вручну. Але коли створюється запити ви будите використовувати числені методи розширеня які вбудовані в платформу. Кожен оператор LINQ є скороченою нотацією для ручного виклику методу розширення. Вони як правило визначені в службовому класі System.Linq.Enumerable.

### Анонімні типи.

Анотімні типи дозволяють швидко створити "форму" даних, дозволяючи компілятору генерувати нове визначення класу, під час компіляції, на основі наданого набору пар назва-значення.

```cs
void UseAnonymousType()
{
    var purchaseItem = new
    {
        TimeBought = DateTime.Now,
        ItemBought = new
        {
            Color = "Grey",
            Make = "BMW",
        },
        Price = 35000
    };

    Console.WriteLine(purchaseItem.GetType());
    Console.WriteLine(purchaseItem.ItemBought.Color);
}
```
```
<>f__AnonymousType0`3[System.DateTime,<>f__AnonymousType1`2[System.String,System.String],System.Int32]
Grey
```
Щоб визначити ананімний тип оголошується неявно тепізована змінна та вказується форма данних для за допомогою синтаксису ініціалізації об'єктів.
LINQ часто використовує анонімні типи, коли потрібно проектувати нові форми даних на льоту. Припустимо із колекції об'єктів Person за допомогою запиту LINQ треба отримати призвище та вік кожного. Використовуючи проекцію LINQ ви вкажете компілятору створити новий анонімний тип який буде мати цю інформацію.

## Роль LINQ.

Якшо говорити про дані то це не тілки те шо зберігається в базі данних. Це можуть бути XML документи або текстовий файл. Дані можуть знаходитись в багатьох місцях. Це може бути масив або список List<T>. Однозначно виникне потреба пошуку та виділення підмножини елементів. 
Колись для роботи з різними даними використовувалось дуже різне API. Наче нічого паганого немає в використані різних підходів до різних даних. Однак основна проблема в тому що кожний такій підхід є окремим "островом".
LINQ API - це спроба забезпечити послідовний симетричний спосіб, за допомогою якого можна отримати і маніпулювати "даними" в широкому розумінні цього терміну.
Використовуючи LINQ, можна сворювати конструкції безпосередньо в C#, які називають виразами запитів. Вираз запиту можна використовувати для взаємодії з багатьма типами даних. По суті це опис загального підходу до доступу до даних. 
Однак залежно від того, де ви застосовуєте свої запити LINQ, ви зустрінете різні терміни, наприклад такі:
|Термін|Опис|
|------|----|
|LINQ to Objects|цей термін відноситься до акту застосування запитів LINQ до масивів і колекцій.|
|LINQ to XML|цей термін стосується використання LINQ для роботи з документами XML і запитів.|
|LINQ to Entities|цей аспект LINQ дозволяє використовувати запити LINQ в основному API ADO.NET Entity Framework (EF).|
|Parallel LINQ (він же PLINQ)|це дозволяє паралельно обробляти дані, отримані із запиту LINQ.|

Зараз LINQ є невід’ємною частиною бібліотек базових класів .NET, керованих мов і самої Visual Studio.
Запити LINQ строго типізовані. Це забезпечує компілятор. 
Роботу ціх запитів забезпечує System.Linq як задіюється завдяки global using.

## Застосування LINQ до массивів.

Створимо допоміжний метод.
LinqOverArray.Program.cs

```cs
void CollectionToConsole<T>( IEnumerable<T> collection)
{
    foreach (var item in collection)
    {
        Console.WriteLine(item);
    }
}
```
При роботі з масивом даних, зазвичай вибираеться підмножина на основі вимоги. Наприклад необхідні єлементи з числами. Хоча можна зробити виборку за допомогою членів класу System.Array, з деякими доробками, запити в вигляді виразів LINQ спрощують процес

```cs
void QuryesOverStringsArray()
{
    string[] games =
    {
        "Morrowind",
        "Uncharted 2",
        "Fallout 3",
        "Daxter",
        "System Shock 2"
    };

    // Build a query expression to find the items in the array
    // that have an embedded space.

    IEnumerable<string> longNames = 
        from ng in games
        where ng.Contains(' ')
        orderby ng
        select ng;

    CollectionToConsole(longNames);
}
QuryesOverStringsArray();
```
```
Fallout 3
System Shock 2
Uncharted 2

```
Тут використовуються оператори LINQ from, in, where, orderby, and select. Вираз досить лаконіяний і зрозумілий. Кожний елемент який задовольняє крітерію отримав ім'я ng (names of game). Зверніть увагу шо повернута послідовність зберігається в змінній яка реалізує загальний тип IEnumerable<T>, де T є System.String.

## LINQ у вигляді методів розширення.

У попередньому прикладі використовувався синтаксис у вигляді запита LINQ. Існує ще один синтаксис, який використовує методи розширення.
```cs
void QuryesOverStringsArrayWithExtentionMethods()
{
    string[] games =
    {
        "Morrowind",
        "Uncharted 2",
        "Fallout 3",
        "Daxter",
        "System Shock 2"
    };

    IEnumerable<string> longNames =
        games.Where(ng => ng.Contains(" ")).OrderBy(ng => ng).Select(ng => ng);

    CollectionToConsole(longNames);
}
QuryesOverStringsArrayWithExtentionMethods();
```
```
Fallout 3
System Shock 2
Uncharted 2
```
Цей синтаксис використовує лямбда-вирази для кожного методу. Наприклад в методі Where функція у вигляді лямда-виразу визначає умови фільтрації. Значеня літери або змінна в лямбда-втразі може буди довільною. 
Для більшості сценаріїв оба синтаксиси підходять і є ваємозамінними.

## Без LINQ.

Можна отримати той самий результат використовуючи оператори if, for.
```cs
void QueryOverStringsWithoutLINQ()
{
    string[] games =
    {
        "Morrowind",
        "Uncharted 2",
        "Fallout 3",
        "Daxter",
        "System Shock 2"
    };

    string[] gamesWithSpace = new string[5];

    // Selection
    for (int i = 0; i < games.Length; i++)
    {
        if (games[i].Contains(" "))
        {
            gamesWithSpace[i] = games[i];
        }
    }

    //Sort
    Array.Sort(gamesWithSpace);
    
    CollectionToConsole(gamesWithSpace);
}
QueryOverStringsWithoutLINQ();
```
```
Fallout 3
System Shock 2
Uncharted 2
```
Таким чином видно що LINQ можна використовувати для радикального спрощеня вилученоя і отримання підмножин набору даних. Замість створення вкладених циклів та складних умов логіки компілятор зробить цю роботу від вашого імені, коли ви створите відповідний запит LINQ. 

## Рефлексія (видслідковуваня власної структури під час виконання) над наборами результатів LINQ.

Для визначення деталей типів наборів результатів LINQ використаємо допоміжний метод.
```cs
void ReflectOverQueryResult(object resultSet, string queryType = "Query Expressions")
{
    Console.WriteLine($"Query type:{queryType}");
    Console.WriteLine($"Result is type of:{resultSet.GetType()}");
    Console.WriteLine($"This type locate:{resultSet.GetType().Assembly.GetName().Name}");
}
```
Використаємо його для дослідження запиту LINQ.
```cs
void ExploreResultSetQueryExpression()
{
    string[] games =
{
        "Morrowind",
        "Uncharted 2",
        "Fallout 3",
        "Daxter",
        "System Shock 2"
    };

    IEnumerable<string> longNames =
        from ng in games
        where ng.Contains(" ")
        orderby ng
        select ng;

    ReflectOverQueryResult(longNames);
}
ExploreResultSetQueryExpression();
```
```
Query type:Query Expressions
Result is type of:System.Linq.OrderedEnumerable`2[System.String,System.String]
This type locate:System.Linq
```
Ми бачимо що змінна результату екземпляр типу OrderedEnumerable<TElement,TKey> яка є внутрішнфм абстрактним типом, що міститься в збірці System.Linq.

Зробито такеж саме дослідження для LINQ у вигляді методів розширення.
```cs
void ExploreResultSetExtensionMethods()
{
    string[] games =
{
        "Morrowind",
        "Uncharted 2",
        "Fallout 3",
        "Daxter",
        "System Shock 2"
    };

    IEnumerable<string> longNames =
        games
        .Where(ng => ng.Contains(" "))
        .OrderBy(ng => ng)
        .Select(ng => ng);

    ReflectOverQueryResult(longNames,"Extension Methods.");
}

ExploreResultSetExtensionMethods();
```
```
Query type:Extension Methods.
Result is type of:System.Linq.Enumerable+SelectIPartitionIterator`2[System.String,System.String]
This type locate:System.Linq
```
Як бачимо змінна підмножини є екземпляр типу SelectIPartitionIterator. Якщо виділити Select(ng => ng) ми матимемо той сами тип OrderedEnumerable<TElement,TKey>. 
Обидва типи походять від IEnumerable<T> обидва можуть повторюватися, та обидва можуть створювати список або масив із своїх значень.

## LINQ та неявне типізовані локальні змінні.

Хоча в попередньому прикладі можна було охопити що результат може бути об'єкт, яки можно перебрати по елементах шо є строками (IEnumerable<string>), але не досі ясно шо зміна є типу OrderedEnumerable<TElement,TKey>.
Враховуючи що набори результатів можуть бути представлені за допомогою великої кількісті типів у різних просторах імен, орієнтованих на LINQ, утомливо визначати визначати належний тип для зберігання результату. В багатох випадках базовий тип може бути неочевидним. У деяких випадках тип генерується під час компіляції.

Розглянемо приклад
```cs
void QueryOverInts()
{
    int[] ints = { 10, 20, 30, 40, 1, 2, 3, 8 };

    IEnumerable<int> intsLeesThan10 =
        from i in ints
        where i < 10
        select i;

    foreach (var item in intsLeesThan10)
    {
        Console.WriteLine(item);
    }

    ReflectOverQueryResult(intsLeesThan10);
}
QueryOverInts();
```
```
1
2
3
8

Query type: Query Expressions
Result is type of:WhereArrayIterator`1
This type locate:System.Linq
```
У цьому випадку змінна підмножини має зовсім інший базовий тип. Цього разу типом що реалізує інтерфейс IEnumerable<int> є низкорівневий клас  WhereArrayIterator<T>.
Ці приклади використати IEnumerable<T> для зміних підмножин, де T це тип даних.
Оскілки IEnumerable<T> розширює IEnumerable можна булоб використати цей тип.
В цій ситуацію спрощує створеня запитів неявна типізація.

```cs
void QueryOverIntsUseImplicitlyTypedLocalVariables()
{
    int[] ints = { 10, 20, 30, 40, 1, 2, 3, 8 };

    var intsLeesThan10 =
        from i in ints
        where i < 10
        select i;

    CollectionToConsole(intsLeesThan10);
}
QueryOverIntsUseImplicitlyTypedLocalVariables();
```
Як правило нам не потібно знати точне значеня типу який повертає запит. Важливо що в більшості випадків він реалізовує інтерфейс який походить від IEnumerable, наприклад IEnumerable<T>. В операторі foreach також можна використоваувати var до елементів.

## LINQ і методи розширення.

Хоча в прикладах не створювалися методи розширеня, вони використовуються як основа в фоновому режиті. Вирази запиту LINQ можна використовувати для повернення контейнерів даних, які реалізовують загальний інтерфейс IEnumerable<T>. Але System.Array не реалізовує цей інтерфейс.
```cs
public abstract class Array : ICollection, IEnumerable, IList, IStructuralComparable, IStructuralEquatable, ICloneable
```
Він опосередковано отримує необхідну функціональнисть через статичний клас System.Linq.Enumerable. Цей службовий клас визначає велику кількість методів розширення( Aggregate<T>(), First<T>(), Max<T>(),...) які Array набуває у фоновому режимі. Тому коли поставити крапу після масиву можна побачити велику кількість методів, які не визначені в Array.   

## Роль відкладенного виконання.

Для запитів LINQ важливо визначити, що коли вони повертають послідовність, вони не визначать значеня до тих пір пока не буде ітерації над послідовністю. Виконання визначення відкладається. Перевага підходу полягає в тому що можна застосовувати той самий запит кілька разів до того самого контейнера. Прицому ви будете отримувати найновіший результат.

```cs
void DeferredExecution()
{
    Console.WriteLine("Use query expression.");

    int[] ints = { 10, 20, 30, 40, 1, 2, 3, 8 };

    var result = from i in ints where i < 10 select i;

    // query executed here
    CollectionToConsole(result);
    Console.WriteLine("\n");

    ints[0] = 4;
    
    // and execute again
    CollectionToConsole(result);

    Console.WriteLine("\nUse extentions method."  );

    var result1 = ints.Where(n => n < 10).Select(n => n);

    CollectionToConsole(result1);
    Console.WriteLine("\n");

    ints[2] = 5;
    
    CollectionToConsole(result1);

}
DeferredExecution();
```
```
Use query expression.
1
2
3
8


4
1
2
3
8

Use extentions method.
4
1
2
3
8


4
5
1
2
3
8
```
Виключеня із правила коли вибираеться один елемент за допомогою методів First/FirstOrDefault, Single/SingleOrDefault або будь-який оператор агрегації. В цьому випадку запит виконуеться одразу.   

При роботі з налагодженям коду в Visual Studio є корисний інструмент. Якшо поставити точку зупинки в місті де починається визначатися послідовність, можна побачити складові послідоності в Result View. Для цього треба підвести курсор до зміної.

![View result](/13%20LINQ%20до%20об'єктів/Result.jpg "View result in VS").

Метод розширення DefaultIfEmpty повертає послідовність без змін або значення за замовчуванням якщо послідосність порожня.

```cs
void ImmediateExecution()
{
    int[] ints = { 10, 20, 30, 40, 1, 2, 3, 8 };
    Console.WriteLine( $"int[] ints = {{ 10, 20, 30, 40, 1, 2, 3, 8 }};");

    int number = (from i in ints select i).First();
    Console.WriteLine($"(from i in ints select i).First() : {number}");

    number = (from i in ints orderby i select i).First();
    Console.WriteLine($"(from i in ints orderby i select i).First() : {number}");

    number = (from i in ints where i > 30 select i).Single();
    Console.WriteLine($"(from i in ints where i > 30 select i).Single() : {number}");

    number = (from i in ints where i > 0 select i).FirstOrDefault();
    Console.WriteLine($"(from i in ints where i > 0 select i).FirstOrDefault() : {number}");

    number = (from i in ints where i > 99 select i).FirstOrDefault();
    Console.WriteLine($"(from i in ints where i > 99 select i).FirstOrDefault() : {number}");

    number = (from i in ints where i > 99 select i).SingleOrDefault();
    Console.WriteLine($"(from i in ints where i > 99 select i).SingleOrDefault() : {number}");

    try
    {
        number = (from i in ints where i > 99 select i).First();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        number = (from i in ints where i > 10 select i).Single();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    // Get data right now
    int[] intsAsArray = (from i in ints where i < 5 select i).ToArray();
    CollectionToConsole(intsAsArray);

    // Get data right now
    List<int> intsAsList = (from i in ints where i < 5 select i).ToList();
    CollectionToConsole(intsAsList);
}
ImmediateExecution();
```
```
int[] ints = { 10, 20, 30, 40, 1, 2, 3, 8 };
(from i in ints select i).First() : 10
(from i in ints orderby i select i).First() : 1
(from i in ints where i > 30 select i).Single() : 40
(from i in ints where i >0 select i).FirstOrDefault() : 10
(from i in ints where i > 99 select i).FirstOrDefault() : 0
(from i in ints where i > 99 select i).SingleOrDefault() : 0
Sequence contains no elements
Sequence contains more than one element
1
2
3
1
2
3
```
Метод First повертае перший елемент послідовності(і краше шоб використовувався з OrderBy() чи OrderByDescending()).  FirstOrDefault повертає значеня за замовченям якшо послідовність пуста , інакще перший елемент. Метод Single передбачає що в послідовності повинен бути лише один елемент який задоволняє крітерію. Якшо це так поверає цей елемент, інакще викилується виняток. SingleOrDefault повертае значеня за замовчуванням якшо елемента не знайдено.
Якшо жодного елемента запит не знаходить методи First та Single викидають виняток, тому FirstOrDefault, SingleOrDefault не потребують додадкових перевірок і більш практичні.
В прикладі запити в дужках аби привести його до правільного базового типу для якого можна викликати методи розширення.
Також слід зазначити що для методів ToArray(),ToList() компілятор сам однозначно визначає базовий тип (тобто фактично виконує ToArray<int>(), ToList<int>() ).
Негайне визначення послідовності потрібно для роботи визиваючого коду.

Для методів FirstOrDefault, SingleOrDefault та схожі можна встановити значення яке буде повертатися у разі якщо запит не занайшов відповідних значень.

```cs
void SetDefaultValue()
{
    int[] ints = Array.Empty<int>();

    var query = from i in ints where i > 0 select i;
    var number = query.FirstOrDefault(404);

    Console.WriteLine(number);
}
SetDefaultValue();
```
```
404
```

## Повернення результату запиту LINQ.

Можна визначити поле класу або структури значення якого є результат запиту LINQ. Але при цьому не можна використовувати неявну типізацію. Крім того метою запиту не може бути дані рівня екземпляра, тому він має бути статичним. Враховуючи це ця можливість використовується рідко.
Часто запити LINQ визначають в межах методу або властивості. Зміна яка використовуєтся для зберігання результату визначається за допомогою var як неявна типізована. Неявнотипізовані зміни не можна використовувати для визначення параметрів, значень поверненя методу або полів стуктур. Враховуючи це повстає питання як саме ви можете повернути результат запиту зовнішньому коду шо його запитує. Відповідь для кожного випадку різна і залежить від контексту і потреб. Якщо у вас є набір результатів із строго типізованих даних, таких як масив рядків або List<Car> можна відмовитися від використання var і використовувати IEnumerable<T> або IEnumerable.  
```cs
void LinqReturnValues()
{
    IEnumerable<string> GetAllWithRed()
    {
        string[] colors = { "Light Red", "Green", "Yellow", "Dark Red", "Red", "Purple" };

        IEnumerable<string> result = from c in colors where c.Contains("Red") select c;
        return result; 
    }

    CollectionToConsole(GetAllWithRed());    

}
LinqReturnValues();
```
```
Light Red
Dark Red
Red
```
Таким чином виконуючи сторгу типізацію можна повернути результат запиту. 

Якшо працювати з IEnumerable<T> трохи незручно, можна виконати запит а потім повернути результат.

```cs
void LinqReturnValuesAfterExecution()
{
    string[] GetAllWithRed()
    {
        string[] colors = { "Light Red", "Green", "Yellow", "Dark Red", "Red", "Purple" };

        IEnumerable<string> query = from c in colors where c.Contains("Red") select c;
        return query.ToArray();
    }
    CollectionToConsole(GetAllWithRed());
}
LinqReturnValuesAfterExecution();
```
```
Light Red
Dark Red
Red
```

# Застосування запитів LINQ до коллекції об'єктів.

Запити LINQ можна застосовувати не лише до простих масивів, а також маніпулюватими в членах простору імен до коллекцій з классу System.Collections.Generic. Наприклад такіх як List<T>.

## Доступ до підоб'єктів.

Застосування запиту до узагальненого контейнера ні чим не відрізняється від застосування до звичайного массиву, окрім того шо в контейнері можуе бути колекція більш скаладних даних. LINQ можна використовувати до будь-якого типу шо реалізує IEnumerable<T>.

Нехай у нас в проекті є клас.

LinqOverCollections\Car.cs
```cs
    internal class Car
    {
        public string PetName { get; set; } = "";
        public string Color { get; set; } = "";
        public int Speed { get; set; }
        public string Make { get; set; } = "";

        public override string? ToString()
        {
            return $"{PetName}\t{Color}\t{Speed}\t{Make}\t\t"+base.ToString();
        }
    }
```
Розглянемо коллекцію. 
```cs
List<Car> myCars = new()
{
    new Car{ PetName = "Henry", Color = "Silver", Speed = 100, Make = "BMW"},
    new Car{ PetName = "Daisy", Color = "Tan", Speed = 90, Make = "BMW"},
    new Car{ PetName = "Mary", Color = "Black", Speed = 55, Make = "VW"},
    new Car{ PetName = "Clunker", Color = "Rust", Speed = 5, Make = "Yugo"},
    new Car{ PetName = "Melvin", Color = "White", Speed = 43, Make = "Ford"}
};
```
Додамо допоміжний метод.
```cs
void CollectionToConsole<T>(IEnumerable<T>? collection)
{
    if (collection == null) return;

    foreach (var item in collection)
    {
        Console.WriteLine(item);
    }
}
```
Виканоємо запит.
```cs
void UseLinqForComplexObject()
{
    GetFastCars(myCars);

    void GetFastCars(List<Car> cars)
    {
        var queryFastCars = from c in cars where c.Speed > 55 select c;

        CollectionToConsole(queryFastCars);
    }
}
UseLinqForComplexObject();
```
```
Henry   Silver  100     BMW             LinqOverCollections.Car
Daisy   Tan     90      BMW             LinqOverCollections.Car
```
Таким чином можна робити вибір враховуючи властивості об'єктів колекції. Але умови вибору можна робити біль складнішими.

```cs
void UseLinqForComplexObjectWithComplexCriteria()
{
    GetFastCars(myCar);

    void GetFastCars(List<Car> cars)
    {
        var queryFastCars = from c in cars where c.Speed > 90 && c.Make == "BMW" select c;

        CollectionToConsole(queryFastCars);
    }
}
UseLinqForComplexObjectWithComplexCriteria();
```
```
Henry   Silver  100     BMW             LinqOverCollections.Car
```
Як видно це можно зробити за допомогою усладнення логічного виразу.

## Як застосовувати LINQ до неузагальнених коллекцій.

LINQ розроблені для роботи з будь-яким типом шо реалізує інтерфейс IEnumerable<T>.
Неузагальнені(застарілі) контейнети з System.Сollection теж мають можливість використовувати інфраструктуру яка є в System.Array.
```cs
void LinqOverArrayList()
{
    // Here is a nongeneric collection of cars.
    ArrayList myArrayCars = new ArrayList()
    {
        new Car{ PetName = "Henry", Color = "Silver", Speed = 100, Make = "BMW"},
        new Car{ PetName = "Daisy", Color = "Tan", Speed = 90, Make = "BMW"},
        new Car{ PetName = "Mary", Color = "Black", Speed = 55, Make = "VW"},
        new Car{ PetName = "Clunker", Color = "Rust", Speed = 5, Make = "Yugo"},
        new Car{ PetName = "Melvin", Color = "White", Speed = 43, Make = "Ford"}
    };

    // ArrayList don't have methods of LINQ
    //var result = myArrayCars.


    // Transform ArrayList into an IEnumerable<Car>-compatible type.
    var myCarsGeneric = myArrayCars.OfType<Car>();

    var queryFastCars = from c in myCarsGeneric where c.Speed > 55 select c;

    CollectionToConsole(queryFastCars);

}
LinqOverArrayList();
```
```
Henry   Silver  100     BMW             LinqOverCollections.Car
Daisy   Tan     90      BMW             LinqOverCollections.Car
```
Для того аби маніпулювати данними з неузагальненого контейнера можна використати метод розширення OfType<T> який трансформує коллекцію в IEnumerable<T>.

Контейнер неузагальненого типу складаеться із елементів які є прототипом System.Object і відповідно в коллекції можуть бути будь-які данні.
За допомогою методу OfType<T> можна провести фільтрацію неузагальненого списку.

```cs
void FilteringNoGenericCollection()
{
    ArrayList myStuff = new();

    myStuff.AddRange(new object[]
    {
        10, 400, 8, false, new Car(), "Hi girl"
    }); 

    var myInts = myStuff.OfType<int>();

    CollectionToConsole(myInts);
}

FilteringNoGenericCollection();
```
```
10
400
8

```
Якшо методу TypeOf<T> вказати певний тип він проводячи трасформацію відбере в ітерації всі елементи які відповідають цьому типу.
Метод TypeOf<T> може бути корисним якшо треба зробити фільтр в іерархії типів.
```cs
void FilteringByType()
{
    List<Exception> exceptions = new()
    {
      new ArgumentException(),
      new SystemException(),
      new IndexOutOfRangeException(),
      new InvalidOperationException(),
      new NullReferenceException(),
      new InvalidCastException(),
      new OverflowException(),
      new DivideByZeroException(),
      new ApplicationException()
    };

var queryArithmeticException = exceptions.OfType<ArithmeticException>();

CollectionToConsole(queryArithmeticException);
}
FilteringByType();
```
```
System.OverflowException: Arithmetic operation resulted in an overflow.
System.DivideByZeroException: Attempted to divide by zero.
```

# Вирази запросів LINQ

В С# є багато операторів для запиту LINQ. Крім операторів в просторі імен System.Linq.Enumerable є методи розширення яки не мають прямої скороченної нотації в С#.

Де які зазвичай часто викорустовуємі оператори.

|Оператор|Опис|
|--------|----|
|from, in|Використовуються для визначення з якого контейнера брати данні.|
|where|Використовуються як обмеження для визначення які елементів брати із  контейнера.|
|select|Для вказівкі що брати із контейнера.|
|join, on, equals, into|Виконує об'єднання на основі вказаного ключа. Пам’ятайте, що ці «з’єднання» не обов’язково мають бути пов’язані з даними в реляційній базі даних.|
|orderby, ascending, descending|Дозволяє впорядкувати отриману підмножину в різних порядках(зростання спадання).|
|groupby|видає підмножину з даним, згрупованим за вказаним значенням.|


Для розгляду виразів Linq використаємо наступний контекст.

LinqExpressions\ProductInfo.cs
```cs
    internal class ProductInfo
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int NumberInStock { get; set; } = 0;
        public override string? ToString()
        {
            return string.Format("{0,-30}{1,-30}{2,-20}", Name, Description, NumberInStock);
        }
    }
```
```cs
ProductInfo[] itemsInStock = new[] 
{
    new ProductInfo{ Name = "Mac's Coffee", Description = "Coffee with TEETH", NumberInStock = 24},
    new ProductInfo{ Name = "Milk Maid Milk", Description = "Milk cow's love", NumberInStock = 100},
    new ProductInfo{ Name = "Pure Silk Tofu", Description = "Bland as Possible", NumberInStock = 120},
    new ProductInfo{ Name = "Crunchy Pops", Description = "Cheezy, peppery goodness", NumberInStock = 2},
    new ProductInfo{ Name = "RipOff Water", Description = "From the tap to your wallet", NumberInStock = 100},
    new ProductInfo{ Name = "Classic Valpo Pizza", Description = "Everyone loves pizza!",  NumberInStock = 73}
};

void CollectionToConsole<T>(IEnumerable<T>? collection)
{
    if (collection == null) return;

    foreach (var item in collection)
    {
        Console.WriteLine(item);
    }
}
CollectionToConsole(itemsInStock);
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73
```
Для виразів запросу LINQ запросів порядок операторів критичний. Більшість запросів мають оператори from, in, and select. Зальний шаблон такий:
```cs
var result =
  from matchingItem in container
  select matchingItem;
```
Елемент після оператору from представляє елемент, який відповідає крітеріям запиту LINQ і його можна назвати як завгодно. Елемент після опертору in представляє контейнер даних для пошуку( массив, коллекція, документ XML, тощо).

## select. Базовий вибор за допомогою select

Виберемо всі дані контейнера без будь яких додадкових дій і обмежень.
```cs
void SelectAllContainer()
{
    SelectEverything(itemsInStock);

    void SelectEverything(ProductInfo[] products)
    {
        //Get everything.
        var allProducts = from p in products select p;
        CollectionToConsole(allProducts);
    }       
}
SelectAllContainer();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73
```
Особливої користі від такого запиту немає. Його можна модіфікувати аби отримати окреме поле.
```cs
void SelectAllNames()
{
    SelectEveryNames(itemsInStock);

    void SelectEveryNames(ProductInfo[] products)
    {
        //Get names.
        var allNames = from p in products select p.Name;
        CollectionToConsole(allNames);
    }
}
SelectAllNames();
```
```
Mac's Coffee
Milk Maid Milk
Pure Silk Tofu
Crunchy Pops
RipOff Water
Classic Valpo Pizza
```

## where. Отримання підмножин даних.

Для отриманя специфічних підмножин з контейнера даних можна поставить обмеження за допомогою where. Загальний шаблон такий:

```cs
var result =
  from item  in container
  where BooleanExpression
  select item;
```
Оператор where очикує результат логічного виразу. 

```cs
void UseOperatorWhere()
{
    SelectOverstock(itemsInStock,25);

    void SelectOverstock(ProductInfo[] products, int quantity)
    {
        var overstock =
            from p in products
            where p.NumberInStock > quantity
            select p;

        CollectionToConsole(overstock);
    }
}
UseOperatorWhere();
```
```
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73
```
При створені логічного виразу використовується змінна яка вказана в from. Для where логічний вираз може бути складнішим але повиненбути валідним. 
```cs
void UseOperatorWhereWithComplexClause()
{
    SelectWithMilk(itemsInStock, 0);

    void SelectWithMilk(ProductInfo[] products, int quantity)
    {
        var overstock =
            from p in products
            where p.NumberInStock > quantity && p.Name.Contains("Milk")
            select p;

        CollectionToConsole(overstock);
    }
}
UseOperatorWhereWithComplexClause();
```
```
Milk Maid Milk                Milk cow's love               100
```

## Фільтрація більш складних об'єктів.

Припустимо ми маємо тип 
```cs
    record class Person(string Name, int Age, List<string> Languages);
```
В колекції такіх об'єктів кожен елемент маже мати колекцію рядків по якій треба зробити додадкову фільтрацію.

```cs
void UseWhereForComplexObject()
{
    List<Person> people = new()
    {
        new("Fedja",25,new(){"Russian"}),
        new("Anna",40,new(){"Russian","Deutch"}),
        new("Julia",30,new(){"Russian","Ukraine","English" }),
        new("Sava",28,new(){"Russain","Deutch"}),
        new("Olga",25,new(){ "Ukrainian", "English","Russian"}),
        new("Mikola",25,new(){ "Ukrainian", "English"}),
        new("Alex",30,new(){ "Ukrainian", "English","Russian","C#"})
    };

    var selected = from person in people
                   from language in person.Languages
                   where person.Age < 26
                   where language == "English"
                   select person.Name;

    CollectionToConsole(selected);
}
UseWhereForComplexObject();
```
```
Olga
Mikola
```
Тут колекція фільтруеться по властівослі об'єкту а також по властивості яка є коллекцією.


## Вибір частинами. (paging data)

Якшо потрібно отримати певну кількість записів з виборки можна використати методи Take()/TakeWhile()/TakeLast() и Skip()/SkipWhile()/SkipLast(). Ці методи визначені в IEnumerable, тому можна їх використати до результату запиту LINQ або в випдку методів розширення безпосередньо. Ці методи також видкладають виконання.

### Take

```cs
void UseTake()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithTake(itemsInStock, 2);

    void SelectWithTake(ProductInfo[] products, int quantity)
    {
        var query = from p in products select p;
        var result = query.Take(quantity); //!!!
        CollectionToConsole(result);
    }
}
UseTake();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73


Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
```
Метод Take повертає вказану кількисть записів з послідовності результату.

### TakeWhile

```cs
void UseTakeWhile()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithTakeWhile(itemsInStock, 20);

    void SelectWithTakeWhile(ProductInfo[] products, int quantityProduct)
    {
        var query = from p in products select p;
        var result = query.TakeWhile(sp => sp.NumberInStock > quantityProduct);//!!!
        CollectionToConsole(result);
    }
}
UseTakeWhile();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73


Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
```

TakeWhile бере записи з послідовності результату до тих пір покі виконується умова. Умова складається з лямда-виразу. Якшо навести курсор на цей метод можна побачити більш детальний осип шо визначає компілятор як параметр та як він типізований.

IEnumerable<ProductInfo> IEnumerable<ProductInfo>.TakeWhile<ProductInfo>(Func<ProductInfo, bool> predicate);

З щого видно що лямда-вираз по суті є функція яка отримує з послідовності об'єкт типу ProductInfo та повертає логічний результат типу Boolain.
Якшо, наприклад треба виконання умови для всіх елементів результату, можна його попередньо відсортувати.

### TakeLast

```cs
void UseTakeLast()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithTakeLast(itemsInStock, 2);

    void SelectWithTakeLast(ProductInfo[] products, int count)
    {
        var query = from p in products select p;
        var result = query.TakeLast(count);//!!!
        CollectionToConsole(result);
    }
}

UseTakeLast();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73


RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73
```
TakeLast повертає задану кількість останіх записів.

Методи Skip, SkipWhile та SkipLast працюють в тій самій манері, тілки пропускають записи замість їx видбирання.


### Skip

```cs
void UseSkip()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithTake(itemsInStock, 2);

    void SelectWithTake(ProductInfo[] products, int quantity)
    {
        var query = from p in products select p;
        var result = query.Skip(quantity); //!!!
        CollectionToConsole(result);
    }
}

UseSkip();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73


Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73
```
Skip пропускає вказану кількість записів і повертає що залишилися.

### SkipWhile

```cs
void UseSkipWhile()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithSkipWhile(itemsInStock, 20);

    void SelectWithSkipWhile(ProductInfo[] products, int quantityProduct)
    {
        var query = from p in products select p;
        var result = query.SkipWhile(sp => sp.NumberInStock > quantityProduct);//!!!
        CollectionToConsole(result);
    }
}

UseSkipWhile();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73


Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73
```
SkipWhile пропускає записи покі виконується умова. Так само як і TakeWhile цей метод корисно використовувати в парі з сортуванням.

### SkipLast

```cs
void UseSkipLast()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithSkipLast(itemsInStock, 2);

    void SelectWithSkipLast(ProductInfo[] products, int count)
    {
        var query = from p in products select p;
        var result = query.SkipLast(count);//!!!
        CollectionToConsole(result);
    }
}

UseSkipLast();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73


Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
```
SkipLast пропускає вказану кількість останіх записів.

### Комбінація.

Можна використовувати комбінації ціх методів щоб отримати дані для "сторінок".

```cs
void UseSkipAndTake()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithSkipAndTake(itemsInStock, 2, 2);

    void SelectWithSkipAndTake(ProductInfo[] products, int skip, int take)
    {
        var query = from p in products select p;
        var result = query.Skip(skip).Take(take);//!!!
        CollectionToConsole(result);
    }
}

UseSkipAndTake();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73


Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
```
### Вибір частинами в діапазоні.

Метод Take підтримує використання діапазонів. Це дало змогу не комбінувати методи Skip та Take. 

```cs
void PagingWithRanges()
{
    IEnumerable<ProductInfo> selectedProducts;
    var queryForSelectedProduct = from p in itemsInStock select p;


    selectedProducts = queryForSelectedProduct.Take(..3);
    WriteResult("The first three item",selectedProducts);

    selectedProducts = queryForSelectedProduct.Take(3..);
    WriteResult("Skippint the first three", selectedProducts);

    selectedProducts = queryForSelectedProduct.Take(3..5);
    WriteResult("Skip three take two", selectedProducts);

    selectedProducts = queryForSelectedProduct.Take(^2..);
    WriteResult("The last two", selectedProducts);

    selectedProducts = queryForSelectedProduct.Take(..^2);
    WriteResult("Skip the last two", selectedProducts);

    void WriteResult(string message, IEnumerable<ProductInfo> products)
    {
        Console.Clear();
        Console.WriteLine("\tAll product");
        CollectionToConsole(itemsInStock);

        Console.WriteLine("\n\t"+message);
        CollectionToConsole(products);
        Console.ReadLine();
    }
}

PagingWithRanges();
```
Цей приклад вивовдить по черзі вказані діапазони.


### Вибір фрагмента (Chunk).

```cs
void PagingWithChunks()
{
    var queryForSelectedProduct = from p in itemsInStock select p;

    IEnumerable<ProductInfo[]> chunks = queryForSelectedProduct.Chunk(2);

    var counter = 1;
    foreach (var item in chunks)
    {
        WriteResult($"Chunk {counter}", item);
        counter++;
    }

    void WriteResult(string message, IEnumerable<ProductInfo> products)
    {
        Console.Clear();
        Console.WriteLine("\tAll product");
        CollectionToConsole(itemsInStock);

        Console.WriteLine("\n\t" + message);
        CollectionToConsole(products);

        Console.ReadLine();
    }
}
PagingWithChunks();
```
Цей приклад виводить по черзі по два елементи. Метод бере один параметр size, а потім ділить джерело даних на частини цього розміру створючи об'єкт IEnumerable<ProductInfo[]>, тобто послідовність з массивів.


## Проекція нових типів даних.

На основі існуючої послідовності даних можна проектувати нові форми даних. 
```cs
void ProjectingNewDataType()
{
    GetNameAndDescription(itemsInStock);

    void GetNameAndDescription(ProductInfo[] products)
    {
        var prodeuctNameAndDescription =
            from p in products
            select new { p.Name, p.Description };

        CollectionToConsole(prodeuctNameAndDescription);
        Console.WriteLine("\n"+prodeuctNameAndDescription.GetType());
    }
}
ProjectingNewDataType();
```
```
{ Name = Mac's Coffee, Description = Coffee with TEETH }
{ Name = Milk Maid Milk, Description = Milk cow's love }
{ Name = Pure Silk Tofu, Description = Bland as Possible }
{ Name = Crunchy Pops, Description = Cheezy, peppery goodness }
{ Name = RipOff Water, Description = From the tap to your wallet }
{ Name = Classic Valpo Pizza, Description = Everyone loves pizza! }

System.Linq.Enumerable+SelectArrayIterator`2[LinqExpressions.ProductInfo,<>f__AnonymousType0`2[System.String,System.String]]
```
В цьому прикладі з массиву об'єктів створюється нова послідовність з новим анонімним типом. Тип який створює послідовність надто складний аби визначать його і визначається під час компіляції, тому тут без неявної типізації не обійтись. Крім того не можна зробити метод який поверне результат запиту з використанням var.

```cs
//Do not work
static var GetProjectedSubset(ProductInfo[] products)
{
 ...
}

```
Коли потрібно повернути нову спроектовану послідовність визиваючому коду, одним із підходів є перетворення результатів запиту в массив за допомогою методу розширення ToArray.  

```cs
void ReturnProjection()
{
    var arrayOfNewType = GetNameAndDescription(itemsInStock);

    foreach (var item in arrayOfNewType)
    {
        Console.WriteLine(item);
    }

    Array GetNameAndDescription(ProductInfo[] products)
    {
        var productNameAndDescription =
            from p in products
            select new { p.Name, p.Description };

        return productNameAndDescription.ToArray();
    }
}
ReturnProjection();
```
```
{ Name = Mac's Coffee, Description = Coffee with TEETH }
{ Name = Milk Maid Milk, Description = Milk cow's love }
{ Name = Pure Silk Tofu, Description = Bland as Possible }
{ Name = Crunchy Pops, Description = Cheezy, peppery goodness }
{ Name = RipOff Water, Description = From the tap to your wallet }
{ Name = Classic Valpo Pizza, Description = Everyone loves pizza! }

```
Зверніть увагу шо для визначення масиву використовується літерал Array шо означає тип System.Array. Це зробленно тому що ми не знаємо точне визначення якє зробив компілятор для анонімного типу. Також ми не вказуємо типу для узагальненого методу ToArray з тоїж самої причини. Втрачається жорста типізація і таким чином кожен елемент є System.Object.
Коли треба повернути результат запиту потрібне така трансформація.

## Проектування на різних типах.

Окрім проектування в анонімні типи є можливість на базі послідовності проектувати з використанням спеціально вказаних вами типів. 

Створимо необхідний нам тип.
```cs
    internal class ProductNameDescription
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public override string? ToString()
        {
            return string.Format("{0,-30}{1,-30}", Name, Description);
        }
    }
```
Тепер можно створити проекцію за допомогою цого типу

```cs
void ProjectionWithProductNameDescription()
{
    CollectionToConsole( GetNameAndDescription(itemsInStock) );
  

    IEnumerable<ProductNameDescription> GetNameAndDescription(ProductInfo[] products)
    {
        var productNameAndDescription =
            from p in products
            select new ProductNameDescription { Name = p.Name, Description = p.Description };
        return productNameAndDescription;
    }
}
ProjectionWithProductNameDescription();
```
```
Mac's Coffee                  Coffee with TEETH
Milk Maid Milk                Milk cow's love
Pure Silk Tofu                Bland as Possible
Crunchy Pops                  Cheezy, peppery goodness
RipOff Water                  From the tap to your wallet
Classic Valpo Pizza           Everyone loves pizza!
```
Таким чином врезультаті запиту ми отримуємо коллекцію з необхідним нам типом. Таким чином, залежно від потреб, можна мати вібір як робити проекцію. 

## Змінні в запросах та оператор let.

В межах запиту можан визначити зміну для проміжних обчислень.

Припустимо у нас є класс.
LinqExpressions\Types.cs
```cs
    record class Car(string Manufacturer, string Name, int Year);
```
Ми можемо створити наступний запит.

```cs
void UseLet()
{
    List<Car> garage = new List<Car>
    {
        new("VW","T2",1995),
        new("VW","Caddy",2001),
        new("VW","LT",2001),
        new("Mercedes","Sprinter",1998),
        new("Mercedes","Vaito",2000)
    };

    CollectionToConsole(garage);
    Console.WriteLine();

    var otherGarage = from c in garage
                      let model = $"{c.Manufacturer} {c.Name}"
                      let age = DateTime.Now.Year - c.Year
                      where age > 23
                      select new
                      {
                          Model = model,
                          Age = age
                      };
    CollectionToConsole(otherGarage);
}
UseLet();
```
```
Car { Manufacturer = VW, Name = T2, Year = 1995 }
Car { Manufacturer = VW, Name = Caddy, Year = 2001 }
Car { Manufacturer = VW, Name = LT, Year = 2001 }
Car { Manufacturer = Mercedes, Name = Sprinter, Year = 1998 }
Car { Manufacturer = Mercedes, Name = Vaito, Year = 2000 }

{ Model = VW T2, Age = 28 }
{ Model = Mercedes Sprinter, Age = 25 }

```
Як бачимо досить зручно використовувати проміжні змінні в запиті. В методах роширеня такої можливості немає.


## Отримання кількості елементів (Count).

Частою потребою в проектувані нових пакетів даних є визначення загальної кількості елементів.

```cs
void UseEnumerableMethodCount()
{
    string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };

    var queryNameBigerThan6 =
        from g in games
        where g.Length > 6
        select g;

    CollectionToConsole(games);
    Console.WriteLine("\n");

    CollectionToConsole(queryNameBigerThan6);
    Console.WriteLine("\n");

    Console.WriteLine(GetCount(queryNameBigerThan6));

    int GetCount<T>(IEnumerable<T> collection)
    {
        return collection.Count();
    }
}
UseEnumerableMethodCount();
```
```
Morrowind
Uncharted 2
Fallout 3
Daxter
System Shock 2


Morrowind
Uncharted 2
Fallout 3
System Shock 2


4
```

## Отримання кількості ненумерованої послідовносту.

Метод TryGetNonEnumeratedCount намагається отримати загальну кількість елементів без фактичного перерахунку елементів і таким чином прискорює процес виконання.

```cs
void UseTryGetNonEnumeratedCount()
{
    WriteCount(itemsInStock);

    void WriteCount(ProductInfo[] products)
    {
        var query = from p in products select p;

        bool result = query.TryGetNonEnumeratedCount(out int count);
        if (result)
        {
            Console.WriteLine(count);
        }
        else
        {
            Console.WriteLine("Try get count failed."  );
        }
    }

}
UseTryGetNonEnumeratedCount();
```
```
6
```
Не всі колекції дають результат з цім методом.

```cs
void NoWorkTryGetNonEnumeratedCount()
{
    var collection = GetProducts(itemsInStock);

    bool result = collection.TryGetNonEnumeratedCount(out int count);

    if (result)
    {
        Console.WriteLine(count);
    }
    else
    {
        Console.WriteLine("Try get count failed.");
    }

    Console.WriteLine(collection.Count());

    static IEnumerable<ProductInfo> GetProducts(ProductInfo[] products)
    {
        for (int i = 0; i < products.Length; i++)
        {
           yield return products[i];
        }
    }
}

NoWorkTryGetNonEnumeratedCount();
```
```
Try get count failed.
6
```
В цьому випадку немає готового значення кількості при виконанні.
цей метод корисний при великих виборках.


## Реверс результату.

Отримати зворотній порядок елементів можна застосувавши метод розширення Reverse.

```cs

void UseReverse()
{
    CollectionToConsole(itemsInStock);

    Console.WriteLine("\n");

    CollectionToConsole( SelectWithRevers(itemsInStock) );

    IEnumerable<ProductInfo> SelectWithRevers(ProductInfo[] products)
    {
        var queryForAllProduct =
            from p in products
            select p;

        return queryForAllProduct.Reverse();
    }
}
UseReverse();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73


Classic Valpo Pizza           Everyone loves pizza!         73
RipOff Water                  From the tap to your wallet   100
Crunchy Pops                  Cheezy, peppery goodness      2
Pure Silk Tofu                Bland as Possible             120
Milk Maid Milk                Milk cow's love               100
Mac's Coffee                  Coffee with TEETH             24
```

## Сортування результату.(orderby)

За допомогою оператора orderby можна відсортувати послідовнисть за вказаним значенням.

```cs
void UseOrderByName()
{
    CollectionToConsole(itemsInStock);

    Console.WriteLine("\n");

    CollectionToConsole(SelectWithOrderby(itemsInStock));



    IEnumerable<ProductInfo> SelectWithOrderby(ProductInfo[] products)
    {
       var queryForAllWithSorting =
            from p in products
            orderby p.Name
            select p;

        return queryForAllWithSorting;
    }
}

UseOrderByName();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73


Classic Valpo Pizza           Everyone loves pizza!         73
Crunchy Pops                  Cheezy, peppery goodness      2
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
RipOff Water                  From the tap to your wallet   100
```
Якшо вказано сортувати по Name послідовність сортуеться по алфавітному порядку цього рядка. Для чисел сортування по замовченню іде від меньших до більших. Тобто по замовенню сортування ведеться по зростанню (ascending). Такий самий результат якшо запит буде таким:
```cs
       var queryForAllWithSorting =
            from p in products
            orderby p.Name ascending
            select p;
```

Але порядок сортування модна змінити.
```cs
void UseOrderByNameDescending()
{
    CollectionToConsole(itemsInStock);

    Console.WriteLine("\n");

    CollectionToConsole(SelectWithOrderby(itemsInStock));

    IEnumerable<ProductInfo> SelectWithOrderby(ProductInfo[] products)
    {
        var queryForAllWithSorting =
             from p in products
             orderby p.Name descending
             select p;

        return queryForAllWithSorting;
    }
}
UseOrderByNameDescending();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73


RipOff Water                  From the tap to your wallet   100
Pure Silk Tofu                Bland as Possible             120
Milk Maid Milk                Milk cow's love               100
Mac's Coffee                  Coffee with TEETH             24
Crunchy Pops                  Cheezy, peppery goodness      2
Classic Valpo Pizza           Everyone loves pizza!         73
```


## Отримання даних з декількох джерел.

За допомогою LINQ можна отримани дані з декількох джерел.

LinqExpressions\Types.cs
```cs
    record class Place(string Name);

    record class Person(string Name, int Age, List<string>? Languages);
```
```cs
void SelectionFromTwoSource()
{
    List<Place> places = new()
    {
        new("Job"),
        new("Home")
    };

    List<Person> people = new()
    {
        new("Valja",25,null),
        new("Fedja",30,null)
    };

    var regularLife = from person in people
                      from place in places
                      select new { Person = person.Name, Place = place.Name };

    CollectionToConsole(regularLife);
}
SelectionFromTwoSource();
```
```
{ Person = Valja, Place = Job }
{ Person = Valja, Place = Home }
{ Person = Fedja, Place = Job }
{ Person = Fedja, Place = Home }
```
Таким чином кожному елементу першої коллекції співставляється кожний елемент другої.


## LINQ як краший инструмент діаграми Венна.

Клас Enumerable має набір методів розширення, що дозволяють викороистовувати два або більше запитів LINQ як основу для пошуку об'єднань, відміностей, конкатинацій та перетинів даних. 

Нехай ми маємо наступні дані та допоміжний метод.

```cs
List<string> myCars = new List<string> { "Yugo", "Aztec", "BMW" };
List<string> yourCars = new List<string> { "BMW", "Saab", "Aztec" };

void ListCarToConsole<T>(IEnumerable<T> collection, string note = "Collection")
{
    Console.Write($"{note}\t:\t ");
    foreach (var item in collection)
    {
        Console.Write(item +"\t"  );
    }
    Console.WriteLine();
}
```

### Except. (крім, за винятком)  

```cs
void UseExcept()
{
    var queryMyCars =
        from c in myCars
        select c;
    var queryYourCars =
        from c in yourCars
        select c;
    ListCarToConsole(queryMyCars,"My    ");
    ListCarToConsole(queryYourCars, "Your  ");

    Console.WriteLine();

    var carDiff = queryMyCars.Except(yourCars);//!!!
    ListCarToConsole(carDiff, "Except");
}
UseExcept();
```
```
My      :        Yugo   Aztec   BMW
Your    :        BMW    Saab    Aztec

Except  :        Yugo
```
Except повертає набір результатів LINQ якій містить всі елементи однієї послідовності за винятком тих шо присутьні в другій.

### Intersect (перетинаються).
```cs
void UseIntersect()
{
    var queryMyCars =
        from c in myCars
        select c;
    var queryYourCars =
        from c in yourCars
        select c;
    ListCarToConsole(queryMyCars, "My      ");
    ListCarToConsole(queryYourCars, "Your    ");

    Console.WriteLine();

    var carIntersect = queryMyCars.Intersect(yourCars);//!!!
    ListCarToConsole(carIntersect, "Intersect");
}
UseIntersect();
```
```
My              :        Yugo   Aztec   BMW
Your            :        BMW    Saab    Aztec

Intersect       :        Aztec  BMW
```
Intersect повертає загальні для обох контейнеров елементи.

### Union (з'єднання).

```cs
void UseUnion()
{
    var queryMyCars =
        from c in myCars
        select c;
    var queryYourCars =
        from c in yourCars
        select c;
    ListCarToConsole(queryMyCars, "My  ");
    ListCarToConsole(queryYourCars, "Your ");

    Console.WriteLine();

    var carIntersect = queryMyCars.Union(yourCars);//!!!
    ListCarToConsole(carIntersect, "Union");
}
UseUnion();
```
```
My      :        Yugo   Aztec   BMW
Your    :        BMW    Saab    Aztec

Union   :        Yugo   Aztec   BMW     Saab
```
Union  повертає всі члени обох контейнерів. Як в будь-якому правільному об'єднанні, якщо значеня зявилося воно не повторюється. 

Коли послідовності складаються з складний об'єктів то при об'єднанні, для порівняння  використовуються методи GetHeshCode() та Equals. Якщо потрібно можно перевизначити ці методі в класі і тоді однакові за цією логікою об'єкти не будуть повторюватися. По замовченю ці об'єкти будуть порівнюватися за адресами в пам'яті.  

### Concat (зчеплення).
```cs
void UseConcat()
{
    var queryMyCars =
        from c in myCars
        select c;
    var queryYourCars =
        from c in yourCars
        select c;
    ListCarToConsole(queryMyCars, "My  ");
    ListCarToConsole(queryYourCars, "Your ");

    Console.WriteLine();

    var carConcat = queryMyCars.Concat(yourCars);//!!!
    ListCarToConsole(carConcat, "Concat");
}
```
```
My      :        Yugo   Aztec   BMW
Your    :        BMW    Saab    Aztec

Concat  :        Yugo   Aztec   BMW     BMW     Saab    Aztec
```
Concat до першого контейнера просто додає другий.

### Діаграма Венна з селектором.

Для методів що об'єднують, виднімають і перетинають набори даних присутня можливість вказувати селектори. Сетлектори використовують певні властивості об'єктів, шоб визначити дію, яку потрібно виконати.

Допоміжний метод:
```cs
void CollectionToConsoleInLine<T>(IEnumerable<T> collection, string aboutCollection)
{
    Console.Write(aboutCollection);
    foreach (var item in collection)
    {
        Console.Write(item + "\t");
    }
    Console.WriteLine();
}
```
```cs

void UseExceptWithSelector()
{
    var first = new (string Name, int Age)[] { ("Francis", 20), ("Lindsey", 30), ("Ashley", 40) };
    var second = new (string Name, int Age)[] { ("Claire", 30), ("Pat", 30), ("Drew", 33) };

    CollectionToConsoleInLine(first, "First     :");
    CollectionToConsoleInLine(second, "Second    :");

    var exceptBy = first.ExceptBy(second.Select(x => x.Age), fp => fp.Age); //!!!
    CollectionToConsoleInLine(exceptBy, "ExceptBy  :");

}
UseExceptWithSelector();
```
```
First     :(Francis, 20)        (Lindsey, 30)   (Ashley, 40)
Second    :(Claire, 30) (Pat, 30)       (Drew, 33)
ExceptBy  :(Francis, 20)        (Ashley, 40)
```
Метод розширення ExceptBy() використовує селектор для видалення записів із першого набору, де значення селектора існує в другому наборі. Наступний метод створює два списки кортежів і використовує властивість Age для селектора. І Клер, і Пет мають той самий вік, що й Ліндсі, але значення віку для Френсіса та Ешлі не входять до другого списку. 

```cs
void UseIntersectByWithSelector()
{
    var first = new (string Name, int Age)[] { ("Francis", 20), ("Lindsey", 30), ("Ashley", 40) };
    var second = new (string Name, int Age)[] { ("Claire", 30), ("Pat", 30), ("Drew", 33) };

    CollectionToConsoleInLine(first, "First     :");
    CollectionToConsoleInLine(second, "Second    :");

    var intersectBy = first.IntersectBy(second.Select(x => x.Age), fp => fp.Age); //!!!
    CollectionToConsoleInLine(intersectBy, "IntersectBy:");

}
UseIntersectByWithSelector();
```
```
First     :(Francis, 20)        (Lindsey, 30)   (Ashley, 40)
Second    :(Claire, 30) (Pat, 30)       (Drew, 33)
IntersectBy:(Lindsey, 30)
```
Метод IntersectBy() поверне набір результатів, який містить загальні елементи даних у наборі контейнерів на основі селектора. Зауважте, що хоча Клер і Пет також мають те саме значення віку, що й Ліндсі, вони не повертаються, оскільки метод IntersectBy() повертає лише один результат на значення селектора.

### Відкідання повторювань (Distinct).

В багатьох випадках треба уникати дублювання в послідовності.

```cs
void UseDistinct()
{
    var queryMyCars =
        from c in myCars
        select c;
    var queryYourCars =
        from c in yourCars
        select c;
    ListCarToConsole(queryMyCars, "My          ");
    ListCarToConsole(queryYourCars, "Your        ");

    Console.WriteLine();

    var carConcat = queryMyCars.Concat(yourCars);
    ListCarToConsole(carConcat, "Concat     ");

    var carConacatDistinct = carConcat.Distinct();//!!! 
    ListCarToConsole(carConacatDistinct, "Concat Distinct");
}
UseDistinct();
```
```
My              :        Yugo   Aztec   BMW
Your            :        BMW    Saab    Aztec

Concat          :        Yugo   Aztec   BMW     BMW     Saab    Aztec
Concat Distinct :        Yugo   Aztec   BMW     Saab
```
Як бачите метод просто прибирає повторення не змінюючит порядок.

### Вилучення повторень з селектором.
```cs
void UseDistinctWithSelector()
{
    var first = new (string Name, int Age)[] { ("Francis", 20), ("Lindsey", 30), ("Ashley", 40) };
    var second = new (string Name, int Age)[] { ("Claire", 30), ("Pat", 30), ("Drew", 33) };

    
    CollectionToConsoleInLine(first,"First     :");
    CollectionToConsoleInLine(second, "Second    :");

    var concat = first.Concat(second);
    CollectionToConsoleInLine(concat, "Concat    :");

    var concatDistinctBy = concat.DistinctBy(x => x.Age);
    CollectionToConsoleInLine(concatDistinctBy,"DistinctBy:");

}
UseDistinctWithSelector();
```
```
First     :(Francis, 20)        (Lindsey, 30)   (Ashley, 40)
Second    :(Claire, 30) (Pat, 30)       (Drew, 33)
Concat    :(Francis, 20)        (Lindsey, 30)   (Ashley, 40)    (Claire, 30)    (Pat, 30)       (Drew, 33)
DistinctBy:(Francis, 20)        (Lindsey, 30)   (Ashley, 40)    (Drew, 33)
```

## Методи для агрегації.

Результат, який отриманий в результаті виконання запиту, можна додадково обробити методами які агрегують данні. Один із методів вже розглянули це Count. Клас Enumerable має інші методи агрегації.

```cs
void UseAggregateOperations()
{
    double[] winterTemperatures = { 2.0, -21.3, 8, -4, 0, 8.2 };

    PrintAll(winterTemperatures);

    var quertAllTemperatures = from t in winterTemperatures select t;

    double max = quertAllTemperatures.Max();
    PrintResult(max, "Max");

    double min = quertAllTemperatures.Min();
    PrintResult(min, "Min");

    double average = quertAllTemperatures.Average();
    PrintResult(average, "Average");

    double sum = quertAllTemperatures.Sum();
    PrintResult(sum, "Sum");


    void PrintResult(double value, string note)
    {
        Console.WriteLine($"{note}\t:{value}");
    }

    void PrintAll(double[] collection)
    {
        foreach (var item in collection)
        {
            Console.Write($"{item}\t");
        }
        Console.WriteLine();
    }
}
UseAggregateOperations();
```
```
2       -21,3   8       -4      0       8,2
Max     :8,2
Min     :-21,3
Average :-1,1833333333333336
Sum     :-7,100000000000001
```

## Операції агрегації з селектором

```cs

void UseAggregateOperationsWithSelector()
{

    CollectionToConsole(itemsInStock);

    Console.WriteLine("\n");

    Console.WriteLine("Product with maximum instock" );
    Console.WriteLine(SelectWithMaxBy(itemsInStock));
    Console.WriteLine("Product with minimum instock");
    Console.WriteLine(SelectWithMinBy(itemsInStock));

    ProductInfo? SelectWithMaxBy(ProductInfo[] products)
    {
       return products.MaxBy(p => p.NumberInStock);
    }
    ProductInfo? SelectWithMinBy(ProductInfo[] products)
    {
        return products.MinBy(p => p.NumberInStock);
    }

}
UseAggregateOperationsWithSelector();
```
```
Mac's Coffee                  Coffee with TEETH             24
Milk Maid Milk                Milk cow's love               100
Pure Silk Tofu                Bland as Possible             120
Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
Classic Valpo Pizza           Everyone loves pizza!         73


Product with maximum instock
Pure Silk Tofu                Bland as Possible             120
Product with minimum instock
Crunchy Pops                  Cheezy, peppery goodness      2

```
Агрегація виконується з використанням властивості об'єктів послідовності

## group .. by. Групування.

Коллекцію об'єктів можна згрупувати за властивостю.
```cs
record class Car(string Manufacturer, string Name, int Year);
```
```cs
void UseGroupBy()
{
    Car[] garage =
    {
        new("VW","e-UP",2015),
        new("Mercedes","W164",2005),
        new("VW","Käfer",1937),
        new("ЗАЗ","ЗАЗ-1102 Таврія",1992),
        new("Mercedes","W123",1981),
        new("ЗАЗ","ЗАЗ-965",1965),
        new("VW","Golf",1975),
        new("ЗАЗ","Lanos",2010),
    };

    var carGroups = from car in garage
                    group car by car.Manufacturer;

    foreach (var group in carGroups)
    {
        Console.WriteLine("\t"+group.Key);
        CollectionToConsole(group);
        Console.WriteLine();
    }

    // Same with the method
    var carGroupByMethod = garage.GroupBy(c => c.Manufacturer);
}
UseGroupBy();
```
```
        VW
Car { Manufacturer = VW, Name = e-UP, Year = 2015 }
Car { Manufacturer = VW, Name = Kafer, Year = 1937 }
Car { Manufacturer = VW, Name = Golf, Year = 1975 }

        Mercedes
Car { Manufacturer = Mercedes, Name = W164, Year = 2005 }
Car { Manufacturer = Mercedes, Name = W123, Year = 1981 }

        ЗАЗ
Car { Manufacturer = ЗАЗ, Name = ЗАЗ-1102 Тавр?я, Year = 1992 }
Car { Manufacturer = ЗАЗ, Name = ЗАЗ-965, Year = 1965 }
Car { Manufacturer = ЗАЗ, Name = Lanos, Year = 2010 }

```
Оператору group вказується шо треба групувати і за яким крітерієм. Результатом є набір об'єктів IGrouping<K,V>, тобто групп елементів початкової колеції зрупованих по крітерію. Цей крітерій групи можна отримати з властивості Key. 

### Групування з створенням нових об'єктів

Маючи колекцію груп можна створювати нові сутності.

```cs
void GroupingWithNewObjects()
{
    Car[] garage =
    {
        new("VW","e-UP",2015),
        new("Mercedes","W164",2005),
        new("VW","Käfer",1937),
        new("ЗАЗ","ЗАЗ-1102 Таврія",1992),
        new("Mercedes","W123",1981),
        new("ЗАЗ","ЗАЗ-965",1965),
        new("VW","Golf",1975),
        new("ЗАЗ","Lanos",2010),
    };

    var manufacturers = from car in garage
                    group car by car.Manufacturer into carGroup
                    select new { Manufacturer = carGroup.Key, Count = carGroup.Count() };
    
    CollectionToConsole(manufacturers);

    //same with method
    var manufacturerByMethod = garage
        .GroupBy(c => c.Manufacturer)
        .Select(g => new { Manufacturer = g.Key, Count = g.Count() });
}

GroupingWithNewObjects();
```
```
{ Manufacturer = VW, Count = 3 }
{ Manufacturer = Mercedes, Count = 2 }
{ Manufacturer = ЗАЗ, Count = 3 }
```
В цьому прикладі за допомогою into визначається змінна яка зберігає групу. Даці ця змінна використоаується для стоврення нового анонімного типу. 

### Вкладені запити.

Оскілки при групувані створюється колекція до неє можна зробити додадковий запит.

```cs
void NestedQuery()
{
    Car[] garage =
    {
        new("VW","e-UP",2015),
        new("Mercedes","W164",2005),
        new("VW","Käfer",1937),
        new("ЗАЗ","ЗАЗ-1102 Таврія",1992),
        new("Mercedes","W123",1981),
        new("ЗАЗ","ЗАЗ-965",1965),
        new("VW","Golf",1975),
        new("ЗАЗ","Lanos",2010),
    };

    var garageByManufacturer = from car in garage
                               group car by car.Manufacturer into carGroup
                               select new
                               {
                                   Manufacturer = carGroup.Key,
                                   Count = carGroup.Count(),
                                   Cars = from c in carGroup select c
                               };

    foreach (var group in garageByManufacturer)
    {
        Console.WriteLine($"\tManufacturer:{group.Manufacturer} Count:{group.Count}");
        CollectionToConsole(group.Cars);
        Console.WriteLine();
    }

    //same with method
    var garageByManufacturerWithMethod = garage
        .GroupBy(c => c.Manufacturer)
        .Select(g => new
        {
            Manufacturer = g.Key,
            Count = g.Count(),
            Cars = g.Select(car => car)
        });
}

NestedQuery();
```
```
        Manufacturer:VW Count:3
Car { Manufacturer = VW, Name = e-UP, Year = 2015 }
Car { Manufacturer = VW, Name = Kafer, Year = 1937 }
Car { Manufacturer = VW, Name = Golf, Year = 1975 }

        Manufacturer:Mercedes Count:2
Car { Manufacturer = Mercedes, Name = W164, Year = 2005 }
Car { Manufacturer = Mercedes, Name = W123, Year = 1981 }

        Manufacturer:ЗАЗ Count:3
Car { Manufacturer = ЗАЗ, Name = ЗАЗ-1102 Тавр?я, Year = 1992 }
Car { Manufacturer = ЗАЗ, Name = ЗАЗ-965, Year = 1965 }
Car { Manufacturer = ЗАЗ, Name = Lanos, Year = 2010 }
```
Для об'єкта анонімного типу властивость Cars створюеться за допомогою запиту до щойностворенної групи, який вибирає всі елементи групи. 

## join. Приєднання однієї колекції до іншої.

## Оператор join.

Дві разнотипні коллекції можуть мати щось спільне. Маючи крітерій спільності можно з'єднувати колеції.

Оператор join можно описати наступним псевдо-кодом

```cs
from object1 in collection1
join object2 in collection2 
on object2.PropertyFromClasc2 equals object1.ProperyFromClass1 
```
Тут до об'єкту першої коллекції приєднується об'єкт другої якшо його властивість співпадає з властивістю до якого іде приєднання.

Нехай у нас є наступні класи.
```cs
record class Cart_item(int Id, int Product_Id, int Quantyty);
record class Product(int Id, string Name, double Price);
```
Тут Cart_item слугує для зберігання одного рядка кошику купівлі товарів які зберігаються в Product. Спільні властивості які можуть їх пов'язувати це Cart_item.Product_Id та Product.Id 

```cs
void UseOperatorJoin()
{
    Product[] products =
    {
        new(1,"Jacket",100),
        new(2,"Shirt",15),
        new(3,"Head",20),
        new(4,"Toothbrash",2),
        new(5,"Eggs",2.5),
        new(6,"Bread",0.5)
    };

    List<Cart_item> cart = new()
    {
        new(1,3,1),
        new(2,5,1),
        new(3,6,2),
        new(4,10,1)
    };

    CollectionToConsole(cart);

    var cartAndProduct = from item in cart
                         join product in products
                         on item.Product_Id equals product.Id
                         select new
                         {
                             Item = item,
                             Product = product
                         };

    CollectionToConsole(cartAndProduct); Console.WriteLine();

    var purshase = from item in cart
                   join product in products
                   on item.Product_Id equals product.Id
                   select new
                   {
                       N = item.Id,
                       Name = product.Name,
                       Price = product.Price,
                       Quantity = item.Quantyty,
                       Amount = item.Quantyty * product.Price
                   };

    CollectionToConsole(purshase);

    var purshaseSum = purshase.Sum(i => i.Amount);
    Console.WriteLine(purshaseSum);
}
UseOperatorJoin();
```
```
Cart_item { Id = 1, Product_Id = 3, Quantyty = 1 }
Cart_item { Id = 2, Product_Id = 5, Quantyty = 1 }
Cart_item { Id = 3, Product_Id = 6, Quantyty = 2 }
Cart_item { Id = 4, Product_Id = 10, Quantyty = 1 }

{ Item = Cart_item { Id = 1, Product_Id = 3, Quantyty = 1 }, Product = Product { Id = 3, Name = Head, Price = 20 } }
{ Item = Cart_item { Id = 2, Product_Id = 5, Quantyty = 1 }, Product = Product { Id = 5, Name = Eggs, Price = 2,5 } }
{ Item = Cart_item { Id = 3, Product_Id = 6, Quantyty = 2 }, Product = Product { Id = 6, Name = Bread, Price = 0,5 } }


{ N = 1, Name = Head, Price = 20, Quantity = 1, Amount = 20 }
{ N = 2, Name = Eggs, Price = 2,5, Quantity = 1, Amount = 2,5 }
{ N = 3, Name = Bread, Price = 0,5, Quantity = 2, Amount = 1 }

23,5
```
Як ми бачимо в результат запиту до об'єктів першої послідовності приє'днуються об'єкти другою які відповідають крітерію. Також зверніть увагу шо якшо в послідоності яка приєднується нема об'єкта шо задовольняє крітерію то в резутьтат не попадає і об'єкт першої послідовності.

### Метод Join.

Аналогічно оператору коллекції можно поєднати за допомогою методу.

```cs
void UseMethodJoin()
{
    Product[] products =
    {
        new(1,"Jacket",100),
        new(2,"Shirt",15),
        new(3,"Head",20),
        new(4,"Toothbrash",2),
        new(5,"Eggs",2.5),
        new(6,"Bread",0.5)
    };

    List<Cart_item> cart = new()
    {
        new(1,3,1),
        new(2,5,1),
        new(3,6,2),
        new(4,10,1)
    };

    CollectionToConsole(cart);

    var purshase = cart.Join(
        products,
        item => item.Product_Id,
        product => product.Id,
        (item, product) => new
        {
            N = item.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = item.Quantyty,
            Amount = item.Quantyty * product.Price
        }
        );

    CollectionToConsole(purshase);

    Console.WriteLine(purshase.Sum(p=>p.Amount));
}
UseMethodJoin();
```
```
Cart_item { Id = 1, Product_Id = 3, Quantyty = 1 }
Cart_item { Id = 2, Product_Id = 5, Quantyty = 1 }
Cart_item { Id = 3, Product_Id = 6, Quantyty = 2 }
Cart_item { Id = 4, Product_Id = 10, Quantyty = 1 }

{ N = 1, Name = Head, Price = 20, Quantity = 1, Amount = 20 }
{ N = 2, Name = Eggs, Price = 2,5, Quantity = 1, Amount = 2,5 }
{ N = 3, Name = Bread, Price = 0,5, Quantity = 2, Amount = 1 }

23,5
```
Метод Join має декілька праметрів значення яких можна подивитись в документації ставши на метод та нажавши F12.
```cs
        //
        // Summary:
        //     Correlates the elements of two sequences based on matching keys. The default
        //     equality comparer is used to compare keys.
        //
        // Parameters:
        //   outer:
        //     The first sequence to join.
        //
        //   inner:
        //     The sequence to join to the first sequence.
        //
        //   outerKeySelector:
        //     A function to extract the join key from each element of the first sequence.
        //
        //   innerKeySelector:
        //     A function to extract the join key from each element of the second sequence.
        //
        //   resultSelector:
        //     A function to create a result element from two matching elements.
        //
        // Type parameters:
        //   TOuter:
        //     The type of the elements of the first sequence.
        //
        //   TInner:
        //     The type of the elements of the second sequence.
        //
        //   TKey:
        //     The type of the keys returned by the key selector functions.
        //
        //   TResult:
        //     The type of the result elements.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that has elements of type TResult
        //     that are obtained by performing an inner join on two sequences.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     outer or inner or outerKeySelector or innerKeySelector or resultSelector is null.
        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector);
```
Метод потребує такі параметри:
Список що приєднуеться.
Делегат шо визначає властивість об'єкта з поточного списку по якому йде приєднання.
Делегат шо визначає властивість об'єкта з другого списку по якому йде приєднання.
Делегат що визначає новий об'єкт результату.

### GroupJoin
Цей метод встановлює співвідношення між двоам коллекціями і групує результат.

Нехай в нас є класи.
```cs
record class Driver(string Name, int Experience);
record class Vehile(string Name, Driver Owner);
```
Створимо запит над двома коллекціями.
```cs
void UseGroupJoin()
{
    Driver petja = new("Petro", 5);
    Driver viktor = new("Viktor", 3);
    Driver olga = new("Olga", 2);

    List<Driver> drivers = new() { petja, viktor, olga };

    List<Vehile> garage = new()
    {
        new("Mersedes Sprinter",petja),
        new("VW Caddy", viktor),
        new("Peugeot Partner", olga),
        new("Mersedes Vito",viktor),
        new("VW Transorter",petja)
    };

    var query = drivers.GroupJoin(
        garage,
        driver => driver,
        vehile => vehile.Owner,
        (d, vehileCollection) => new
        {
            Driver = d,
            Cars = vehileCollection.Select(c => c.Name)
        });

    foreach (var item in query)
    {
        Console.WriteLine(item.Driver);
        CollectionToConsole(item.Cars);
    }

    //same with operator
    var queryO = from person in drivers
                 join car in garage
                 on person equals car.Owner into g
                 select new
                 {
                     Driver = person,
                     Cars = from c in g select c.Name
                 };
}
UseGroupJoin();
```
```
Driver { Name = Petro, Experience = 5 }
Mersedes Sprinter
VW Transorter

Driver { Name = Viktor, Experience = 3 }
VW Caddy
Mersedes Vito

Driver { Name = Olga, Experience = 2 }
Peugeot Partner
```
Метод GroupJoin приймає тіж самі параметри що Join окрім останнього. Останій параметр делегат який приймає парметр по якому йде группування і параметр групи з яких можна скласти новий анонімний тип який буде в результаті.

# Методи перевірки та отриманя даних з коллекції.

## All

```cs
void UseAll()
{
    List<Person> people = new()
    {
        new("Olja",28,new()),
        new("Petja",20,new())
    };

    bool moreThanEighteen = people.All(p => p.Age >18);

    Console.WriteLine(moreThanEighteen);

    bool lengthNameIs3 = people.All(p => p.Name.Length == 3);

    Console.WriteLine(lengthNameIs3);

}
UseAll();
```
```
True
False
```
Метод приймає в якості параметра делегат Func(Person,bool) predicate. Це умова яка провіряється для всіх елементів 
коллеції. Метод поверає true якшо умова виконується для всіх.

## Any
```cs
void UseAny()
{
    List<Person> people = new()
    {
        new("Olja",18,new()),
        new("Petja",20,new())
    };

    bool IsSomeoneOlderEighteen = people.Any(p => p.Age > 18);

    Console.WriteLine(IsSomeoneOlderEighteen);

    bool IsShortName = people.Any(p => p.Name.Length == 3);

    Console.WriteLine(IsShortName);
}
UseAny();
```
```
True
False
```
Метод приймає в якості параметра делегат Func(Person,bool) predicate. Це умова для елементів коллеції. Метод поверає true якшо хоча б для одиного елемента виконується умова.

## Contains

```cs
void UseConains()
{
    Person girl1 = new("Olga", 25, new());
    Person girl2 = new("Julia", 30, new());
    Person boy1 = new("Vova", 30, new());
    Person boy2 = new("Vitja", 28, new());

    List<Person> meeting = new()
    {
        girl1,girl2,boy1
    };

    bool IsSheOnMeeting = meeting.Contains(girl2);
    Console.WriteLine(IsSheOnMeeting);

    bool IsHeOnMeeting = meeting.Contains(boy2);
    Console.WriteLine(IsHeOnMeeting);
}
UseConains();
```
```
True
False

```
Для порівняння об'єктів використовується метод System.Object.Equlas.
Icнyє перезагрузка методу з другим параметром тип якого IComparer.

## First/FirstOrDefault

Метод повертає перший елемент з вказаною умовою або просто самої послідовності.
```cs
void UseFirstAndFirstOrDefault()
{
    List<Person> people = new()
    {
        new("Ira",27,new()),
        new("Petro",32,new()),
        new("Mikola",62,new()),
        new("Olga",30,new()),
        new("Marina",35,new())
    };
    
    CollectionToConsole(people);

    Console.WriteLine("people.First()");
    Console.WriteLine(people.First());  Console.WriteLine("\n");

    Console.WriteLine("people.First(p => p.Age == 30)");
    Console.WriteLine(people.First(p => p.Age == 30)); Console.WriteLine("\n");

    Console.WriteLine("people.FirstOrDefault(p => p.Age == 40)"); 
    Console.WriteLine($"Default is null:"+(people.FirstOrDefault(p => p.Age == 40) is null)); Console.WriteLine("\n");

    Console.WriteLine("people.FirstOrDefault(p => p.Age == 40, new(\"Someone\", 40, new()))"); 
    Console.WriteLine(people.FirstOrDefault(p=>p.Age == 40,new("Someone",40,new()))); Console.WriteLine("\n");


    Console.WriteLine("people.First(p => p.Age == 40)");
    Console.WriteLine(people.First(p => p.Age == 40));

}

UseFirstAndFirstOrDefault();
```
```
Person { Name = Marina, Age = 35, Languages = System.Collections.Generic.List`1[System.String] }

people.First()
Person { Name = Ira, Age = 27, Languages = System.Collections.Generic.List`1[System.String] }


people.First(p => p.Age == 30)
Person { Name = Olga, Age = 30, Languages = System.Collections.Generic.List`1[System.String] }


people.FirstOrDefault(p => p.Age == 40)
Default is null:True


people.FirstOrDefault(p => p.Age == 40, new("Someone", 40, new()))
Person { Name = Someone, Age = 40, Languages = System.Collections.Generic.List`1[System.String] }


people.First(p => p.Age == 40)
Unhandled exception. System.InvalidOperationException: Sequence contains no matching element
   at System.Linq.ThrowHelper.ThrowNoMatchException()
   at System.Linq.Enumerable.First[TSource](IEnumerable`1 source, Func`2 predicate)
   at Program.<<Main>$>g__UseFirsAndFirsOrDefault|0_47() in D:\MyWork\CS-Step-by-Step\13 LINQ до об'єкт?в\LINQ\LinqExpressions\Program.cs:line 1102
   at Program.<Main>$(String[] args) in D:\MyWork\CS-Step-by-Step\13 LINQ до об'єкт?в\LINQ\LinqExpressions\Program.cs:line 1106
```
Треба зауважити, коли в послідовності немає елемента який відповідає умові, або якщо послідовність порожня система генерує виняток як показанов прикладі. З цієї причини більш практично використовувати метод FirstOrDefault який вже робить перевірку і в який в якості другого параметру можна передати знасеня за замовчуванням.

## Last/LastOrDefault

Ці методи аналогічні First/FirstOrDefault з тією різницею шо шукають елементи з кінця послідовності.

# Внутрішне представлення запитів LINQ.

Використовуючи оператори from, in, select, where можна створити вирази запитів. API деяких функцію LINQ можно отримати під час виклику методів розширення класу Enumerable. При компіляції оператори запитів LINQ перетворюються в виклики методів класу Enumerable. Велика кількість методів була протипована для прийому делегатів як аргумент. Для багатьох методів потрібен загальний депутат Func<>. 
```cs
public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)

public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
```
Делегат Func представляє шаблон для заданої функції з набором до 16 аргументів і значенням що повертається.
```cs
public delegate TResult Func<TResult>()
public delegate TResult Func<T1,T2,T3,T4,TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
```
Враховуючи, що багато методів System.Linq.Enumerable вимагають делегата як вхідні данні, під час їх виклику, можна вручну створити новий тип та створити необхідні цільові методи, використати анонімний метод  або створити правільний лямбда-вираз. Давайте розглянемо кожен з ціх підходів.

## Як простіше стоврювати запити.

Ше раз розлянемо простий запит.
LinqUsingEnumerable\Program.cs
```cs
void QueryStringWithOretators()
{
    string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };

    var subset =
        from game in games
        where game.Contains(" ")
        select game;

    CollectionToConsoleInLine(subset);      

}
QueryStringWithOretators();

```
```
Uncharted 2     Fallout 3       System Shock 2

```
Очевидна перевага використання операторів для створення запиту полягає в тому, що делегати Func та виклики типу Enumerable абстрагуються від вашого коду. Компілятор сам виконує цей переклад. Побудова запитів таким чином найпоширений і найпростіший підхід.

## Побудова виразів запиту з використанням типу Enumerable та лябда-виразів.

Оператори запитів є скорченими версіями виклику методів роширення класу Enumerable. 

```cs
void QueryStringWithEnumerableAndLambdas()
{
    string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };
    CollectionToConsoleInLine(games);

    var subset = games
        .Where(game => game.Contains(" "))
        .OrderBy(game => game)
        .Select(game => game);
    CollectionToConsoleInLine(subset);
}
QueryStringWithEnumerableAndLambdas();
```
```
Morrowind       Uncharted 2     Fallout 3       Daxter  System Shock 2
Fallout 3       System Shock 2  Uncharted 2

```
В цьому прикладі безпосередьно викликаються методи розширення. З початку викликається метод Where для масиву рядків. Цей метод клас Array отримує як метод розширення з класу Enumerable. Для методу потрібен делегат System.Func<T1, TResult>. Перший переметр тип сумістний з коллекцією даних а другий є результатом виразу над елементом послідовності який може бути лямбда виразом. Результатом роботи методу є теж послідовність типу Enumerable для якої знову використовується метод розширення OrderBy. Для цього методу знову потрібен делегат Func. Цього разу ви передаєте кожен елемент почерзі через відповідний лямда вираз вказуючи шо ключ сортування є самє значення елементу. В результаті методу ми оримуємо упорядковану послідовність. Аналогічно при визові Select в лямбда виразі ми вказуемо шо ми вибираємо самє значення.
Визови методів трохи складніши ніж оперетори запиту. Цю послідовність визовів можна розгянути окремо.
```cs
void QueryStringWithEnumerableAndLambdasLong()
{
    string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };
    CollectionToConsoleInLine(games);

    var subset = games
        .Where(game => game.Contains(" "))
        .OrderBy(game => game)
        .Select(game => game);
    CollectionToConsoleInLine(subset);

    Console.WriteLine("\n");
    
    CollectionToConsoleInLine(games);

    var gamesWithSpaces = games
        .Where(game => game.Contains(" "));
    CollectionToConsoleInLine(gamesWithSpaces);
    
    var gamesWithSpacesAndOrderby = gamesWithSpaces
        .OrderBy(game => game);
    CollectionToConsoleInLine(gamesWithSpacesAndOrderby);

    var gamesWithSpacesAndOrderbyAndSelect = gamesWithSpacesAndOrderby
        .Select(game => game);
    CollectionToConsoleInLine(gamesWithSpacesAndOrderbyAndSelect);

}
QueryStringWithEnumerableAndLambdasLong();
```
```
Morrowind       Uncharted 2     Fallout 3       Daxter  System Shock 2
Fallout 3       System Shock 2  Uncharted 2


Morrowind       Uncharted 2     Fallout 3       Daxter  System Shock 2
Uncharted 2     Fallout 3       System Shock 2
Fallout 3       System Shock 2  Uncharted 2
Fallout 3       System Shock 2  Uncharted 2
```
Використовуючи методи ми вказуємо більш детально складові ніж використовуючи оператори. Крім того треба створювати цільові методи делегатів як параметри методів. 

## Побудова виразів запиту з використанням типу Enumerable та анонімних типів.

Враховуючи що лямбда-вираз це скороченя нотація анонімних методів, вираз запиту можна записати інакше.

```cs
void QueryStringWithWithAnonymousMethods()
{
    string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };
    CollectionToConsoleInLine(games);

    // Build the necessary Func<> delegates using anonymous methods.
    Func<string,bool> searchFilter = delegate(string gameName)
    {
        return gameName.Contains(" ");
    };
    Func<string, string> itemToProcess = delegate (string gameName)
    {
        return gameName;
    };

    var subset = games
        .Where(searchFilter)
        .OrderBy(itemToProcess)
        .Select(itemToProcess);

    CollectionToConsoleInLine(subset);

}
QueryStringWithWithAnonymousMethods();
```
```
Morrowind       Uncharted 2     Fallout 3       Daxter  System Shock 2
Fallout 3       System Shock 2  Uncharted 2
```
Цей варіант ще більше багатослівний оскільки створюються окремі методі делегатів. Позитивним є те, що синтаксис анонімного методу зберігає всю обробку делегату в одному визначенні методу.

## Побудова виразів запиту з використанням розширенного створеня делегатів.

Самий великий за кодом варіант не використовувати скороченя які дають анонімні методи. 
Створимо новий клас
```cs
internal class VeryComplexQueryExpression
{
    //MethodForCall
    public static void QueryStringsWithRawDelegates()
    {
        string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };
        CollectionToConsoleInLine(games);

        // Build the necessary Func<> delegates.
        Func<string, bool> searchFilter = new Func<string, bool>(Filter);
        Func<string, string> itemToProcess = new Func<string, string>(ProcessItem);

        var subset = games
            .Where(searchFilter)
            .OrderBy(itemToProcess)
            .Select(itemToProcess);
        CollectionToConsoleInLine(subset);            

    }

    // Delegate targets.
    public static bool Filter(string item)
    {
        return item.Contains(" ");
    }

    public static string ProcessItem(string  item)
    {
        return item;
    }

    // Helper mathod
    public static void CollectionToConsoleInLine<T>(IEnumerable<T>? collection)
    {
        if (collection == null) return;

        foreach (var item in collection)
        {
            Console.Write(item + "\t");
        }
        Console.WriteLine();
    }
}

```
```cs
VeryComplexQueryExpression.QueryStringsWithRawDelegates();
```
```
Morrowind       Uncharted 2     Fallout 3       Daxter  System Shock 2
Fallout 3       System Shock 2  Uncharted 2
```
В цьому варіанти створені окремі методи яки використовуються.

## Висновки.

Language-Integrated Query (LINQ) це набір технологій як спроба зробити запити до даних безпосередьно язиковими конструкціями. Спроба створити єдиний підхід до різних форм даних. LINQ може взаємодіяти з будь-яким типом що реалізує IEnumerable<T>, включаючи масива та узагальнені і неузагальнені коллекції.
Технологія LINQ виконується за допомогою кількох можливостей мови. Для визначеня типу результатту використовується var. Лямбда-вирази, синтаксис ініціалізації об’єктів і анонімні типи можна використовувати для створення функціональних і компактних запитів LINQ.
Більшисть статичних методів взаємодії з даними визначено в System.Linq.Enumerable.Більшість членів Enumerable працюють з типами делегатів Func<T>, які можуть приймати буквальні адреси методів, анонімні методи або лямбда-вирази як вхідні дані для оцінки запиту.