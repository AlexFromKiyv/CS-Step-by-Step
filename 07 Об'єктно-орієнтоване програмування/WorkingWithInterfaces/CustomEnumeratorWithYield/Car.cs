namespace CustomEnumeratorWithYield;
class Car
{
    // Constant for maximum speed.
    public const int MaxSpeed = 100;

    // Car properties.
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = "NoName";

    // Is the car still operational?
    private bool _carIsDead;

    private readonly Radio _radio = new Radio();

    public Car()
    {
    }
    public Car(string petName, int currentSpeed)
    {
        CurrentSpeed = currentSpeed;
        PetName = petName;
    }

    public void CrankTunes(bool state)
    {
        // Delegate request to inner object.
        _radio.TurnOn(state);
    }
 }
