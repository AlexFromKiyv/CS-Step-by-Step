Додамо проект під назвою If

# Логічні вирази

```CS
LogicalExpression();

static void LogicalExpression()
{
    int weight = 70;

    Console.WriteLine($"weight = {weight}");
    Console.WriteLine($"weight == 70 : {weight == 70}");
    Console.WriteLine($"weight != 70 : {weight != 70}");
    Console.WriteLine($"weight > 70  : {weight > 70}");
    Console.WriteLine($"weight < 70  : {weight < 70}");
    Console.WriteLine($"weight >= 70  : {weight >= 70}");
    Console.WriteLine($"weight <= 70  : {weight <= 70}");
}
```

# IF/ELSE

```cs
SimpleIf();

static void SimpleIf()
{
    bool logicalExpression = true;
    if (logicalExpression)
    {
        Console.WriteLine("Logical expression is true");
    }

}


SimpleIfElse();

static void SimpleIfElse()
{
    bool logicalExpression = false;
    if (logicalExpression)
    {
        Console.WriteLine("Logical expression is true");
    }
    else
    {
        Console.WriteLine("Logical expression is false");
    }
}
```
# IS

```cs
UsingIs();
static void UsingIs()
{
    string myVariable1 = "Hi";

    if (myVariable1 is string newString)
    {
        Console.WriteLine(newString);
    }
    
    int myVariable2 = 70;
    if (myVariable2 is int newInt)
    {
        Console.WriteLine(newInt);
    }
}
```

В цьому прикладі перевіряеться тип об'єкту і якщо умова виконується присваюється новій змінній для того шоб потім використовувати.

# Matching patterns

```cs
UsingTypePattern();
static void UsingTypePattern()
{
    Type myType = typeof(short);

    if(myType is Type)
    {
        Console.WriteLine($"{myType} is type.");
    }

}
```
Перевірка чи є зміна тип.




