namespace CustomInterfaces;

class ThreeDCircle1 : Circle1 ,IDraw3D
{
    // Hide the PetName property above me.
    public new string PetName { get; set; } = null!;

    // Hide any Draw() implementation above me.
    public new void Draw()
    {
        Console.WriteLine("Drawing a 3D Circle");
    }

    public void Draw3D()
    {
        Console.WriteLine("Drawing Circle in 3D");
    }
}
