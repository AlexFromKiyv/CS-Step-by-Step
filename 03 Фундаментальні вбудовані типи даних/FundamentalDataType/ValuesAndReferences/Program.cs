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

    Point point = new Point(2);
    Console.WriteLine($"Structure is ValueType: {point is ValueType}");

    Season season = Season.Winter;
    Console.WriteLine($"Enum is ValueType: {season is ValueType}");

    Console.WriteLine("\n\nReference type: ");


    object myObject = new();
    Console.WriteLine($"оbject is ValueType: {myObject is ValueType}");

    House house = new House();
    Console.WriteLine($"class is ValueType: {house is ValueType}");

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

class House
{
    public string Adress { get; set; }
    public double Square { get; set; }
}