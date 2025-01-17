namespace WorkWithRecordStructs;

public record struct Point(double X,double Y, double Z);

public record struct Point1
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Point1()  
    {
    }

    public Point1(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}

public readonly record struct Point2(double X, double Y, double Z);

public readonly record struct Point3
{
    public double X { get; init; } 
    public double Y { get; init; }
    public double Z { get; init; }

    public Point3()
    {
    }

    public Point3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}