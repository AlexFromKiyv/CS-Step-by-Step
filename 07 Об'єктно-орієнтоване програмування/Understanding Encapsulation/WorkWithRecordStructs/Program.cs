
using WorkWithRecordStructs;

void UsingMutableRecordStruct()
{
    Point point1 = new(0, 1, 1);
    Console.WriteLine(point1);
    point1.X = 1; 
    Console.WriteLine(point1);

    Console.WriteLine();

    Point1 point2 = new(0, 2, 2);
    Console.WriteLine(point2);
    point2.X = 2;
    Console.WriteLine(point2);

}
//UsingMutableRecordStruct();

void UsingImmutableRecordStruct()
{
    Point2 point1 = new(0, 1, 1);
    Console.WriteLine(point1);
    //Compiler Error:
    //point1.X = 1;

    Console.WriteLine();

    Point3 point2 = new(0, 2, 2);
    Console.WriteLine(point2);
    //Compiler Error:
    //point2.X = 2;
}
//UsingImmutableRecordStruct();

void DeconstructingRecordStructs()
{
    Point point1 = new(1, 1, 1);
    var (x1, y1, z1) = point1;
    Console.WriteLine($"{x1} {y1} {z1}");

    Point2 point2 = new(2, 2, 2);

    point2.Deconstruct(out double x2, out double y2, out double z2);
    Console.WriteLine($"{x2} {y2} {z2}");
}
//DeconstructingRecordStructs();