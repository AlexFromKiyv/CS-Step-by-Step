
namespace CarDelegate;

class Car
{
    // Internal state data.
    public int CurrentSpeed { get; set; }
    public int MaxSpeed { get; set; } = 100;
    public string PetName { get; set; } = string.Empty;

    // Is the car alive or dead?
    private bool _carIsDead;

    // Class constructors.
    public Car() { }

    public Car(string petName, int maxSpeed, int currentSpeed)
    {
        CurrentSpeed = currentSpeed;
        MaxSpeed = maxSpeed;
        PetName = petName;
    }

    // 1) Define a delegate type.
    public delegate void CarEngineHandler(string messageForCaller);

    // 2) Define a member variable of this delegate.
    private CarEngineHandler? _listOfHandlers;

    // 3) Add registration function for the caller.

    // Now with multicasting support!
    // Note we are now using the += operator, not
    // the assignment operator (=).
    public void RegisterWithCarEngine(CarEngineHandler methodToCall)
    {
        _listOfHandlers += methodToCall;
    }

    public void UnRegisterWithCarEngine(CarEngineHandler methodName)
    {
        _listOfHandlers -= methodName;
    }

    // 4) Implement the Accelerate() method to invoke the delegate's
    //    invocation list under the correct circumstances.
    public void Accelerate(int delta)
    {
        // If this car is 'dead,' send dead message.
        if (_carIsDead)
        {
            _listOfHandlers?.Invoke("Sorry, this car is dead...");
        }
        else
        {
            CurrentSpeed += delta;
            // Is this car 'almost dead'?
            if ((MaxSpeed - CurrentSpeed) <= 10)
            {
                _listOfHandlers?.Invoke("Careful buddy! Gonna blow!");
            }

            if (CurrentSpeed >= MaxSpeed)
            {
                _carIsDead = true;
            }
            else
            {
                Console.WriteLine($"CurrentSpeed = {CurrentSpeed}");
            }
        }
    }

}
