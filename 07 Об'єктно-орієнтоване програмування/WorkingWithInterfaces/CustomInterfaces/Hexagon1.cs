namespace CustomInterfaces;

class Hexagon1 : Shape1, IPointy, IDraw3D
{
    public Hexagon1() { }
    public Hexagon1(string name) : base(name) { }

    public override void Draw()
    {
        Console.WriteLine($"Drawing {PetName} the Hexagon");
    }

    public byte Points => 6;
    public void Draw3D()
    {
        Console.WriteLine("Drawing Hexagon in 3D");
    }
}
