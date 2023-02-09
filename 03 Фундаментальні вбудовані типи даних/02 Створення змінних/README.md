# Створення змінних

Для початку створимо нове рішеня FundamentalDataType з проектом CreateVarіable.

Програми зазвичай виконують обробку даних. Данні приходять зовні, обробляються повертаються зовні. Зміні потрібні для збереження даних локально в межах методу, классу а також для предачі параметрів та отримання результату від метода. Використовуючи змінни треба розуміти в якому типі її зберігати, скількі місця вона займає і як швидко обробляеться. Крім того зміним треба давати назву згідно того що вони зберігають. Змінна var pribor35 не зовсім зрозуміла.

Щоб створити змінну треба вказати тип і назву. Краще зразу присвоїти значеня або вказати default.

```cs
CreateVarable();

static void CreateVarable()
{
    string name = "";
    decimal price = default;
    bool electric = false, forChildren = false;
    double weight = default, height=default;

    name = "ВЕЛОСИПЕД 20 DOROZHNIK ONYX 2022";
    weight = 14.23;
    price = 12853; 
    
    Console.WriteLine($"{name} {price} {weight} ");
} 
```

Дозволено в одній строчці обявляти типи декількох змінних. Літерал default дозволяє присвоїти змінній значення для типу за замовченням. 

Всі вбудовані типи мають конструктор за замовчуванням який можна визвати за допомогою new. Подимивось які значеня присваює цей конструктор для різних типів.

```cs
UsingNew();
static void UsingNew()
{
    int myInt = new int();
    double myDouble = new();
    bool myBool = new();
    DateTime myDateTime = new();

    Console.WriteLine("By default:");
    Console.WriteLine($"int:{myInt}");
    Console.WriteLine($"double:{myDouble}");
    Console.WriteLine($"bool:{myBool}");
    Console.WriteLine($"DataTime:{myDateTime}");
}
```
# Неявна типізація 

В C# є можливість замість типу вказати ключеве слово var. Компілятор на основі даних що ініціалізують змінну визначає її тип.

```cs
ImplicitDeclarations();
static void ImplicitDeclarations()
{
    var code = "025441";
    var name = "Bicycle";
    var weight = 14.23;
    var inStock = true;
   
    //var something; //must be initialized


    Console.WriteLine($"{code} {name} {weight} {inStock}");
    Console.WriteLine($"{code.GetType()} {name.GetType()} {weight.GetType()} {inStock.GetType()}");
}
```

Зміну з var треба ініціалізувати і не значенням null. Але коли змінна вже ініціалызована reference типом їй можна присваювать значення null.

Var можна використовувати не тільки для примітивних вбудованих типів але і для складних які ви створюєте самі. 

Неявну типізацію можна викорустовувати лише для локальних змінних. Не можна використовувати var для значеня шо поверає метод або для параметрів. Не можно використовувати для полів даних класу. 

```cs
Console.WriteLine(GetVarString("Hi girl"));

static string GetVarString(string enterString)
{
    var myString =  enterString+"!";

    //myString = 45;// Cannot implicitly convert int to string

    return myString;
}

```
Дозволено повернати неявно визначену зміну ящо її тип співпадае з зазначеним у методі.
Змінна визначена за допомогою var по суті строго типізована і інколи скорочуе ввод тексту. Під час копіляції тип визначаеться. Компілятор не дозволить змінній типа string ,оголошеный неявно, призначити int.

# Коли корисно використовувати var.

Для локальних змінних використовування неявної типізації мало чого дає. Можливо тільки коли тип дуже довгий меньше набирати. Але явна типізація більше зрозуміла для тих хто читає код. 

```cs
 static void ReadebleCode()
{
    var power = 12;

    var sum = GetSum(10,15);

    var squareSum = sum * sum;

    var result = squareSum * power;

    static double GetSum(int a,int b)
    {
        return a + b;
    }
}
```

Хоча компілятор підказуе якого типу в результаті буде result, але щоб зрозуміти чому треба пробігтись по всім рядкам.

Var дійсно корисний коли тип даних складно прописати при визначенні запитів LINQ.

```cs
UsinVarForLinq();

static void UsinVarForLinq()
{
    int[] temperaturs = { 5, 12, 4, 15, 10, 8, 17 };

    var normal = from t in temperaturs where t > 10 select t;

    foreach (var item in normal)
    {
        Console.WriteLine(item);
    }

    Console.WriteLine(normal.GetType().Name);
}
```
Як ви бачите normal не масив int. На щастя, при практичному використанні LINQ немає потреби точно вказувати тип який повертає запит. 

В інших випадках використання var може зробити код нечитабельним і привести до неправільного використання.






