partial class Program
{
    /// <summary>
    /// Calculates the area for a rectangle with dimensions x, y
    /// </summary>
    /// <param name="x">length</param>
    /// <param name="y">width</param>
    /// <returns>The area of a rectangle with length and width rounded to a whole value.</returns>
    static int RoundedSquare(double x,double y)
    {
        return (int) Math.Round(x*y);
    }
}
