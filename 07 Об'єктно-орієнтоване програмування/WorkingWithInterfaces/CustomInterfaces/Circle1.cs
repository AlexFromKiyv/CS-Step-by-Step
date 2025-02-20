namespace CustomInterfaces;

class Circle1 : Shape1
{
    public Circle1() { }
    public Circle1(string name) : base(name) { }
    public override void Draw()
    {
        Console.WriteLine($"Drawing {PetName} the Circle");
    }
}
