
namespace AnonymousMethods;

public class Car
{
    public int CurrentSpeed { get; set; }
    public int MaxSpeed { get; set; }
    public string PetName { get; set; } = string.Empty;

    private bool _carIsDead;

    public Car() { MaxSpeed = 100; }
    public Car(string name, int maxSp, int currSp)
    {
        CurrentSpeed = currSp;
        MaxSpeed = maxSp;
        PetName = name;
    }

    public event EventHandler<CarEventArgs>? Exploded;
    public event EventHandler<CarEventArgs>? AboutToBlow;

    public void Accelerate(int delta)
    {
        // If the car is dead, fire Exploded event.
        if (_carIsDead)
        {
            Exploded?.Invoke(this, new CarEventArgs("Sorry, this car is dead..."));
        }
        else
        {
            CurrentSpeed += delta;

            // Check speed
            if (CurrentSpeed >= MaxSpeed)
            {
                _carIsDead = true;
            }
            else
            {
                Console.WriteLine($"CurrentSpeed = {CurrentSpeed}");
            }

            //Almost dead
            if ((MaxSpeed - CurrentSpeed) <= 10)
            {
                AboutToBlow?.Invoke(this, new CarEventArgs("Careful buddy! Gonna blow!"));
            }
        }
    }
}