# Structure

Додамо проект Structs.

Структури добре підходять для моделювання математичних, геометричних і атомарних сутностей.
Структури можуть містити можуть містити поля даних та медоди роботи з ними.

```cs
UsingSimpleStructure();

static void UsingSimpleStructure()
{
    Point point = new Point();

    point.X = 1;
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

Структури можуть реалізовувати інтерфейси але не можна успадковувати і робити основою класа. 
