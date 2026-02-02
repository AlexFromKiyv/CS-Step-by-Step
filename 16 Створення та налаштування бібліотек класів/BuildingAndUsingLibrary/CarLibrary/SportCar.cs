namespace CarLibrary;

public class SportCar : Car
{
    public SportCar()
    {
    }

    public SportCar(string name, int maxSpeed, int currentSpeed) : base(name, maxSpeed, currentSpeed)
    {
    }

    public override void TurboBoost()
    {
        Console.WriteLine("Ramming speed! Faster is better...");
    }
}
