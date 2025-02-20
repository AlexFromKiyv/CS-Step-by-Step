namespace MiInterfaceHierarchy;

class Square : IShape
{
    void IDrawable.Draw()
    {
        Console.WriteLine("Drawing for IDrawable...");
    }
    void IPrintable.Draw()
    {
        Console.WriteLine("Drawing for IPrintable...");
    }
    public void Print()
    {
        Console.WriteLine("Printing...");
    }
    public int GetNumberOfSide() => 4;
}