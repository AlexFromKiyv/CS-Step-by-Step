namespace CustomClonable;

public class Point1 : ICloneable
{
    public int X { get; set; }
    public int Y { get; set; }
    public PointDescription Description { get; set; } = new PointDescription();

    public Point1()
    {
    }
    public Point1(int x, int y)
    {
        X = x;
        Y = y;
    }
    public Point1(int x, int y, string petName) : this(x,y)
    {
        Description.PetName = petName;
    }
    public override string? ToString() => $"{X}:{Y}\t{Description.PetName}\t{Description.PointId}";

    //public object Clone() => MemberwiseClone();

    // Now we need to adjust for the PointDescription member.
    public object Clone()
    {
        // First get a shallow copy.
        Point1 newPoint = (Point1)MemberwiseClone();

        // Then fill in the gaps.
        PointDescription newDescription = new()
        {
            PetName = Description.PetName
        };
        newPoint.Description = newDescription;

        return newPoint;
    }

}
