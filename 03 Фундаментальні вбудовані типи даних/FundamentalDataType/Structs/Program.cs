//UsingSimpleStructure();

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





