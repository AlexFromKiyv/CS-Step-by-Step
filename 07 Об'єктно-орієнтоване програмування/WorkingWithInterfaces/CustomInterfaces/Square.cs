namespace CustomInterfaces;

class Square : Shape1, IRegularPointy
{
    public Square() {}
    public Square(string petName) : base(petName) {}

    //These come from the IRegularPointy interface
    public int SideLength { get; set; }
    public int NumberOfSides { get; set; }

    //This comes from the IPointy interface
    public byte Points => 4;
    //Draw comes from the Shape base class
    public override void Draw()
    {
        Console.WriteLine($"Drawing a square {PetName}");
    }
    //Note that the Perimeter property is not implemented
}

