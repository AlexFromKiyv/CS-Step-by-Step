
using Initializers;

//ObjectsInitialisation();
void ObjectsInitialisation()
{
    //1
    Point point_1= new Point();
    point_1.X = 1;
    point_1.Y = 1;
    point_1.Z = 1;
    point_1.ToConsole();

    //2
    Point point_2= new Point(2,2);
    point_2.Z = 2;
    point_2.ToConsole();

    //3
    Point point_3= new Point { X=3,Y=3,Z=3 };
    point_3.ToConsole();
}


//UsingInitOnly();
void UsingInitOnly()
{
    Point_v1 point_1 = new(1, 1);
    //point_1.X = 2; // Don't work.  Init-only property 
    point_1.ToConsole();

    Point_v1 point_2 = new Point_v1 { X = 2, Y = 2 };
    point_2.ToConsole();
}

//CustomConstuctorAndInizialiser();
void CustomConstuctorAndInizialiser()
{
    Point point = new(1, 1) { X = 2, Y = 2, Z = 2 };
    point.ToConsole();
}

//UsingPoint_v2();
void UsingPoint_v2()
{
    Point_v2 point_1 = new() { X = 1, Y = 1 };
    point_1.ToConsole();
    
    Point_v2 point_2 = new Point_v2(PointColorEnum.Red) { X = 2, Y = 2 };
    point_2.ToConsole();
   
}

//UsingRectangle();
void UsingRectangle()
{
    Rectangle rectangle = new()
    {
        TopLeft = new(1, 1),
        BottomRight = new(2, 2)
    };

    rectangle.ToConsole();
}

UsingRectangle_v1();
void UsingRectangle_v1()
{
    Rectangle_v1 rectangle = new()
    {
        TopLeft = new(2, 2),
        BottomRight = new(3, 3)
    };

    rectangle.ToConsole();
}