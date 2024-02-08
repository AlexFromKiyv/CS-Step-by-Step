// Make use of types defined the MyShapes namespace.
using CustomNamespaces.MyShape;
using CustomNamespaces.My3DShape;
// Resolve the ambiguity for a type using a custom alias.
using The3DCircle = CustomNamespaces.My3DShape.Circle;
using ThreeDShapes = CustomNamespaces.My3DShape;

//void CreateCircle()
//{
//    Circle circle = new Circle();
//    circle.InfoAboutShape();
//}
////CreateCircle();

void CreateSquare()
{
    // Note we are not importing CustomNamespaces.MyShapes anymore!
    CustomNamespaces.MyShape.Circle circle = new CustomNamespaces.MyShape.Circle(5);
    CustomNamespaces.MyShape.Square square = new CustomNamespaces.MyShape.Square();
    CustomNamespaces.MyShape.Hexagon hexagon = new CustomNamespaces.MyShape.Hexagon();
    Console.WriteLine(circle);
    Console.WriteLine(square);
    Console.WriteLine(hexagon);
}
//CreateSquare();

void CreateAnyCircle()
{
    CustomNamespaces.MyShape.Circle circle = new();
    CustomNamespaces.My3DShape.Circle circle_3d = new();

    circle.InfoAboutShape();
    circle_3d.InfoAboutShape();
}
//CreateAnyCircle();

void CreateCircleWithAlias()
{
    ThreeDShapes.Circle circle1 = new();
    circle1.InfoAboutShape();
}
CreateCircleWithAlias();



