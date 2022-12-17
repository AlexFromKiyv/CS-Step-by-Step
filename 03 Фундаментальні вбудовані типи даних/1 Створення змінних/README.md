# Створення змінних

Зміні потрібні для збереження даних локально в межах методу, классу а також для предачі параметрів та отримання результату від метода.

Щоб створити змінну треба вказати тип і назву. Краще зразу присвоїти значеня або вказати default.

```
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








