
using ObjectInitializers;

void CreateObjects()
{
    // Make a Point by setting each property manually.
    Point point1 = new Point();
    point1.X = 10;
    point1.Y = 10;
    point1.DisplayState();

    // Or make a Point via a custom constructor.
    Point point2 = new Point(20,20);
    point2.DisplayState();

    // Or make a Point using object init syntax.
    Point point3 = new Point() { X = 30, Y = 30 };
    point3.DisplayState();
}
//CreateObjects();

void UsingInitOnlySetters()
{
    Point1 point = new(10,10);
    point.DisplayState();
    //The next two lines will not compile
    //point.X = 10;

}
//UsingInitOnlySetters();

void CallingCustomConstructorsWithInitializationSyntax()
{
    // Here, the default constructor is called implicitly.
    Point1 point1 = new Point1 { X = 10, Y = 10 };

    // Here, the default constructor is called explicitly.
    Point1 point2 = new Point1() { X = 20, Y = 20 };

    Point1 point3 = new Point1(30, 30) { X = 40, Y = 40 };
    point3.DisplayState();

    Point2 point4 = new Point2(PointColorEnum.Gold) { X = 50, Y = 50 };
    point4.DisplayState();
}
//CallingCustomConstructorsWithInitializationSyntax();

void InitializingDataWithInitializationSyntax()
{
    Rectangle rectangle = new Rectangle
    {
        TopLeft = new Point2 { X = 10, Y = 10 },
        BottomRight = new Point2 { X = 200, Y = 200 }
    };

    rectangle.DisplayState();
}
InitializingDataWithInitializationSyntax();