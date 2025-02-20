namespace CustomClonable;
// The Point now supports 'clone-ability.'
public class Point : ICloneable
{
    public int X { get; set; }
    public int Y { get; set; }
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    public Point()
    {
    }
    public override string? ToString() => $"{X}:{Y}";


    //public object Clone() => new Point(X, Y);

    public object Clone() => this.MemberwiseClone() ;
    
}
