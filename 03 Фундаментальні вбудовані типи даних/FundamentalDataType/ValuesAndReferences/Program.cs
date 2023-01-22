//CheckingValueType();
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


static void UsingValueInStack()
{
    static void ValueInStack()
    {
        int myInt = 5;
        Season season = Season.Autumn;
    } //at now myInt, season is not in the stack, memory
}

AssignValueType();
static void AssignValueType()
{
    int a = 5;
    int b = a;
    Console.WriteLine(b);
    b = 3;
    Console.WriteLine(b);
    Console.WriteLine(a);

    Point pointA = new Point(1,1);
    Point pointB = pointA;
    pointB.Display();
    
    pointB.X = 2;
    pointB.Y = 2;
    pointB.Display();
    pointA.Display();
}

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

    public void Info()
    {
        Console.WriteLine($"Apartment:{Number} Square:{Square}");
    }
}

