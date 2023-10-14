# LINQ до об'єктів.

Більша частина програм обробляє дані в пеаних формах при виконанні. Це може бути массив, коллекція в пам'яті, база даних, файл XML. Доступ до ціх даних схожий але довгий час виористовувався різний API. Набір технологій LINQ забезпечує стислий, симетричний і строго типізований спосіб доступу до широкого спектру сховищ даних.

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

Якшо говорити про дані то це не тілки те шо зберігається в базі данних. Це можуть бути XML документи або текстовий файл. Дані можуть знаходитись в багатьох місцях. Це може бути масив або список List<T>. Однозначно виникне потреба пошукі та виділення підмножини елементів. 
Колись для роботи з різними даними використовувалось дуже різне API. Наче нічого паганого немає в використані різних підходів до різних даних. Однак основна проблема в тому що кожний такій підхід є окремим "островом".
LINQ API - це спроба забезпечити послідовний симетричний спосіб, за допомогою якого можна отримати і маніпулювати "даними" в широкому розумінні цього терміну.
Використовуючи LINQ, можна сворювати конструкції безпосередньов в C#, які називають виразами запитів. Вираз запиту можна використовувати для взаємодії з багатьма типами даних. По суті це опис загального підходу до доступу до даних. 
Однак залежно від того, де ви застосовуєте свої запити LINQ, ви зустрінете різні терміни, наприклад такі:

    LINQ to Objects: цей термін відноситься до акту застосування запитів LINQ до масивів і колекцій.

    LINQ to XML: цей термін стосується використання LINQ для роботи з документами XML і запитів.

    LINQ to Entities: цей аспект LINQ дозволяє використовувати запити LINQ в основному API ADO.NET Entity Framework (EF).

    Parallel LINQ (він же PLINQ): це дозволяє паралельно обробляти дані, отримані із запиту LINQ.

Зараз LINQ є невід’ємною частиною бібліотек базових класів .NET, керованих мов і самої Visual Studio.
Запити LINQ строго типізовані. Це забезпечує компілятор. 
Роботу ціх запитів забезпечує System.Linq як задіюється завдяки global using.

## Застосування LINQ до массивів.
Хай ми маємо масив.
```cs
    string[] currentVideoGames =
    {
        "Morrowind",
        "Uncharted 2",
        "Fallout 3",
        "Daxter",
        "System Shock 2"
    };
```
А також допоміжний метод.
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

LinqOverArray
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
        where ng.Contains(" ")
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
Тут використовуються оператори LINQ from, in, where, orderby, and select. Вираз досить лаконіяний і зрозумілий. Кожний елемент який задовольняє крітерію отримав ім'я ng (names of game). Зверніть увагу шо повернута послідовність збкрівється в змінній яка реалізує загальний тип IEnumerable<T>, де T є System.String.

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

    //Print
    foreach (string s in gamesWithSpace)
    {
        if (s != null)
        {        
            Console.WriteLine(s);
        }
    }
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

Хоча в прикладах не створювалися методи розширеня, вони використовуються в як основа  в фоновому режиті. Вирази запиту LINQ можна використовувати для повернення контейнерів даних, які реалізовують загальний інтерфейс IEnumerable<T>. Але System.Array не реалізовує цей інтерфейс.
```cs
public abstract class Array : ICollection, IEnumerable, IList, IStructuralComparable, IStructuralEquatable, ICloneable
```
Він опосередковано отримує необхідну функціональнисть через статичний клас System.Linq.Enumerable. Цей службовий клас визначає велику кількість методів розширення( Aggregate<T>(), First<T>(), Max<T>(),...) які Array набуває у фоновому режимі. Тому коли поставити крапу після масиву можна побачити велику кількість методів, які не визначені в Array.   

## Роль відкладенного виконання.

Для запитів LINQ dажливо визначити, що коли вони повертають послідовність, вони не визначать значеня до тих пір пока не буде ітерації над послідовністю. виконання визначення відкладається. Перевага підходу полягає в тому що можна застосовувати той самий запит кілька разів до того самого контейнера. Прицому ви будете отримувати найновіший результат.

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

    number = (from i in ints where i >0 select i).FirstOrDefault();
    Console.WriteLine($"(from i in ints where i >0 select i).FirstOrDefault() : {number}");

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

