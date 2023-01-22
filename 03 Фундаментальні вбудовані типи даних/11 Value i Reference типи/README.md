# Value i Reference типи

Додамо проект ValuesAndReferences

System.Object є предком всіх інших типів. Від нього походять тип ValueType а також типи посилання(Reference type).

Типи які походять від ValueType називають Value типами або тип значення. До них відносяться     

- числа (int,double,... )   
- bool                      
- void      
- char                      
- час (DataTime,TimeSpan,... )
- struct
- enum


До типів посилання (Reference type) відноситься 

- object      
- class    (System.Type)
- string   (System.String)
- array    (System.Array)
- exeption (System.Exeption)
- delegate (System.Delegate) 

```cs
CheckingValueType();

static void CheckingValueType()
{
    Console.WriteLine("Value Type:");

    int myInt = 1;
    Console.WriteLine($"int is ValueType: {myInt is ValueType}");

    double myDouble = 1.5;
    Console.WriteLine($"double is ValueType: {myDouble is ValueType}");

    bool myBool = true;
    Console.WriteLine($"bool is ValueType: {myBool is ValueType}");

    char myChar = 'c';
    Console.WriteLine($"char is ValueType: {myChar is ValueType}");

    DateTime time = DateTime.Now;
    Console.WriteLine($"DateTime is ValueType: {time is ValueType}");

    Point point = new Point(1,2);
    Console.WriteLine($"Structure is ValueType: {point is ValueType}");

    Season season = Season.Winter;
    Console.WriteLine($"Enum is ValueType: {season is ValueType}");
        
    
    Console.WriteLine("\n\nReference type: ");

    object myObject = new();
    Console.WriteLine($"оbject is ValueType: {myObject is ValueType}");

    Apartment apartment = new();
    Console.WriteLine($"class is ValueType: {apartment is ValueType}");

    string myString = "Hi";
    Console.WriteLine($"string is ValueType: {myString is ValueType}");


    int[] myArray = new int[] { 1, 2, 3, };
    Console.WriteLine($"array is ValueType: {myArray is ValueType}");

    Exception exception = new Exception();
    Console.WriteLine($"exception is ValueType: {exception is ValueType}");
}

struct Point
{
    public int X { get; set; }
    public Point(int x)
    {
        X = x;
    }
}

enum Season
{
    Autumn,
    Winter,
    Spring,
    Summer
}

class Apartment
{
    public int Number { get; set; }
    public double Square { get; set; }

    public Apartment()
    {
    }

    public Apartment(int number, double square)
    {
        Number = number;
        Square = square;
    }
}
```
Результат
```
Value Type:
int is ValueType: True
double is ValueType: True
bool is ValueType: True
char is ValueType: True
DateTime is ValueType: True
Structure is ValueType: True
Enum is ValueType: True


Reference type:
оbject is ValueType: False
class is ValueType: False
string is ValueType: False
array is ValueType: False
exception is ValueType: False
```

Головне для чого існує ValueType це гарантувати щоб похідний тип розішувався в stack, а не в garbage-collected heap. Данні розміщені в стек швидко створюються та знишуються і час їх життя  визначається визначальною областю. Коли змінна типу значення випадає з області визначення, вона негайно видаляється з пам’яті. З іншого боку, дані, виділені в динамічній пам’яті, контролюються збирачем сміття .NET Core і мають тривалість життя, яка визначається багатьма факторами.


```cs
static void UsingValueInStack()
{
    static void ValueInStack()
    {
        int myInt = 5;
        Season season = Season.Autumn;
    } //at now myInt, season is not in the stack, memory
}
```

# Присвоення

Коли одній змінній ValueType приваюється друга зміна то відбуваеться копіювання даних.
```cs

AssignValueType();
static void AssignValueType()
{
    int a = 5;
    int b = a;
    Console.WriteLine(b);

    Point pointA = new Point(1,1);
    Point pointB = pointA;
    pointB.Display();
    
    pointB.X = 2;
    pointB.Y = 2;
    pointB.Display();
    pointA.Display();
}

struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Display()
    {
        Console.WriteLine($"X:{X} Y:{Y}");
    }
}
```
Для тип int для а і b в стеку створюеться окремі місця і значення з одного місця копіюється в інше. Оскільки Point це Value type відбуваеться аналогічне. В стеку виділяються окремі місця і значення X і Y коіюються почерзі. Оскільіки змінні в стеку пребувають окремо змінна однієї не впливає на іншу. 

Інша справа коли змінна належить до Reference type
```cs

AssignReferenceType();
static void AssignReferenceType()
{
    Apartment apartment5 = new Apartment(5, 42); 
    Apartment apartment7 = new();
    
    apartment7 = apartment5; 
    apartment7.Info();

    apartment7.Number = 7;
    apartment5.Info();
}

class Apartment
{
    public int Number { get; set; }
    public double Square { get; set; }

    public Apartment()
    {
    }

    public Apartment(int number, double square)
    {
        Number = number;
        Square = square;
    }

    public void Info()
    {
        Console.WriteLine($"Apartment:{Number} Square:{Square}");
    }
}
```
В цьому випадку спочатку створюється в стеку змінна apartment5 в якій зберігаеться посилання на створенний у manadged heap єкземпляр класу Apartment. Потім створюється в стеку друга зміна якій в стеку копіюється посилання на той самий об'єкт. Таким чином обі змінні вказують на той самий обїект в пам'яті. Використання будь якої зних впливає на той самий об'єкт. 



