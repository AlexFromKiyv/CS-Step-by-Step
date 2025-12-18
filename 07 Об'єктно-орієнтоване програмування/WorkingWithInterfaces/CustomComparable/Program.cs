
using CustomComparable;
using System.Collections.Immutable;

void ShowCarArray()
{
    Car[] cars =
    [
        new Car(1, "Rusty",80),
        new Car(234, "Mary", 40),
        new Car(34, "Viper", 30),
        new Car(4, "Mel", 70),
        new Car(5, "Chucky", 80),
    ];
    Array.Sort(cars);

    foreach (Car car in cars)
    {
        Console.WriteLine($"{car.CarId}\t{car.PetName}\t{car.CurrentSpeed}");
    }
}
//ShowCarArray();

void ShowCarArrayWithComparer()
{
    Car[] cars = new Car[5];
    cars[0] = new Car(2, "Rusty", 80);
    cars[1] = new Car(7, "Mary", 40);
    cars[2] = new Car(4, "Viper", 30);
    cars[3] = new Car(5, "Mel", 70);
    cars[4] = new Car(8, "Chucky", 80);




    //Array.Sort(cars,new PetNameComparer());
    Array.Sort(cars, Car.SortByPetName);


    foreach (Car car in cars)
    {
        Console.WriteLine($"{car.CarId}\t{car.PetName}\t{car.CurrentSpeed}");
    }
}
ShowCarArrayWithComparer();
