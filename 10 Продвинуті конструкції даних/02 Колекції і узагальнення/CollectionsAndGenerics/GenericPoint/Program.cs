using GenericPoint;

void UsePoint()
{
    // Point using ints.
    Point<int> p = new Point<int>(10, 10);
    Console.WriteLine($"p.ToString()={p}");
    p.ResetPoint();
    Console.WriteLine($"p.ToString()={p}");
    Console.WriteLine();

    // Point using double.
    Point<double> p2 = new Point<double>(5.4, 3.3);
    Console.WriteLine($"p2.ToString()={p2}");
    p2.ResetPoint();
    Console.WriteLine($"p2.ToString()={p2}");
    Console.WriteLine();

    // Point using strings.
    Point<string> p3 = new Point<string>("i", "3i");
    Console.WriteLine($"p3.ToString()={p3}");
    p3.ResetPoint();
    Console.WriteLine($"p3.ToString()={p3}");
    Console.WriteLine();

}
//UsePoint();

void UseDefault()
{
    Point<string> p4 = default;
    Console.WriteLine($"p4.ToString()={p4}");
    Console.WriteLine();
    Point<int> p5 = default;
    Console.WriteLine($"p5.ToString()={p5}");
}
//UseDefault();

static void PatternMatching<T>(Point<T> p)
{
    switch (p)
    {
        case Point<string> pString:
            Console.WriteLine("Point is based on strings");
            return;
        case Point<int> pInt:
            Console.WriteLine("Point is based on ints");
            return;
    }
}

void UsePatternMatching()
{
    Point<string> p1 = default;
    Point<int> p2 = default;
    PatternMatching(p1);
    PatternMatching(p2);
}
//UsePatternMatching();

// Compiler error! Cannot apply
// operators to type parameters!
//public class BasicMath<T> 
//{
//    public T Add(T arg1, T arg2)
//    { return arg1 + arg2; }
//    public T Subtract(T arg1, T arg2)
//    { return arg1 - arg2; }
//    public T Multiply(T arg1, T arg2)
//    { return arg1 * arg2; }
//    public T Divide(T arg1, T arg2)
//    { return arg1 / arg2; }
//}