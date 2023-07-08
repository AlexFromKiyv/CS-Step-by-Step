
using Generics;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

void SpecifyingGenericParameters()
{
    List<Car> cars = new()
    {
        new Car(1,"VW", "Beetle", 2020),
        new Car(2,"VW", "E-Buster", 2025)
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
        Console.Write(item + "\t");
    }
}

//UseGenericMemeber();

void UsingGeneticInterface()
{
    List<Car_v1?> cars = new()
    {
        new Car_v1(3,"VW","New Beetle",1998),
        new Car_v1(1,"VW", "Käfer", 1938),
        null,
        new Car_v1(2,"VW", "Golf", 1974),
        null
    };

    Console.WriteLine("\n Before sorting.");
    PrintListCar(cars);

    cars.Sort();

    Console.WriteLine("\n After sorting.");
    PrintListCar(cars);

    void PrintListCar(List<Car_v1?> list)
    {
        foreach (var item in list)
        {
            if (item is null)
            {
                Console.WriteLine();
            }
            else
            {
                item.ToConsole();
            }
        }
    }
}

//UsingGeneticInterface();

void InitialiazationCollection()
{
    int[] simpleArray = { 1, 2, 3 };
    PrintCollection(simpleArray);

    List<int> simpleList = new() { 1, 2, 3 };
    PrintCollection(simpleList);

    List<Point> listOfPoint = new()
    {
        new(1,2),
        new(2,3),
        new(3,4)
    };
    PrintCollection(listOfPoint);

    List<Rectangle> listOfRectangle = new()
    {
        new(){Height =90,Width =40, Location = new(1,2) },
        new(){Height =100,Width =50, Location = new(2,4) },

    };
    PrintCollection(listOfRectangle);

    void PrintCollection(ICollection collection)
    {
        Console.WriteLine(collection);
        foreach (var item in collection)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine();
    }
}

InitialiazationCollection();
