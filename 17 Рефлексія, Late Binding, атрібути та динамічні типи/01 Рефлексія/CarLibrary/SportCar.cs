namespace CarLibrary;

public class SportCar : Car
{
    public SportCar()
    {
    }
    public SportCar(string name, int currentSpeed, int maxSpeed) : base(name, currentSpeed, maxSpeed)
    {
    }
    public override void TurboBoost()
    {
        Console.WriteLine("Ramming speed!!! It's not good!");
    }
}