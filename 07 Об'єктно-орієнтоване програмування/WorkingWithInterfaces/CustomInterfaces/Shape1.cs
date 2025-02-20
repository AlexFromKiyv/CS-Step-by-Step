namespace CustomInterfaces;

abstract class Shape1
{
    public string PetName { get; set; }

    protected Shape1(string petName = "NoName")
    {
        PetName = petName;
    }
    public abstract void Draw();
}