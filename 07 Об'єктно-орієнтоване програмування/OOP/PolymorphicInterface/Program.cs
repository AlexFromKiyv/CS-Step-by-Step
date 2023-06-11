
using PolymorphicInterface;
//ExploreAbstractClassMemebers();
void ExploreAbstractClassMemebers()
{
    Circle_v1 circle_1 = new();
    circle_1.ToConsole();
    circle_1.Draw();


    Circle_v1 circle_2 = new("Ball");
    circle_2.ToConsole();
    circle_2.Draw();

    Hexagon_v1 hexagon_1 = new("Max");
    hexagon_1.ToConsole();
    hexagon_1.Draw();
}

//ExploreAbstractMethods();
void ExploreAbstractMethods()
{

    Shape_v2[] shapes = {
        new Circle_v2(),
        new Circle_v2("Ball"),
        new Hexagon_v2(),
        new Hexagon_v2("Max")
        };

    foreach(Shape_v2 shape in shapes)
    {
        Console.WriteLine(shape.GetType());
        shape.Draw();
    }
}

//ExploreShadowing();
void ExploreShadowing()
{
    Circle_v3 circle_1 = new("VeriBiGThreeDCircle");
    circle_1.Draw();

    ThreeDCircle_v3 circle_2 = new("VeriBiGThreeDCircle");
    circle_2.Draw();
    ((Circle_v3)circle_2).Draw();
}

//DifferenceBetweenOverrideAndNew_Override();
void DifferenceBetweenOverrideAndNew_Override()
{
    Person person = new Employee_v1("Viktory", "Farmak");
    person.ToConsole();
}

DifferenceBetweenOverrideAndNew_New();
void DifferenceBetweenOverrideAndNew_New()
{
    Person person = new Employee_v2("Viktory", "Farmak");
    person.ToConsole();
}