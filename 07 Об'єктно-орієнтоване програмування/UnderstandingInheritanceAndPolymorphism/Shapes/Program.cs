using Shapes;

void UsingVirualMethodInAbstractClass()
{
    Hexagon hexagon = new Hexagon("Beth");
    hexagon.Draw();

    Circle circle = new Circle("Cindy");
    circle.Draw();
}
//UsingVirualMethodInAbstractClass();

void UsingPolymorphism()
{
    Shape1[] shapes = {
        new Hexagon1(), new Circle1(),
        new Hexagon1("Mick"), new Circle1("Beth"),
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
UsingNew();