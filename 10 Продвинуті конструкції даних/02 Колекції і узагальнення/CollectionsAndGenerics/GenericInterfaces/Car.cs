namespace GenericInterfaces;

class Car : IComparable
{
    // Constant for maximum speed.
    public const int MaxSpeed = 100;

    // Car properties.
    public int CarId { get; set; }
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = "No-name";
    public override string? ToString() => $"{CarId}\t{PetName}\t{CurrentSpeed}";

    public Car()
    {
    }

    public Car(int carId, string petName, int currentSpeed)
    {
        CarId = carId;
        CurrentSpeed = currentSpeed;
        PetName = petName;
    }

    public int CompareTo(object? obj)
    {
        if (obj is Car anotherCar)
        {
            return CarId.CompareTo(anotherCar.CarId);
        }
        else
        {
            throw new ArgumentException("Parameter is not a type Car!");
        }
    }
}