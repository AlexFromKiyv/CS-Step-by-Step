using OperatorOverloading;

static void UsingOperatorPlus()
{
    // The + operator with ints.
    int a = 100;
    int b = 240;
    int c = a + b;
    Console.WriteLine(c);

    // + operator with strings.
    string s1 = "Hi";
    string s2 = " girl";
    string s3 = s1 + s2;
    Console.WriteLine(s3);
}
//UsingOperatorPlus();

static void UsingPointWithOperators()
{
    // Make two points.
    Point ptOne = new Point(100, 100);
    Point ptTwo = new Point(40, 40);
    Console.WriteLine($"ptOne = {ptOne}");
    Console.WriteLine($"ptTwo = {ptTwo}");
    // Add the points to make a bigger point?
    Console.WriteLine($"ptOne + ptTwo: {ptOne + ptTwo}");
    // Subtract the points to make a smaller point?
    Console.WriteLine($"ptOne - ptTwo: {ptOne - ptTwo}");

    Console.WriteLine();

    Point point = new Point(100, 100);
    Console.WriteLine($"point = {point}");
    Console.WriteLine($"point + 10 = {point+10}");
    Console.WriteLine($"10 + point = {10+point}");

    Console.WriteLine();

    Point point1 = new Point(10, 10);
    point += point1;
    Console.WriteLine(point);

    point1 -= point;
    Console.WriteLine(point1);

    Console.WriteLine();

    point = new(0, 0);
    point++; Console.WriteLine(point);
    point--; Console.WriteLine(point);
    ++point; Console.WriteLine(point);
    --point; Console.WriteLine(point);

    Console.WriteLine();

    point = new(0, 0);
    point1 = new(1, 1);
    Console.WriteLine(point == point1);
    Console.WriteLine(point != point1);

    Console.WriteLine();

    point = new(1, 1);
    point1 = new(2, 2);
    Console.WriteLine(point > point1);
    Console.WriteLine(point < point1);

}
UsingPointWithOperators();