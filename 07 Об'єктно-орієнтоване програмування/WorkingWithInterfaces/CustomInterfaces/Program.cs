
using CustomInterfaces;

void UsingInterfaceIClonable()
{
    // All of these classes support the ICloneable interface.
    string myString = "Hi girl";
    OperatingSystem unix = new OperatingSystem(PlatformID.Unix, new Version());

    // Therefore, they can all be passed into a method taking ICloneable.
    CloneMe(myString);
    CloneMe(unix);


    // Helper function
    // Clone whatever we get and print out the name.
    static void CloneMe(ICloneable clonable)
    {
        object theClone = clonable.Clone();
        Console.WriteLine($"Your clone is a: {theClone.GetType().Name}");
    }
}
//UsingInterfaceIClonable();

void InvokingInterfaceMembersAtTheObjectLevel()
{
    Hexagon1 hexagon = new();
    Console.WriteLine(hexagon.Points);
}
//InvokingInterfaceMembersAtTheObjectLevel();

void InvokingInterfaceMembersWithExplicitCasting()
{
    Hexagon1 hexagon = new();
    Circle1 circle = new();

    List<Shape1> shapes = [ circle, hexagon ];

    IPointy? pointy = null;

    foreach (var shape in shapes)
    {
        try
        {
            pointy = (IPointy)shape;
            Console.WriteLine(pointy.Points);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
//InvokingInterfaceMembersWithExplicitCasting();

void InvokingInterfaceMembersWithAs()
{
    Hexagon1 hexagon = new();
    Circle1 circle = new();

    List<Shape1> shapes = [circle, hexagon];

    IPointy? pointy = null;

    foreach (var shape in shapes)
    {
        pointy = shape as IPointy;

        if (pointy != null)
        {
            Console.WriteLine(pointy.Points);
        }
    }
}
//InvokingInterfaceMembersWithAs();

void InvokingInterfaceMembersWithIs()
{
    Hexagon1 hexagon = new();
    Circle1 circle = new();

    List<Shape1> shapes = [circle, hexagon];

    foreach (var shape in shapes)
    {
        if (shape is IPointy pointy)
        {
            Console.WriteLine(pointy.Points);
        }
    }
}
//InvokingInterfaceMembersWithIs();

void DefaultImplementations1()
{
    Square square = new("Boxy") { SideLength = 10, NumberOfSides = 4 } ;
    square.Draw();

    //This won't compile 
    //Console.WriteLine($"Perimeter: {square.Perimeter}");

    Console.WriteLine($"Perimeter: {((IRegularPointy)square).Perimeter}"  );
}
//DefaultImplementations1();

void DefaultImplementations2()
{
    IRegularPointy pointy = new Square("Boxy") { SideLength = 20, NumberOfSides = 4 };
    
    if(pointy is Square square)
    {
        square.Draw();
    }
    Console.WriteLine($"Perimeter: {pointy.Perimeter}");
}
//DefaultImplementations2();

void StaticMembers()
{
    Console.WriteLine(IRegularPointy.ExampleProperty);
    IRegularPointy.ExampleProperty = "Something red";
    Console.WriteLine(IRegularPointy.ExampleProperty);
}
//StaticMembers();

void InterfaceAsParameter()
{
    Shape1[] shapes = { new Hexagon1(),new Circle1(),
        new Triangle1(),new ThreeDCircle1() };

    for (int i = 0; i < shapes.Length; i++)
    {
        if (shapes[i] is IDraw3D shape)
        {
            DrawIn3D(shape);
        }
    }

    // I'll draw anyone supporting IDraw3D.
    void DrawIn3D(IDraw3D shapeWith3D)
    {
        Console.WriteLine($"Drawing 3D shape");
        shapeWith3D.Draw3D();
    }
}
//InterfaceAsParameter();

void InterfacesAsReturnValues()
{
    Shape1[] shapes = { new Hexagon1(),new Circle1(),
        new Triangle1(),new ThreeDCircle1() };

    Console.WriteLine(FindFirstPointyShape(shapes));

    // This method returns the first object in the
    // array that implements IPointy.
    static IPointy? FindFirstPointyShape(Shape1[] shapes)
    {
        foreach (var shape in shapes)
        {
            if (shape is IPointy resultShape)
            {
                return resultShape;
            }
        }
        return null;
    }
}
//InterfacesAsReturnValues();

void ArraysOfInterfaceTypes()
{
    IPointy[] pointies = {new Hexagon1(), new Knife(),
        new Triangle1(), new Fork(), new PitchFork()};

    foreach (var pointy in pointies)
    {
        Console.WriteLine(pointy.Points+"\t"+pointy);
    }
}
ArraysOfInterfaceTypes();