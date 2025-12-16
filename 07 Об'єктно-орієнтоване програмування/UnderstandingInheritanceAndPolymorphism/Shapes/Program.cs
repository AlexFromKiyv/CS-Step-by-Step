using Shapes;

void UsingVirualMethodInAbstractClass()
{
    Hexagon hexagon = new Hexagon("Beth");
    hexagon.Draw();

    Circle circle = new Circle("Cindy");
    circle.Draw();
}
//UsingVirualMethodInAbstractClass();

void UsingVirualMethodInAbstractClass1()
{
    Hexagon1 hexagon = new Hexagon1("Beth");
    hexagon.Draw();

    Circle1 circle = new Circle1("Cindy");
    circle.Draw();
}
//UsingVirualMethodInAbstractClass1();



void UsingPolymorphism()
{
    Shape1[] shapes = {
        new Hexagon1(),
        new Circle1(),
        new Hexagon1("Mick"), 
        new Circle1("Beth"),
        };

    foreach (Shape1 shape in shapes)
    {
        shape.Draw();
    }
}
//UsingPolymorphism();

void UsingNew()
{
    ThreeDCircle1 circle = new();
    circle.Draw();

   ((Circle1)circle).Draw();
}
//UsingNew();