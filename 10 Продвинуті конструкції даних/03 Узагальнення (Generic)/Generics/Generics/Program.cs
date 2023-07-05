
using Generics;

void SpecifyingGenericParameters()
{
    List<Car> cars = new() 
    {
        new Car("VW", "Beetle", 2020), 
        new Car("VW", "E-Buster", 2025) 
    };

    Car car = cars[0];
    car.Year = 2016;

    foreach (Car? item in cars)
    {
        item.ToConsole();
    }
}

//SpecifyingGenericParameters();

void UseGenericMemeber()
{
    int[] ints = { 31, 22, 13, 4, 25 };

    Array.Sort<int>(ints);

    foreach (int item in ints)
    {
        Console.Write(item+"\t");
    }
}

UseGenericMemeber();