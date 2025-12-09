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

    Point point = new Point(1);
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
CheckingValueType();

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

## Встановлення значення

Коли одній змінній ValueType приваюється друга зміна то відбуваеться копіювання даних.
```cs

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
AssignValueType();

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
```
5
X:1 Y:1
X:2 Y:2
X:1 Y:1
```

Для типів int для а і b в стеку створюеться окремі місця і значення з одного місця копіюється в інше. Оскільки Point це Value type відбуваеться аналогічне. В стеку виділяються окремі місця і значення X і Y коіюються почерзі. Оскільіки змінні в стеку пребувають окремо змінна однієї не впливає на іншу. 

Інша справа коли змінна належить до Reference type
```cs
static void AssignReferenceType()
{
    Apartment apartment5 = new Apartment(5, 42); 
    Apartment apartment7 = new();
    
    apartment7 = apartment5; 
    apartment7.Info();

    apartment7.Number = 7;
    apartment5.Info();
}
AssignReferenceType();

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
```
Apartment:5 Square:42
Apartment:7 Square:42
```
В цьому випадку спочатку створюється в стеку змінна apartment5 в якій зберігаеться посилання на створенний у manadged heap екземпляр класу Apartment. Потім створюється в стеку друга зміна якій в стеку копіюється посилання на той самий об'єкт. Таким чином обі змінні вказують на той самий обїект в пам'яті. Використання будь якої зних впливає на той самий об'єкт. 

## Reference тип в Value типі.

В середині структурі може бути тип посилання.
```cs
static void ReferenceTypeWithinValueType()
{
    Console.WriteLine("Create point2 = point1");

    PointOnLine point1 = new PointOnLine("point1","X",100);
    PointOnLine point2 = point1;

    point1.Display();
    point2.Display();

    Console.WriteLine("Change value type fild. point2 200 ");
    point2.Name = "point2";
    point2.Value = 200;
    point1.Display();
    point2.Display();

    Console.WriteLine("\nVariant 1");
    Console.WriteLine("Change regerence type fild. point2.Axis = new Axis(\"Y\");  ");

    point2.Axis = new Axis("Y");
    point1.Display();
    point2.Display();

    Console.WriteLine("\nAgain point2 = point1");
    point2 = point1;
    point1.Display();
    point2.Display();

    Console.WriteLine("\nVariant 2");
    Console.WriteLine("point2.Axis.Name = \"Y\";");
    point2.Axis.Name = "Y";
    point1.Display();
    point2.Display();
}
ReferenceTypeWithinValueType();

class Axis
{
    public string Name;

    public Axis(string name)
    {
        Name = name;
    }
}

struct PointOnLine
{
    public string Name;
    public Axis Axis;
    public double Value;

    public PointOnLine(string name, string axisName, double value)
    {
        Name = name;
        Axis = new Axis(axisName);
        Value = value;
    }

    public void Display()
    {
        Console.WriteLine($"{Name} - {Axis.Name} : {Value}");
    }

}
```
```
Create point2 = point1
point1 - X : 100
point1 - X : 100
Change value type fild. point2 200
point1 - X : 100
point2 - X : 200

Variant 1
Change regerence type fild. point2.Axis = new Axis("Y");
point1 - X : 100
point2 - Y : 200

Again point2 = point1
point1 - X : 100
point1 - X : 100

Variant 2
point2.Axis.Name = "Y";
point1 - Y : 100
point1 - Y : 100

```

При створені PointOnLine point2 = point1; в стеку зберігаеться окремо змінні з індентичними данними в тому чилі посилання на один и той самий об'єкт в heap. Коли виконуеться point2.Axis = new Axis("Y"); тоді в heap створюеться новій окремий єкзкмпляр класу Axis і point2.Axis зберігає посилання на нього. Це ніяк не впливає на point1 тому що вони окрумі. При повторному point2 = point1; point1.Axis і point2.Axis знову вказує на один і той самий об'єкт в heap. І коли ми виконуємо  point2.Axis.Name = "Y"; тим самим міняємо об'єкт на який вказує і point1.Axis.

Важливо пам'ятати що якшо тип маю в собі посиланя то при присваюванні копіюється посилання на той самий об'єкт, а не створюється копія об'єкту. 

Для глибокого копіювання потрібно де стан копіюється в новий об'єкт можна реалізувати інтерфейс IClonable.

## Передача параметра Reference типа як значення.

```cs
static void UsingReferenceTypeAsParameterAsValue()
{
    Person girl  = new Person("Julia", 29);
    
    Console.Write("Before:");
    girl.Dislpay();

    AgePlusOne(girl);
    
    Console.Write("After:");
    girl.Dislpay();
  
    static void AgePlusOne(Person person)
    {
        person.Age++;
        person = new Person("Olga", 27);
        Console.WriteLine("----Person within method----");
        person.Dislpay();
        Console.WriteLine("----------------------------");
    }
}
UsingReferenceTypeAsParameterAsValue();

class Person
{
    public string Name;
    public int Age;

    public Person()
    {
    }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Dislpay() => Console.WriteLine($"Name: {Name}  Age: {Age}");

}

```
```
Before:Name: Julia  Age: 29
----Person within method----
Name: Olga  Age: 27
----------------------------
After:Name: Julia  Age: 30
```

Коли метод має параметри типу reference без модіфікаторів він створюе в стеку методу зміну person в якій копіюється посилання на той самий об'єкт в heap що і зміна girl. Тому person.Age++; змінює цей об'єкт.  При визові person = new Person("Olga", 27); в person записуеться посилання на інший об'єкт. Оскілки person окрема копія данних запис нового посиланя не впливає на посилання в girl.

Таким чином якщо посилальний тип передається за значенням, виклик може змінити значення даних стану об’єкта, але не об’єкт, на який він посилається.

## Передача параметра Reference типа як посилання.

По іншому працює метод коли параметр має модіфікатор ref і предаеться як посилання.

```cs
UsingReferenceTypeAsParameterAsReference();
static void UsingReferenceTypeAsParameterAsReference()
{
    Person girl = new Person("Julia", 29);

    Console.Write("Before:");
    girl.Dislpay();

    AgePlusOne(ref girl);

    Console.Write("After:");
    girl.Dislpay();

    static void AgePlusOne(ref Person person)
    {
        person.Age++;
        person = new Person("Olga", 27);
        Console.WriteLine("----Person within method----");
        person.Dislpay();
        Console.WriteLine("----------------------------");
    }
}

class Person
{
    public string Name;
    public int Age;

    public Person()
    {
    }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Dislpay() => Console.WriteLine($"Name: {Name}  Age: {Age}");

}
```
```
Before:Name: Julia  Age: 29
----Person within method----
Name: Olga  Age: 27
----------------------------
After:Name: Olga  Age: 27
```
В цьому випадку метод може змінити не тільки стан об'єкту на який вказуе person, а також може змінити об'ект на який посилаеться girl.