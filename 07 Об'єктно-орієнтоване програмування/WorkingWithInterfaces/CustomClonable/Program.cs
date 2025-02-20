using CustomClonable;

void PointsInMemory() 
{
    Point point1 = new(10, 10);
    Point point2 = point1;

    point2.X = 20;

    Console.WriteLine(point1);
}
//PointsInMemory();

void ShallowCloning()
{
    Point point1 = new(10, 10);
    Point point2 = (Point)point1.Clone();
   
    point2.X = 20;

    Console.WriteLine(point1);
}
//ShallowCloning();

void ShallowCloning1()
{
    Point1 point1 = new(10, 10, "Jane");
    Point1 point2 = (Point1)point1.Clone();

    Console.WriteLine("Before modification:");
    Console.WriteLine(point1);
    Console.WriteLine(point2);

    point2.X += 10;
    point2.Description.PetName = "My new name";

    Console.WriteLine();
    Console.WriteLine("After modification:");
    Console.WriteLine(point1);
    Console.WriteLine(point2);
}
ShallowCloning1();