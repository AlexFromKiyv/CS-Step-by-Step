namespace GenericInterfaces;

class Car1 : IComparable<Car1>
{
    // Constant for maximum speed.
    public const int MaxSpeed = 100;

    // Car properties.
    public int CarId { get; set; }
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = "No-name";
    public override string? ToString() => $"{CarId}\t{PetName}\t{CurrentSpeed}";
    public Car1()
    {
    }

    public Car1(int carId, string petName, int currentSpeed)
    {
        CarId = carId;
        CurrentSpeed = currentSpeed;
        PetName = petName;
    }

    public int CompareTo(Car1? car)
    {
        return CarId.CompareTo(car?.CarId);
    }
}