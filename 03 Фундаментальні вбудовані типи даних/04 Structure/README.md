# Structure

Структури добре підходять для моделювання математичних, геометричних і атомарних сутностей. Структури можуть реалізовувати інтерфейси але не можна успадковувати і бути основою класа. Структури можуть містити поля даних та медоди роботи з ними.
Проект Structs
```cs
UsingSimpleStructure();

static void UsingSimpleStructure()
{
    Point point;
    //point.Display(); don't work

    point.X = 1;
    //point.Display(); don't work

    point.Y = 2;
    point.Display();

    point.Increment();
    point.Display();

    point.Increment();
    point.Display();

    point.Decrement();
    point.Display();

    point.Decrement();
    point.Display();

    Console.WriteLine(point);
    Console.WriteLine(point.GetType());
    Console.WriteLine(point.ToString());
}

struct Point
{
    // coordinates 
    public int X;
    public int Y;


    /// <summary>
    /// Add 1 to coordinates 
    /// </summary>
    public void Increment()
    {
        X++; Y++;
    }

    /// <summary>
    /// Subtract 1 from coordinates 
    /// </summary>
    public void Decrement()
    {
        X--; Y--;
    }

    /// <summary>
    /// Display position of point
    /// </summary>
    public void Display()
    {
        Console.WriteLine($"X:{X} Y:{Y}");
    }

}
```
У цьому прикладі визначена структура з двома загально досупними полями і методами. Модіфікатор public дозволяе мати доступ до поля змінної. Але робити поля напряму загальнодоступними пагана практика. Краще для цього використовувати властивості, а поля робити приватним.


## Конструктори

Як видно із прикладу перед тим як змінну типа струтура використовувати вона повина бути проініціалізована. Тобта всі поля повини мати значення. Ви можете створити змінну структури використовуючи new, який виключе конструктор за замовчуванням. 

```cs
UsingStructureConstructor();
static void UsingStructureConstructor()
{
    Point point = new Point();
    point.Display();

    Point point1 = new Point(1, 1);
    point1.Display();
}


struct Point
{
    // coordinates 
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Add 1 to coordinates 
    /// </summary>
    public void Increment()
    {
        X++; Y++;
    }

    /// <summary>
    /// Subtract 1 from coordinates 
    /// </summary>
    public void Decrement()
    {
        X--; Y--;
    }

    /// <summary>
    /// Display position of point
    /// </summary>
    public void Display()
    {
        Console.WriteLine($"X:{X} Y:{Y}");
    }

}
```
Всі поля після цого заповнюються значеннями за замовчуванням (default).

## Ініціалізація

Структури можна ініціалізувати при декларуванні.

```cs
UsingInizializersStructure();

static void UsingInizializersStructure()
{
    Coordinates coordinates = new Coordinates();
    coordinates.Display();
}

struct Coordinates
{
    public int X = 1;
    public int Y = 1;

    public Coordinates()
    {
    }

    public void Display()
    {
        Console.WriteLine($"{X}:{Y}");
    }
}
```
## Read-only структури

Структури можуть бути тіки для читання і це означає шо вони незмінні. Вони можуть бути більш продуктивними. 
```cs
static void UsingReadonlyStructure()
{
    ApartmentSquare myApartment = new ApartmentSquare(59);

    //myApartment.Square = 72; it don't work

    myApartment.Display();

}

readonly struct ApartmentSquare
{
    public double Square { get; }

    public ApartmentSquare(double square)
    {
        Square = square;
    }

    public void Display()
    {
        Console.WriteLine(Square);
    }
}
```
Незмінні поля можна встановити при створенні.

Redonly можуть бути деякі поля, властивості і методи структури.

```cs
UsingStructureWithRedonlyMemebers();
static void UsingStructureWithRedonlyMemebers()
{
    ApartmentWithPeople apartment = new ApartmentWithPeople(7,48,2);
    apartment.Display();
    //apartment.Number = 8; don't work
    apartment.NumberOfResidents = 3;
    apartment.Display();
}

struct ApartmentWithPeople
{
    public readonly int Number;
    public readonly double Square;
    public int NumberOfResidents;

    public ApartmentWithPeople(int number, double square, int numberOfResidents)
    {
        Number = number;
        Square = square;
        NumberOfResidents = numberOfResidents;
    }

    public readonly void Display()
    {
        Console.WriteLine($"Apartment :{Number} Square:{Square} Number of residents:{NumberOfResidents}" );
    }
}
```

Типи int,double,decimal та інші це структури.

## Структури з властивостями.
Розглянемо Point_v1.cs
```cs
    struct Point_v1
    {
        public int X { get; set; } = default;
        public int Y { get; set; } = default;

        public Point_v1(int x, int y)
        {
            X = x;
            Y = y;
        }
         public void ToConsole() => Console.WriteLine($"[{X},{Y}]"); 
    }
```
```cs
UsingStructureWithProperties();
void UsingStructureWithProperties()
{
    Point_v1 point = new(10, 10);
    point.ToConsole();

    point.X = 20;
    point.Y = 30;
    point.ToConsole();

    Point_v1 point1 = new();
    point1.ToConsole();
}
```
```
[10,10]
[20,30]
[0,0]
```
Цей приклад аналогічний інкапсуляції в класах. 

