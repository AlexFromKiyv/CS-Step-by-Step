using System.Collections;

namespace CustomComparable;
class Car : IComparable
{
    //Constant
    public const int MaxSpeed = 100;

    // Properties.
    public int CarId { get; set; }
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = "No-name";
    private bool _carIsDead;
    private readonly Radio _radio = new Radio();

    // Property to return the PetNameComparer.
    public static IComparer SortByPetName => new PetNameComparer();

    public Car()
    {
    }
    public Car(string petName, int currentSpeed)
    {
        CurrentSpeed = currentSpeed;
        PetName = petName;
    }

    public Car(int carId, string petName, int currentSpeed):this(petName,currentSpeed)
    {
        CarId = carId;
    }

    public void CrankTunes(bool state)
    {
        // Delegate request to inner object.
        _radio.TurnOn(state);
    }
    //Change current speed.
    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{PetName} is out of order...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = 0;
                _carIsDead = true;
                throw new Exception($"{PetName} has overheated!")
                {
                    Data =
                    {
                        {"Timestamp",DateTime.Now},
                        {"Cause","You have a lead foot." }
                    }
                };

            }
            Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
        }
    }

    //public int CompareTo(object? obj)
    //{
    //    if (obj is Car anotherCar)
    //    {
    //        if (CarId > anotherCar.CarId)
    //        {
    //           return 1;
    //        }
    //        if(CarId < anotherCar.CarId)
    //        {
    //            return -1;                
    //        }
    //        return 0;
    //    }
    //    else
    //    {
    //        throw new ArgumentException("Parameter is not a type Car!");
    //    }
    //}

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