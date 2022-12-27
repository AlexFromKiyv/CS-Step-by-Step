# Створення змінних

Для початку створимо нове рішеня FundamentalDataType з проектом CreateVarіable.

Зміні потрібні для збереження даних локально в межах методу, классу а також для предачі параметрів та отримання результату від метода.

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

Дозволено в одній строчці обявляти типи декількох змінних. Літерал <em>default</em> дозволяє присвоїти змінній значення для типу за замовченням. 

Всі вбудовані типи мають конструктор за замовчуванням який можна визвати за допомогою <em>new</em>. Подимивось які значеня присваює цей конструктор для різних типів.

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

    return myString;
}

```

Дозволено повернати неявно визначену зміну ящо її тип співпадае з зазначеним у методі.







