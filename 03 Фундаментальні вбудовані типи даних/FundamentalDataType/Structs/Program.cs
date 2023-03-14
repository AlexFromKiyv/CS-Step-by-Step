//Example methods

//UsingSimpleStructure();

using Structs;

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

//UsingStructureConstructor();
static void UsingStructureConstructor()
{
    Point point = new Point();
    point.Display();

    Point point1 = new Point(1, 1);
    point1.Display();
}

//UsingInizializersStructure();

static void UsingInizializersStructure()
{
    Coordinates coordinates = new Coordinates();
    coordinates.Display();
}

//UsingReadonlyStructure();
static void UsingReadonlyStructure()
{
    ApartmentSquare myApartment = new ApartmentSquare(59);

    //myApartment.Square = 72; it don't work

    myApartment.Display();
}

//UsingStructureWithRedonlyMemebers();
static void UsingStructureWithRedonlyMemebers()
{
    ApartmentWithPeople apartment = new ApartmentWithPeople(7,48,2);
    apartment.Display();
    //apartment.Number = 8; don't work
    apartment.NumberOfResidents = 3;
    apartment.Display();
}


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




// Structures definitions
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



