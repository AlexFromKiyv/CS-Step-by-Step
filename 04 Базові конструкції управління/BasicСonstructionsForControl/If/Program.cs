//LogicalExpression();

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


//SimpleIf();

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