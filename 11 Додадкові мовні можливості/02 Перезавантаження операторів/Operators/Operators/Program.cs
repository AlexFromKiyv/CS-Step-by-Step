
using Operators;
void UseOverlodingOperatorAdditionSubtraction()
{
    Point_v1 point = new(1, 1);
    Point_v1 point1 = new(2, 2);
    
    Console.WriteLine(point+point1);

    Console.WriteLine(point1-point);

    Console.WriteLine(point-point1+point1+point+new Point_v1(10,10));

    Point_v2 point2 = new(20, 20);

    Console.WriteLine(point2 + 100 - 12 + 27);

    //Operator '+' cannot...
    //Console.WriteLine(100 + point2);

    Point_v3 point3 = new(30, 30);

    Console.WriteLine( point3 + 3);
    Console.WriteLine( 3 + point3 );
}

//UseOverlodingOperatorAdditionSubtraction();

void UseShorthandOperator()
{
    Point_v1 point1 = new(1, 1);
    Point_v1 point2 = new(2, 2);

    point1 += point2;

    Console.WriteLine(point1);

}

//UseShorthandOperator();

void UseIncrement()
{
    Point_v4 point = new(1, 1);

    point++;

    Console.WriteLine(point);
}

//UseIncrement();

void UseEqualityOperators()
{
    Point_v5 point1 = new(1, 1);
    Point_v5 point2 = new(1, 1);
    Point_v5 point3 = new(2, 3);

    Console.WriteLine(point1 == point2);
    Console.WriteLine(point1 == point3);
}

UseEqualityOperators();
