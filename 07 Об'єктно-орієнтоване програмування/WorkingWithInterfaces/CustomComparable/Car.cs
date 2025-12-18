using System.Collections;

namespace CustomComparable;
class Car : IComparable
{
    //Constant
    public const int MaxSpeed = 100;

    // Properties.
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = "No-name";
    public int CarId { get; set; }

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