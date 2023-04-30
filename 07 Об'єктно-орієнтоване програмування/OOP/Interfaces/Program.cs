
using Interfaces;

//ExplorationICloneable();
void ExplorationICloneable()
{
    CloneMe("Hi");

    int[] myArray = new int[3] { 1, 2, 3 };
    CloneMe(myArray);

    OperatingSystem operatingSystem = new OperatingSystem(PlatformID.Unix, new Version());
    CloneMe(operatingSystem);


    void CloneMe(ICloneable cloneable)
    {
        object TheClone = cloneable.Clone();
        Console.WriteLine("\n" + cloneable);
        Console.WriteLine($"Clone:{TheClone}");
        Console.WriteLine($"Type:{TheClone.GetType()}");
        Console.WriteLine($"ReferenceEquals:{ReferenceEquals(cloneable, TheClone)}");

    }
}

//InvokeInterfaceMembers();
void InvokeInterfaceMembers()
{
    Hexagon hexagon = new();
    Console.WriteLine(hexagon.Points);
}

//CheckInterfaceTypeByCast();
void CheckInterfaceTypeByCast()
{
    Shape[] shapes = new Shape[] 
    { 
        new Triangle(), 
        new Circle(), 
        new Hexagon(), 
        new ThreeDCircle() 
    };

    foreach (Shape shape in shapes)
    {
        ShowPoints(shape);
    }

    void ShowPoints(Shape shape)
    {
        try
        {
            IPointy pointy = (IPointy) shape;
            Console.WriteLine(pointy.Points);
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

//CheckInterfaceTypeByAs();
void CheckInterfaceTypeByAs()
{
    Shape[] shapes = new Shape[]
    {
        new Triangle(),
        new Circle(),
        new Hexagon(),
        new ThreeDCircle()
    };

    foreach (Shape shape in shapes)
    {
        ShowPoints(shape);
    }

    void ShowPoints(Shape shape)
    {
        IPointy? pointy = shape as IPointy;
        if (pointy != null)
        {
            Console.WriteLine(pointy.Points);
        }
        else
        {
            Console.WriteLine("The shape has no points.");
        }

    }
}

//CheckInterfaceTypeByIs();
void CheckInterfaceTypeByIs()
{
    Shape[] shapes = new Shape[]
    {
        new Triangle(),
        new Circle(),
        new Hexagon(),
        new ThreeDCircle()
    };

    foreach (Shape shape in shapes)
    {
        ShowPoints(shape);
    }

    void ShowPoints(Shape shape)
    {
        if (shape is IPointy pointy)
        {
            Console.WriteLine(pointy.Points);
        }
        else
        {
            Console.WriteLine("The shape has no points.");
        }
    }
}

//ExplorationDefaultImplamentationInterface();
void ExplorationDefaultImplamentationInterface()
{
    var square = new Square();
    square.Draw();
    square.SideLength = 10;

    //Console.WriteLine(square.Perimeter); // does not contain definition Perimeter

    Console.WriteLine( ((IRegularPointy)square).Perimeter );

    IRegularPointy regularSquare = new Square() {Name = "Garden", SideLength = 20};

    Console.WriteLine(regularSquare.Perimeter);

    //regularSquare.Draw(); // does not contain definition Perimeter

    ((Square)regularSquare).Draw();

}

//ExplorationStaticConstructorAndMemeberOfInterface();

void ExplorationStaticConstructorAndMemeberOfInterface()
{
    Console.WriteLine(IRegularPointy.Inscription);

    IRegularPointy.Inscription = "Shape ...";

    Console.WriteLine(IRegularPointy.Inscription );
}

InterfaceAsParameters();
void InterfaceAsParameters()
{

    Shape[] shapes = new Shape[]
    {
        new Triangle(),
        new Circle(),
        new Hexagon(),
        new ThreeDCircle()
    };

    foreach (Shape shape in shapes)
    {
        if (shape is IDraw3D s)
        {
            DrawIn3D(s);
        }
    }


    Shape shape1 = new ThreeDCircle();
    DrawIn3D((IDraw3D)shape1);
    
    
    void DrawIn3D(IDraw3D shape3D)
    {
        shape3D.Draw3D();
    }
}