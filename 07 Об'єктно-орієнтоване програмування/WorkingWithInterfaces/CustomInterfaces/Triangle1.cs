namespace CustomInterfaces;

class Triangle1 : Shape1, IPointy
{
    public Triangle1() {}
    public Triangle1(string petName) : base(petName) {}
    public override void Draw()
    {
        Console.WriteLine($"Drawing {PetName} the Triangle");
    }
    public byte Points => 3;
}
