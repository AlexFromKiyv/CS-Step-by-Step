using CustomGenericType;

void UseGenericStruct()
{
    Point<int> point = new(1, 2);
    Console.WriteLine(point);

    Point<double> point1 = new(1.01, 2.02);
    Console.WriteLine(point1);

    Point<string> point2 = new("one", "two");
    Console.WriteLine(point2);

    point1.Reset();
    Console.WriteLine(point1);

    point2.Reset();
    Console.WriteLine(point2);

    Point<long> point3 = default;
    Console.WriteLine(point3);
}

//UseGenericStruct();

void UsePatternMatching()
{
    Point<int> point_1 = new(1, 2);
    DetailOfPoint(point_1);

    Point<string> point_2 = new("one", "two");
    DetailOfPoint(point_2);
 

    void DetailOfPoint<T>(Point<T> point)
    {
        switch (point)
        {
            case Point<string> pointString:
                Console.WriteLine($"Point have string data {pointString}");
                break;
            case Point<double> pointDouble:
                Console.WriteLine($"Point have real data {pointDouble}");
                break;
            case Point<int> pointInt:
                Console.WriteLine($"Point have whole data {pointInt}");
                break;
        }
    }
}

UsePatternMatching();