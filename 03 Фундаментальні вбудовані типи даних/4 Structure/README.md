# Structure

Додамо проект Structs.

## Створення
Структури добре підходять для моделювання математичних, геометричних і атомарних сутностей.
Структури можуть містити можуть містити поля даних та медоди роботи з ними.

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
Як видно із прикладу перед тим як змінну типа струттура використовувати вона повина бути проініціалізована. Тобта всі поля повини мати значення. Ви можете створити змінну структури використовуючи new, який виключе конструктор за замовчуванням. Всі поля після цого заповнюються значеннями за замовчуванням (default).

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









Структури можуть реалізовувати інтерфейси але не можна успадковувати і робити основою класа. 
