using GenericInterfaces;

void ShowCarArray()
{
    Car[] cars =
    [
        new Car(1, "Rusty", 80),
        new Car(234, "Mary", 40),
        new Car(34, "Viper", 30),
        new Car(4, "Mel", 70),
        new Car(5, "Chucky", 80),
    ];
    Array.Sort(cars);

    foreach (Car car in cars)
    {
        Console.WriteLine(car);
    }
}
//ShowCarArray();

void ShowCarArray1()
{
    Car1[] cars =
    [
        new Car1(1, "Rusty", 80),
        new Car1(234, "Mary", 40),
        new Car1(34, "Viper", 30),
        new Car1(4, "Mel", 70),
        new Car1(5, "Chucky", 80),
    ];
    Array.Sort(cars);

    foreach (Car1 car in cars)
    {
        Console.WriteLine(car);
    }
}
//ShowCarArray1();