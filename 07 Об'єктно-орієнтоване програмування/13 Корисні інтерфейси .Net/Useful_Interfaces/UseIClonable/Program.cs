

using UseIClonable;

//ExplorationAssignObject();
void ExplorationAssignObject()
{
    Point point1 = new Point(1, 1);
    Point point2 = point1;

    Console.WriteLine(point1);
    Console.WriteLine(point2);

    point2.X = 0; Console.WriteLine("point2.X = 0;");

    Console.WriteLine(point1);
    Console.WriteLine(point2);
}

//ExamineIClonable_1();
void ExamineIClonable_1()
{
    Point_v1 point1 = new(1, 1);
    Point_v1 point2 = (Point_v1)point1.Clone();

    Console.WriteLine(point1);
    Console.WriteLine(point2);

    point2.X = 0; Console.WriteLine("point2.X = 0;");

    Console.WriteLine(point1);
    Console.WriteLine(point2);
}

//ExamineIClonable_2();
void ExamineIClonable_2()
{
    Point_v2 point1 = new(1, 1);
    Point_v2 point2 = (Point_v2)point1.Clone();

    Console.WriteLine(point1);
    Console.WriteLine(point2);

    point2.X = 0; Console.WriteLine("point2.X = 0;");

    Console.WriteLine(point1);
    Console.WriteLine(point2);
}

//ExamineIClonable_3();
void ExamineIClonable_3()
{
    Point_v3 point1 = new Point_v3(1, 1, new("Start"));
    Point_v3 point2 = (Point_v3)point1.Clone();

    Console.WriteLine(point1);
    Console.WriteLine(point2);

    point2.X = 0; Console.WriteLine("point2.X = 0;");
    point2.Description.Name = "End"; Console.WriteLine("point2.Description.Name = \"End\";");

    Console.WriteLine(point1);
    Console.WriteLine(point2);
}

ExamineIClonable_4();
void ExamineIClonable_4()
{
    Point_v4 point1 = new Point_v4(1, 1, new("Start"));
    Point_v4 point2 = (Point_v4)point1.Clone();

    Console.WriteLine(point1);
    Console.WriteLine(point2);

    point2.X = 0; Console.WriteLine("point2.X = 0;");
    point2.Description.Name = "End"; Console.WriteLine("point2.Description.Name = \"End\";");

    Console.WriteLine(point1);
    Console.WriteLine(point2);
}