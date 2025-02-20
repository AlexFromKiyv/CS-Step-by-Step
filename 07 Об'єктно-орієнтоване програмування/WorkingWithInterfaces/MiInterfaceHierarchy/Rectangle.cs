namespace MiInterfaceHierarchy;

class Rectangle : IShape
{
    public void Draw()
    {
        Console.WriteLine("Drawing...");
    }
    public void Print()
    {
        Console.WriteLine("Printing...");
    }
    public int GetNumberOfSide() => 4;
}