Для методів FirstOrDefault, SingleOrDefault та схожі можна встановити значення яке буде повертатися у разі якщо запит не занйшов відповідних значень.

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
Часто запити LINQ визначабть в межах методу або властивості. Зміна яка використовуєтся для зберігання результату визначається за допомогою var як неявна типізована. Неявнотипізовані зміни не можна використовувати длдя визначення параметрів, значень поверненя методу або полів стуктур. Враховуючи це повстає питання як саме ви можете повернути результат запиту зовнішньому коду шо його запитує. Відповідь для кожного випадку різна і залежить від контексту і потреб. Якщо у вас є набір результатів із строго типізованих даних, таких як масив рядків або List<Car> можна відмовитися від використання var і використовувати IEnumerable<T> або IEnumerable.  
```cs
void LinqReturnValues()
{

    CollectionToConsole(GetAllWithRed());    

    IEnumerable<string> GetAllWithRed()
    {
        string[] colors = { "Light Red", "Green", "Yellow", "Dark Red", "Red", "Purple" };

        IEnumerable<string> result = from c in colors where c.Contains("Red") select c;
        return result; 
    }
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
    CollectionToConsole(GetAllWithRed());

    string[] GetAllWithRed()
    {
        string[] colors = { "Light Red", "Green", "Yellow", "Dark Red", "Red", "Purple" };

        IEnumerable<string> query = from c in colors where c.Contains("Red") select c;
        return query.ToArray();
    }
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

Розглянемо коллекцію. 

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

```cs
List<Car> myCar = new()
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
    GetFastCars(myCar);

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
Таким чином можна робити вибор враховуючи властивості об'єктів колекції. Але умови вибору можна робити біль складнішими.

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
    ArrayList myCars = new ArrayList() 
    {
        new Car{ PetName = "Henry", Color = "Silver", Speed = 100, Make = "BMW"},
        new Car{ PetName = "Daisy", Color = "Tan", Speed = 90, Make = "BMW"},
        new Car{ PetName = "Mary", Color = "Black", Speed = 55, Make = "VW"},
        new Car{ PetName = "Clunker", Color = "Rust", Speed = 5, Make = "Yugo"},
        new Car{ PetName = "Melvin", Color = "White", Speed = 43, Make = "Ford"}
    };

    // Transform ArrayList into an IEnumerable<Car>-compatible type.
    var myCarsGeneric = myCar.OfType<Car>();

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

Контейнер неузагальненого типу скоадаеться із елементів які є прототипом System.Object і відповідно в коллекції можуть бути будь-які данні.
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
Якшо методу TypeOf<T> вказати певний тип він проводячи трасформацію відбере в ітерації всі елементи які відповідають цтому типу.

# Вирази запросів LINQ .

В С# є багато операторів для запиту LINQ. Крім операторів в проторі імен System.Linq.Enumerable є методи розширення яки не мають прямої скороченної нотації в С#.
Де які зазвичай часто викорустовуємі оператори.

    from, in : Використовуються для визначення з якого контейнера брати данні.

    where : Використовуються як обмеження для визначення які елементів брати із  контейнера.

    select : Для вказівкі що брати із контейнера.

    join, on, equals, into: Виконує об'єднання на основі вказаного ключа. Пам’ятайте, що ці «з’єднання» не обов’язково мають бути пов’язані з даними в реляційній базі даних.

    orderby, ascending, descending : Дозволяє впорядкувати отриману підмножину в різних порядках(зростання спадання).

    groupby : видає підмножину з даним, згрупованим за вказаним значенням.

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

## Базовий вибор за допомогою select

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

## Отримання підмножин даних.

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

## Вибір частинами. (paging data)

Якшо потрібно отримати певну кількість записів з виборки можна використати методи Take()/TakeWhile()/TakeLast() и Skip()/SkipWhile()/SkipLast(). Ці методи визначені в IEnumerable, тому можна їх викаристати до результату запиту LINQ або в випдку методів розширення безпосередньо. Ці методи також видкладають виконання.   

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

Можна використовувати комбінації ціх методів.

```cs
void UseSkipAndTake()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithSkipAndTake(itemsInStock, 3, 2);

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


Crunchy Pops                  Cheezy, peppery goodness      2
RipOff Water                  From the tap to your wallet   100
```

