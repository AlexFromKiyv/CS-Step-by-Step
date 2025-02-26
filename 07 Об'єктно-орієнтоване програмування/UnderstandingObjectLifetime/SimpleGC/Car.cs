namespace SimpleGC;

public class Car
{
    public int CurrentSpeed { get; set; }
    public string? PetName { get; set; }

    public Car(string petName,int currentSpeed)
    {
        CurrentSpeed = currentSpeed;
        PetName = petName;
    }

    public Car()
    {
    }

    public override string? ToString() => $"{PetName} is going {CurrentSpeed} MPH";
}
